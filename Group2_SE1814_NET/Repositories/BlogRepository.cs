using Group2_SE1814_NET.Models;
using Microsoft.EntityFrameworkCore;

namespace Group2_SE1814_NET.Repositories {
    public class BlogRepository : IBlogRepository {
        private readonly WebkinhdoanhquanaoContext _context;

        public BlogRepository(WebkinhdoanhquanaoContext context) {
            _context = context;
        }

        public async Task<List<Post>> GetPostsAsync(string? search, int? categoryId) {
            // Bắt đầu từ một truy vấn IQueryable
            var query = _context.Posts
                                .Include(p => p.Category)
                                .Include(p => p.User)
                                .OrderByDescending(p => p.Id)
                                .AsQueryable();

            // Thêm điều kiện tìm kiếm (nếu có)
            if (!string.IsNullOrEmpty(search)) {
                query = query.Where(p => p.Title.Contains(search));
            }

            // Thêm điều kiện lọc theo categoryId (nếu có)
            if (categoryId.HasValue) {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            // Thực hiện truy vấn và trả về danh sách
            return await query.ToListAsync();
        }

        public async Task<Post> GetPostsById(int id) {
            return await _context.Posts
                                 .Include(p => p.Category)
                                 .Include(p => p.User)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Post>> Top4PostsNewest() {
            return await _context.Posts.OrderByDescending(p => p.Id).Take(4).ToListAsync();
        }
    }
}
