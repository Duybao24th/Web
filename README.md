# DrinkStore – ASP.NET Core MVC (.NET 8)

Website bán nước giải khát trực tuyến (danh mục, sản phẩm, giỏ hàng, đặt hàng, đăng ký/đăng nhập) kèm **Trang Quản Trị (Admin)** để quản lý danh mục, sản phẩm, đơn hàng, khách hàng.

Dự án này được xây dựng lại thành ứng dụng **ASP.NET Core MVC hoàn chỉnh** dựa trên bản mô phỏng/demo (`drinkstore.zip` – dự án AI Studio dạng web tĩnh + bộ sinh mã mẫu) mà bạn đã cung cấp: cùng bộ 7 danh mục, 20 sản phẩm mẫu, luồng nghiệp vụ giỏ hàng – đặt hàng – quản trị.

## Công nghệ sử dụng

- ASP.NET Core MVC 8 (.NET 8)
- Entity Framework Core 8 (Code First) + SQL Server
- Repository Pattern (`IRepository<T>` / `Repository<T>`)
- Session (giỏ hàng, đăng nhập demo không dùng Identity)
- Bootstrap 5 + Bootstrap Icons (giao diện, tải qua CDN)

## Cấu trúc thư mục chính

```
DrinkStore/
├── Models/                 # Category, Product, Customer, Order, OrderDetail, CartItem...
├── Data/DrinkStoreContext.cs  # DbContext + Seed Data (7 danh mục, 20 sản phẩm, tài khoản admin)
├── Interfaces/ + Repository/  # Repository Pattern tổng quát
├── Controllers/             # Home, Product, Cart, Account, Order (trang bán hàng)
├── Areas/Admin/              # Dashboard, Category, Product, Order, Customer (trang quản trị)
├── Views/                    # Razor Views (Bootstrap 5)
└── wwroot/                   # css/js tĩnh
```

## Cài đặt & chạy dự án

**Yêu cầu:** .NET 8 SDK, SQL Server (LocalDB / Express / Docker đều được).

1. Mở thư mục `DrinkStore` bằng Visual Studio 2022 hoặc VS Code.
2. Kiểm tra chuỗi kết nối trong `appsettings.json` (`ConnectionStrings:DefaultConnection`), sửa lại tên server SQL Server cho phù hợp máy bạn, ví dụ:
   ```
   Server=(localdb)\\mssqllocaldb;Database=DrinkStoreDB;Trusted_Connection=True;TrustServerCertificate=True
   ```
3. Tạo migration đầu tiên và cập nhật database:
   ```bash
   dotnet tool install --global dotnet-ef   # nếu chưa có
   cd DrinkStore
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
   *(Ứng dụng cũng tự động gọi `db.Database.Migrate()` khi khởi động, nên bước `database update` có thể bỏ qua nếu bạn chạy thẳng ứng dụng sau khi tạo migration).*
4. Chạy ứng dụng:
   ```bash
   dotnet run
   ```
5. Truy cập `https://localhost:xxxx` để xem trang bán hàng.

## Tài khoản quản trị mặc định (seed sẵn)

| Tài khoản | Mật khẩu | Vai trò |
|---|---|---|
| `admin` | `admin123` | Admin |

Đăng nhập tại `/Account/Login` bằng tài khoản trên sẽ tự động chuyển vào `/Admin/Dashboard`.

Khách hàng có thể tự đăng ký tài khoản mới tại `/Account/Register`.

## Ghi chú quan trọng

- **Mật khẩu đang lưu dạng văn bản thuần (plain text)** để bám sát đúng cấu trúc `Models/Customer.cs` trong bản mã nguồn mẫu bạn cung cấp (`mvcCodebase.js`). Trước khi triển khai thật, bạn **nên băm mật khẩu** (ví dụ dùng `ASP.NET Core Identity` hoặc `BCrypt`) và cân nhắc chuyển xác thực sang Cookie Authentication/Identity thay vì Session thủ công.
- Giỏ hàng được lưu trong **Session** (giống `CartController` gốc), không lưu vào database.
- Ảnh sản phẩm dùng đường dẫn URL ngoài (giống dữ liệu mẫu gốc); nếu ảnh lỗi sẽ tự thay bằng ảnh placeholder.
- Khi xóa sản phẩm đã phát sinh đơn hàng, hệ thống sẽ tự chuyển sang "ẩn khỏi cửa hàng" thay vì xóa cứng, để tránh vỡ dữ liệu `OrderDetail`.

## Các chức năng chính

**Trang bán hàng:**
- Trang chủ: danh mục, sản phẩm nổi bật, hàng mới về
- Danh sách sản phẩm: lọc theo danh mục, tìm kiếm, sắp xếp, phân trang
- Chi tiết sản phẩm + sản phẩm liên quan
- Giỏ hàng: thêm/sửa số lượng/xóa sản phẩm
- Đặt hàng (yêu cầu đăng nhập), theo dõi đơn hàng, hủy đơn khi còn "Chờ xử lý"
- Đăng ký / đăng nhập / hồ sơ cá nhân

**Trang quản trị (`/Admin`, yêu cầu tài khoản Role = Admin):**
- Dashboard: doanh thu, đơn hàng, sản phẩm bán chạy, sản phẩm sắp hết hàng
- CRUD Danh mục
- CRUD Sản phẩm
- Quản lý đơn hàng + cập nhật trạng thái
- Danh sách khách hàng
