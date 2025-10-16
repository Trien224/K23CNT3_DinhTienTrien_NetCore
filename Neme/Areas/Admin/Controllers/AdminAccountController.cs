using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neme.Models;

namespace Neme.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public AdminAccountController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Nếu đã đăng nhập thì không cho vào lại trang Login
            if (HttpContext.Session.GetString("AdminEmail") != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Bảo vệ chống CSRF
        public async Task<IActionResult> Login(string email, string matKhau)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(matKhau))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ email và mật khẩu!";
                return View();
            }

            var admin = await _context.QuanTriViens
                .FirstOrDefaultAsync(x => x.Email == email && x.MatKhau == matKhau);

            if (admin != null)
            {
                HttpContext.Session.SetString("AdminEmail", admin.Email);
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            ViewBag.Error = "Sai email hoặc mật khẩu quản trị!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xoá toàn bộ session thay vì chỉ 1 key
            return RedirectToAction("Login");
        }
    }
}
