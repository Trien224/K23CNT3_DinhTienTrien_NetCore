using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Neme.Filters;
using Neme.Models;


namespace Neme.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeAdmin]
    public class DanhGiaSanPhamsController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public DanhGiaSanPhamsController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // ===== INDEX =====
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var danhGias = _context.DanhGiaSanPhams
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaSpNavigation);

            return View(await danhGias.ToListAsync());
        }

        // ===== DETAILS =====
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var danhGiaSanPham = await _context.DanhGiaSanPhams
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.MaDanhGia == id);

            if (danhGiaSanPham == null) return NotFound();

            return View(danhGiaSanPham);
        }

        // ===== CREATE =====
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "HoTen");
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "TenSp");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] DanhGiaSanPhamDto dto)
        {
            if (dto == null)
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });

            if (!dto.MaKh.HasValue || !dto.MaSp.HasValue || !dto.SoSao.HasValue)
                return Json(new { success = false, message = "Thiếu dữ liệu bắt buộc" });

            if (dto.SoSao < 1 || dto.SoSao > 5)
                return Json(new { success = false, message = "Số sao phải từ 1 đến 5" });

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage)
                                        .ToList();
                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            var entity = new DanhGiaSanPham
            {
                MaKh = dto.MaKh.Value,
                MaSp = dto.MaSp.Value,
                SoSao = dto.SoSao.Value,
                NoiDung = dto.NoiDung,
                LaYeuThich = dto.LaYeuThich,
                NgayTao = DateTime.Now,
                TrangThai = dto.TrangThai
            };

            try
            {
                _context.DanhGiaSanPhams.Add(entity);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Thêm đánh giá thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        // ===== EDIT =====
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var danhGia = await _context.DanhGiaSanPhams.FindAsync(id);
            if (danhGia == null) return NotFound();

            var dto = new DanhGiaUpdateTrangThaiDto
            {
                MaDanhGia = danhGia.MaDanhGia,
                TrangThai = danhGia.TrangThai
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] DanhGiaUpdateTrangThaiDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("; ", errors) });
            }

            var entity = await _context.DanhGiaSanPhams.FindAsync(dto.MaDanhGia);
            if (entity == null)
                return Json(new { success = false, message = "Không tìm thấy đánh giá" });

            entity.TrangThai = dto.TrangThai;

            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        // ===== DELETE =====
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var danhGiaSanPham = await _context.DanhGiaSanPhams
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.MaDanhGia == id);

            if (danhGiaSanPham == null) return NotFound();

            return View(danhGiaSanPham);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhGiaSanPham = await _context.DanhGiaSanPhams.FindAsync(id);
            if (danhGiaSanPham != null)
            {
                _context.DanhGiaSanPhams.Remove(danhGiaSanPham);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DanhGiaSanPhamExists(int id)
        {
            return _context.DanhGiaSanPhams.Any(e => e.MaDanhGia == id);
        }
    }
}
