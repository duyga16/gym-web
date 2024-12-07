using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace gym.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Tên tài khoản là bắt buộc.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc."), EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$", ErrorMessage = "Mật khẩu phải có ít nhất 6 kí tự gồm số và chữ cái.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc.")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Bạn phải đồng ý với điều khoản")]
        public bool AcceptTerms { get; set; }
    }
}
