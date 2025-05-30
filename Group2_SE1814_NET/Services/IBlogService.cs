using Group2_SE1814_NET.Models;

namespace Group2_SE1814_NET.Services
{
    public interface IBlogService
    {
        public Task<List<Post>> GetPostsAsync(string? search, int? categoryId);
        public Task<List<Post>> Top4PostsNewest();
        public Task<Post> GetPostsById(int id);
    }
}
