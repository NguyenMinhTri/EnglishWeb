using Framework.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ơ kìa, Email đâu")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email có gì đó sai sai")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Ơ kìa kìa, còn mật khẩu đâu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Thiếu họ kìa")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Thiếu tên kìa")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Thiếu Email kìa")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email có gì đó sai sai")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Thiếu mật khẩu kìa")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Sao chưa nhập ngày sinh")]
        public DateTime? Birthday { get; set; }
        public bool Sex { get; set; }
    }
}
