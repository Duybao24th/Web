using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrinkStore.Data;

namespace DrinkStore.Areas.Admin.Controllers
{
    public class OrderController : BaseAdminController
    {
        private readonly DrinkStoreContext _context;
        private static readonly string[] StatusFlow = { "Chờ xử lý", "Đang giao hàng", "Đã giao hàng", "Đã hủy" };

        public OrderController(DrinkStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string status, int page = 1)
        {
            var query = _context.Orders.Include(o => o.Customer).AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(o => o.Status == status);

            var pageSize = 10;
            var total = await query.CountAsync();
            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Status = status;
            ViewBag.StatusFlow = StatusFlow;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            ViewBag.StatusFlow = StatusFlow;
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = status;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã cập nhật trạng thái đơn hàng.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
