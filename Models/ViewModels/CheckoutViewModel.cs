using System.ComponentModel.DataAnnotations;

namespace DrinkStore.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên người nhận")]
        [Display(Name = "Người Nhận")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        [Display(Name = "SĐT Nhận Hàng")]
        public string ReceiverPhone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [Display(Name = "Địa Chỉ Giao Hàng")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        [Display(Name = "Phương Thức Thanh Toán")]
        public string PaymentMethod { get; set; } = "COD";

        [Display(Name = "Ghi Chú")]
        public string Note { get; set; }

        public List<CartItem> CartItems { get; set; } = new();

        public decimal TotalMoney => CartItems.Sum(c => c.SubTotal);
    }
}
