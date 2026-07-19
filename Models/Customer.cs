using System.ComponentModel.DataAnnotations;

namespace DrinkStore.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
        [Display(Name = "Họ và Tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        [StringLength(20, ErrorMessage = "Số điện thoại tối đa 20 ký tự")]
        [Display(Name = "Số Điện Thoại")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không hợp lệ")]
        [StringLength(150)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Địa chỉ nhận hàng không được để trống")]
        [StringLength(255)]
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

        // "Customer" hoặc "Admin". Tài khoản "admin" mặc định được seed với Role = "Admin"
        [StringLength(20)]
        [Display(Name = "Vai Trò")]
        public string Role { get; set; } = "Customer";

        [Display(Name = "Ngày Đăng Ký")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
