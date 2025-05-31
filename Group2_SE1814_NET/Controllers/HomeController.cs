using System.Diagnostics;
using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.Services;
using Microsoft.AspNetCore.Mvc;

namespace Group2_SE1814_NET.Controllers {
    public class HomeController : Controller {
        private readonly IProductService _productService;

        public HomeController(IProductService productService) {
            _productService = productService;
        }


        public async Task<IActionResult> Index() {
            var products = await _productService.top8NewestProduct();
            return View(products);
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
