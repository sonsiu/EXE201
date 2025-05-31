using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.Services;
using Group2_SE1814_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Group2_SE1814_NET.Controllers {
    public class BlogController : Controller {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;

        public BlogController(IBlogService blogService, ICategoryService categoryService) {
            _blogService = blogService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(int? categoryId, string? search, int page = 1) {
            int pageSize = 3;
            var posts = await _blogService.GetPostsAsync(search, categoryId);
            // Sử dụng X.PagedList để phân trang
            var pagedBlogs = posts.ToPagedList(page, pageSize);

            var categories = await _categoryService.GetAllCategoryAsync();

            // Cập nhật số lượng bài viết cho từng danh mục dựa trên điều kiện search
            foreach (var category in categories) {
                category.Posts = await _blogService.GetPostsAsync(search, category.Id);
            }

            // Thêm mục "All" cho tất cả bài viết
            var allPosts = await _blogService.GetPostsAsync(search, null); // Không lọc theo category
            categories.Insert(0, new Category
            {
                Id = null,
                Name = "All",
                Posts = allPosts
            });

            //top 4 blog newest
            var top4Newest = await _blogService.Top4PostsNewest();

            // Chuẩn bị ViewModel
            var viewModel = new BlogViewModel
            {
                Blogs = pagedBlogs,
                Top4 = top4Newest,
                Categories = categories,
                CategoryId = categoryId,
                Search = search,
                CurrentPage = page
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Detail(int id) {
            var blog = await _blogService.GetPostsById(id);

            if (blog == null) {
                return NotFound();
            }

            var categories = await _categoryService.GetAllCategoryAsync();

            // Cập nhật số lượng bài viết cho từng danh mục dựa trên điều kiện search
            foreach (var category in categories) {
                category.Posts = await _blogService.GetPostsAsync(null, category.Id);
            }

            // Thêm mục "All" cho tất cả bài viết
            var allPosts = await _blogService.GetPostsAsync(null, null);
            categories.Insert(0, new Category
            {
                Id = null,
                Name = "All",
                Posts = allPosts
            });

            //top 4 blog newest
            var top4Newest = await _blogService.Top4PostsNewest();

            ViewBag.Categories = categories;
            ViewBag.Top4 = top4Newest;

            return View(blog);
        }
    }
}
