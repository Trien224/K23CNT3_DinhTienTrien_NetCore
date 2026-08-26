using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Neme.Filters;
using Neme.Models;

namespace Neme.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeAdmin]
    public class ChiTietHoaDonsController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public ChiTietHoaDonsController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // ===== INDEX =====
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var chiTiet = _context.ChiTietHoaDons
                .Include(c => c.MaHdNavigation)
                .Include(c => c.MaSpNavigation);

            return View(await chiTiet.ToListAsync());
        }

        // ===== DETAILS =====
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var chiTietHoaDon = await _context.ChiTietHoaDons
                .Include(c => c.MaHdNavigation)
                .Include(c => c.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.MaCthd == id);

            if (chiTietHoaDon == null) return NotFound();

            return View(chiTietHoaDon);
        }

        // ===== CREATE =====
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["MaHd"] = new SelectList(_context.HoaDons, "MaHd", "MaHd");
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "TenSp");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChiTietHoaDonDto dto)
        {
            if (dto.SoLuong <= 0)
            {
                ModelState.AddModelError("SoLuong", "Số lượng phải lớn hơn 0");
            }

            if (ModelState.IsValid)
            {
                var sp = await _context.SanPhams.FindAsync(dto.MaSp);
                if (sp == null)
                {
                    ModelState.AddModelError("MaSp", "Sản phẩm không tồn tại");
                }
                else
                {
                    var entity = new ChiTietHoaDon
                    {
                        MaHd = dto.MaHd,
                        MaSp = dto.MaSp,
                        SoLuong = dto.SoLuong,
                        DonGia = sp.DonGia,
                        ThanhTien = sp.DonGia * dto.SoLuong
                    };

                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["MaHd"] = new SelectList(_context.HoaDons, "MaHd", "MaHd", dto.MaHd);
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "TenSp", dto.MaSp);
            return View(dto);
        }

        // ===== EDIT =====
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var entity = await _context.ChiTietHoaDons.FindAsync(id);
            if (entity == null) return NotFound();

            var dto = new ChiTietHoaDonDto
            {
                MaCthd = entity.MaCthd,
                MaHd = entity.MaHd,
                MaSp = entity.MaSp,
                SoLuong = entity.SoLuong,
                DonGia = entity.DonGia,
                ThanhTien = entity.ThanhTien
            };

            ViewData["MaHd"] = new SelectList(_context.HoaDons, "MaHd", "MaHd", dto.MaHd);
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "TenSp", dto.MaSp);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ChiTietHoaDonDto dto)
        {
            if (id != dto.MaCthd) return NotFound();

            if (dto.SoLuong <= 0)
            {
                ModelState.AddModelError("SoLuong", "Số lượng phải lớn hơn 0");
            }

            if (ModelState.IsValid)
            {
                var entity = await _context.ChiTietHoaDons.FindAsync(id);
                if (entity == null) return NotFound();

                var sp = await _context.SanPhams.FindAsync(entity.MaSp);
                if (sp == null) return NotFound();

                entity.SoLuong = dto.SoLuong;
                entity.DonGia = sp.DonGia;
                entity.ThanhTien = sp.DonGia * dto.SoLuong;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietHoaDonExists(entity.MaCthd))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["MaHd"] = new SelectList(_context.HoaDons, "MaHd", "MaHd", dto.MaHd);
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "TenSp", dto.MaSp);
            return View(dto);
        }

        // ===== DELETE =====
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var chiTietHoaDon = await _context.ChiTietHoaDons
                .Include(c => c.MaHdNavigation)
                .Include(c => c.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.MaCthd == id);

            if (chiTietHoaDon == null) return NotFound();

            return View(chiTietHoaDon);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chiTietHoaDon = await _context.ChiTietHoaDons.FindAsync(id);
            if (chiTietHoaDon != null)
            {
                _context.ChiTietHoaDons.Remove(chiTietHoaDon);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ChiTietHoaDonExists(int id)
        {
            return _context.ChiTietHoaDons.Any(e => e.MaCthd == id);
        }
    }
}
