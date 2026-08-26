using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NemeChuaShop.Models;

namespace NemeChuaShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly NemeChuaShopContext _context;

        public AccountController(NemeChuaShopContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1. Kiểm tra đăng nhập Quản trị viên
            var admin = await _context.QuanTriViens
                .FirstOrDefaultAsync(q => q.TaiKhoan == model.UserName && q.MatKhau == model.Password && q.TrangThai);

            if (admin != null)
            {
                HttpContext.Session.SetInt32("UserId", admin.Id);
                HttpContext.Session.SetString("UserName", admin.TaiKhoan);
                HttpContext.Session.SetString("UserRole", "Admin");
                HttpContext.Session.SetString("DisplayName", "Quản trị viên (" + admin.TaiKhoan + ")");

                TempData["SuccessMessage"] = "Đăng nhập quyền Quản trị viên thành công!";

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "SanPhams");
            }

            // 2. Kiểm tra đăng nhập Khách hàng
            var customer = await _context.KhachHangs
                .FirstOrDefaultAsync(k => (k.Email == model.UserName || k.MaKhachHang == model.UserName) && k.MatKhau == model.Password && k.TrangThai);

            if (customer != null)
            {
                HttpContext.Session.SetInt32("UserId", customer.Id);
                HttpContext.Session.SetString("UserName", customer.Email);
                HttpContext.Session.SetString("UserRole", "Customer");
                HttpContext.Session.SetString("DisplayName", customer.HoTen);

                TempData["SuccessMessage"] = "Xin chào " + customer.HoTen + ", đăng nhập thành công!";

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không chính xác.");
            return View(model);
        }

        // POST: /Account/AjaxLogin (Đăng nhập nhanh từ Modal Popup)
        [HttpPost]
        public async Task<IActionResult> AjaxLogin([FromBody] LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password))
            {
                return Json(new { success = false, message = "Vui lòng nhập đầy đủ tài khoản và mật khẩu." });
            }

            // 1. Kiểm tra Admin
            var admin = await _context.QuanTriViens
                .FirstOrDefaultAsync(q => q.TaiKhoan == model.UserName && q.MatKhau == model.Password && q.TrangThai);

            if (admin != null)
            {
                HttpContext.Session.SetInt32("UserId", admin.Id);
                HttpContext.Session.SetString("UserName", admin.TaiKhoan);
                HttpContext.Session.SetString("UserRole", "Admin");
                HttpContext.Session.SetString("DisplayName", "Quản trị viên (" + admin.TaiKhoan + ")");

                return Json(new { success = true, message = "Đăng nhập Admin thành công!", role = "Admin", redirectUrl = "/SanPhams" });
            }

            // 2. Kiểm tra Khách hàng
            var customer = await _context.KhachHangs
                .FirstOrDefaultAsync(k => (k.Email == model.UserName || k.MaKhachHang == model.UserName) && k.MatKhau == model.Password && k.TrangThai);

            if (customer != null)
            {
                HttpContext.Session.SetInt32("UserId", customer.Id);
                HttpContext.Session.SetString("UserName", customer.Email);
                HttpContext.Session.SetString("UserRole", "Customer");
                HttpContext.Session.SetString("DisplayName", customer.HoTen);

                return Json(new { success = true, message = "Đăng nhập thành công!", role = "Customer", redirectUrl = "/" });
            }

            return Json(new { success = false, message = "Tài khoản hoặc mật khẩu không chính xác." });
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.KhachHangs.AnyAsync(k => k.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                    return View(model);
                }

                string maKH = "KH" + (await _context.KhachHangs.CountAsync() + 1).ToString("D4");

                var customer = new KhachHang
                {
                    MaKhachHang = maKH,
                    HoTen = model.HoTen,
                    Email = model.Email,
                    MatKhau = model.MatKhau,
                    DienThoai = model.DienThoai,
                    DiaChi = model.DiaChi,
                    TrangThai = true,
                    NgayDangKy = DateOnly.FromDateTime(DateTime.Now),
                    NgayTao = DateTime.Now,
                    NgayCapNhat = DateTime.Now
                };

                _context.KhachHangs.Add(customer);
                await _context.SaveChangesAsync();

                // Tự động đăng nhập
                HttpContext.Session.SetInt32("UserId", customer.Id);
                HttpContext.Session.SetString("UserName", customer.Email);
                HttpContext.Session.SetString("UserRole", "Customer");
                HttpContext.Session.SetString("DisplayName", customer.HoTen);

                TempData["SuccessMessage"] = "Đăng ký tài khoản thành công! Chào mừng " + customer.HoTen;
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // GET: /Account/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Đã đăng xuất tài khoản thành công.";
            return RedirectToAction("Index", "Home");
        }
    }
}
