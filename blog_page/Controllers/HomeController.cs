using blog_page.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace blog_page.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        List<Models.User> vars = new List<Models.User>();

        public IActionResult listUser() {

        for(int i = 0; i < 10; i++)
            {
                var veri = new Models.User()
                {
                    Id = 1,
                    Email = "cukadar.mertkaan@gmail.com",
                    CreatedAt = DateTime.Now,
                    Username = "mertkaan",
                };

                vars.Add(veri);
            }
           


            return View("Kullanicilar" , vars);
        }
    }
}
