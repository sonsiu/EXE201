using Group2_SE1814_NET.Models;

namespace Group2_SE1814_NET.Repositories
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllBrandAsync();
    }
}
