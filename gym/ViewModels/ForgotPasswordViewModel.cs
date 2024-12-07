using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace gym.ViewModels
{
    public class ForgotPasswordViewModel 
    {
        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email.")]
        [Display(Name = "Email")]
        public string? Email { get; set; }
    }
}
