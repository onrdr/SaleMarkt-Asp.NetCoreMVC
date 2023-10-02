using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebUI.Controllers;
public class BaseController : Controller
{
    protected UserManager<AppUser> UserManager { get; }
    protected SignInManager<AppUser> SignInManager { get; }
    protected IWebHostEnvironment WebHostEnvironment { get; } 
    protected IMapper Mapper { get; } 

    public BaseController(
        UserManager<AppUser> userManager = null, 
        SignInManager<AppUser> signInManager = null,
        IWebHostEnvironment webHostEnvironment = null,
        IMapper mapper = null)
    {
        UserManager = userManager;
        SignInManager = signInManager;
        WebHostEnvironment = webHostEnvironment;
        Mapper = mapper;
    }

    protected AppUser? CurrentUser => UserManager.GetUserAsync(HttpContext.User).Result;


    protected async Task<AppUser?> FindUserByEmailAsync(string email) => await UserManager.FindByEmailAsync(email);
}
