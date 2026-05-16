using blog_page.Models;
using blog_page.Models.ViewModels;
using blog_page.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;


namespace blog_page.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly BlogPageContext _context;

        public UserController(BlogPageContext blogPageContext)
        {
            _context = blogPageContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(currentUserId, out int userId))
            {
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.RoleFk)
                    .Include(u => u.BlogLikes)
                    .Include(u => u.BlogPosts)
                    .FirstOrDefaultAsync(u => u.Id == userId);


                if (user == null) {
                    return Redirect("/Auth/Login");
                }

                var UserViewModel = new CurrentUserViewModel
                {
                    UserName = user.Username,
                    Roles = user.UserRoles.Select(ur => ur.RoleFk.Name).ToList(),
                    UserMail = user.Email,
                    BlogCount = user.BlogPosts.Count(),
                    AccountStatus = user.IsBanned,
                    ReadCount = user.BlogPosts.Select(b => b.ViewCount).Sum(),
                    AccountCreateDate = user.CreatedAt


                };


                if (UserViewModel != null)
                {
                    return View(UserViewModel);
                }
                else return NotFound();

            }

            return Redirect("/Auth/Login");

        }



        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == currentUserId);

            if (user == null)
            {
                return NotFound();
            }

            byte[] storedHash = Convert.FromBase64String(user.PasswordHash);
            byte[] storedSalt = Convert.FromBase64String(user.PasswordSalt);


            
            if (!PasswordHasher.VerifyPasswordHash(model.CurrentPassword, storedHash, storedSalt))
            {
                ModelState.AddModelError("CurrentPassword", "Mevcut şifrenizi yanlış girdiniz.");
                return View(model);
            }


            PasswordHasher.CreatePasswordHash(model.NewPassword, out byte[] hash, out byte[] salt);

            user.PasswordHash = Convert.ToBase64String(hash);
            user.PasswordSalt = Convert.ToBase64String(salt);

            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Şifreniz başarıyla güncellenmiştir.";

            return RedirectToAction("Index", "User");
        }



    }
}
