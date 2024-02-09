using AutoMapper;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Controllers;

[Authorize(Roles = RoleNames.SuperAdmin)]
public class SuperAdminController : BaseController
{
    public SuperAdminController(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        IMapper mapper)
            : base(userManager: userManager, roleManager: roleManager, mapper: mapper)
    {

    }

    #region UserList
    public IActionResult ListUsers()
    {
        return View();
    }
    #endregion

    #region Assign Role 
    public async Task<IActionResult> RoleAssign(string userId)
    {
        var user = await UserManager.FindByIdAsync(userId);
        if (user is null)
        {
            TempData["ErrorMessage"] = Messages.UserNotFound;
            return RedirectToAction(nameof(ListUsers));
        }

        ViewBag.UserId = userId;
        TempData["userId"] = userId;
        var roles = await RoleManager.Roles.ToListAsync();
        var userRoles = await UserManager.GetRolesAsync(user);
        var roleAssignViewModels = new List<RoleAssignViewModel>();

        foreach (var role in roles)
        {
            if (role.Name is not null && role.Name is not RoleNames.SuperAdmin)
            {
                var roleVm = new RoleAssignViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Exist = userRoles.Contains(role.Name)
                };

                roleAssignViewModels.Add(roleVm);
            }
        }

        return View(roleAssignViewModels);
    }

    [HttpPost]
    public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> list, string userId)
    {
        var viewBagUserId = ViewBag.UserId;
        var user = await UserManager.FindByIdAsync(userId);
        if (user is null)
        {
            TempData["ErrorMessage"] = Messages.UserNotFound;
            return RedirectToAction(nameof(ListUsers));
        }

        foreach (var model in list)
        {
            if (model.Exist)
            {
                var addResult = await UserManager.AddToRoleAsync(user, model.RoleName);
                continue;
            }
            var removeResult = await UserManager.RemoveFromRoleAsync(user, model.RoleName);
        }
        return RedirectToAction(nameof(ListUsers));
    }
    #endregion

    #region API Calls 
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await UserManager.Users.ToListAsync();
        var superAdminUser = users.FirstOrDefault(user =>
            UserManager.IsInRoleAsync(user, RoleNames.SuperAdmin).Result);
        if (superAdminUser != null)
        {
            users.Remove(superAdminUser);
            users.Insert(0, superAdminUser);
        }

        var userDataWithRoles = users.Select(user => new
        {
            user.Name,
            user.Email,
            Address = $"{user.Address} / {user.City} / {user.Country}",
            user.Id,
            Roles = UserManager.GetRolesAsync(user).Result,
            user.IsSuspended
        }).ToList();

        return Json(new { data = userDataWithRoles });
    }

    [HttpPost]
    public async Task<IActionResult> ChangeSuspendStatus(Guid userId, bool isSuspended)
    {
        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return Json(new { result = false, message = Messages.UserNotFound });
        }

        user.IsSuspended = isSuspended;
        var updateResult = await UserManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return Json(new { result = false, message = Messages.UserUpdateError });
        }

        return Json(new { result = true, message = Messages.UserUpdateSuccessfull });
    }
    #endregion
}
