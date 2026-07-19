using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrinkStore.Data;
using DrinkStore.Models.ViewModels;

namespace DrinkStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly DrinkStoreContext _context;

        public ProductController(DrinkStoreContext context)
        {
            _context = context;
        }

        // GET: /Product?categoryId=1&search=coca&sort=price_asc&page=1
        public async Task<IActionResult> Index(int? categoryId, string search, string sort, int page = 1)
        {
            var query = _context.Products.Include(p => p.Category).Where(p => p.Status).AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.ProductName.Contains(search) || p.Brand.Contains(search));

            query = sort switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "name_asc" => query.OrderBy(p => p.ProductName),
                "newest" => query.OrderByDescending(p => p.CreatedDate),
                _ => query.OrderByDescending(p => p.IsFeatured).ThenByDescending(p => p.CreatedDate)
            };

            var pageSize = 8;
            var totalItems = await query.CountAsync();
            var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var vm = new ProductListViewModel
            {
                Products = products,
                Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync(),
                SelectedCategoryId = categoryId,
                SearchTerm = search,
                SortOrder = sort,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(vm);
        }

        // GET: /Product/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            ViewBag.RelatedProducts = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != id && p.Status)
                .Take(4)
                .ToListAsync();

            return View(product);
        }
    }
}
