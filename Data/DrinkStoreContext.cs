using Microsoft.EntityFrameworkCore;
using DrinkStore.Models;

namespace DrinkStore.Data
{
    public class DrinkStoreContext : DbContext
    {
        public DrinkStoreContext(DbContextOptions<DrinkStoreContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetail>().Property(od => od.UnitPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Order>().Property(o => o.TotalMoney).HasPrecision(18, 2);
            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);

            // Ngăn EF xóa Product khi Category bị xóa (tránh multiple cascade paths)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // ================= SEED DATA =================
            var seedDate = new DateTime(2026, 1, 1);

            // ---- Danh mục ----
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Nước ngọt", Description = "Các loại nước ngọt có ga, sảng khoái và đã khát" },
                new Category { Id = 2, Name = "Trà", Description = "Trà xanh, trà đóng chai thanh mát, giải độc" },
                new Category { Id = 3, Name = "Cà phê", Description = "Cà phê đóng lon, pha sẵn thơm ngon, tỉnh táo tức thì" },
                new Category { Id = 4, Name = "Nước ép", Description = "Nước ép trái cây tự nhiên, giàu vitamin" },
                new Category { Id = 5, Name = "Nước khoáng", Description = "Nước tinh khiết và nước khoáng bổ sung vi chất" },
                new Category { Id = 6, Name = "Sữa", Description = "Sữa tươi, sữa tiệt trùng thơm béo, bổ dưỡng" },
                new Category { Id = 7, Name = "Nước tăng lực", Description = "Bổ sung năng lượng mạnh mẽ cho ngày dài hoạt động" }
            );

            // ---- Sản phẩm ----
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, CategoryId = 1, ProductName = "Coca-Cola Original vị nguyên bản", Description = "Nước ngọt có ga Coca-Cola hương vị độc đáo nguyên bản, giúp đập tan cơn khát, mang lại cảm giác sảng khoái tức thì cùng bữa ăn ngon.", Price = 10000, Quantity = 120, Image = "https://upload.wikimedia.org/wikipedia/commons/2/27/Coca_Cola_Flasche_-_Original_Taste.jpg", Brand = "Coca-Cola", Volume = "330ml", Status = true, IsFeatured = true, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 2, CategoryId = 1, ProductName = "Pepsi vị chanh không calo", Description = "Nước ngọt Pepsi vị chanh không calo cực kỳ thích hợp cho người ăn kiêng, giải khát sảng khoái mà không lo tăng cân.", Price = 10000, Quantity = 150, Image = "https://upload.wikimedia.org/wikipedia/commons/d/dd/Pepsi_Can.jpg", Brand = "Pepsi", Volume = "330ml", Status = true, IsFeatured = true, IsNew = true, CreatedDate = seedDate },
                new Product { Id = 3, CategoryId = 1, ProductName = "Nước ngọt Sprite vị chanh", Description = "Nước giải khát có ga hương chanh tự nhiên, mang lại cảm giác sảng khoái tột đỉnh cho người thưởng thức.", Price = 9500, Quantity = 80, Image = "https://images.unsplash.com/photo-1625772291427-39f64aa5eea7?w=500&auto=format&fit=crop&q=60", Brand = "Sprite", Volume = "330ml", Status = true, IsFeatured = false, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 4, CategoryId = 1, ProductName = "Fanta hương Cam", Description = "Nước ngọt Fanta hương cam thơm mát bùng nổ, có ga cực kỳ sảng khoái, kích thích vị giác.", Price = 9500, Quantity = 90, Image = "https://images.unsplash.com/photo-1624517452488-04869289c4ca?w=500&auto=format&fit=crop&q=60", Brand = "Fanta", Volume = "330ml", Status = true, IsFeatured = false, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 5, CategoryId = 7, ProductName = "Nước tăng lực Sting hương Dâu đỏ", Description = "Sting dâu tây đỏ bùng nổ năng lượng, nhân sâm thơm ngon kích thích tỉnh táo sảng khoái tức thì.", Price = 12000, Quantity = 200, Image = "https://cdnv2.tgdd.vn/bhx-static/bhx/Products/Images/3226/76520/bhx/nuoc-tang-luc-sting-dau-pet-330ml_202509291516185862.jpg", Brand = "Sting", Volume = "330ml", Status = true, IsFeatured = true, IsNew = true, CreatedDate = seedDate },
                new Product { Id = 6, CategoryId = 7, ProductName = "Nước tăng lực Red Bull Thái Lan", Description = "Nước tăng lực bò húc Red Bull nhập khẩu Thái Lan giúp tỉnh táo tinh thần, bổ sung năng lượng dồi dào.", Price = 15000, Quantity = 350, Image = "https://images.unsplash.com/photo-1613946069412-38f7f1ff0b65?w=500&auto=format&fit=crop&q=60", Brand = "Red Bull", Volume = "250ml", Status = true, IsFeatured = true, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 7, CategoryId = 7, ProductName = "Nước tăng lực Number One chai", Description = "Nước uống năng lượng Number One đóng chai tiện dụng giúp bạn vượt qua mệt mỏi, chinh phục mọi thử thách.", Price = 10000, Quantity = 110, Image = "https://images.unsplash.com/photo-1543257580-7269da773bf5?w=500&auto=format&fit=crop&q=60", Brand = "Number One", Volume = "250ml", Status = true, IsFeatured = false, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 8, CategoryId = 7, ProductName = "Nước bù khoáng Revive Chanh Muối", Description = "Revive chanh muối bù nước bù khoáng tức thì cho người hoạt động thể thao, đổ mồ hôi nhiều.", Price = 11000, Quantity = 140, Image = "https://images.unsplash.com/photo-1527960656306-b3a951473295?w=500&auto=format&fit=crop&q=60", Brand = "Revive", Volume = "390ml", Status = true, IsFeatured = true, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 9, CategoryId = 5, ProductName = "Nước tinh khiết Aquafina", Description = "Nước uống tinh khiết Aquafina qua hệ thống lọc 7 bước chuẩn Hoa Kỳ mang lại nguồn nước thanh khiết tuyệt đối.", Price = 6000, Quantity = 500, Image = "https://aquafinawater.vn/library/module_new/san-pham-cua-aquafina_s5874.jpg", Brand = "Aquafina", Volume = "500ml", Status = true, IsFeatured = false, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 10, CategoryId = 5, ProductName = "Nước khoáng thiên nhiên La Vie", Description = "La Vie chứa nhiều khoáng chất tự nhiên quý giá tốt cho sức khỏe, thích hợp uống mỗi ngày.", Price = 6500, Quantity = 400, Image = "https://images.unsplash.com/photo-1560011536-1855e82a0ecf?w=500&auto=format&fit=crop&q=60", Brand = "La Vie", Volume = "500ml", Status = true, IsFeatured = false, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 11, CategoryId = 5, ProductName = "Nước tinh khiết Dasani", Description = "Nước đóng chai Dasani tinh khiết chất lượng cao của hãng Coca-Cola Việt Nam, an toàn cho cả gia đình.", Price = 6000, Quantity = 300, Image = "https://dasaniwater.vn/library/module_new/cac-thac-mac-khach-hang-tieu-dung_s3464.jpg", Brand = "Dasani", Volume = "500ml", Status = true, IsFeatured = false, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 12, CategoryId = 2, ProductName = "Trà Xanh Không Độ hương Chanh", Description = "Được làm từ lá trà xanh tươi ngon vùng Thái Nguyên kết hợp với hương chanh tự nhiên, chứa EGCG chống oxy hóa mạnh mẽ.", Price = 10000, Quantity = 250, Image = "https://cdnv2.tgdd.vn/bhx-static/bhx/Products/Images/8938/85739/bhx/fgjiol_202410140914265805.jpg", Brand = "THP", Volume = "455ml", Status = true, IsFeatured = true, IsNew = true, CreatedDate = seedDate },
                new Product { Id = 13, CategoryId = 2, ProductName = "Trà đào C2 hương chanh", Description = "Trà xanh đóng chai C2 thơm mát hương đào kết hợp chanh chua nhẹ, thanh lọc cơ thể cực kỳ hiệu quả.", Price = 9000, Quantity = 180, Image = "https://cdn.hstatic.net/products/200001149339/c2_chanh_953ef6250c37477db3a55e21e051b037_1024x1024.jpg", Brand = "C2", Volume = "360ml", Status = true, IsFeatured = false, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 14, CategoryId = 2, ProductName = "Trà Bí Đao Wonderfarm lon", Description = "Wonderfarm trà bí đao thanh ngọt tự nhiên, giải độc mát gan, là thức uống truyền thống tuyệt hảo.", Price = 8500, Quantity = 160, Image = "https://images.unsplash.com/photo-1513558161293-cdaf765ed2fd?w=500&auto=format&fit=crop&q=60", Brand = "Wonderfarm", Volume = "310ml", Status = true, IsFeatured = false, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 15, CategoryId = 2, ProductName = "Lipton Ice Tea vị Chanh hộp pha sẵn", Description = "Lipton trà chanh lon mát lạnh, thơm mùi trà đen và chua chua vị chanh, uống cùng đá cực ngon.", Price = 11000, Quantity = 130, Image = "https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=500&auto=format&fit=crop&q=60", Brand = "Lipton", Volume = "330ml", Status = true, IsFeatured = true, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 16, CategoryId = 6, ProductName = "Sữa lúa mạch Nestlé Milo lon", Description = "Nestlé Milo bổ sung canxi và năng lượng hoạt chất Active-Go từ mầm lúa mạch, giúp trẻ em luôn năng động khỏe mạnh.", Price = 14000, Quantity = 210, Image = "https://images.unsplash.com/photo-1553530666-ba11a7da3888?w=500&auto=format&fit=crop&q=60", Brand = "Nestlé", Volume = "180ml", Status = true, IsFeatured = true, IsNew = true, CreatedDate = seedDate },
                new Product { Id = 17, CategoryId = 6, ProductName = "Sữa tươi tiệt trùng TH True Milk ít đường", Description = "Sữa tươi sạch TH True Milk ít đường được làm hoàn toàn từ sữa tươi sạch nguyên chất của trang trại TH.", Price = 9000, Quantity = 300, Image = "https://images.unsplash.com/photo-1563636619-e9143da7973b?w=500&auto=format&fit=crop&q=60", Brand = "TH True Milk", Volume = "180ml", Status = true, IsFeatured = false, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 18, CategoryId = 6, ProductName = "Sữa dinh dưỡng Vinamilk có đường", Description = "Sữa tươi tiệt trùng Vinamilk thơm ngon béo ngậy, giàu vitamin A, D3 và canxi giúp sáng mắt khỏe xương.", Price = 8500, Quantity = 280, Image = "https://www.lottemart.vn/media/catalog/product/cache/0x0/8/9/8934673500357-2.jpg.webp", Brand = "Vinamilk", Volume = "180ml", Status = true, IsFeatured = false, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 19, CategoryId = 3, ProductName = "Nescafé sữa đá đóng lon", Description = "Cà phê sữa đá Nescafé mang lại hương vị cà phê phin đậm đà kết hợp sữa béo ngậy chuẩn gu Việt.", Price = 15000, Quantity = 170, Image = "https://cdn.tgdd.vn/Products/Images/8966/195217/bhx/ca-phe-sua-nescafe-170ml-201904171555454406.jpg", Brand = "Nescafé", Volume = "180ml", Status = true, IsFeatured = true, IsNew = false, CreatedDate = seedDate },
                new Product { Id = 20, CategoryId = 3, ProductName = "Highlands Coffee Cà Phê Sữa Đá lon", Description = "Hương vị cà phê sữa đá trứ danh của Highlands Coffee nay được đóng lon tiện lợi, đậm đà tỉnh táo.", Price = 19000, Quantity = 220, Image = "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__NEW_CAN_CFS_INTERNATIONAL_185ml.jpg", Brand = "Highlands Coffee", Volume = "235ml", Status = true, IsFeatured = true, IsNew = true, CreatedDate = seedDate }
            );

            // ---- Tài khoản quản trị mặc định ----
            // Username: admin / Password: admin123
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FullName = "Quản trị viên",
                    Phone = "0123456789",
                    Email = "admin@drinkstore.com",
                    Address = "Hệ thống DrinkStore",
                    Username = "admin",
                    Password = "admin123",
                    Role = "Admin",
                    CreatedDate = seedDate
                }
            );
        }
    }
}
