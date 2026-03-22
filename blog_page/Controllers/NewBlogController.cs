using Microsoft.AspNetCore.Mvc;

namespace blog_page.Controllers
{
    public class NewBlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
