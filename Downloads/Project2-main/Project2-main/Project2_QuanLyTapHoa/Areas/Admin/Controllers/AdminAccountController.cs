using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2_QuanLyTapHoa.Models;

namespace Project2_QuanLyTapHoa.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountController : Controller
    {
        private readonly QuanLyTapHoaContext _context;
        public AdminAccountController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string matKhau)
        {
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
            HttpContext.Session.Remove("AdminEmail");
            return RedirectToAction("Login");
        }
    }
}
