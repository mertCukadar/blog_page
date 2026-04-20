using blog_page.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            // 1. Sorguyu hazırlıyoruz (Henüz veritabanına gitmedi)
            var blogQuery = _context.BlogPosts
                .Include(b => b.AuthorFkuser)
                .Include(b => b.CategoryFk)
                .Where(b => b.Status == true)
                .AsQueryable();

            // 2. Eğer URL'den bir slug gelmişse (Kategoriye tıklandıysa) filtreyi ekle
            if (!string.IsNullOrEmpty(slug))
            {
                blogQuery = blogQuery.Where(b => b.CategoryFk.Slug == slug);
            }

            // 3. Veriyi tek seferde ve doğru şekilde çekiyoruz
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

            // 1. Önce yazıyı status bağımsız çekelim
            var post = await _context.BlogPosts
                .Include(b => b.AuthorFkuser)
                .Include(b => b.CategoryFk)
                .FirstOrDefaultAsync(b => b.Slug == slug);

            if (post == null) return NotFound();

            // 2. Kritik Kontrol: Eğer yazı yayında değilse (false)
            if (post.Status == false)
            {
                // Kullanıcı giriş yapmış mı ve Admin/Mod/Yazar mı?
                bool canView = User.Identity.IsAuthenticated &&
                               (User.IsInRole("Admin") ||
                                User.IsInRole("Mod") ||
                                post.AuthorFkuserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

                if (!canView)
                {
                    // Yetkisi yoksa 404 ver (yazı yokmuş gibi davranmak güvenlidir)
                    return NotFound();
                }
            }

            // İzlenme sayısını artıralım (Admin saymasın istiyorsan buraya da check koyabilirsin)
            if (post.Status == true)
            {
                post.ViewCount += 1;
                await _context.SaveChangesAsync();
            }

            return View(post);
        }
    }
}
