using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DrinkStore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Volume = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsNew = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalMoney = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReceiverName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReceiverPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Các loại nước ngọt có ga, sảng khoái và đã khát", "Nước ngọt" },
                    { 2, "Trà xanh, trà đóng chai thanh mát, giải độc", "Trà" },
                    { 3, "Cà phê đóng lon, pha sẵn thơm ngon, tỉnh táo tức thì", "Cà phê" },
                    { 4, "Nước ép trái cây tự nhiên, giàu vitamin", "Nước ép" },
                    { 5, "Nước tinh khiết và nước khoáng bổ sung vi chất", "Nước khoáng" },
                    { 6, "Sữa tươi, sữa tiệt trùng thơm béo, bổ dưỡng", "Sữa" },
                    { 7, "Bổ sung năng lượng mạnh mẽ cho ngày dài hoạt động", "Nước tăng lực" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "CreatedDate", "Email", "FullName", "Password", "Phone", "Role", "Username" },
                values: new object[] { 1, "Hệ thống DrinkStore", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@drinkstore.com", "Quản trị viên", "admin123", "0123456789", "Admin", "admin" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "CategoryId", "CreatedDate", "Description", "Image", "IsFeatured", "IsNew", "Price", "ProductName", "Quantity", "Status", "Volume" },
                values: new object[,]
                {
                    { 1, "Coca-Cola", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nước ngọt có ga Coca-Cola hương vị độc đáo nguyên bản, giúp đập tan cơn khát, mang lại cảm giác sảng khoái tức thì cùng bữa ăn ngon.", "https://upload.wikimedia.org/wikipedia/commons/2/27/Coca_Cola_Flasche_-_Original_Taste.jpg", true, false, 10000m, "Coca-Cola Original vị nguyên bản", 120, true, "330ml" },
                    { 2, "Pepsi", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nước ngọt Pepsi vị chanh không calo cực kỳ thích hợp cho người ăn kiêng, giải khát sảng khoái mà không lo tăng cân.", "https://upload.wikimedia.org/wikipedia/commons/d/dd/Pepsi_Can.jpg", true, true, 10000m, "Pepsi vị chanh không calo", 150, true, "330ml" },
                    { 3, "Sprite", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nước giải khát có ga hương chanh tự nhiên, mang lại cảm giác sảng khoái tột đỉnh cho người thưởng thức.", "https://images.unsplash.com/photo-1625772291427-39f64aa5eea7?w=500&auto=format&fit=crop&q=60", false, false, 9500m, "Nước ngọt Sprite vị chanh", 80, true, "330ml" },
                    { 4, "Fanta", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nước ngọt Fanta hương cam thơm mát bùng nổ, có ga cực kỳ sảng khoái, kích thích vị giác.", "https://images.unsplash.com/photo-1624517452488-04869289c4ca?w=500&auto=format&fit=crop&q=60", false, false, 9500m, "Fanta hương Cam", 90, true, "330ml" },
                    { 5, "Sting", 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sting dâu tây đỏ bùng nổ năng lượng, nhân sâm thơm ngon kích thích tỉnh táo sảng khoái tức thì.", "https://cdnv2.tgdd.vn/bhx-static/bhx/Products/Images/3226/76520/bhx/nuoc-tang-luc-sting-dau-pet-330ml_202509291516185862.jpg", true, true, 12000m, "Nước tăng lực Sting hương Dâu đỏ", 200, true, "330ml" },
                    { 6, "Red Bull", 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nước tăng lực bò húc Red Bull nhập khẩu Thái Lan giúp tỉnh táo tinh thần, bổ sung năng lượng dồi dào.", "https://images.unsplash.com/photo-1613946069412-38f7f1ff0b65?w=500&auto=format&fit=crop&q=60", true, false, 15000m, "Nước tăng lực Red Bull Thái Lan", 350, true, "250ml" },
                    { 7, "Number One", 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nước uống năng lượng Number One đóng chai tiện dụng giúp bạn vượt qua mệt mỏi, chinh phục mọi thử thách.", "https://images.unsplash.com/photo-1543257580-7269da773bf5?w=500&auto=format&fit=crop&q=60", false, false, 10000m, "Nước tăng lực Number One chai", 110, true, "250ml" },
                    { 8, "Revive", 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Revive chanh muối bù nước bù khoáng tức thì cho người hoạt động thể thao, đổ mồ hôi nhiều.", "https://images.unsplash.com/photo-1527960656306-b3a951473295?w=500&auto=format&fit=crop&q=60", true, false, 11000m, "Nước bù khoáng Revive Chanh Muối", 140, true, "390ml" },
                    { 9, "Aquafina", 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nước uống tinh khiết Aquafina qua hệ thống lọc 7 bước chuẩn Hoa Kỳ mang lại nguồn nước thanh khiết tuyệt đối.", "https://aquafinawater.vn/library/module_new/san-pham-cua-aquafina_s5874.jpg", false, false, 6000m, "Nước tinh khiết Aquafina", 500, true, "500ml" },
                    { 10, "La Vie", 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "La Vie chứa nhiều khoáng chất tự nhiên quý giá tốt cho sức khỏe, thích hợp uống mỗi ngày.", "https://images.unsplash.com/photo-1560011536-1855e82a0ecf?w=500&auto=format&fit=crop&q=60", false, false, 6500m, "Nước khoáng thiên nhiên La Vie", 400, true, "500ml" },
                    { 11, "Dasani", 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nước đóng chai Dasani tinh khiết chất lượng cao của hãng Coca-Cola Việt Nam, an toàn cho cả gia đình.", "https://dasaniwater.vn/library/module_new/cac-thac-mac-khach-hang-tieu-dung_s3464.jpg", false, false, 6000m, "Nước tinh khiết Dasani", 300, true, "500ml" },
                    { 12, "THP", 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Được làm từ lá trà xanh tươi ngon vùng Thái Nguyên kết hợp với hương chanh tự nhiên, chứa EGCG chống oxy hóa mạnh mẽ.", "https://cdnv2.tgdd.vn/bhx-static/bhx/Products/Images/8938/85739/bhx/fgjiol_202410140914265805.jpg", true, true, 10000m, "Trà Xanh Không Độ hương Chanh", 250, true, "455ml" },
                    { 13, "C2", 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trà xanh đóng chai C2 thơm mát hương đào kết hợp chanh chua nhẹ, thanh lọc cơ thể cực kỳ hiệu quả.", "https://cdn.hstatic.net/products/200001149339/c2_chanh_953ef6250c37477db3a55e21e051b037_1024x1024.jpg", false, false, 9000m, "Trà đào C2 hương chanh", 180, true, "360ml" },
                    { 14, "Wonderfarm", 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wonderfarm trà bí đao thanh ngọt tự nhiên, giải độc mát gan, là thức uống truyền thống tuyệt hảo.", "https://images.unsplash.com/photo-1513558161293-cdaf765ed2fd?w=500&auto=format&fit=crop&q=60", false, false, 8500m, "Trà Bí Đao Wonderfarm lon", 160, true, "310ml" },
                    { 15, "Lipton", 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lipton trà chanh lon mát lạnh, thơm mùi trà đen và chua chua vị chanh, uống cùng đá cực ngon.", "https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=500&auto=format&fit=crop&q=60", true, false, 11000m, "Lipton Ice Tea vị Chanh hộp pha sẵn", 130, true, "330ml" },
                    { 16, "Nestlé", 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nestlé Milo bổ sung canxi và năng lượng hoạt chất Active-Go từ mầm lúa mạch, giúp trẻ em luôn năng động khỏe mạnh.", "https://images.unsplash.com/photo-1553530666-ba11a7da3888?w=500&auto=format&fit=crop&q=60", true, true, 14000m, "Sữa lúa mạch Nestlé Milo lon", 210, true, "180ml" },
                    { 17, "TH True Milk", 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sữa tươi sạch TH True Milk ít đường được làm hoàn toàn từ sữa tươi sạch nguyên chất của trang trại TH.", "https://images.unsplash.com/photo-1563636619-e9143da7973b?w=500&auto=format&fit=crop&q=60", false, false, 9000m, "Sữa tươi tiệt trùng TH True Milk ít đường", 300, true, "180ml" },
                    { 18, "Vinamilk", 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sữa tươi tiệt trùng Vinamilk thơm ngon béo ngậy, giàu vitamin A, D3 và canxi giúp sáng mắt khỏe xương.", "https://www.lottemart.vn/media/catalog/product/cache/0x0/8/9/8934673500357-2.jpg.webp", false, false, 8500m, "Sữa dinh dưỡng Vinamilk có đường", 280, true, "180ml" },
                    { 19, "Nescafé", 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cà phê sữa đá Nescafé mang lại hương vị cà phê phin đậm đà kết hợp sữa béo ngậy chuẩn gu Việt.", "https://cdn.tgdd.vn/Products/Images/8966/195217/bhx/ca-phe-sua-nescafe-170ml-201904171555454406.jpg", true, false, 15000m, "Nescafé sữa đá đóng lon", 170, true, "180ml" },
                    { 20, "Highlands Coffee", 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hương vị cà phê sữa đá trứ danh của Highlands Coffee nay được đóng lon tiện lợi, đậm đà tỉnh táo.", "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__NEW_CAN_CFS_INTERNATIONAL_185ml.jpg", true, true, 19000m, "Highlands Coffee Cà Phê Sữa Đá lon", 220, true, "235ml" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
