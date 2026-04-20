using blog_page.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Security.Claims;

namespace blog_page.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogPageContext _context;

        public HomeController(BlogPageContext blogPageContext)
        {
            _context = blogPageContext;
        }



        public async Task<IActionResult> Index(string slug)
        {
            var blogQuery = _context.BlogPosts
                .Include(b => b.AuthorFkuser)
                .Include(b => b.CategoryFk)
                .Where(b => b.Status == true)
                .AsQueryable();

            if (!string.IsNullOrEmpty(slug))
            {
                blogQuery = blogQuery.Where(b => b.CategoryFk.Slug == slug);
            }

            var blogs = await blogQuery
                .OrderByDescending(b => b.PublishedAt)
                .ToListAsync();

            return View(blogs);
        }

        [HttpGet]
        [Route("Details/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return NotFound();

            var post = await _context.BlogPosts
                .Include(b => b.AuthorFkuser)
                .Include(b => b.CategoryFk)
                .FirstOrDefaultAsync(b => b.Slug == slug);

            if (post == null) return NotFound();

            if (post.Status == false)
            {
                bool canView = User.Identity.IsAuthenticated &&
                               (User.IsInRole("Admin") ||
                                User.IsInRole("Mod") ||
                                post.AuthorFkuserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

                if (!canView)
                {
                    return NotFound();
                }
            }

            if (post.Status == true)
            {
                post.ViewCount += 1;
                await _context.SaveChangesAsync();
            }

            return View(post);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyBlogs()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userBlogs = await _context.BlogPosts
                .Include(b => b.AuthorFkuser)
                .Include(b => b.CategoryFk)
                .Where(b => b.AuthorFkuserId == int.Parse(currentUserId))
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(userBlogs);
        }
    }
}
