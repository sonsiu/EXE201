using Group2_SE1814_NET.Models;
using X.PagedList;

namespace Group2_SE1814_NET.ViewModels {
    public class ShopViewModel {
        public IPagedList<Product> Products { get; set; } = default!;
        public List<Brand> Brands { get; set; }
        public List<Category> Categories { get; set; }
        public List<Post> Top4 { get; set; }
        public string? Search { get; set; }
        public string? Sort { get; set; } = "default"; //default
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public int CurrentPage { get; set; } = 1;
    }
}
