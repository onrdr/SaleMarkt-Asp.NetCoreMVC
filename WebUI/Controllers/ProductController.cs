using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace WebUI.Controllers;


[Authorize(Roles = $"{RoleNames.SuperAdmin}, {RoleNames.Admin}")]
public class ProductController : BaseController
{
    private readonly IProductService _productService; 

    public ProductController(
        IProductService productService,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
            : base(webHostEnvironment: webHostEnvironment, mapper: mapper)
    {
        _productService = productService;  
    }

    #region Read
    public async Task<IActionResult> Index()
    {
        var result = await _productService.GetAllProductsAsync(c => true);
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
    public async Task<IActionResult> Create(ProductViewModel model, IFormFile? file)
    {
        HandleImageUpload(model, file);

        var result = await _productService.CreateProductAsync(model);
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
        var result = await _productService.GetProductByIdAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        return View(Mapper.Map<ProductViewModel>(result.Data));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductViewModel model, IFormFile? file)
    {
        HandleImageUpload(model, file);

        var result = await _productService.UpdateProductAsync(model);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }
    #endregion  

    #region API Calls

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsWithCategoryAsync(c => true);
        return Json(new { data = products.Data });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {  
        var productResult = await _productService.GetProductByIdAsync(id);
        if (!productResult.Success)
        {
            return Json(productResult);
        }

        var deleteResult = await _productService.DeleteProductAsync(id);
        if (!deleteResult.Success)
        {
            TempData["ErrorMessage"] = deleteResult.Message;
            return Json(deleteResult);
        }

        DeleteOldImage(productResult.Data.ImageUrl, WebHostEnvironment.WebRootPath);
        TempData["SuccessMessage"] = deleteResult.Message;
        return Json(deleteResult);
    }
    #endregion
}
