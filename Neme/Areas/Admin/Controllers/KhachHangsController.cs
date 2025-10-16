using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neme.Filters;
using Neme.Models;
using System.Security.Cryptography;
using System.Text;

namespace Neme.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        // GET: KhachHangs/Details/5
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
                // Hash mật khẩu trước khi lưu
                khachHang.MatKhau = HashPassword(khachHang.MatKhau);

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
                    // Nếu mật khẩu không rỗng, hash trước khi lưu
                    if (!string.IsNullOrEmpty(khachHang.MatKhau))
                        khachHang.MatKhau = HashPassword(khachHang.MatKhau);
                    else
                    {
                        // Giữ nguyên mật khẩu cũ nếu người dùng không nhập
                        var oldPassword = await _context.KhachHangs
                            .Where(k => k.MaKh == id)
                            .Select(k => k.MatKhau)
                            .FirstOrDefaultAsync();
                        khachHang.MatKhau = oldPassword;
                    }

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
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
            return _context.KhachHangs.Any(e => e.MaKh == id);
        }

        // ================= HASH PASSWORD =================
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
