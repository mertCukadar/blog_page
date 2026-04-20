using blog_page.Controllers.Helpers;
using blog_page.Models;
using blog_page.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize(Roles = "Admin, Mod, Member")]
public class CreateBlogController : Controller
{
    private readonly BlogPageContext _blogPageContext;

    public CreateBlogController(BlogPageContext blogpageContext)
    {
        _blogPageContext = blogpageContext;
    }

    // Kategorileri liste olarak hazırlayan yardımcı metod
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
        // View'a modeli boş ama kategori listesi dolu gönderiyoruz
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
        // Resim seçilmediği için ModelState hata veriyorsa onu temizleyelim
        ModelState.Remove("ImageFile");

        if (!ModelState.IsValid)
        {
            model.CategoryList = GetCategories();
            // RedirectToAction yerine View döndürürken isme dikkat
            return View("Index", model);
        }

        // 1. Önce Slug kontrolü yapalım (Resmi boşuna yüklemeyelim)
        string genereated_slug = SlugHalpers.toUrlSlug(model.Title);
        bool isSlugTaken = await _blogPageContext.BlogPosts.AnyAsync(p => p.Slug == genereated_slug);

        if (isSlugTaken)
        {
            ModelState.AddModelError("Title", "Bu başlık zaten mevcut.");
            model.CategoryList = GetCategories();
            return View("Index", model);
        }

        // 2. Resim İşlemi
        string uploadedImageUrl = "/images/default-blog.jpg";
        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            var cloudinary = new CloudinaryService();
            uploadedImageUrl = await cloudinary.UploadImageAsync(model.ImageFile);
        }

        // 3. Kayıt İşlemi
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
    
}