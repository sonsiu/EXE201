using Group2_SE1814_NET.Models;

namespace Group2_SE1814_NET.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoryAsync();
    }
}
