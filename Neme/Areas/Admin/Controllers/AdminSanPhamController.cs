using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neme.Models;

namespace Neme.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminSanPhamController : Controller
    {
        private readonly QuanLyTapHoaContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminSanPhamController(QuanLyTapHoaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ===== LIST =====
        public async Task<IActionResult> Index()
        {
            var products = await _context.SanPhams.ToListAsync();
            return View(products);
        }

        // ===== CREATE =====
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(SanPham sp, IFormFile HinhAnhFile)
        {
            if (HinhAnhFile != null)
            {
                string fileName = Path.GetFileName(HinhAnhFile.FileName);
                string path = Path.Combine(_env.WebRootPath, "images", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await HinhAnhFile.CopyToAsync(stream);
                }
                sp.HinhAnh = fileName;
            }

            _context.SanPhams.Add(sp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ===== EDIT =====
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var sp = await _context.SanPhams.FindAsync(id);
            if (sp == null) return NotFound();
            return View(sp);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SanPham sp, IFormFile? HinhAnhFile)
        {
            var existing = await _context.SanPhams.FindAsync(id);
            if (existing == null) return NotFound();

            existing.TenSp = sp.TenSp;
            existing.MoTa = sp.MoTa;
            existing.DonGia = sp.DonGia;
            existing.SoLuong = sp.SoLuong;
            existing.MaLoai = sp.MaLoai;
            existing.PhanTramGiam = sp.PhanTramGiam;
            existing.TrangThai = sp.TrangThai;

            if (HinhAnhFile != null)
            {
                string fileName = Path.GetFileName(HinhAnhFile.FileName);
                string path = Path.Combine(_env.WebRootPath, "images", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await HinhAnhFile.CopyToAsync(stream);
                }
                existing.HinhAnh = fileName;
            }

            _context.SanPhams.Update(existing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ===== DELETE =====
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var sp = await _context.SanPhams.FindAsync(id);
            if (sp == null) return NotFound();

            _context.SanPhams.Remove(sp);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
