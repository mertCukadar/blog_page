using System.ComponentModel.DataAnnotations;

namespace blog_page.Models.ViewModels
{
    public class CurrentUserViewModel
    {
        [Required]
        public string UserName { get; set; }
        
        public string UserMail { get; set; }

        public int BlogCount { get; set; }
        
        public int ReadCount { get; set; }

        public bool AccountStatus { get; set;}

        public List<string> Roles { get; set; }

        public DateTime AccountCreateDate { get; set; }
    }
}
