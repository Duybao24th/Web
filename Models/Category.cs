using System.ComponentModel.DataAnnotations;

namespace DrinkStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100, ErrorMessage = "Tên danh mục không quá 100 ký tự")]
        [Display(Name = "Tên Danh Mục")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không quá 500 ký tự")]
        [Display(Name = "Mô Tả")]
        public string Description { get; set; }

        // Quan hệ 1 - Nhiều: Một danh mục có nhiều sản phẩm
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
