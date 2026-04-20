using blog_page.Models;
using blog_page.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using blog_page.Controllers.Helpers;
using Microsoft.EntityFrameworkCore;

namespace blog_page.Controllers
{
    public class  ControlPanelController: Controller
    {
        private readonly BlogPageContext _context;
        public ControlPanelController(BlogPageContext blogPageContext){ 
            _context = blogPageContext;
        }

        [Authorize(Roles = "Admin , Mod")]
        public IActionResult Index()
        {
            var viewModel = new ControlPanelViewModel()
            {
                blogCount = _context.BlogPosts.Count(),
                activeBlogCount = _context.BlogPosts.Count(b => b.Status == true),
                totalUserCount = _context.Users.Count(),
                toalBannedUserCount = _context.Users.Count(b => b.IsBanned == true),
                // Bekleyen blogları çekerken yazar ve kategoriyi de getiriyoruz
                pendingBlogs = _context.BlogPosts
                    .Include(b => b.AuthorFkuser)
                    .Include(b => b.CategoryFk)
                    .Where(p => p.Status == false)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToList()
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin , Mod")]
        public async Task<IActionResult> PendingBlogs() { 
            var pendingBlogs = await _context.BlogPosts
                .Include(b => b.AuthorFkuser)
                .Include(b => b.CategoryFk)
                .Where(b => b.Status == false)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
            return View(pendingBlogs);
        }

        [HttpPost]
        [Authorize(Roles = "Admin , Mod")]
        public async Task<IActionResult> ApproveBlog(int id) { 
            var blog = await _context.BlogPosts.FindAsync(id);
            if (blog != null) { 

                blog.Status = true;
                blog.PublishedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                TempData["Success"] = "Blog başarıyla yayınlandı!";
            }
            return RedirectToAction(nameof(PendingBlogs));
        }

        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> Users(string searchTerm)
        {
            var usersQuery = _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.RoleFk)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                usersQuery = usersQuery.Where(u => u.Username.Contains(searchTerm) || u.Email.Contains(searchTerm));
            }

            var users = await usersQuery.OrderByDescending(u => u.CreatedAt).ToListAsync();
            ViewBag.SearchTerm = searchTerm; // Arama kutusunda yazı kalsın diye
            return View(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Mod")]
      
        public async Task<IActionResult> BanUser(int id)
        {
            // Banlanacak kullanıcıyı rolleriyle beraber çekiyoruz
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.RoleFk)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
               
                bool isAdmin = user.UserRoles.Any(ur => ur.RoleFk.Name == "Admin");

                if (isAdmin)
                {
                    TempData["Error"] = "Bir yöneticiyi (Admin) banlayamazsınız";
                    return RedirectToAction(nameof(Users));
                }

                user.IsBanned = true;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Kullanıcı banlandı";
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> UnbanUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsBanned = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Users));
        }


        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.BlogCategories
                .OrderBy(c => c.Name)
                .ToListAsync();
            return View(categories);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<IActionResult> CreateCategory(string categoryName)
        {
            if (!string.IsNullOrEmpty(categoryName))
            {
                var category = new BlogCategory { Name = categoryName };
                category.Slug = SlugHalpers.toUrlSlug(categoryName);
                category.CreatedAt = DateTime.Now;

                _context.BlogCategories.Add(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.BlogCategories.FindAsync(id);
            if (category != null)
            {
                _context.BlogCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Categories));
        }

    }
}
