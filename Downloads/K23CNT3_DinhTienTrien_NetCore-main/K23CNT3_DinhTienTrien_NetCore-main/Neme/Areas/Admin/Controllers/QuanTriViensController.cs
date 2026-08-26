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
    public class QuanTriViensController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public QuanTriViensController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.QuanTriViens.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var quanTriVien = await _context.QuanTriViens
                .FirstOrDefaultAsync(m => m.MaQtv == id);

            if (quanTriVien == null) return NotFound();

            return View(quanTriVien);
        }

        public IActionResult Create()
        {
            return View(new QuanTriVien { TrangThai = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaQtv,HoTen,Email,MatKhau,DienThoai,DiaChi,TrangThai")] QuanTriVien quanTriVien)
        {
            if (ModelState.IsValid)
            {
                quanTriVien.MatKhau = HashPassword(quanTriVien.MatKhau);
                _context.Add(quanTriVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quanTriVien);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var quanTriVien = await _context.QuanTriViens.FindAsync(id);
            if (quanTriVien == null) return NotFound();

            return View(quanTriVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaQtv,HoTen,Email,MatKhau,DienThoai,DiaChi,TrangThai")] QuanTriVien quanTriVien)
        {
            if (id != quanTriVien.MaQtv) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(quanTriVien.MatKhau))
                    {
                        quanTriVien.MatKhau = HashPassword(quanTriVien.MatKhau);
                    }
                    _context.Update(quanTriVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuanTriVienExists(quanTriVien.MaQtv)) return NotFound();
                    ModelState.AddModelError("", "Có người khác đã cập nhật dữ liệu này. Vui lòng thử lại.");
                    return View(quanTriVien);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(quanTriVien);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var quanTriVien = await _context.QuanTriViens
                .FirstOrDefaultAsync(m => m.MaQtv == id);

            if (quanTriVien == null) return NotFound();

            return View(quanTriVien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quanTriVien = await _context.QuanTriViens.FindAsync(id);
            if (quanTriVien != null)
            {
                _context.QuanTriViens.Remove(quanTriVien);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool QuanTriVienExists(int id)
        {
            return _context.QuanTriViens.Any(e => e.MaQtv == id);
        }
    }
}
