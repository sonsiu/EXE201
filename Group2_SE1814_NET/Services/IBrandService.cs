using Group2_SE1814_NET.Models;

namespace Group2_SE1814_NET.Services {
    public interface IBrandService {
        public Task<List<Brand>> GetAllBrandAsync();
    }
}
