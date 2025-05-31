using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Group2_SE1814_NET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Group2_SE1814_NET.Areas.Admin.Controllers {
    [Area("Admin")]
    public class ProductController : Controller {
        private readonly WebkinhdoanhquanaoContext _context;
        private readonly Cloudinary _cloudinary;

        public ProductController(WebkinhdoanhquanaoContext context, Cloudinary cloudinary) {
            _context = context;
            _cloudinary = cloudinary;
        }

        // GET: Admin/Product
        public async Task<IActionResult> Index() {
            var webkinhdoanhquanaoContext = _context.Products.Include(p => p.Brand).Include(p => p.Category);
            return View(await webkinhdoanhquanaoContext.ToListAsync());
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Product/Create
        public IActionResult Create() {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BrandId,CategoryId,Name,Price,Quantity,ShortDescription,Description,Status")] Product product, IFormFile uploadPhoto) {
            if (ModelState.IsValid) {
                if (uploadPhoto != null && uploadPhoto.Length > 0) {
                    var uploadResult = new ImageUploadResult();

                    using (var stream = uploadPhoto.OpenReadStream()) {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(uploadPhoto.FileName, stream),
                            Folder = "SWDUAT" // Folder in Cloudinary
                        };
                        uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    }

                    if (uploadResult.SecureUrl != null) {
                        product.ImageUrl = uploadResult.SecureUrl.AbsoluteUri;
                    }
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null) {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrandId,CategoryId,Name,ImageUrl,Price,Quantity,ShortDescription,Description,Status")] Product product, IFormFile? uploadPhoto) {
            if (id != product.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                // Handle file upload
                if (uploadPhoto != null && uploadPhoto.Length > 0) {
                    // Get the file extension
                    var extension = Path.GetExtension(uploadPhoto.FileName);
                    var fileName = Guid.NewGuid().ToString() + extension;

                    // Set the file path (you can modify the path as needed)
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img/product", fileName);

                    // Save the file to the specified path
                    using (var stream = new FileStream(filePath, FileMode.Create)) {
                        await uploadPhoto.CopyToAsync(stream);
                    }

                    // Set the ImageUrl to the path of the saved image
                    product.ImageUrl = "img/product/" + fileName;
                }
                try {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!ProductExists(product.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            try {
                var product = await _context.Products.FindAsync(id);
                if (product != null) {
                    _context.Products.Remove(product);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                TempData["ErrorMessage"] = "Delete failed because it is in use";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ProductExists(int id) {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
