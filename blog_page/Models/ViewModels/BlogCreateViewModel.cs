using Microsoft.AspNetCore.Mvc.Rendering;

namespace blog_page.Models.ViewModels
{
    public class BlogCreateViewModel
    {
        public int AuthorID { get; set; }
        public int CategorieID { get; set; }
        public string Title { get; set; }
        public string BlogContent { get; set; }
        public bool Status { get; set; }
        public DateTime Created { get; set; }
        public IFormFile ?ImageFile { get; set; }

        public List<SelectListItem>? CategoryList { get; set; }

    }
}
