using System.ComponentModel.DataAnnotations;

namespace DrinkStore.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100)]
        [Display(Name = "Họ và Tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        [Display(Name = "Số Điện Thoại")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Địa chỉ nhận hàng không được để trống")]
        [Display(Name = "Địa Chỉ Nhận Hàng")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Tài khoản từ 4 đến 50 ký tự")]
        [Display(Name = "Tài Khoản")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 ký tự trở lên")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        [Display(Name = "Xác Nhận Mật Khẩu")]
        public string ConfirmPassword { get; set; }
    }
}
