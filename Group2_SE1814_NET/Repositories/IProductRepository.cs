using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.ViewModels;

namespace Group2_SE1814_NET.Repositories {
    public interface IProductRepository {
        public Task<List<Product>> GetProductsAsync(int? brand, int? category, decimal? minPrice, decimal? maxPrice, string? searchQuery, string sortOrder);
        public Product GetProductById(int id);

        public Product GetProductByIdIncludeCategory(int id);
        public void reduceQuantity(List<Item> cart);

        public Task<List<Product>> top8NewestProduct();
    }
}
