using AutoMapper;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Identity;

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

    public IActionResult ListUsers()
    {
        return View();
    }

    #region API Calls
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await UserManager.Users.ToListAsync(); 
        var superAdminUser = users.FirstOrDefault(user => user.Role == RoleNames.SuperAdmin);

        if (superAdminUser != null)
        {
            users.Remove(superAdminUser);
            users.Insert(0, superAdminUser);
        }

        return Json(new { data = users});
    }

    [HttpGet]
    public async Task<IActionResult> ListRoles()
    {
        var roles = await RoleManager.Roles.ToListAsync();
        return View(roles);
    }

    [HttpPut]
    public async Task<IActionResult> ChangeSuspendStatus(Guid userId, bool isSuspended)
    {
        var user = await UserManager.FindByIdAsync(userId.ToString()); 
        if (user == null)
        {
            TempData["ErrorMessage"] = Messages.UserNotFound;
            return Json(Messages.UserNotFound);
        }

        if (user.Role == RoleNames.SuperAdmin)
        { 
            return BadRequest();
        }

        user.IsSuspended = isSuspended;
        var updateResult = await UserManager.UpdateAsync(user);
        if (updateResult.Succeeded)
        {
            TempData["SuccessMessage"] = Messages.UserUpdateSuccessfull;
        }
        else
        {
            TempData["SuccessMessage"] = Messages.UserUpdateError;
        }

        return Json(TempData["SuccessMessage"]);
    }
    #endregion
}
