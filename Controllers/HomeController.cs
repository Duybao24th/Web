using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrinkStore.Data;
using DrinkStore.Models;

namespace DrinkStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly DrinkStoreContext _context;

        public HomeController(DrinkStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();

            ViewBag.FeaturedProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Status && p.IsFeatured)
                .OrderByDescending(p => p.CreatedDate)
                .Take(8)
                .ToListAsync();

            ViewBag.NewProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Status && p.IsNew)
                .OrderByDescending(p => p.CreatedDate)
                .Take(8)
                .ToListAsync();

            return View();
        }

        public IActionResult About() => View();

        public IActionResult Contact() => View();

        [HttpPost]
        public IActionResult Contact(string fullName, string email, string message)
        {
            TempData["ContactSuccess"] = "Cảm ơn bạn đã liên hệ! DrinkStore sẽ phản hồi sớm nhất.";
            return RedirectToAction(nameof(Contact));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View();
    }
}
