using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Controllers;

public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(
           ICategoryService categoryService,
           IMapper mapper,
           UserManager<AppUser> userManager,
           SignInManager<AppUser> signInManager,
           IWebHostEnvironment webHostEnvironment)
            : base(userManager, signInManager, webHostEnvironment, mapper)
    {
        _categoryService = categoryService;
    }

    #region Read
    public async Task<IActionResult> Index()
    {
        var result = await _categoryService.GetAllAsync(c => true);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        return View(result.Data);
    }
    #endregion

    #region Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryViewModel model, IFormFile? file)
    {
        HandleImageUpload(model, file);

        var result = await _categoryService.CreateCategory(model);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }
    #endregion

    #region Update
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        TempData["SuccessMessage"] = result.Message;
        return View(Mapper.Map<CategoryViewModel>(result.Data));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryViewModel model, IFormFile? file)
    {
        HandleImageUpload(model, file);

        var result = await _categoryService.UpdateCategory(model);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(model);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }
    #endregion

    #region Private Functions
    private void HandleImageUpload(CategoryViewModel model, IFormFile? file)
    {
        var wwwRootPath = WebHostEnvironment.WebRootPath;

        if (file is not null && !string.IsNullOrEmpty(model.ImageUrl))
            DeleteOldImage(model.ImageUrl, wwwRootPath);

        if (file is not null)
            CreateNewImage(model, file, wwwRootPath);
    }

    private static void CreateNewImage(CategoryViewModel model, IFormFile file, string wwwRootPath)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var imagePath = Path.Combine(wwwRootPath, "images", "category", fileName);

        using var fileStream = new FileStream(imagePath, FileMode.Create);
        file.CopyTo(fileStream);

        model.ImageUrl = @"images\category\" + fileName;
    }

    private static void DeleteOldImage(string? imageUrl, string wwwRootPath)
    {
        if (string.IsNullOrEmpty(imageUrl)) { return; }

        var oldImagePath = Path.Combine(wwwRootPath, imageUrl.TrimStart('/'));
        if (System.IO.File.Exists(oldImagePath))
            System.IO.File.Delete(oldImagePath);
    }
    #endregion

    #region API Calls
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync(c => true);
        return Json(new { data = categories.Data });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var categoryResult = await _categoryService.GetByIdAsync(id);
        if (!categoryResult.Success)
        {
            return Json(categoryResult);
        }

        var deleteResult = await _categoryService.DeleteCategory(id);
        if (!deleteResult.Success)
        {
            TempData["ErrorMessage"] = deleteResult.Message;
            return Json(deleteResult);
        }

        DeleteOldImage(categoryResult.Data.ImageUrl, WebHostEnvironment.WebRootPath);
        TempData["SuccessMessage"] = deleteResult.Message;
        return Json(deleteResult);
    }
    #endregion
}
