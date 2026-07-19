using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrinkStore.Data;
using DrinkStore.Extensions;
using DrinkStore.Models;
using DrinkStore.Models.ViewModels;

namespace DrinkStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly DrinkStoreContext _context;
        public const string SESSION_USER_KEY = "CurrentUser";

        public AccountController(DrinkStoreContext context)
        {
            _context = context;
        }

        // ---------------- REGISTER ----------------
        public IActionResult Register() => View(new RegisterViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _context.Customers.AnyAsync(c => c.Username == model.Username))
            {
                ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập đã tồn tại, vui lòng chọn tên khác.");
                return View(model);
            }

            if (await _context.Customers.AnyAsync(c => c.Email == model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), "Email này đã được đăng ký.");
                return View(model);
            }

            var customer = new Customer
            {
                FullName = model.FullName,
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address,
                Username = model.Username,
                Password = model.Password, // Demo: lưu văn bản thuần theo đúng cấu trúc gốc. Môi trường thật cần hash mật khẩu.
                Role = "Customer"
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập để tiếp tục mua sắm.";
            return RedirectToAction(nameof(Login));
        }

        // ---------------- LOGIN ----------------
        public IActionResult Login(string returnUrl = null) => View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var customer = await _context.Customers.FirstOrDefaultAsync(
                c => c.Username == model.Username && c.Password == model.Password);

            if (customer == null)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không chính xác.");
                return View(model);
            }

            var sessionUser = new SessionCustomer
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Username = customer.Username,
                Email = customer.Email,
                Role = customer.Role
            };

            HttpContext.Session.SetObject(SESSION_USER_KEY, sessionUser);

            if (customer.Role == "Admin")
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        // ---------------- LOGOUT ----------------
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SESSION_USER_KEY);
            return RedirectToAction("Index", "Home");
        }

        // ---------------- PROFILE ----------------
        public async Task<IActionResult> Profile()
        {
            var session = HttpContext.Session.GetObjectOrNull<SessionCustomer>(SESSION_USER_KEY);
            if (session == null) return RedirectToAction(nameof(Login), new { returnUrl = "/Account/Profile" });

            var customer = await _context.Customers.FindAsync(session.Id);
            if (customer == null) return RedirectToAction(nameof(Login));

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(Customer model)
        {
            var session = HttpContext.Session.GetObjectOrNull<SessionCustomer>(SESSION_USER_KEY);
            if (session == null) return RedirectToAction(nameof(Login));

            var customer = await _context.Customers.FindAsync(session.Id);
            if (customer == null) return RedirectToAction(nameof(Login));

            customer.FullName = model.FullName;
            customer.Phone = model.Phone;
            customer.Address = model.Address;
            // Email/Username giữ nguyên để tránh trùng lặp; chỉ cập nhật thông tin liên hệ

            await _context.SaveChangesAsync();

            var sessionUser = new SessionCustomer
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Username = customer.Username,
                Email = customer.Email,
                Role = customer.Role
            };
            HttpContext.Session.SetObject(SESSION_USER_KEY, sessionUser);

            TempData["Success"] = "Cập nhật thông tin thành công.";
            return RedirectToAction(nameof(Profile));
        }
    }
}
