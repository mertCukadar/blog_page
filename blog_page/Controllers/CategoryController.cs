using AspNetCoreGeneratedDocument;
using blog_page.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using blog_page.Controllers.Helpers;
using Microsoft.EntityFrameworkCore;

namespace blog_page.Controllers
{
    public class CategoryController : Controller
    {
        private readonly BlogPageContext _context;

        public CategoryController(BlogPageContext blogPageContext)
        {
            _context = blogPageContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin , Mod")]
        public IActionResult Index()
        {
            var CategoryList = _context.BlogCategories.OrderBy(c => c.Name).ToList();
            return View(CategoryList);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Mod")] 
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                TempData["Error"] = "Kategori adı boş olamaz!";
                return RedirectToAction("Index");
            }

            bool exists = await _context.BlogCategories.AnyAsync(c => c.Name.ToLower() == categoryName.ToLower());
            if (exists)
            {
                TempData["Error"] = "Bu kategori zaten mevcut!";
                return RedirectToAction("Index");
            }

            var newCategory = new BlogCategory
            {
                Name = categoryName.Trim(), // Başındaki sonundaki boşlukları temizleyelim
                Slug = SlugHalpers.toUrlSlug(categoryName),
                CreatedAt = DateTime.Now
            };

            _context.BlogCategories.Add(newCategory);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Kategori başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin , Mod")]
        public IActionResult Create() {
            return View();
        }



    }
}
