using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace WebUI.Controllers;

[Authorize(Roles = $"{RoleNames.SuperAdmin}, {RoleNames.Admin}")]
public class CompanyController : BaseController
{
    private readonly ICompanyService _companyService;
    private readonly IMapper _mapper;

    public CompanyController(
        ICompanyService companyService,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
            : base(webHostEnvironment: webHostEnvironment)
    {
        _companyService = companyService;
        _mapper = mapper;
    }

    #region Company Details Info & Edit
    public async Task<IActionResult> Details()
    {
        var result = await _companyService.GetCompanyAsync();
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        var companyViewModel = _mapper.Map<CompanyViewModel>(result.Data);
        return View(companyViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Details(CompanyViewModel model, IFormFile? file)
    {
        HandleImageUpload(model, file);

        var result = await _companyService.UpdateCompany(model);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Details));
    }
    #endregion 
}
