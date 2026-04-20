using Microsoft.AspNetCore.Mvc;
using blog_page.Models;
using Microsoft.EntityFrameworkCore;

namespace blog_page.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {

        private readonly BlogPageContext _context;

        public CategoryMenuViewComponent(BlogPageContext blogPageContext)
        {
            _context = blogPageContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.BlogCategories.OrderBy(c => c.Name).ToListAsync();
            return View(categories);
        }

    }
}
