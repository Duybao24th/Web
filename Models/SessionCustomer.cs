namespace DrinkStore.Models
{
    // Thông tin khách hàng lưu trong Session sau khi đăng nhập (không chứa mật khẩu)
    public class SessionCustomer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public bool IsAdmin => Role == "Admin";
    }
}
