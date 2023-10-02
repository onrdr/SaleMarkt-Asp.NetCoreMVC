using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Controllers;

public class HomeController : BaseController
{
    private readonly IProductService _productService;
    private readonly List<string> ErrorList = new();

    public HomeController(
        IProductService productService,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IMapper mapper)
            : base(userManager: userManager, signInManager: signInManager, mapper: mapper)
    {
        _productService = productService;
    }


    #region HomePage Product List & Details
    public async Task<IActionResult> Index()
    {
        var productResult = await _productService.GetAllProductsWithCategory(c => true);

        return View(productResult.Data);
    }

    public async Task<IActionResult> Details(Guid productId)
    {
        var productResult = await _productService.GetProductWithCategory(productId);

        return View(productResult.Data);
    }
    #endregion

    #region Login
    public IActionResult Login(string email = null, string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(returnUrl))
        {
            TempData["ReturnUrl"] = returnUrl;
        }

        return email is null ? View() : View(new LoginViewModel { Email = email });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        TempData["ModelError"] = ErrorList;

        if (!ModelState.IsValid)
        {
            return AddModelErrorsAndSendToClient(loginModel: model);
        }

        var user = await UserManager.FindByEmailAsync(model.Email);

        var userAccountResult = await CheckIfUserExistsAndAccountNotLocked(user);
        if (!userAccountResult)
        {
            return View(model);
        }

        var result = await GetLoginResult(model, user);
        if (!result.Succeeded)
        {
            await IncreaseAccessFailCountAndLockIfNeccessary(user);
            return View(model);
        }

        return await ResetAccessFailCountAndLogin(user);
    }
    private IActionResult AddModelErrorsAndSendToClient(LoginViewModel? loginModel = null, RegisterViewModel? registerModel = null)
    {
        var errorMessagesFinal = ModelState.Values
        .SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        TempData["ModelError"] = errorMessagesFinal;

        return loginModel is not null ? View(loginModel) : View(registerModel);
    }

    private async Task<bool> CheckIfUserExistsAndAccountNotLocked(AppUser? user)
    {
        if (user is null)
        {
            ErrorList.Add("User not found");
            return false;
        }

        var isLocked = await UserManager.IsLockedOutAsync(user);
        if (isLocked)
        {
            var endTime = await UserManager.GetLockoutEndDateAsync(user);
            var timeDifference = (endTime - DateTimeOffset.UtcNow).Value;
            var minutes = (int)timeDifference.TotalMinutes;
            var seconds = timeDifference.Seconds;

            ErrorList.Add($"Your account has been locked. Please try again in {minutes}min:{seconds}sec");
            return false;
        }
        return true;
    }

    private async Task<Microsoft.AspNetCore.Identity.SignInResult> GetLoginResult(LoginViewModel loginModel, AppUser user)
    {
        await SignInManager.SignOutAsync();
        return await SignInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, true);
    }

    private async Task IncreaseAccessFailCountAndLockIfNeccessary(AppUser user)
    {
        int fail = await UserManager.GetAccessFailedCountAsync(user);
        if (fail >= 3)
        {
            await UserManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(5)));
            ErrorList.Add($"Your account has been locked for 5 minutes");
            await UserManager.ResetAccessFailedCountAsync(user);
            return;
        }

        ErrorList.Add($"Login failed #{fail}: Invalid Username or password. \nRemaining Attempt: {3 - fail}");
    }

    private async Task<IActionResult> ResetAccessFailCountAndLogin(AppUser user)
    {
        await UserManager.ResetAccessFailedCountAsync(user);

        return TempData["ReturnUrl"] as string is not null
            ? Redirect(TempData["ReturnUrl"] as string)
            : RedirectToAction(nameof(Index), "Home");
    }
    #endregion

    #region Register
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        TempData["ModelError"] = ErrorList;

        if (!ModelState.IsValid)
        {
            return AddModelErrorsAndSendToClient(registerModel: model);
        }

        AppUser user = Mapper.Map<AppUser>(model);
        var result = await UserManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return AddCreateErrorsAndSendToClient(model, result);
        }

        TempData["SuccessMessage"] = "Register Successfull";
        return RedirectToAction(nameof(Login), routeValues: new { email = model.Email, retunUrl = string.Empty });
    }

    private IActionResult AddCreateErrorsAndSendToClient(RegisterViewModel model, IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ErrorList.Add(error.Description);
        }
        return View(model);
    }
    #endregion

    #region Contact
    public IActionResult Contact()
    {
        return View();
    }
    #endregion
}