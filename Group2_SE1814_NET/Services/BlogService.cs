using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Group2_SE1814_NET.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<List<Post>> GetPostsAsync(string? search, int? categoryId)
        {
            return await _blogRepository.GetPostsAsync(search, categoryId);
        }

        public async Task<Post> GetPostsById(int id)
        {
            return await _blogRepository.GetPostsById(id);
        }

        public async Task<List<Post>> Top4PostsNewest()
        {
            return await _blogRepository.Top4PostsNewest();
        }
    }
}
