using AutoMapper;
using Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Controllers;

public class UserController : BaseController
{
    private static readonly List<string> ErrorList = new();

    public UserController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IMapper mapper)
            : base(userManager, signInManager, mapper: mapper)
    {

    }

    #region Company Details Info & Edit
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
        CurrentUser.UserName = model.Name.Trim().Replace(" ", "-");
        CurrentUser.Email = model.Email;
        CurrentUser.PhoneNumber = model.PhoneNumber;
        CurrentUser.Address = model.Address;
        CurrentUser.City = model.City;
        CurrentUser.Country = model.Country;
        CurrentUser.PostalCode = model.PostalCode;

        return await UserManager.UpdateAsync(CurrentUser);
    }
    #endregion

    #region Logout
    public async Task<IActionResult> Logout()
    {
        await SignInManager.SignOutAsync();

        TempData["SuccessMessage"] = "Logout Successfull";
        return RedirectToAction(nameof(Index), "Home");
    }
    #endregion

    #region Change Password      
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        ErrorList.Clear();
        TempData["ModelError"] = ErrorList;

        if (!ModelState.IsValid)
        {
            return AddModelErrorsAndSendToClient(model);
        }

        if (CurrentUser is null)
        {
            TempData["ErrorMessage"] = Messages.UserNotFound;
            return View();
        }

        bool oldPasswordCorrect = await UserManager.CheckPasswordAsync(CurrentUser, model.OldPassword);
        if (!oldPasswordCorrect)
        {
            ErrorList.Add("Old Password is wrong");
            return View(model);
        }

        var changeResult = await CheckIfChangePasswordSuccessfull(model);
        return !changeResult
            ? View(model)
            : await CompleteChangePasswordProcess(model);
    }

    private async Task<bool> CheckIfChangePasswordSuccessfull(ChangePasswordViewModel model)
    {
        IdentityResult result = await UserManager.ChangePasswordAsync(CurrentUser, model.OldPassword, model.NewPassword);
        if (result.Succeeded)
        {
            return true;
        }

        foreach (var error in result.Errors)
        {
            ErrorList.Add(error.Description);
        };
        return false;
    }

    private async Task<IActionResult> CompleteChangePasswordProcess(ChangePasswordViewModel model)
    {
        await UserManager.UpdateSecurityStampAsync(CurrentUser);
        await SignInManager.SignOutAsync();
        await SignInManager.PasswordSignInAsync(CurrentUser, model.NewPassword, true, false);
        TempData["SuccessMessage"] = "Password successfully changed";

        return View(model);
    }

    private IActionResult AddModelErrorsAndSendToClient(ChangePasswordViewModel model)
    {
        var errorMessagesFinal = ModelState.Values
        .SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        TempData["ModelError"] = errorMessagesFinal;

        return View(model);
    }
    #endregion
}
