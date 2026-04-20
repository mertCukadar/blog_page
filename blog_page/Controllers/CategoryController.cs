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
        [Route("Category/{slug}")]
        public IActionResult CategoryFilter(string slug)
        {
            // Listeyi burada çekme, sadece Index'e "şu slug'ı filtrele" de.
            return RedirectToAction("Index", "Home", new { slug = slug });
        }
        public IActionResult Index()
        {
            var CategoryList = _context.BlogCategories.OrderBy(c => c.Name).ToList();
            return View(CategoryList);
        }

        
        public async Task<IActionResult> Index(string slug) { 
            var category_blog = await _context.BlogPosts
                .Include(b => b.AuthorFkuser)
                .Include(b => b.CategoryFk)
                .Where(b => b.CategoryFk.Slug == slug && b.Status == true)
                .OrderByDescending(b => b.PublishedAt)
                .ToListAsync();

            return RedirectToAction("Index", "Home", category_blog);

        }
       




    }
}
