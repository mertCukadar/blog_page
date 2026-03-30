namespace blog_page.Controllers.Helpers
{
    public class SlugHalpers
    {

        public static string toUrlSlug(string text) { 
            return text.ToLower().Replace(" ", "-")
                .Replace(" ", "-")
                .Replace("ş", "s").Replace("ı", "i").Replace("ğ", "g")
                .Replace("ü", "u").Replace("ö", "o").Replace("ç", "c");
        }

    }
}
