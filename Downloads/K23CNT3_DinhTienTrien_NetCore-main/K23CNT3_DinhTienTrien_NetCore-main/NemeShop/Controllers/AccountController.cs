using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NemeShop.Models;
using System.Net;
using System.Threading.Tasks;

namespace NemeShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly QuanLyTapHoaContext _context;
        public AccountController(QuanLyTapHoaContext context) => _context = context;

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("MaKh") != null ||
                !string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("Index", "Home");
            }
            return View("~/Views/User/Account/Login.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string matKhau)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(matKhau))
                {
                    ViewBag.Error = "Vui lòng nhập email và mật khẩu.";
                    return View("~/Views/User/Account/Login.cshtml");
                }

                // Khách hàng
                var kh = await _context.KhachHangs
                    .FirstOrDefaultAsync(x => x.Email == email && x.MaKh > 0 && x.TrangThai == true);

                if (kh != null && kh.MatKhau == matKhau)
                {
                    HttpContext.Session.SetInt32("MaKh", kh.MaKh);
                    HttpContext.Session.SetString("UserEmail", kh.Email ?? "");
                    HttpContext.Session.SetString("HoTen", WebUtility.HtmlEncode(kh.HoTen) ?? "");
                    HttpContext.Session.SetString("UserRole", "Customer");
                    HttpContext.Session.SetString("ShowWelcome", "true");

                    return RedirectToAction("Index", "Home");
                }

                // Admin
                var user = await _context.QuanTriViens
                    .FirstOrDefaultAsync(x => x.Email == email && x.TrangThai == true);

                if (user != null && user.MatKhau == matKhau)
                {
                    HttpContext.Session.SetString("AdminEmail", user.Email);
                    HttpContext.Session.SetString("AdminId", user.MaQtv.ToString());
                    HttpContext.Session.SetString("HoTen", WebUtility.HtmlEncode(user.HoTen) ?? "Quản trị viên");
                    HttpContext.Session.SetString("UserRole", "Administrator");
                    HttpContext.Session.SetString("ShowWelcome", "true");

                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Error = "Sai email hoặc mật khẩu. Vui lòng thử lại.";
                return View("~/Views/User/Account/Login.cshtml");
            }
            catch (Exception ex)
            {
                // Log lỗi
                System.Diagnostics.Debug.WriteLine($"Login Error: {ex.Message}");
                ViewBag.Error = "Có lỗi xảy ra khi đăng nhập. Vui lòng thử lại.";
                return View("~/Views/User/Account/Login.cshtml");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32("MaKh") != null ||
                !string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("Index", "Home");
            }
            return View("~/Views/User/Account/Register.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
    string HoTen,
    string Email,
    string MatKhau,
    string ConfirmPassword,
    string DienThoai,
    string DiaChi)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(HoTen) || string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(MatKhau) || string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    ViewBag.Error = "Vui lòng điền đầy đủ thông tin bắt buộc.";
                    return View("~/Views/User/Account/Register.cshtml");
                }

                // Check email format
                if (!IsValidEmail(Email))
                {
                    ViewBag.Error = "Email không đúng định dạng.";
                    return View("~/Views/User/Account/Register.cshtml");
                }

                // Check password match
                if (MatKhau != ConfirmPassword)
                {
                    ViewBag.Error = "Mật khẩu xác nhận không khớp.";
                    return View("~/Views/User/Account/Register.cshtml");
                }

                // Check password length
                if (MatKhau.Length < 6)
                {
                    ViewBag.Error = "Mật khẩu phải có ít nhất 6 ký tự.";
                    return View("~/Views/User/Account/Register.cshtml");
                }

                // Check if email exists
                var existingEmail = await _context.KhachHangs
                    .FirstOrDefaultAsync(x => x.Email == Email && x.TrangThai == true);
                if (existingEmail != null)
                {
                    ViewBag.Error = "Email đã được sử dụng. Vui lòng chọn email khác.";
                    return View("~/Views/User/Account/Register.cshtml");
                }

                // QUAN TRỌNG: KHÔNG tự gán MaKh vì nó là IDENTITY trong database
                var khachHang = new KhachHang
                {
                    // MaKh sẽ được database tự động gán (IDENTITY)
                    HoTen = HoTen?.Trim(),
                    Email = Email?.Trim().ToLower(),
                    MatKhau = MatKhau,
                    DienThoai = DienThoai?.Trim(),
                    DiaChi = DiaChi?.Trim(),
                    TrangThai = true
                };

                _context.KhachHangs.Add(khachHang);
                await _context.SaveChangesAsync();

                // Auto login after registration - Sửa thành MaKh
                HttpContext.Session.SetInt32("MaKh", khachHang.MaKh); // Dùng MaKh từ entity sau khi save
                HttpContext.Session.SetString("UserEmail", khachHang.Email ?? "");
                HttpContext.Session.SetString("HoTen", WebUtility.HtmlEncode(khachHang.HoTen) ?? "");
                HttpContext.Session.SetString("UserRole", "Customer");
                HttpContext.Session.SetString("ShowWelcome", "true");

                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateException dbEx)
            {
                // Log chi tiết lỗi database
                var innerException = dbEx.InnerException;
                var errorMessage = innerException?.Message ?? dbEx.Message;

                System.Diagnostics.Debug.WriteLine($"Database Error: {errorMessage}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {innerException?.StackTrace}");

                ViewBag.Error = $"Lỗi cơ sở dữ liệu: {errorMessage}";
                return View("~/Views/User/Account/Register.cshtml");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Registration Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");

                ViewBag.Error = "Có lỗi xảy ra khi đăng ký. Vui lòng thử lại.";
                return View("~/Views/User/Account/Register.cshtml");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public IActionResult HideWelcomeMessage()
        {
            try
            {
                HttpContext.Session.Remove("ShowWelcome");
                return Ok();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"HideWelcome Error: {ex.Message}");
                return StatusCode(500);
            }
        }

        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home", new { logout = "success" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Logout Error: {ex.Message}");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View("~/Views/User/Account/AccessDenied.cshtml");
        }
        // GET: Account/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            // Chỉ cho phép khách hàng đổi mật khẩu
            if (HttpContext.Session.GetInt32("MaKh") == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để đổi mật khẩu.";
                return RedirectToAction("Login");
            }
            return View("~/Views/User/Account/ChangePassword.cshtml");
        }

        // POST: Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
            string oldPassword,
            string newPassword,
            string confirmPassword)
        {
            try
            {
                if (HttpContext.Session.GetInt32("MaKh") == null)
                {
                    TempData["Error"] = "Bạn cần đăng nhập để đổi mật khẩu.";
                    return RedirectToAction("Login");
                }

                // Validation thủ công
                if (string.IsNullOrEmpty(oldPassword))
                {
                    TempData["Error"] = "Vui lòng nhập mật khẩu cũ.";
                    return View("~/Views/User/Account/ChangePassword.cshtml");
                }

                if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
                {
                    TempData["Error"] = "Mật khẩu mới phải có ít nhất 6 ký tự.";
                    return View("~/Views/User/Account/ChangePassword.cshtml");
                }

                if (newPassword != confirmPassword)
                {
                    TempData["Error"] = "Mật khẩu mới và xác nhận mật khẩu không khớp.";
                    return View("~/Views/User/Account/ChangePassword.cshtml");
                }

                var maKh = HttpContext.Session.GetInt32("MaKh").Value;
                var khachHang = await _context.KhachHangs.FindAsync(maKh);

                if (khachHang == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                    return RedirectToAction("Login");
                }

                // Kiểm tra mật khẩu cũ
                if (khachHang.MatKhau != oldPassword)
                {
                    TempData["Error"] = "Mật khẩu cũ không đúng.";
                    return View("~/Views/User/Account/ChangePassword.cshtml");
                }

                // Kiểm tra mật khẩu mới không trùng mật khẩu cũ
                if (oldPassword == newPassword)
                {
                    TempData["Error"] = "Mật khẩu mới không được trùng với mật khẩu cũ.";
                    return View("~/Views/User/Account/ChangePassword.cshtml");
                }

                // Cập nhật mật khẩu mới
                khachHang.MatKhau = newPassword;
                _context.Update(khachHang);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Đổi mật khẩu thành công!";
                return RedirectToAction("Profile", "KhachHangs");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ChangePassword Error: {ex.Message}");
                TempData["Error"] = "Có lỗi xảy ra khi đổi mật khẩu. Vui lòng thử lại.";
                return View("~/Views/User/Account/ChangePassword.cshtml");
            }
        }
    }
}