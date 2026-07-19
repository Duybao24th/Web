using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrinkStore.Data;
using DrinkStore.Extensions;
using DrinkStore.Models;
using DrinkStore.Models.ViewModels;

namespace DrinkStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly DrinkStoreContext _context;

        public OrderController(DrinkStoreContext context)
        {
            _context = context;
        }

        private SessionCustomer CurrentUser => HttpContext.Session.GetObjectOrNull<SessionCustomer>(AccountController.SESSION_USER_KEY);

        // GET: /Order/Checkout
        public IActionResult Checkout()
        {
            if (CurrentUser == null)
                return RedirectToAction("Login", "Account", new { returnUrl = "/Order/Checkout" });

            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartController.CART_KEY);
            if (cart == null || cart.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index", "Cart");
            }

            var vm = new CheckoutViewModel
            {
                CartItems = cart,
                ReceiverName = CurrentUser.FullName
            };

            return View(vm);
        }

        // POST: /Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            if (CurrentUser == null)
                return RedirectToAction("Login", "Account", new { returnUrl = "/Order/Checkout" });

            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartController.CART_KEY);
            if (cart == null || cart.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index", "Cart");
            }

            model.CartItems = cart;

            if (!ModelState.IsValid) return View(model);

            var order = new Order
            {
                CustomerId = CurrentUser.Id,
                OrderDate = DateTime.Now,
                TotalMoney = cart.Sum(c => c.SubTotal),
                Status = "Chờ xử lý",
                PaymentMethod = model.PaymentMethod,
                ReceiverName = model.ReceiverName,
                ReceiverPhone = model.ReceiverPhone,
                ShippingAddress = model.ShippingAddress,
                Note = model.Note
            };

            foreach (var item in cart)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                });

                // Trừ số lượng tồn kho
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity = Math.Max(0, product.Quantity - item.Quantity);
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove(CartController.CART_KEY);

            return RedirectToAction(nameof(Success), new { id = order.Id });
        }

        // GET: /Order/Success/5
        public async Task<IActionResult> Success(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();
            return View(order);
        }

        // GET: /Order/MyOrders
        public async Task<IActionResult> MyOrders()
        {
            if (CurrentUser == null)
                return RedirectToAction("Login", "Account", new { returnUrl = "/Order/MyOrders" });

            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.CustomerId == CurrentUser.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // GET: /Order/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (CurrentUser == null)
                return RedirectToAction("Login", "Account");

            var order = await _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();
            if (order.CustomerId != CurrentUser.Id && !CurrentUser.IsAdmin) return Forbid();

            return View(order);
        }

        // POST: /Order/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            if (CurrentUser == null) return RedirectToAction("Login", "Account");

            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            if (order.CustomerId != CurrentUser.Id) return Forbid();

            if (order.Status == "Chờ xử lý")
            {
                order.Status = "Đã hủy";
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đơn hàng đã được hủy.";
            }
            else
            {
                TempData["Error"] = "Không thể hủy đơn hàng ở trạng thái hiện tại.";
            }

            return RedirectToAction(nameof(MyOrders));
        }
    }
}
