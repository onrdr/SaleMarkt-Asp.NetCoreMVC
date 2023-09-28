using Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var productResult = await _productService.GetAllProductsWithCategory(c => true); 

            return View(productResult.Data);
        }

        public async Task<IActionResult> Details(Guid productId)
        {
            var productResult = await _productService.GetProductWithCategory(productId);

            return View(productResult.Data);
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}