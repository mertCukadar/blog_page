using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using blog_page.Models;
using Microsoft.EntityFrameworkCore;
using blog_page.Models.ViewModels;
using blog_page.Services;


namespace blog_page.Controllers
{
    public class AuthController : Controller
    {
        private readonly BlogPageContext _blogPageContext;

        public AuthController(BlogPageContext blogPageContext)
        {
            _blogPageContext = blogPageContext;
        }


        [HttpGet]
        public IActionResult Register() { 
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if (ModelState.IsValid)
            {
                PasswordHasher.CreatePasswordHash(model.Password, out byte[] hash, out byte[] salt);

                var newUser = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = Convert.ToBase64String(hash),
                    PasswordSalt = Convert.ToBase64String(salt),
                    CreatedAt = DateTime.Now,
                    IsBanned = false 
                };

                _blogPageContext.Users.Add(newUser);
                await _blogPageContext.SaveChangesAsync();

                // 3. İsteğe bağlı: Kayıt olunca otomatik default bir rol ata (örn: 'User' rolü)
                // Bu kısım senin Roles tablondaki ID'ye göre değişir.
                var defaultRole = new UserRole
                { 
                    UserFkid = newUser.Id,
                    RoleFkid = 4
                };

                _blogPageContext.UserRoles.Add(defaultRole);
                await _blogPageContext.SaveChangesAsync();


                return RedirectToAction("Login");
            }
            return View(model);
        }
        
        
        
        public IActionResult Login()
        {
            return View();
        }

      

        public RedirectResult RedirectLogin() {

            return Redirect("/Auth/login");
        }
        public IActionResult RedirectLoginAction() {
            String[] names = { "mert", "deneme", "gizem" };
            return View("Login" , names);
                }
    }
}
