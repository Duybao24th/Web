using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DrinkStore.Data;
using DrinkStore.Models;

namespace DrinkStore.Areas.Admin.Controllers
{
    public class ProductController : BaseAdminController
    {
        private readonly DrinkStoreContext _context;

        public ProductController(DrinkStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search, int? categoryId, int page = 1)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.ProductName.Contains(search) || p.Brand.Contains(search));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId);

            var pageSize = 10;
            var total = await query.CountAsync();
            var products = await query
                .OrderByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Categories = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name", categoryId);
            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name");
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            ModelState.Remove(nameof(Product.Category));
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name", model.CategoryId);
                return View(model);
            }

            model.CreatedDate = DateTime.Now;
            _context.Products.Add(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Thêm sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product model)
        {
            if (id != model.Id) return NotFound();

            ModelState.Remove(nameof(Product.Category));
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name", model.CategoryId);
                return View(model);
            }

            _context.Products.Update(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var hasOrders = await _context.OrderDetails.AnyAsync(od => od.ProductId == id);
            if (hasOrders)
            {
                // Không xóa cứng nếu đã phát sinh đơn hàng, chỉ ẩn khỏi cửa hàng
                product.Status = false;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Sản phẩm đã có đơn hàng nên được ẩn khỏi cửa hàng thay vì xóa.";
            }
            else
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa sản phẩm.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
