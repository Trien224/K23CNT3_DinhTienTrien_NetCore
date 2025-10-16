using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neme.Models;
using System.Security.Cryptography;
using System.Text;

namespace Neme.Controllers
{
    public class CustomerAccountController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public CustomerAccountController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // GET: Login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Handle login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string matKhau)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(matKhau))
            {
                ViewBag.Error = "Email và mật khẩu không được để trống!";
                return View();
            }

            string hashedPassword = ComputeSha256Hash(matKhau);

            var kh = await _context.KhachHangs
                .FirstOrDefaultAsync(x => x.Email == email && x.MatKhau == hashedPassword);

            if (kh != null)
            {
                // Save email to session
                HttpContext.Session.SetString("CustomerEmail", kh.Email);
                HttpContext.Session.SetInt32("CustomerId", kh.MaKh); // optional: store user ID

                return RedirectToAction("Index", "Shop");
            }

            ViewBag.Error = "Sai email hoặc mật khẩu!";
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CustomerEmail");
            HttpContext.Session.Remove("CustomerId"); // optional
            return RedirectToAction("Login");
        }

        // Utility: Hash password with SHA256
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
