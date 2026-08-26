using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Neme.Filters;
using Neme.Models;

namespace Neme.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeAdmin]
    public class SanPhamsController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public SanPhamsController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // ===== LIST =====
        public async Task<IActionResult> Index()
        {
            var products = _context.SanPhams.Include(s => s.MaLoaiNavigation);
            return View(await products.ToListAsync());
        }

        // ===== DETAILS =====
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var sp = await _context.SanPhams
                .Include(s => s.MaLoaiNavigation)
                .FirstOrDefaultAsync(x => x.MaSp == id);

            if (sp == null) return NotFound();
            return View(sp);
        }

        // ===== CREATE =====
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["MaLoai"] = new SelectList(_context.LoaiSanPhams, "MaLoai", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenSp,MoTa,DonGia,SoLuong,MaLoai,PhanTramGiam,TrangThai")] SanPhamDto dto, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                ViewData["MaLoai"] = new SelectList(_context.LoaiSanPhams, "MaLoai", "TenLoai", dto.MaLoai);
                return View(dto);
            }

            var sp = new SanPham
            {
                TenSp = dto.TenSp,
                MoTa = dto.MoTa,
                DonGia = dto.DonGia,
                SoLuong = dto.SoLuong,
                MaLoai = dto.MaLoai,
                PhanTramGiam = dto.PhanTramGiam,
                TrangThai = dto.TrangThai
            };

            // Upload file
            var path = await UploadFileAsync(file);
            if (path != null) sp.HinhAnh = path;

            _context.Add(sp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ===== EDIT =====
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sp = await _context.SanPhams.FindAsync(id);
            if (sp == null) return NotFound();

            var dto = new SanPhamDto
            {
                MaSp = sp.MaSp,
                TenSp = sp.TenSp,
                MoTa = sp.MoTa,
                DonGia = sp.DonGia,
                SoLuong = sp.SoLuong,
                MaLoai = sp.MaLoai,
                PhanTramGiam = sp.PhanTramGiam,
                TrangThai = sp.TrangThai,
                HinhAnh = sp.HinhAnh
            };

            ViewData["MaLoai"] = new SelectList(_context.LoaiSanPhams, "MaLoai", "TenLoai", sp.MaLoai);
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> EditAjax([FromForm] SanPhamDto dto, IFormFile? file)
        {
            var sp = await _context.SanPhams.FindAsync(dto.MaSp);
            if (sp == null) return Json(new { success = false, message = "Không tìm thấy sản phẩm" });

            sp.TenSp = dto.TenSp;
            sp.MoTa = dto.MoTa;
            sp.DonGia = dto.DonGia;
            sp.SoLuong = dto.SoLuong;
            sp.MaLoai = dto.MaLoai;
            sp.PhanTramGiam = dto.PhanTramGiam;
            sp.TrangThai = dto.TrangThai;

            // Upload file mới
            var path = await UploadFileAsync(file);
            if (path != null) sp.HinhAnh = path;

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Cập nhật thành công" });
        }

        // ===== DELETE =====
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sp = await _context.SanPhams
                .Include(s => s.MaLoaiNavigation)
                .FirstOrDefaultAsync(x => x.MaSp == id);

            if (sp == null) return NotFound();
            return View(sp);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sp = await _context.SanPhams.FindAsync(id);
            if (sp != null)
            {
                _context.SanPhams.Remove(sp);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // ===== HELPER UPLOAD FILE =====
        private async Task<string?> UploadFileAsync(IFormFile? file, string folder = "images/sanpham")
        {
            if (file == null || file.Length == 0) return null;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            try
            {
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
                return folder + "/" + fileName;
            }
            catch
            {
                return null; // có thể log lỗi ở đây
            }
        }

        private bool SanPhamExists(int id) => _context.SanPhams.Any(e => e.MaSp == id);
    }
}
