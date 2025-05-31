using Group2_SE1814_NET.Models;
using Microsoft.EntityFrameworkCore;

namespace Group2_SE1814_NET.Repositories {
    public class BrandRepository : IBrandRepository {
        private readonly WebkinhdoanhquanaoContext _context;

        public BrandRepository(WebkinhdoanhquanaoContext context) {
            _context = context;
        }
        public async Task<List<Brand>> GetAllBrandAsync() {
            return await _context.Brands.ToListAsync();
        }
    }
}
