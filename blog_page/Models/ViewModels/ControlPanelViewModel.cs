using Microsoft.EntityFrameworkCore;

namespace blog_page.Models.ViewModels
{
    public class ControlPanelViewModel
    {
        public int blogCount { get; set; }
        public int activeBlogCount { get; set; }
        public int totalUserCount { get; set; }
        public int toalBannedUserCount { get; set; }

        public List<BlogPost> pendingBlogs { get; set; }


    }
}
