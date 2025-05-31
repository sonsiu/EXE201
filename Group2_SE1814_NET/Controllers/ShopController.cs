using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.Services;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Group2_SE1814_NET.Controllers {
    public class ShopController : Controller {
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;

        public ShopController(IProductService productService, IBrandService brandService, ICategoryService categoryService) {
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(int? brand, int? category, string? searchQuery, string sortOrder, int page = 1, decimal? minPrice = 0, decimal? maxPrice = 1000) {
            int pageSize = 6;
            var products = await _productService.GetProductsAsync(brand, category, minPrice, maxPrice, searchQuery, sortOrder);
            // Sử dụng X.PagedList để phân trang
            var pagedProducts = products.ToPagedList(page, pageSize);

            var categories = await _categoryService.GetAllCategoryAsync();
            categories.Insert(0, new Category
            {
                Id = -1,
                Name = "All",
                Products = products
                //Posts = allPosts
            });

            ViewBag.Categories = categories;

            var brands = await _brandService.GetAllBrandAsync();
            brands.Insert(0, new Brand
            {
                Id = -1,
                Name = "All",
                Products = products
            });
            ViewBag.Brands = brands;
            return View(pagedProducts);
        }
        public async Task<IActionResult> ProductList(int? brand, int? category, decimal? minPrice, decimal? maxPrice, string? searchQuery, string sortOrder, int page = 1) {

            int pageSize = 6;
            if (brand == -1) {
                brand = null;  // Hoặc không dùng bộ lọc theo brand
            }
            if (category == -1) {
                category = null;  // Hoặc không dùng bộ lọc theo brand
            }
            var products = await _productService.GetProductsAsync(brand, category, minPrice, maxPrice, searchQuery, sortOrder);
            // Sử dụng X.PagedList để phân trang
            var pagedProducts = products.ToPagedList(page, pageSize);

            return PartialView("ProductList", pagedProducts);
        }

        public IActionResult Detail(int id) {
            var product = _productService.GetProductByIdIncludeCategory(id);
            return View(product);
        }
    }
}
