using AutoMapper;
using Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Controllers;

public class UserController : BaseController
{ 
    public UserController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IMapper mapper) 
            : base(userManager, signInManager, mapper: mapper) 
    {

    }
    public IActionResult Details()
    { 
        if (CurrentUser is null)
        {
            TempData["ErrorMessage"] = Messages.UserNotFound;
            return View();
        }

        return View(Mapper.Map<UserViewModel>(CurrentUser));
    }

    [HttpPost]
    public async Task<IActionResult> Details(UserViewModel user)
    {
        if (CurrentUser is null)
        {
            TempData["ErrorMessage"] = Messages.UserNotFound;
            return View();
        }

        var result = await CompleteUpdate(user);        
        if (!result.Succeeded)
        {
            var errors = result.Errors.ToList();
            TempData["ErrorMessage"] = Messages.UserUpdateError;
            return View();
        }

        TempData["SuccessMessage"] = Messages.UserUpdateSuccessfull;
        return RedirectToAction(nameof(Details));
    }

    private async Task<IdentityResult> CompleteUpdate(UserViewModel model)
    {
       CurrentUser.Name = model.Name;
       CurrentUser.UserName = model.Name.Replace(" ", "").Trim().ToLower();
       CurrentUser.Email = model.Email;
       CurrentUser.PhoneNumber = model.PhoneNumber;
       CurrentUser.Address = model.Address;
       CurrentUser.City = model.City;
       CurrentUser.Country = model.Country;
       CurrentUser.PostalCode = model.PostalCode;

        return await UserManager.UpdateAsync(CurrentUser);
    }

    #region Logout
    public async Task<IActionResult> Logout()
    {
        await SignInManager.SignOutAsync();

        return RedirectToAction(nameof(Index), "Home");
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
