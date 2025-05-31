using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Group2_SE1814_NET.Repositories {
    public class ProductRepository : IProductRepository {
        private readonly WebkinhdoanhquanaoContext _context;

        public ProductRepository(WebkinhdoanhquanaoContext context) {
            _context = context;
        }

        public Product GetProductById(int id) {
            return _context.Products.Find(id); // Tìm sản phẩm theo ID
        }

        public Product GetProductByIdIncludeCategory(int id) {
            return _context.Products
                   .Include(x => x.Category) // Eager load Category
                   .FirstOrDefault(x => x.Id == id); // Tìm sản phẩm theo ID
        }

        public async Task<List<Product>> GetProductsAsync(int? brand, int? category, decimal? minPrice, decimal? maxPrice, string? searchQuery, string sortOrder) {
            // Lọc sản phẩm theo các điều kiện
            var products = _context.Products.Include(p => p.Category).Where(p => p.Status == true).AsQueryable();

            if (brand.HasValue) {
                products = products.Where(p => p.BrandId == brand.Value);
            }

            if (category.HasValue) {
                products = products.Where(p => p.CategoryId == category.Value);
            }

            if (minPrice.HasValue && maxPrice.HasValue) {
                products = products.Where(p => p.Price >= minPrice.Value && p.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery)) {
                products = products.Where(p => p.Name.Contains(searchQuery));
            }

            if (sortOrder == "price_asc") {
                products = products.OrderBy(p => p.Price);
            } else if (sortOrder == "price_desc") {
                products = products.OrderByDescending(p => p.Price);
            }
            return await products.ToListAsync();
        }

        public void reduceQuantity(List<Item> cart) {
            foreach (var item in cart) {
                Product p = GetProductById(item.Product.Id);
                p.Quantity -= item.Quantity;
                _context.Update(p);
                _context.SaveChanges();
            }
        }

        public async Task<List<Product>> top8NewestProduct() {
            return await _context.Products.Include(p => p.Category).Where(p => p.Status).OrderByDescending(p => p.Id).Take(8).ToListAsync();
        }
    }
}
