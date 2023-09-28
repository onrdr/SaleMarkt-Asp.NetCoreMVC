using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace WebUI.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(
        IProductService productService,
        IMapper mapper,
        IWebHostEnvironment webHostEnvironment)
    {
        _productService = productService;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }

    #region Read
    public async Task<IActionResult> Index()
    {
        var result = await _productService.GetAllAsync(c => true);
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

        var result = await _productService.CreateProduct(model);
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
        var result = await _productService.GetByIdAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        return View(_mapper.Map<ProductViewModel>(result.Data));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductViewModel model, IFormFile? file)
    {
        HandleImageUpload(model, file);

        var result = await _productService.UpdateProduct(model);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }
    #endregion 

    #region Private Functions
    private void HandleImageUpload(ProductViewModel model, IFormFile? file)
    {
        var wwwRootPath = _webHostEnvironment.WebRootPath;

        if (file is not null && !string.IsNullOrEmpty(model.ImageUrl))
            DeleteOldImage(model.ImageUrl, wwwRootPath);

        if (file is not null)
            CreateNewImage(model, file, wwwRootPath);
    }

    private static void CreateNewImage(ProductViewModel model, IFormFile file, string wwwRootPath)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var imagePath = Path.Combine(wwwRootPath, "images", "product", fileName);

        using var fileStream = new FileStream(imagePath, FileMode.Create);
        file.CopyTo(fileStream);

        model.ImageUrl = @"images\product\" + fileName;
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
        var products = await _productService.GetAllProductsWithCategory(c => true);
        return Json(new { data = products.Data });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {  
        var productResult = await _productService.GetByIdAsync(id);
        if (!productResult.Success)
        {
            return Json(productResult);
        }

        var deleteResult = await _productService.DeleteProduct(id);
        if (!deleteResult.Success)
        {
            TempData["ErrorMessage"] = deleteResult.Message;
            return Json(deleteResult);
        }

        DeleteOldImage(productResult.Data.ImageUrl, _webHostEnvironment.WebRootPath);
        TempData["SuccessMessage"] = deleteResult.Message;
        return Json(deleteResult);
    }
    #endregion
}
