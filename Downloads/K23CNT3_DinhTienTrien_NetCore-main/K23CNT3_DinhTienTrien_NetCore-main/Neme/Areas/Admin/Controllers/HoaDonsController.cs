using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Neme.Filters;
using Neme.Models;

namespace Neme.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeAdmin]
    public class HoaDonsController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public HoaDonsController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // GET: HoaDons
        public async Task<IActionResult> Index()
        {
            var hoaDons = _context.HoaDons
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaVoucherNavigation);
            return View(await hoaDons.ToListAsync());
        }

        // GET: HoaDons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var hoaDon = await _context.HoaDons
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaVoucherNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);

            if (hoaDon == null) return NotFound();

            return View(hoaDon);
        }

        // ================== CREATE ==================
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.MaKh = new SelectList(_context.KhachHangs, "MaKh", "HoTen");
            ViewBag.MaVoucher = new SelectList(_context.MaGiamGia.Where(v => v.TrangThai == true), "MaVoucher", "MaVoucher");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAjax([FromBody] HoaDonDto dto)
        {
            if (!ModelState.IsValid || dto == null)
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });

            decimal tienGiam = await TinhTienGiam(dto.MaVoucher, dto.TongTien, true);

            var hoaDon = new HoaDon
            {
                NgayLap = DateTime.Now,
                TongTien = dto.TongTien,
                TienGiamGia = tienGiam,
                ThanhTien = dto.TongTien - tienGiam,
                MaKh = dto.MaKh,
                MaVoucher = string.IsNullOrEmpty(dto.MaVoucher) ? null : dto.MaVoucher,
                DiaChiGiaoHang = dto.DiaChiGiaoHang,
                SdtgiaoHang = dto.SdtgiaoHang,
                GhiChu = dto.GhiChu,
                TrangThai = dto.TrangThai
            };

            _context.HoaDons.Add(hoaDon);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Thêm hóa đơn thành công!" });
        }

        // ================== TÍNH VOUCHER ==================
        [HttpGet]
        public async Task<IActionResult> TinhVoucher(string maVoucher, decimal tongTien)
        {
            var tienGiam = await TinhTienGiam(maVoucher, tongTien, false);

            if (tienGiam == 0)
                return Json(new { success = false, message = "Voucher không hợp lệ hoặc không áp dụng được" });

            return Json(new { success = true, tienGiam, thanhTien = tongTien - tienGiam });
        }

        private async Task<decimal> TinhTienGiam(string? maVoucher, decimal tongTien, bool capNhatSoLanSuDung)
        {
            if (string.IsNullOrEmpty(maVoucher)) return 0;

            var voucher = await _context.MaGiamGia.FirstOrDefaultAsync(v => v.MaVoucher == maVoucher && v.TrangThai);
            if (voucher == null) return 0;

            if (DateTime.Now < voucher.NgayBatDau || DateTime.Now > voucher.NgayKetThuc)
                return 0;

            if (tongTien < (voucher.GiaTriDonToiThieu ?? 0))
                return 0;

            if (voucher.SoLuong <= voucher.DaSuDung)
                return 0;

            decimal tienGiam = 0;
            if (voucher.PhanTramGiam.HasValue)
                tienGiam += tongTien * voucher.PhanTramGiam.Value / 100;

            if (voucher.GiamTienTrucTiep.HasValue)
                tienGiam += voucher.GiamTienTrucTiep.Value;

            if (tienGiam > tongTien) tienGiam = tongTien;

            // Chỉ tăng số lần sử dụng khi Create
            if (capNhatSoLanSuDung)
            {
                voucher.DaSuDung += 1;
                await _context.SaveChangesAsync();
            }

            return tienGiam;
        }

        // ================== EDIT ==================
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var hoaDon = await _context.HoaDons.FirstOrDefaultAsync(h => h.MaHd == id);
            if (hoaDon == null) return NotFound();

            var dto = new HoaDonDto
            {
                MaHd = hoaDon.MaHd,
                NgayLap = hoaDon.NgayLap,
                TongTien = hoaDon.TongTien,
                TienGiamGia = hoaDon.TienGiamGia,
                ThanhTien = hoaDon.ThanhTien,
                MaKh = hoaDon.MaKh,
                MaVoucher = hoaDon.MaVoucher,
                DiaChiGiaoHang = hoaDon.DiaChiGiaoHang,
                SdtgiaoHang = hoaDon.SdtgiaoHang,
                GhiChu = hoaDon.GhiChu,
                TrangThai = hoaDon.TrangThai
            };

            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "HoTen", dto.MaKh);
            ViewData["MaVoucher"] = new SelectList(_context.MaGiamGia, "MaVoucher", "MaVoucher", dto.MaVoucher);

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> EditAjax([FromBody] HoaDonDto dto)
        {
            if (!ModelState.IsValid || dto == null)
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });

            var hoaDon = await _context.HoaDons.FindAsync(dto.MaHd);
            if (hoaDon == null)
                return NotFound(new { message = "Không tìm thấy hóa đơn" });

            decimal tienGiam = await TinhTienGiam(dto.MaVoucher, dto.TongTien, false);

            // Cập nhật dữ liệu (KHÔNG thay đổi Ngày lập)
            hoaDon.TongTien = dto.TongTien;
            hoaDon.TienGiamGia = tienGiam;
            hoaDon.ThanhTien = dto.TongTien - tienGiam;
            hoaDon.MaKh = dto.MaKh;
            hoaDon.MaVoucher = string.IsNullOrEmpty(dto.MaVoucher) ? null : dto.MaVoucher;
            hoaDon.DiaChiGiaoHang = dto.DiaChiGiaoHang;
            hoaDon.SdtgiaoHang = dto.SdtgiaoHang;
            hoaDon.GhiChu = dto.GhiChu;
            hoaDon.TrangThai = dto.TrangThai;

            _context.Update(hoaDon);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Cập nhật thành công" });
        }

        // ================== DELETE ==================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var hoaDon = await _context.HoaDons
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaVoucherNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);

            if (hoaDon == null) return NotFound();

            return View(hoaDon);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDon = await _context.HoaDons
                .Include(h => h.ChiTietHoaDons)
                .FirstOrDefaultAsync(h => h.MaHd == id);

            if (hoaDon != null)
            {
                _context.ChiTietHoaDons.RemoveRange(hoaDon.ChiTietHoaDons);
                _context.HoaDons.Remove(hoaDon);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonExists(int id)
        {
            return _context.HoaDons.Any(e => e.MaHd == id);
        }
    }
}
