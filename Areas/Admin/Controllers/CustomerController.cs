using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrinkStore.Data;

namespace DrinkStore.Areas.Admin.Controllers
{
    public class CustomerController : BaseAdminController
    {
        private readonly DrinkStoreContext _context;

        public CustomerController(DrinkStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            var query = _context.Customers.Where(c => c.Role != "Admin").AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => c.FullName.Contains(search) || c.Email.Contains(search) || c.Username.Contains(search));

            var customers = await query.OrderByDescending(c => c.CreatedDate).ToListAsync();
            ViewBag.Search = search;

            return View(customers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return NotFound();
            return View(customer);
        }
    }
}
