using blog_page.Controllers.Helpers;
using blog_page.Models;
using blog_page.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

[Authorize(Roles = "Admin, Mod, Member")]
public class CreateBlogController : Controller
{
    private readonly BlogPageContext _blogPageContext;

    public CreateBlogController(BlogPageContext blogpageContext)
    {
        _blogPageContext = blogpageContext;
    }

    private List<SelectListItem> GetCategories()
    {
        return _blogPageContext.BlogCategories
            .Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToList();
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Mod, Member")]
    public IActionResult Index()
    {
        var model = new BlogCreateViewModel
        {
            CategoryList = GetCategories()
        };
        return View(model);
    }

    [Authorize(Roles = "Admin, Mod, Member")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBlog(BlogCreateViewModel model)
    {
        ModelState.Remove("ImageFile");

        if (!ModelState.IsValid)
        {
            model.CategoryList = GetCategories();
            return View("Index", model);
        }

        string genereated_slug = SlugHalpers.toUrlSlug(model.Title);
        bool isSlugTaken = await _blogPageContext.BlogPosts.AnyAsync(p => p.Slug == genereated_slug);

        if (isSlugTaken)
        {
            ModelState.AddModelError("Title", "Bu başlık zaten mevcut.");
            model.CategoryList = GetCategories();
            return View("Index", model);
        }

        string uploadedImageUrl = "/images/default-blog.jpg";
        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            var cloudinary = new CloudinaryService();
            uploadedImageUrl = await cloudinary.UploadImageAsync(model.ImageFile);
        }

        var newPost = new BlogPost
        {
            AuthorFkuserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
            CategoryFkid = model.CategorieID,
            Title = model.Title,
            Status = false,
            Slug = genereated_slug,
            BlogContent = model.BlogContent,
            CreatedAt = DateTime.Now,
            ViewCount = 0,
            ImageUrl = uploadedImageUrl,
        };

        _blogPageContext.BlogPosts.Add(newPost);
        await _blogPageContext.SaveChangesAsync();

        // Kayıttan sonra Home/Index'e git
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> EditBlog(int id)
    {
        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var blog = await _blogPageContext.BlogPosts
            .FirstOrDefaultAsync(b => b.PostId == id && b.AuthorFkuserId == currentUserId);

        if (blog == null)
        {
            return NotFound();
        }

        EditBlogViewModel viewBlog = new EditBlogViewModel
        {
            PostId = blog.PostId,                
            Title = blog.Title,
            BlogContent = blog.BlogContent,
            ImageUrl = blog.ImageUrl,
            CategorieID = blog.CategoryFkid.GetValueOrDefault()      
        };

        viewBlog.CategoryList = GetCategories();

        return View(viewBlog);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBlog(int id, EditBlogViewModel model)
    {
        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var existingBlog = await _blogPageContext.BlogPosts
            .FirstOrDefaultAsync(b => b.PostId == id && b.AuthorFkuserId == currentUserId);

        if (existingBlog == null) return NotFound();

        existingBlog.Title = model.Title;
        existingBlog.BlogContent = model.BlogContent;
        existingBlog.CategoryFkid = model.CategorieID;
        existingBlog.Status = false;

        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            var cloudinary = new CloudinaryService();
            existingBlog.ImageUrl = await cloudinary.UploadImageAsync(model.ImageFile);
        }

        _blogPageContext.Update(existingBlog);
        await _blogPageContext.SaveChangesAsync();

        return RedirectToAction("MyBlogs" , "Home");
    }

}