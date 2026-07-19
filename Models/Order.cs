using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinkStore.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Khách Hàng")]
        public int CustomerId { get; set; }

        [Display(Name = "Ngày Đặt")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tổng Tiền (VND)")]
        public decimal TotalMoney { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Trạng Thái")]
        // Chờ xử lý, Đang giao hàng, Đã giao hàng, Đã hủy
        public string Status { get; set; } = "Chờ xử lý";

        [Required]
        [StringLength(50)]
        [Display(Name = "Phương Thức Thanh Toán")]
        public string PaymentMethod { get; set; } = "COD";

        [Required]
        [StringLength(100)]
        [Display(Name = "Người Nhận")]
        public string ReceiverName { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "SĐT Nhận Hàng")]
        public string ReceiverPhone { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Địa Chỉ Giao Hàng")]
        public string ShippingAddress { get; set; }

        [StringLength(500)]
        [Display(Name = "Ghi Chú")]
        public string Note { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        // Quan hệ 1-Nhiều với chi tiết đơn hàng
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
