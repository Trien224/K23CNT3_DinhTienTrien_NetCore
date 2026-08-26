using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NemeShop.Filters;
using NemeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NemeShop.Controllers
{
    [AuthorizeAdmin]
    public class KhachHangsController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public KhachHangsController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // GET: KhachHangs
        public async Task<IActionResult> Index()
        {
            return View(await _context.KhachHangs.ToListAsync());
        }

        // GET: KhachHangs/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null) return NotFound();

            return View(khachHang);
        }

        // GET: KhachHangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KhachHangs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKh,HoTen,Email,DienThoai,DiaChi,TrangThai,MatKhau")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                // ⚠ TODO: Hash mật khẩu trước khi lưu
                _context.Add(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null) return NotFound();

            return View(khachHang);
        }

        // POST: KhachHangs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKh,HoTen,Email,DienThoai,DiaChi,TrangThai,MatKhau")] KhachHang khachHang)
        {
            if (id != khachHang.MaKh) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // ⚠ TODO: Hash mật khẩu trước khi lưu
                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.MaKh)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null) return NotFound();

            return View(khachHang);
        }

        // POST: KhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                _context.KhachHangs.Remove(khachHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
            return _context.KhachHangs.Any(e => e.MaKh == id);
        }
        // GET: KhachHangs/Profile - Hiển thị profile của user đang đăng nhập
        [AllowAnonymous]
        public async Task<IActionResult> Profile()
        {
            try
            {
                // Lấy ID khách hàng từ session - SỬA: dùng GetInt32 thay vì GetString
                var maKh = HttpContext.Session.GetInt32("MaKh");

                // Kiểm tra xem session có tồn tại và hợp lệ không
                if (maKh == null || maKh <= 0)
                {
                    TempData["Error"] = "Vui lòng đăng nhập để xem thông tin cá nhân";
                    return RedirectToAction("Login", "Account");
                }

                var khachHang = await _context.KhachHangs.FindAsync(maKh);

                if (khachHang == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin khách hàng";
                    return RedirectToAction("Login", "Account");
                }

                return View("~/Views/User/Account/Profile.cshtml", khachHang);
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine($"Error in Profile action: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                // Xóa session bị lỗi
                HttpContext.Session.Remove("MaKh");
                HttpContext.Session.Remove("HoTen");

                TempData["Error"] = "Có lỗi xảy ra khi tải thông tin. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
        }
        // GET: KhachHangs/EditProfile - Chỉnh sửa profile cá nhân
        [AllowAnonymous]
        public async Task<IActionResult> EditProfile()
        {
            try
            {
                var maKh = HttpContext.Session.GetInt32("MaKh");
                if (maKh == null || maKh <= 0)
                {
                    TempData["Error"] = "Vui lòng đăng nhập để chỉnh sửa thông tin.";
                    return RedirectToAction("Login", "Account");
                }

                var khachHang = await _context.KhachHangs.FindAsync(maKh);

                if (khachHang == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                    return RedirectToAction("Login", "Account");
                }

                return View("~/Views/User/Account/EditProfile.cshtml", khachHang);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EditProfile GET Error: {ex.Message}");
                TempData["Error"] = "Có lỗi xảy ra khi tải trang chỉnh sửa.";
                return RedirectToAction("Profile");
            }
        }

        // POST: KhachHangs/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> EditProfile(
            string HoTen,
            string Email,
            string DienThoai,
            string DiaChi)
        {
            try
            {
                var maKh = HttpContext.Session.GetInt32("MaKh");
                if (maKh == null || maKh <= 0)
                {
                    TempData["Error"] = "Bạn không có quyền chỉnh sửa thông tin này";
                    return RedirectToAction("Profile");
                }

                // Validation cơ bản
                if (string.IsNullOrEmpty(HoTen))
                {
                    TempData["Error"] = "Họ tên không được để trống";
                    return RedirectToAction("EditProfile");
                }

                if (string.IsNullOrEmpty(Email))
                {
                    TempData["Error"] = "Email không được để trống";
                    return RedirectToAction("EditProfile");
                }

                var khachHang = await _context.KhachHangs.FindAsync(maKh);
                if (khachHang == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin khách hàng";
                    return RedirectToAction("Profile");
                }

                // Kiểm tra email trùng (trừ email của chính user)
                var existingEmail = await _context.KhachHangs
                    .FirstOrDefaultAsync(x => x.Email == Email && x.MaKh != maKh && x.TrangThai == true);
                if (existingEmail != null)
                {
                    TempData["Error"] = "Email đã được sử dụng bởi tài khoản khác";
                    return RedirectToAction("EditProfile");
                }

                // Cập nhật thông tin
                khachHang.HoTen = HoTen?.Trim();
                khachHang.Email = Email?.Trim();
                khachHang.DienThoai = DienThoai?.Trim();
                khachHang.DiaChi = DiaChi?.Trim();

                _context.Update(khachHang);
                await _context.SaveChangesAsync();

                // Cập nhật session
                HttpContext.Session.SetString("HoTen", WebUtility.HtmlEncode(khachHang.HoTen) ?? "");
                HttpContext.Session.SetString("UserEmail", khachHang.Email ?? "");

                TempData["Success"] = "Cập nhật thông tin thành công!";
                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EditProfile POST Error: {ex.Message}");
                TempData["Error"] = "Có lỗi xảy ra khi cập nhật thông tin";
                return RedirectToAction("EditProfile");
            }
        }
    }
}
