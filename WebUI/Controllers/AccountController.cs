using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Controllers;

public class AccountController : BaseController
{
    private readonly IMapper _mapper;
    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IMapper mapper) : base(userManager, signInManager)
    {
        _mapper = mapper;
    }

    public IActionResult Login(string returnUrl)
    {
        TempData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginModel)
    {
        if (!ModelState.IsValid)
            return View(loginModel);

        AppUser user = await UserManager.FindByEmailAsync(loginModel.Email);

        if (user == null)
        {
            ModelState.AddModelError(nameof(LoginViewModel.Email), "User not found");
            return View(loginModel);
        }

        if (await UserManager.IsLockedOutAsync(user))
        {
            ModelState.AddModelError("", "Your account has been locked. Please try again later");
            return View(loginModel);
        }

        await SignInManager.SignOutAsync();
        Microsoft.AspNetCore.Identity.SignInResult result = await SignInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, true);

        if (!result.Succeeded)
        {
            await UserManager.AccessFailedAsync(user);
            int fail = await UserManager.GetAccessFailedCountAsync(user);
            ModelState.AddModelError("", $"Login Failed #{fail}");

            if (fail == 3)
            {
                await UserManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(20)));
                ModelState.AddModelError("", "Login Failed #3: Your account has been locked for 20 minutes");
            }
            else
                ModelState.AddModelError("", "Invalid Username or password");

            return View(loginModel);
        }

        await UserManager.ResetAccessFailedCountAsync(user);
        if (TempData["ReturnUrl"] != null)
            return Redirect(TempData["ReturnUrl"].ToString());

        if (user.UserName == "admin")
        {
            return RedirectToAction("Index", "Admin");
        }

        return RedirectToAction(nameof(Index), "Home");
    }

    public IActionResult Register()
    {
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await SignInManager.SignOutAsync();

        return RedirectToAction(nameof(Index), "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        AppUser user = _mapper.Map<AppUser>(model);
        var result = await UserManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", result.ToString());
            return View(model);
        }

        return RedirectToAction(nameof(Login));
    }

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
