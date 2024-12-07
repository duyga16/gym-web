using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace gym.ViewModels
{
    public class VerifyOtpViewModel
    {
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mã không hợp lệ")]
        [Display(Name = "Otp")]
        public string? Otp { get; set; }

    }
}
