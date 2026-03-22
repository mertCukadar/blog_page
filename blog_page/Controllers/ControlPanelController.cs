using Microsoft.AspNetCore.Mvc;

namespace blog_page.Controllers
{
    public class  ControlPanelControllers : Controller
    {
        public IActionResult AdminDashboard()
        {
            return View();
        }

        public IActionResult ModDashboard()
        {
            return View();
        }
    }
}
