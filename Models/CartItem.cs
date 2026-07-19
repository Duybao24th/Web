namespace DrinkStore.Models
{
    // Đối tượng đại diện cho một dòng sản phẩm trong giỏ hàng, được lưu dưới dạng JSON trong Session
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Volume { get; set; }
        public int Quantity { get; set; }

        public decimal SubTotal => Price * Quantity;
    }
}
