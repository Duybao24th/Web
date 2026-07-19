using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrinkStore.Data;
using DrinkStore.Models.ViewModels;

namespace DrinkStore.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        private readonly DrinkStoreContext _context;

        public DashboardController(DrinkStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new AdminDashboardViewModel
            {
                TotalProducts = await _context.Products.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                TotalCustomers = await _context.Customers.CountAsync(c => c.Role != "Admin"),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalRevenue = await _context.Orders.Where(o => o.Status != "Đã hủy").SumAsync(o => (decimal?)o.TotalMoney) ?? 0,
                PendingOrders = await _context.Orders.CountAsync(o => o.Status == "Chờ xử lý"),
                RecentOrders = await _context.Orders
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .ToListAsync(),
                LowStockProducts = await _context.Products
                    .Where(p => p.Quantity < 50)
                    .OrderBy(p => p.Quantity)
                    .Take(5)
                    .ToListAsync()
            };

            vm.TopProducts = await _context.OrderDetails
                .Include(od => od.Product)
                .GroupBy(od => od.Product.ProductName)
                .Select(g => new TopProductViewModel
                {
                    ProductName = g.Key,
                    TotalSold = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(5)
                .ToListAsync();

            return View(vm);
        }
    }
}
