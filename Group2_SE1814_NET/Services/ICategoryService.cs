using Group2_SE1814_NET.Models;

namespace Group2_SE1814_NET.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoryAsync();
    }
}
