using Microsoft.AspNetCore.Mvc;
using blog_page.Models;
using blog_page.Models.ViewModels;
using blog_page.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


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


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (ModelState.IsValid) {

                var user = await _blogPageContext.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.RoleFk)
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null || user.IsBanned) {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı ya da Banlı");
                    return View(model);
                }

                if (string.IsNullOrEmpty(user.PasswordHash) || string.IsNullOrEmpty(user.PasswordSalt))
                {
                    ModelState.AddModelError("", "Kullanıcı için parola verisi eksik.");
                    return View(model);
                }

                byte[] storedHash = Convert.FromBase64String(user.PasswordHash);
                byte[] storedSalt = Convert.FromBase64String(user.PasswordSalt);

                if (!PasswordHasher.VerifyPasswordHash(model.Password, storedHash, storedSalt))
                {
                    ModelState.AddModelError("", "Hatalı şifre!");
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                foreach (var userRole in user.UserRoles)
                {
                    var roleName = userRole.RoleFk?.Name;
                    if (!string.IsNullOrEmpty(roleName))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roleName));
                    }
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");


            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
      
    }
}
