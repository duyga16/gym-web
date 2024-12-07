using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace gym.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Tên tài khoản là bắt buộc.")]
        [Display(Name = "Username")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$", ErrorMessage = "Mật khẩu phải có ít nhất 6 kí tự gồm số và chữ cái.")]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
