using System.ComponentModel.DataAnnotations;

namespace blog_page.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mevcut şifrenizi girmeniz zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mevcut Şifre")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre belirlemeniz zorunludur.")]
        [StringLength(100, ErrorMessage = "Şifreniz en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifrenizi tekrar girmeniz zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre (Tekrar)")]
        [Compare("NewPassword", ErrorMessage = "Girdiğiniz yeni şifreler birbiriyle eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}