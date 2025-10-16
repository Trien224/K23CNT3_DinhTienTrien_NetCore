using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NemeShop.Models;
using System;
using System.Threading.Tasks;

namespace NemeShop.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public HoaDonController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // GET: /HoaDon/GetPendingOrderCount
        [HttpGet]
        public async Task<IActionResult> GetPendingOrderCount()
        {
            var maKh = HttpContext.Session.GetInt32("MaKh");
            if (maKh == null)
                return Json(0);

            var count = await _context.HoaDons
                .CountAsync(h => h.MaKh == maKh.Value && h.TrangThai == 0); // Trạng thái 0: Chờ xác nhận

            return Json(count);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MuaNgay(int maSp, int soLuong, string diaChiGiaoHang, string sdtgiaoHang, string ghiChu = "")
        {
            try
            {
                var maKh = HttpContext.Session.GetInt32("MaKh");
                if (maKh == null)
                {
                    return Json(new { success = false, message = "Bạn cần đăng nhập để mua ngay." });
                }

                // Kiểm tra thông tin bắt buộc
                if (string.IsNullOrWhiteSpace(diaChiGiaoHang))
                {
                    return Json(new { success = false, message = "Vui lòng nhập địa chỉ giao hàng." });
                }

                if (string.IsNullOrWhiteSpace(sdtgiaoHang))
                {
                    return Json(new { success = false, message = "Vui lòng nhập số điện thoại." });
                }

                if (soLuong <= 0)
                {
                    return Json(new { success = false, message = "Số lượng phải lớn hơn 0." });
                }

                var sp = await _context.SanPhams.FirstOrDefaultAsync(x => x.MaSp == maSp && x.TrangThai);
                if (sp == null)
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm." });

                if (sp.SoLuong < soLuong)
                    return Json(new { success = false, message = $"Sản phẩm chỉ còn {sp.SoLuong} sản phẩm trong kho." });

                // TÍNH TOÁN GIÁ SAU KHI GIẢM
                decimal giaSauGiam = sp.DonGia;
                if (sp.PhanTramGiam > 0)
                {
                    giaSauGiam = sp.DonGia * (1 - sp.PhanTramGiam / 100);
                }

                // Làm tròn đến hàng nghìn
                giaSauGiam = Math.Round(giaSauGiam / 1000) * 1000;

                // Tạo hóa đơn
                var hoaDon = new HoaDon
                {
                    MaKh = maKh.Value,
                    NgayLap = DateTime.Now,
                    TongTien = sp.DonGia * soLuong,
                    ThanhTien = giaSauGiam * soLuong,
                    DiaChiGiaoHang = diaChiGiaoHang.Trim(),
                    SdtgiaoHang = sdtgiaoHang.Trim(),
                    GhiChu = string.IsNullOrEmpty(ghiChu) ? "Mua ngay" : ghiChu.Trim(),
                    TrangThai = 0 // Chờ xác nhận
                };

                _context.HoaDons.Add(hoaDon);
                await _context.SaveChangesAsync();

                var cthd = new ChiTietHoaDon
                {
                    MaHd = hoaDon.MaHd,
                    MaSp = sp.MaSp,
                    SoLuong = soLuong,
                    DonGia = giaSauGiam,
                    ThanhTien = giaSauGiam * soLuong
                };
                _context.ChiTietHoaDons.Add(cthd);

                // Trừ kho
                sp.SoLuong -= soLuong;

                await _context.SaveChangesAsync();

                // Lấy số lượng đơn hàng mới
                var newOrderCount = await _context.HoaDons
                    .CountAsync(h => h.MaKh == maKh.Value && h.TrangThai == 0);

                return Json(new
                {
                    success = true,
                    message = "Đặt hàng thành công! Đơn hàng của bạn đang chờ xác nhận.",
                    redirectUrl = Url.Action("Details", "HoaDon", new { id = hoaDon.MaHd }),
                    newOrderCount = newOrderCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi xử lý đơn hàng. Vui lòng thử lại." });
            }
        }

        // Các method khác giữ nguyên...
        public async Task<IActionResult> Details(int id)
        {
            var hd = await _context.HoaDons
                .Include(h => h.ChiTietHoaDons)
                .ThenInclude(ct => ct.MaSpNavigation)
                .Include(h => h.MaKhNavigation)
                .FirstOrDefaultAsync(h => h.MaHd == id);

            if (hd == null) return NotFound();

            foreach (var ct in hd.ChiTietHoaDons)
            {
                var sanPham = ct.MaSpNavigation;
                if (sanPham != null && sanPham.PhanTramGiam > 0)
                {
                    ViewData[$"GiaGoc_{ct.MaSp}"] = sanPham.DonGia;
                    ViewData[$"PhanTramGiam_{ct.MaSp}"] = sanPham.PhanTramGiam;
                }
            }

            return View(hd);
        }

        public IActionResult MyOrders()
        {
            var maKh = HttpContext.Session.GetInt32("MaKh");
            if (maKh == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var list = _context.HoaDons
                .Where(h => h.MaKh == maKh)
                .OrderByDescending(h => h.NgayLap)
                .ToList();

            return View(list);
        }

        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var maKh = HttpContext.Session.GetInt32("MaKh");
            if (maKh == null)
            {
                return Unauthorized("Bạn cần đăng nhập.");
            }

            var order = await _context.HoaDons
                .Include(h => h.ChiTietHoaDons)
                .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefaultAsync(h => h.MaHd == id && h.MaKh == maKh);

            if (order == null) return NotFound("Không tìm thấy đơn hàng");

            var result = new
            {
                order.MaHd,
                order.NgayLap,
                order.ThanhTien,
                order.TrangThai,
                SanPhams = order.ChiTietHoaDons.Select(ct => new
                {
                    TenSp = ct.MaSpNavigation.TenSp,
                    ct.SoLuong,
                    ct.DonGia,
                    ct.ThanhTien,
                    HinhAnh = ct.MaSpNavigation.HinhAnh ?? "/images/placeholder.png"
                })
            };

            return Json(result);
        }
    }
}