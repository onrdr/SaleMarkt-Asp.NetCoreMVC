using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace WebUI.Controllers;

[Authorize(Roles = $"{RoleNames.SuperAdmin}, {RoleNames.Admin}")]
public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(
           ICategoryService categoryService,
           IWebHostEnvironment webHostEnvironment,
           IMapper mapper)
            : base(webHostEnvironment: webHostEnvironment, mapper: mapper)
    {
        _categoryService = categoryService;
    }

    #region Read
    public async Task<IActionResult> Index()
    {
        var result = await _categoryService.GetAllCategoriesAsync(c => true);
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

        var result = await _categoryService.CreateCategoryAsync(model);
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
        var result = await _categoryService.GetCategoryByIdAsync(id);
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

        var result = await _categoryService.UpdateCategoryAsync(model);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(model);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }
    #endregion 

    #region API Calls
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllCategoriesWithProductsAsync(c => true);
        return Json(new { data = categories.Data });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var categoryResult = await _categoryService.GetCategoryByIdAsync(id);
        if (!categoryResult.Success)
        {
            return Json(categoryResult);
        }

        var deleteResult = await _categoryService.DeleteCategoryAsync(id);
        if (!deleteResult.Success)
        {
            return Json(deleteResult);
        }

        DeleteOldImage(categoryResult.Data.ImageUrl, WebHostEnvironment.WebRootPath); 
        return Json(deleteResult);
    }
    #endregion
}
