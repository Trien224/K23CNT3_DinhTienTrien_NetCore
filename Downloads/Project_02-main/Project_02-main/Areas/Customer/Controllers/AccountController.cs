using Microsoft.AspNetCore.Mvc;
using Project_02.Data;
using Project_02.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Project_02.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public AccountController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // GET: Customer/Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Customer/Account/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var customer = _context.KhachHangs
                                   .FirstOrDefault(c => c.Email == email && c.MatKhau == password);

            if (customer != null)
            {
                HttpContext.Session.SetInt32("MaKh", customer.MaKh);

                HttpContext.Session.SetString("CustomerEmail", customer.Email);
                HttpContext.Session.SetString("CustomerName", customer.HoTen);

                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }

            ViewBag.Error = "Sai email hoặc mật khẩu!";
            return View();
        }

        // GET: Customer/Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // 
            return RedirectToAction("Login");
        }
    }
}
