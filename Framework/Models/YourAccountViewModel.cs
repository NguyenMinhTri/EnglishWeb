using Framework.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Framework.ViewModels
{
    public class UpdateInfoViewModel : LayoutViewModel, IRef<YourAccountController>
    {
        [Required(ErrorMessage = "Thiếu họ kìa...")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Thiếu tên kìa...")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Thiếu Email kìa...")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email có gì đó sai sai...")]
        public String Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0]{1}[19]{1}[0-9]{8,9}$", ErrorMessage = "Phải số điện thoại thiệt không đó?")]
        public String Phone { get; set; }
        [Required(ErrorMessage = "Sao chưa nhập ngày sinh...")]
        public DateTime? Birthday { get; set; }
        public bool Sex { get; set; }
        public bool? UserStatus { get; set; }
        public String Career { get; set; }
        public String Organization { get; set; }
        public String Address { get; set; }
        public String About { get; set; }
        public String Hobby { get; set; }
        public String FavoriteShow { get; set; }
        public String FavoriteFilm { get; set; }
        public String FavoriteGame { get; set; }
        public String FavoriteBook { get; set; }
        public String FavoriteArtist { get; set; }
        public String FavoriteAuthor { get; set; }
        public String Facebook { get; set; }
    }

    public class UpdatePasswordViewModel : LayoutViewModel, IRef<YourAccountController>
    {
        [Required(ErrorMessage = "Mật khẩu hiện tại của bạn là gì?")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Nhớ nhập mật khẩu mới nữa nha")]
        [StringLength(100, ErrorMessage = "Mật khẩu hơi ngắn, ít nhất 6 kí tự nhá", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Nhập sai mật khẩu xác nhận rồi")]
        public string ConfirmPassword { get; set; }
    }
    public class UpdateSettingViewModel : LayoutViewModel, IRef<YourAccountController>
    {

    }

    public class NotificationViewModel : LayoutViewModel, IRef<YourAccountController>
    {

    }

    public class RequestsViewModel : LayoutViewModel, IRef<YourAccountController>
    {
        public List<NotiFriendViewModel> ListRequest { get; set; }
        public RequestsViewModel()
        {
            ListRequest = new List<NotiFriendViewModel>();
        }
    }
}