using blog_page.Models;
using blog_page.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace blog_page.Controllers
{
    public class  ControlPanelController: Controller
    {
        private readonly BlogPageContext _context;
        public ControlPanelController(BlogPageContext blogPageContext){ 
            _context = blogPageContext;
        }

        [Authorize(Roles = "Admin , Mod")]
        public IActionResult Index() {

            var ViewModel = new ControlPanelViewModel()
            {
                blogCount = _context.BlogPosts.Count(),
                activeBlogCount = _context.BlogPosts.Count(b => b.Status == true),
                totalUserCount = _context.Users.Count(),
                toalBannedUserCount = _context.Users.Count(b => b.IsBanned == true),
                pendingBlogs = _context.BlogPosts.Where(p => p.Status == false).ToList()
            };

            return View(ViewModel);

        }

    
    }
}
