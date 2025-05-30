using Group2_SE1814_NET.Models;
using Microsoft.EntityFrameworkCore;

namespace Group2_SE1814_NET.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly WebkinhdoanhquanaoContext _context;

        public CategoryRepository(WebkinhdoanhquanaoContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetAllCategoryAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
