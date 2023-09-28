using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebUI.Controllers;
public class BaseController : Controller
{
    protected UserManager<AppUser> UserManager { get; }

    protected SignInManager<AppUser> SignInManager { get; }

    public BaseController(
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager)
    {
        UserManager = userManager;
        SignInManager = signInManager;
    }

    protected AppUser? CurrentUser => UserManager.GetUserAsync(HttpContext.User).Result;


    protected async Task<AppUser?> FindUserByEmailAsync(string email) => await UserManager.FindByEmailAsync(email);
}
