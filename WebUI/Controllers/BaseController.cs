using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity; 
using Models.ViewModels.Abstract;
using System.Security.Claims;

namespace WebUI.Controllers;

public class BaseController : Controller
{
    protected UserManager<AppUser> UserManager { get; }
    protected RoleManager<AppRole> RoleManager { get; }
    protected SignInManager<AppUser> SignInManager { get; }
    protected IWebHostEnvironment WebHostEnvironment { get; } 
    protected IMapper Mapper { get; } 

    public BaseController(
        UserManager<AppUser> userManager = null, 
        RoleManager<AppRole> roleManager = null,
        SignInManager<AppUser> signInManager = null,
        IWebHostEnvironment webHostEnvironment = null,
        IMapper mapper = null)
    {
        UserManager = userManager;
        RoleManager = roleManager;
        SignInManager = signInManager;
        WebHostEnvironment = webHostEnvironment;
        Mapper = mapper;
    }

    protected AppUser? CurrentUser => UserManager.GetUserAsync(HttpContext.User).Result;

    protected Guid GetUserId()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
        return userId;
    }

    #region Image Upload 
    protected void HandleImageUpload(IImageViewModel model, IFormFile? file)
    {
        var wwwRootPath = WebHostEnvironment.WebRootPath;

        if (file is not null && !string.IsNullOrEmpty(model.ImageUrl))
            DeleteOldImage(model.ImageUrl, wwwRootPath);

        if (file is not null)
            CreateNewImage(model, file, wwwRootPath);
    }

    protected static void CreateNewImage(IImageViewModel model, IFormFile file, string wwwRootPath)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var imagePath = Path.Combine(wwwRootPath, "images", model.FolderName, fileName);

        using var fileStream = new FileStream(imagePath, FileMode.Create);
        file.CopyTo(fileStream);

        model.ImageUrl = @$"images\{model.FolderName}\" + fileName;
    }

    protected static void DeleteOldImage(string? imageUrl, string wwwRootPath)
    {
        if (string.IsNullOrEmpty(imageUrl)) { return; }

        var oldImagePath = Path.Combine(wwwRootPath, imageUrl.TrimStart('/'));
        if (System.IO.File.Exists(oldImagePath))
            System.IO.File.Delete(oldImagePath);
    }
    #endregion
}
