using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Controllers;

public class AccountController : BaseController
{
    private readonly List<string> ErrorList = new();

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
            : base(userManager, signInManager, webHostEnvironment, mapper) { }

    #region Login & Logout
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
        if (await UserManager.IsLockedOutAsync(user))
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
            ErrorList.Add($"Login Failed #{fail}: Your account has been locked for 5 minutes");
        }

        ErrorList.Add($"Login failed #{fail}: Invalid Username or password");
    }

    private async Task<IActionResult> ResetAccessFailCountAndLogin(AppUser user)
    {
        await UserManager.ResetAccessFailedCountAsync(user);

        return TempData["ReturnUrl"] as string is not null
            ? Redirect(TempData["ReturnUrl"] as string)
            : RedirectToAction(nameof(Index), "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await SignInManager.SignOutAsync();

        return RedirectToAction(nameof(Index), "Home");
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

        return !result.Succeeded
            ? AddCreateErrorsAndSendToClient(model, result)
            : RedirectToAction(nameof(Login), routeValues: new { email = model.Email, retunUrl = string.Empty });
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

    #region Change Password      
    public IActionResult PasswordChange()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> PasswordChange(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        AppUser user = CurrentUser;

        if (user == null)
            return View(model);

        bool exist = await UserManager.CheckPasswordAsync(user, model.OldPassword);

        if (!exist)
        {
            ModelState.AddModelError("", "Old Password is wrong");
            return View(model);
        }

        IdentityResult result = await UserManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Password could not changed");
            return View(model);
        }

        await UserManager.UpdateSecurityStampAsync(user);
        await SignInManager.SignOutAsync();
        await SignInManager.PasswordSignInAsync(user, model.NewPassword, true, false);

        return View(model);
    }
    #endregion
}
