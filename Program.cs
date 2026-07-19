using Microsoft.EntityFrameworkCore;
using DrinkStore.Data;
using DrinkStore.Interfaces;
using DrinkStore.Repository;

var builder = WebApplication.CreateBuilder(args);

// 1. Kết nối cơ sở dữ liệu SQL Server
builder.Services.AddDbContext<DrinkStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Đăng ký Dependency Injection cho Repository Pattern
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// 3. Cấu hình Session (dùng để lưu giỏ hàng & thông tin đăng nhập)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Tự động tạo database & seed dữ liệu mẫu khi khởi động (tiện lợi cho môi trường demo/đồ án).
// Nếu bạn muốn dùng EF Core Migrations (khuyến nghị cho môi trường thực tế), hãy xem hướng dẫn trong README.md
// và đổi dòng dưới thành db.Database.Migrate() sau khi đã tạo migration bằng `dotnet ef migrations add InitialCreate`.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DrinkStoreContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
