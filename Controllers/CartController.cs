using Microsoft.AspNetCore.Mvc;
using DrinkStore.Data;
using DrinkStore.Extensions;
using DrinkStore.Models;

namespace DrinkStore.Controllers
{
    public class CartController : Controller
    {
        private readonly DrinkStoreContext _context;
        public const string CART_KEY = "StoreCart";

        public CartController(DrinkStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null || !product.Status)
            {
                TempData["Error"] = "Sản phẩm không tồn tại hoặc đã ngừng kinh doanh.";
                return RedirectToAction("Index", "Product");
            }

            var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY);
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Image = product.Image,
                    Volume = product.Volume,
                    Quantity = quantity
                });
            }

            HttpContext.Session.SetObject(CART_KEY, cart);
            TempData["Success"] = $"Đã thêm \"{product.ProductName}\" vào giỏ hàng.";

            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer)) return Redirect(referer);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY);
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                if (quantity <= 0) cart.Remove(item);
                else item.Quantity = quantity;
            }

            HttpContext.Session.SetObject(CART_KEY, cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY);
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null) cart.Remove(item);

            HttpContext.Session.SetObject(CART_KEY, cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CART_KEY);
            return RedirectToAction("Index");
        }
    }
}
