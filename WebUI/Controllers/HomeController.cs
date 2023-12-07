using AutoMapper;
using Business.Services.Abstract;
using Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;
using Core.Constants;
using Core.Utilities.Pagination;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace WebUI.Controllers;

public class HomeController : BaseController
{
    private readonly ICompanyService _companyService;
    private readonly IProductService _productService;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IEmailService _emailService;
    private readonly IViewRenderService _viewRenderService;
    private readonly List<string> ErrorList = new();
    private const int PAGE_NUMBER = 1;
    private const int PAGE_SIZE = 8;

    public HomeController(
        ICompanyService companyService,
        IProductService productService,
        IEmailService emailService,
        IMapper mapper,
        IViewRenderService viewRenderService,
        IShoppingCartService shoppingCartService,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager)
            : base(userManager: userManager, signInManager: signInManager, mapper: mapper)
    {
        _companyService = companyService;
        _productService = productService;
        _emailService = emailService;
        _viewRenderService = viewRenderService;
        _shoppingCartService = shoppingCartService;
    }

    #region Home Page
    public IActionResult Index()
    {
        return View();
    }
    #endregion

    #region HomePage Product List
    public async Task<IActionResult> ProductList(int page = PAGE_NUMBER, int pageSize = PAGE_SIZE)
    {
        if (TempData.ContainsKey("FilteredProducts"))
        {
            var serializedModel = TempData["FilteredProducts"] as string;

            if (!string.IsNullOrEmpty(serializedModel))
            {
                var tempDataModel = JsonConvert.DeserializeObject<PaginatedList<Product>>(serializedModel);
                TempData.Remove("FilteredProducts");

                return View(tempDataModel);
            }
        }

        var productResult = await _productService.GetAllProductsWithCategoryAsync(c => true);
        if (!productResult.Success)
        {
            TempData["ErrorMessage"] = "There is no product to show";
            return RedirectToAction(nameof(Index));
        }

        var products = productResult.Data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var model = new PaginatedList<Product>(products, productResult.Data.Count(), page, pageSize);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ProductList(string color, double? minPrice, double? maxPrice, string productName, string category)
    {
        var filterExists = CheckIfAnyFilterExists(color, minPrice, maxPrice, productName, category);
        if (!filterExists)
        {
            return RedirectToAction(nameof(ProductList));
        }

        if (minPrice > maxPrice)
        {
            TempData["ErrorMessage"] = "Max Price cannot be lower than Min Price";
            return RedirectToAction(nameof(ProductList));
        }

        var combinedPredicate = CombineFiltersAndGetFinalPredicate(color, minPrice, maxPrice, productName, category);

        var productResult = await _productService.GetAllProductsWithCategoryAsync(combinedPredicate);
        if (!productResult.Success)
        {
            TempData["ErrorMessage"] = productResult.Message;
            return RedirectToAction(nameof(ProductList));
        }
        var serializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        var model = new PaginatedList<Product>(productResult.Data, productResult.Data.Count(), page: PAGE_NUMBER, pageSize: PAGE_SIZE);
        var serializedModel = JsonConvert.SerializeObject(model, serializerSettings);
        TempData["FilteredProducts"] = serializedModel;

        return RedirectToAction(nameof(ProductList));
    }

    private static Expression<Func<Product, bool>> CombineFiltersAndGetFinalPredicate(
        string color, double? minPrice, double? maxPrice, string productName, string category)
    {
        Expression<Func<Product, bool>> combinedPredicate = p => true;
        combinedPredicate = AddColorFilterIfNotNull(color, combinedPredicate);
        combinedPredicate = AddPriceFilterIfNotNull(minPrice, maxPrice, combinedPredicate);
        combinedPredicate = AddNameFilterIfNotNull(productName, combinedPredicate);
        combinedPredicate = AddCategoryFilterIfNotNull(category, combinedPredicate);
        return combinedPredicate;
    }

    private static bool CheckIfAnyFilterExists(
        string color, double? minPrice, double? maxPrice, string productName, string category)
    {
        if (color is null && productName is null && category is null &&
            minPrice is null && maxPrice is null)
        {
            return false;
        }

        return true;
    }

    private static Expression<Func<Product, bool>> AddNameFilterIfNotNull(
        string productName, Expression<Func<Product, bool>> combinedPredicate)
    {
        if (!string.IsNullOrWhiteSpace(productName))
        {
            combinedPredicate = CombinePredicates(combinedPredicate, p => 
                p.Title.ToLower().Contains(productName.ToLower()));
        }
        return combinedPredicate;
    }

    private static Expression<Func<Product, bool>> AddCategoryFilterIfNotNull(
        string category, Expression<Func<Product, bool>> combinedPredicate)
    {
        if (!string.IsNullOrWhiteSpace(category))
        {
            combinedPredicate = CombinePredicates(combinedPredicate, p => 
                p.Category.Name.ToLower().Contains(category.ToLower()));
        }
        return combinedPredicate;
    }

    private static Expression<Func<Product, bool>> AddPriceFilterIfNotNull(
        double? minPrice, double? maxPrice, Expression<Func<Product, bool>> combinedPredicate)
    {
        if (minPrice.HasValue)
        {
            combinedPredicate = CombinePredicates(combinedPredicate, p => p.Price >= minPrice);
        }

        if (maxPrice.HasValue)
        {
            combinedPredicate = CombinePredicates(combinedPredicate, p => p.Price <= maxPrice);
        }

        return combinedPredicate;
    }

    private static Expression<Func<Product, bool>> AddColorFilterIfNotNull(
        string color, Expression<Func<Product, bool>> combinedPredicate)
    {
        if (!string.IsNullOrWhiteSpace(color) && color != "All Colors")
        {
            combinedPredicate = CombinePredicates(combinedPredicate, p => p.Color == color);
        }

        return combinedPredicate;
    }

    private static Expression<Func<T, bool>> CombinePredicates<T>(
        Expression<Func<T, bool>> predicate1, Expression<Func<T, bool>> predicate2)
    {
        var parameter = Expression.Parameter(typeof(T));
        var combined = Expression.AndAlso(
            Expression.Invoke(predicate1, parameter),
            Expression.Invoke(predicate2, parameter)
        );
        return Expression.Lambda<Func<T, bool>>(combined, parameter);
    }
    #endregion

    #region HomePage Product Details
    public async Task<IActionResult> Details(Guid productId)
    {
        var productResult = await _productService.GetProductByIdWithCategoryAsync(productId);
        var shoppingCart = new ShoppingCart
        {
            Product = productResult.Data,
            Count = 1,
            ProductId = productId
        };

        return View(shoppingCart);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Details(ShoppingCart model)
    {
        model.AppUserId = GetUserId();
        var shoppingCartResult = await _shoppingCartService.GetAllShoppingCartsWithProductAsync(
            s => s.AppUserId == model.AppUserId && 
            s.ProductId == model.ProductId &&
            s.ProductSize == model.ProductSize);

        if (shoppingCartResult.Data is null)
        {
            var addResult = await _shoppingCartService.CreateShoppingCartAsync(model);
            if (!addResult.Success)
            {
                TempData["ErrorMessage"] = addResult.Message;
                return View(model);
            }
        }
        else
        {
            shoppingCartResult.Data.First().Count += model.Count;
            var updateResult = await _shoppingCartService.UpdateShoppingCartAsync(shoppingCartResult.Data.First());
            if (!updateResult.Success)
            {
                TempData["ErrorMessage"] = updateResult.Message;
                return View(model);
            }
        }

        TempData["SuccessMessage"] = "Cart successfully updated";
        return RedirectToAction(nameof(ProductList));
    }
    #endregion

    #region Login
    public IActionResult Login(string email, string returnUrl)
    {
        ModelState.Clear();

        if (!string.IsNullOrEmpty(returnUrl))
        {
            TempData["ReturnUrl"] = returnUrl;
        }

        return email is null 
            ? View() 
            : View(new LoginViewModel { Email = email });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        ErrorList.Clear();
        TempData["ModelError"] = ErrorList;

        if (!ModelState.IsValid)
        {
            return AddModelErrorsAndSendToClient(loginModel: model);
        }

        var user = await UserManager.FindByEmailAsync(model.Email);

        var userAccountResult = await CheckIfUserExistsAndAccountNotLockedOrSuspended(user);
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

    private async Task<bool> CheckIfUserExistsAndAccountNotLockedOrSuspended(AppUser? user)
    {
        if (user is null)
        {
            ErrorList.Add("User not found");
            return false;
        }

        if (user.IsSuspended)
        {
            ErrorList.Add("Your account has been suspended. Please contact administration");
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
        TempData["SuccessMessage"] = "Login Successfull";
        var redirectUrl = TempData["ReturnUrl"] as string; 

        return redirectUrl is not null
            ? Redirect(redirectUrl)
            : RedirectToAction(nameof(Index));
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
            return AddRegisterErrorsAndSendToClient(model, result);
        }

        await UserManager.AddToRoleAsync(user, RoleNames.Customer);
        TempData["SuccessMessage"] = "Register Successfull";
        return RedirectToAction(nameof(Login), routeValues: new { email = model.Email, retunUrl = string.Empty });
    }

    private IActionResult AddRegisterErrorsAndSendToClient(RegisterViewModel model, IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ErrorList.Add(error.Description);
        }
        return View(model);
    }
    #endregion

    #region ResetPassword
    public IActionResult ResetPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        ErrorList.Clear();
        TempData["ModelError"] = ErrorList;

        AppUser? user = await UserManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            ErrorList.Add("This email address is not registered");
            return View(model);
        }

        string passwordResetToken = await UserManager.GeneratePasswordResetTokenAsync(user);
        string? passwordResetLink = Url.Action("ResetPasswordConfirm", "Home", new
        {
            userId = user.Id,
            token = passwordResetToken
        }, Request.Scheme);

        var emailMessage = await _viewRenderService.RenderViewToStringAsync("_ResetPasswordEmailTemplate", passwordResetLink);
        var emailResult = await _emailService.SendResetEmailAsync(user.Email, emailMessage);
        if (!emailResult.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] = "An error occured in email service. Please contact administration";
            return View(model);
        }

        return View();
    }

    public IActionResult ResetPasswordConfirm(string userId, string token)
    {
        var model = new ResetPasswordConfirmViewModel
        {
            UserId = userId,
            Token = token
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPasswordConfirm(ResetPasswordConfirmViewModel model)
    {
        ErrorList.Clear();
        TempData["ModelError"] = ErrorList;
        AppUser? user = await UserManager.FindByIdAsync(model.UserId.ToString());
        if (user is null)
        {
            ErrorList.Add(Messages.UserNotFound);
            return View(model);
        }

        IdentityResult result = await UserManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ErrorList.Add(error.Description);
            }
            return View(model);
        }

        await UserManager.UpdateSecurityStampAsync(user);
        return View(model);
    }
    #endregion

    #region Contact
    public async Task<IActionResult> Contact()
    {
        var companyResult = await _companyService.GetCompanyAsync();
        var company = companyResult.Data;

        return View(Mapper.Map<CompanyViewModel>(company));
    }
    #endregion

    #region About
    public IActionResult About()
    {
        return View();
    }
    #endregion
}