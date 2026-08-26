using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_02.Data;
using Project_02.Models;

namespace Project_02.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ChiTietHoaDonsController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public ChiTietHoaDonsController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // GET: Customer/ChiTietHoaDons
        public async Task<IActionResult> Index()
        {
            var chiTietHoaDons = _context.ChiTietHoaDons
                .Include(c => c.MaHdNavigation)
                .Include(c => c.MaSpNavigation);
            return View(await chiTietHoaDons.ToListAsync());
        }

        // GET: Customer/ChiTietHoaDons/Details/5
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

        // GET: Customer/ChiTietHoaDons/Create
        public IActionResult Create()
        {
            ViewData["MaHd"] = new SelectList(_context.HoaDons, "MaHd", "MaHd");
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "TenSp");
            return View();
        }

        // POST: Customer/ChiTietHoaDons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChiTietHoaDon chiTietHoaDon)
        {
            if (ModelState.IsValid)
            {
                var sanPham = await _context.SanPhams.FindAsync(chiTietHoaDon.MaSp);
                if (sanPham == null) return NotFound();

                chiTietHoaDon.DonGia = sanPham.DonGia;
                chiTietHoaDon.ThanhTien = chiTietHoaDon.SoLuong * chiTietHoaDon.DonGia;

                _context.Add(chiTietHoaDon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaHd"] = new SelectList(_context.HoaDons, "MaHd", "MaHd", chiTietHoaDon.MaHd);
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "TenSp", chiTietHoaDon.MaSp);
            return View(chiTietHoaDon);
        }

        // GET: Customer/ChiTietHoaDons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var chiTietHoaDon = await _context.ChiTietHoaDons.FindAsync(id);
            if (chiTietHoaDon == null) return NotFound();

            return View(chiTietHoaDon);
        }

        // POST: Customer/ChiTietHoaDons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ChiTietHoaDon chiTietHoaDon)
        {
            if (id != chiTietHoaDon.MaCthd) return NotFound();

            if (ModelState.IsValid)
            {
                var chiTiet = await _context.ChiTietHoaDons.FindAsync(id);
                if (chiTiet == null) return NotFound();

                chiTiet.SoLuong = chiTietHoaDon.SoLuong;

                var sanPham = await _context.SanPhams.FindAsync(chiTiet.MaSp);
                if (sanPham == null) return NotFound();

                chiTiet.DonGia = sanPham.DonGia;
                chiTiet.ThanhTien = chiTiet.SoLuong * chiTiet.DonGia;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(chiTietHoaDon);
        }

        // GET: Customer/ChiTietHoaDons/Delete/5
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

        // POST: Customer/ChiTietHoaDons/Delete/5
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
    }
}
