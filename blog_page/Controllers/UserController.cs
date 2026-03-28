using blog_page.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog_page.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly BlogPageContext _blogPageContext;

        public UserController(BlogPageContext blogPageContext)
        {
            _blogPageContext = blogPageContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if(int.TryParse(currentUserId, out int userId))
            {
                var user = await _blogPageContext.Users
                    .Include(u=> u.UserRoles)
                    .ThenInclude(ur => ur.RoleFk)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user != null)
                {
                    return View(user);
                }
                else return NotFound();
            
            }

            return Redirect("/Auth/Login");

        }
    }
}
