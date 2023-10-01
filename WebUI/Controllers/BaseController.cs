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
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
    {
        UserManager = userManager;
        SignInManager = signInManager;
        WebHostEnvironment = webHostEnvironment;
        Mapper = mapper;
    }

    protected AppUser? CurrentUser => UserManager.GetUserAsync(HttpContext.User).Result;


    protected async Task<AppUser?> FindUserByEmailAsync(string email) => await UserManager.FindByEmailAsync(email);
}
