using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace WebUI.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    #region Read
    public async Task<IActionResult> Index()
    {
        var result = await _categoryService.GetAllAsync(c => true);
        if ( !result.Success)
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
    public async Task<IActionResult> Create(CategoryViewModel model)
    {
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
        return View(_mapper.Map<CategoryViewModel>(result.Data));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryViewModel model)
    {
        var result = await _categoryService.UpdateCategory(model);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }
    #endregion

    #region Delete
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        TempData["SuccessMessage"] = result.Message;
        return View(_mapper.Map<CategoryViewModel>(result.Data));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(CategoryViewModel model)
    {
        var result = await _categoryService.DeleteCategory(model.Id); 
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }
    #endregion

}
