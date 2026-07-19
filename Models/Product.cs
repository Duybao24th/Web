using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinkStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh Mục")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(150, ErrorMessage = "Tên sản phẩm không vượt quá 150 ký tự")]
        [Display(Name = "Tên Sản Phẩm")]
        public string ProductName { get; set; }

        [StringLength(2000, ErrorMessage = "Mô tả không vượt quá 2000 ký tự")]
        [Display(Name = "Mô Tả Chi Tiết")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá Bán (VND)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Số Lượng Tồn")]
        public int Quantity { get; set; }

        [StringLength(500)]
        [Display(Name = "Hình Ảnh (URL)")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thương hiệu")]
        [StringLength(100)]
        [Display(Name = "Thương Hiệu")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập dung tích")]
        [StringLength(50)]
        [Display(Name = "Dung Tích")]
        public string Volume { get; set; }

        [Display(Name = "Đang Bán")]
        public bool Status { get; set; } = true;

        [Display(Name = "Sản Phẩm Nổi Bật")]
        public bool IsFeatured { get; set; } = false;

        [Display(Name = "Sản Phẩm Mới")]
        public bool IsNew { get; set; } = false;

        [Display(Name = "Ngày Tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Khóa ngoại liên kết tới bảng Category
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
