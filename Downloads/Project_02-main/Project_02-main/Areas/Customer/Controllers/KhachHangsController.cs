using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Project_02.Data;
using Project_02.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Project_02.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class KhachHangsController : Controller
    {
        private readonly QuanLyTapHoaContext _context;
        private readonly PasswordHasher<KhachHang> _hasher;

        public KhachHangsController(QuanLyTapHoaContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<KhachHang>();
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.KhachHangs.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null) return NotFound();
            return View(khachHang);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoTen,Email,MatKhau,DienThoai,DiaChi,TrangThai")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                if (await _context.KhachHangs.AnyAsync(k => k.Email == khachHang.Email))
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại!");
                    return View(khachHang);
                }
                khachHang.MatKhau = _hasher.HashPassword(khachHang, khachHang.MatKhau);
                _context.Add(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null) return NotFound();
            return View(khachHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKh,HoTen,Email,MatKhau,DienThoai,DiaChi,TrangThai")] KhachHang khachHang)
        {
            if (id != khachHang.MaKh) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.KhachHangs.AsNoTracking().FirstOrDefaultAsync(k => k.MaKh == id);
                    if (existing == null) return NotFound();
                    if (await _context.KhachHangs.AnyAsync(k => k.Email == khachHang.Email && k.MaKh != id))
                    {
                        ModelState.AddModelError("Email", "Email đã tồn tại!");
                        return View(khachHang);
                    }
                    if (string.IsNullOrEmpty(khachHang.MatKhau))
                        khachHang.MatKhau = existing.MatKhau;
                    else
                        khachHang.MatKhau = _hasher.HashPassword(khachHang, khachHang.MatKhau);

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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null) return NotFound();
            return View(khachHang);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                // Soft delete
                khachHang.TrangThai = false;
                _context.Update(khachHang);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
            return _context.KhachHangs.Any(e => e.MaKh == id);
        }
    }
}
