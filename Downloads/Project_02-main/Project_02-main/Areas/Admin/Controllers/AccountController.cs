using Microsoft.AspNetCore.Mvc;
using Project_02.Data;
using Project_02.Models;

namespace Project_02.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public AccountController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // GET: Admin/Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Admin/Account/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var admin = _context.QuanTriViens
                                .FirstOrDefault(a => a.Email == email && a.MatKhau == password);

            if (admin != null)
            {
                // lưu session
                HttpContext.Session.SetString("AdminEmail", admin.Email);

                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            ViewBag.Error = "Sai email hoặc mật khẩu!";
            return View();
        }

        // GET: Admin/Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminEmail");
            return RedirectToAction("Login");
        }
    }
}
