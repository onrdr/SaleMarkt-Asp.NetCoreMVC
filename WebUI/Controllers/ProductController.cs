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

    private void HandleImageUpload(ProductViewModel model, IFormFile? file)
    {
        var wwwRootPath = _webHostEnvironment.WebRootPath;

        if (file is not null && !string.IsNullOrEmpty(model.ImageUrl))
        {
            var oldImagePath = Path.Combine(wwwRootPath, model.ImageUrl.TrimStart('/'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }

        if (file is not null)
        {            
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var imagePath = Path.Combine(wwwRootPath, "images", "product", fileName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            model.ImageUrl = @"images\product\" + fileName; 
        }
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

        TempData["SuccessMessage"] = result.Message;
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

    #region Delete
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _productService.GetByIdAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        TempData["SuccessMessage"] = result.Message;
        return View(_mapper.Map<ProductViewModel>(result.Data));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(ProductViewModel model)
    {
        var result = await _productService.DeleteProduct(model.Id);
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
