using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_02.Data;
using Project_02.Models;

namespace Project_02.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HoaDonsController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public HoaDonsController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // GET: Customer/HoaDons
        public async Task<IActionResult> Index()
        {
            var quanLyTapHoaContext = _context.HoaDons.Include(h => h.MaKhNavigation).Include(h => h.MaVoucherNavigation);
            return View(await quanLyTapHoaContext.ToListAsync());
        }

        // GET: Customer/HoaDons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaVoucherNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

       

        // GET: Customer/HoaDons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaVoucherNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // POST: Customer/HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDon = await _context.HoaDons
                .Include(h => h.ChiTietHoaDons) // nạp chi tiết
                .FirstOrDefaultAsync(h => h.MaHd == id);

            if (hoaDon == null)
                return NotFound();

            // ✅ Xóa toàn bộ chi tiết trước
            if (hoaDon.ChiTietHoaDons != null && hoaDon.ChiTietHoaDons.Any())
            {
                _context.ChiTietHoaDons.RemoveRange(hoaDon.ChiTietHoaDons);
            }

            _context.HoaDons.Remove(hoaDon);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/HoaDons/Create
        public IActionResult Create()
        {
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "HoTen");
            ViewData["MaVoucher"] = new SelectList(_context.MaGiamGia, "MaVoucher", "MaVoucher");
            return View();
        }

        // POST: Admin/HoaDons/CreateAjax
        [HttpPost]
        public async Task<JsonResult> CreateAjax([FromBody] HoaDonDto dto)
        {
            if (dto == null)
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });

            try
            {
                var hoaDon = new HoaDon
                {
                    NgayLap = dto.NgayLap,
                    MaKh = dto.MaKh,
                    MaVoucher = dto.MaVoucher,
                    DiaChiGiaoHang = dto.DiaChiGiaoHang,
                    SdtgiaoHang = dto.SdtgiaoHang,
                    GhiChu = dto.GhiChu,
                    TongTien = dto.TongTien,
                    TienGiamGia = dto.TienGiamGia,
                    ThanhTien = dto.ThanhTien,
                    TrangThai = dto.TrangThai
                };

                _context.HoaDons.Add(hoaDon);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Tạo hóa đơn thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Admin/HoaDons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null) return NotFound();

            // Khách hàng dropdown
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "HoTen", hoaDon.MaKh);

            // Voucher dropdown
            ViewData["MaVoucher"] = new SelectList(_context.MaGiamGia, "MaVoucher", "MaVoucher", hoaDon.MaVoucher);

            // Trạng thái dropdown
            ViewData["TrangThaiList"] = new SelectList(
                new[]
                {
            new { Value = 0, Text = "Chờ xử lý" },
            new { Value = 1, Text = "Đã thanh toán" },
            new { Value = 2, Text = "Đang giao hàng" },
            new { Value = 3, Text = "Đã giao thành công" }
                }, "Value", "Text", hoaDon.TrangThai
            );

            // Convert entity sang DTO
            var dto = new HoaDonDto
            {
                MaHd = hoaDon.MaHd,
                MaKh = hoaDon.MaKh,
                MaVoucher = hoaDon.MaVoucher,
                DiaChiGiaoHang = hoaDon.DiaChiGiaoHang,
                SdtgiaoHang = hoaDon.SdtgiaoHang,
                GhiChu = hoaDon.GhiChu,
                TongTien = hoaDon.TongTien,
                TienGiamGia = hoaDon.TienGiamGia,
                ThanhTien = hoaDon.ThanhTien,
                TrangThai = hoaDon.TrangThai
            };

            return View(dto);
        }


        // POST: Admin/HoaDons/EditAjax
        [HttpPost]
        public async Task<JsonResult> EditAjax([FromBody] HoaDonDto dto)
        {
            if (dto == null || dto.MaHd == null)
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });

            var hoaDon = await _context.HoaDons.FindAsync(dto.MaHd.Value);
            if (hoaDon == null)
                return Json(new { success = false, message = "Hóa đơn không tồn tại" });

            try
            {
                hoaDon.MaKh = dto.MaKh;
                hoaDon.MaVoucher = dto.MaVoucher;
                hoaDon.DiaChiGiaoHang = dto.DiaChiGiaoHang;
                hoaDon.SdtgiaoHang = dto.SdtgiaoHang;
                hoaDon.GhiChu = dto.GhiChu;
                hoaDon.TongTien = dto.TongTien;
                hoaDon.TienGiamGia = dto.TienGiamGia;
                hoaDon.ThanhTien = dto.ThanhTien;
                hoaDon.TrangThai = dto.TrangThai;

                _context.HoaDons.Update(hoaDon);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Cập nhật hóa đơn thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Customer/HoaDons/TinhVoucher
        [HttpGet]
        public async Task<JsonResult> TinhVoucher(string maVoucher, decimal tongTien)
        {
            if (string.IsNullOrWhiteSpace(maVoucher))
                return Json(new { success = false, message = "Vui lòng nhập mã voucher!" });

            var voucher = await _context.MaGiamGia.FirstOrDefaultAsync(v => v.MaVoucher == maVoucher && v.TrangThai == true);
            if (voucher == null)
                return Json(new { success = false, message = "Mã voucher không tồn tại hoặc đã bị khóa!" });

            if (voucher.NgayBatDau > DateTime.Now)
                return Json(new { success = false, message = "Voucher chưa đến ngày bắt đầu áp dụng!" });

            if (voucher.NgayKetThuc < DateTime.Now)
                return Json(new { success = false, message = "Voucher đã hết hạn sử dụng!" });

            if (voucher.SoLuong <= 0)
                return Json(new { success = false, message = "Voucher đã hết số lượt sử dụng!" });

            if (voucher.GiaTriDonToiThieu.HasValue && tongTien < voucher.GiaTriDonToiThieu.Value)
                return Json(new { success = false, message = $"Đơn hàng tối thiểu phải từ {voucher.GiaTriDonToiThieu.Value:N0}đ để áp dụng voucher này!" });

            decimal tienGiam = 0;
            if (voucher.GiamTienTrucTiep.HasValue && voucher.GiamTienTrucTiep.Value > 0)
            {
                tienGiam = voucher.GiamTienTrucTiep.Value;
            }
            else if (voucher.PhanTramGiam.HasValue && voucher.PhanTramGiam.Value > 0)
            {
                tienGiam = tongTien * voucher.PhanTramGiam.Value / 100m;
            }

            if (tienGiam > tongTien) tienGiam = tongTien;
            decimal thanhTien = tongTien - tienGiam;

            return Json(new { success = true, tienGiam, thanhTien, voucherName = voucher.TenVoucher });
        }

        // POST: Customer/HoaDons/CreateOrderAjax
        [HttpPost]
        public async Task<JsonResult> CreateOrderAjax([FromBody] CheckoutRequestDto dto)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any())
            {
                return Json(new { success = false, message = "Giỏ hàng không có sản phẩm nào để thanh toán!" });
            }

            if (string.IsNullOrWhiteSpace(dto.DiaChiGiaoHang) || string.IsNullOrWhiteSpace(dto.SdtgiaoHang))
            {
                return Json(new { success = false, message = "Vui lòng nhập đầy đủ số điện thoại và địa chỉ nhận hàng!" });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Xác định thông tin khách hàng
                int? maKh = HttpContext.Session.GetInt32("MaKh") ?? dto.MaKh;
                if (!maKh.HasValue || maKh.Value <= 0)
                {
                    var existingCustomer = await _context.KhachHangs.FirstOrDefaultAsync(k => k.DienThoai == dto.SdtgiaoHang);
                    if (existingCustomer != null)
                    {
                        maKh = existingCustomer.MaKh;
                    }
                    else
                    {
                        var newCustomer = new KhachHang
                        {
                            HoTen = string.IsNullOrWhiteSpace(dto.HoTenNguoiNhan) ? "Khách Mua Lẻ" : dto.HoTenNguoiNhan,
                            DienThoai = dto.SdtgiaoHang,
                            DiaChi = dto.DiaChiGiaoHang,
                            MatKhau = "123456",
                            TrangThai = true
                        };
                        _context.KhachHangs.Add(newCustomer);
                        await _context.SaveChangesAsync();
                        maKh = newCustomer.MaKh;
                    }
                }

                var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();
                var products = await _context.SanPhams.Where(p => productIds.Contains(p.MaSp)).ToListAsync();

                decimal tongTien = 0;
                var chiTietList = new List<ChiTietHoaDon>();

                foreach (var item in dto.Items)
                {
                    var product = products.FirstOrDefault(p => p.MaSp == item.ProductId);
                    if (product == null || product.TrangThai == false)
                    {
                        await transaction.RollbackAsync();
                        return Json(new { success = false, message = $"Sản phẩm ID {item.ProductId} không tồn tại hoặc đã ngừng kinh doanh!" });
                    }

                    if (item.Quantity <= 0)
                    {
                        await transaction.RollbackAsync();
                        return Json(new { success = false, message = "Số lượng sản phẩm không hợp lệ!" });
                    }

                    if (product.SoLuong < item.Quantity)
                    {
                        await transaction.RollbackAsync();
                        return Json(new { success = false, message = $"Sản phẩm '{product.TenSp}' chỉ còn lại {product.SoLuong} sản phẩm trong kho!" });
                    }

                    decimal donGia = product.DonGia;
                    if (product.PhanTramGiam > 0)
                    {
                        donGia = donGia * (100 - product.PhanTramGiam) / 100m;
                    }

                    decimal thanhTienItem = donGia * item.Quantity;
                    tongTien += thanhTienItem;

                    product.SoLuong -= item.Quantity;

                    chiTietList.Add(new ChiTietHoaDon
                    {
                        MaSp = product.MaSp,
                        SoLuong = item.Quantity,
                        DonGia = donGia,
                        ThanhTien = thanhTienItem
                    });
                }

                // Kiểm tra voucher
                decimal tienGiam = 0;
                MaGiamGium? voucher = null;
                if (!string.IsNullOrWhiteSpace(dto.MaVoucher))
                {
                    voucher = await _context.MaGiamGia.FirstOrDefaultAsync(v => v.MaVoucher == dto.MaVoucher && v.TrangThai == true);
                    if (voucher != null)
                    {
                        if (voucher.SoLuong > 0 &&
                            voucher.NgayBatDau <= DateTime.Now &&
                            voucher.NgayKetThuc >= DateTime.Now &&
                            (!voucher.GiaTriDonToiThieu.HasValue || tongTien >= voucher.GiaTriDonToiThieu.Value))
                        {
                            if (voucher.GiamTienTrucTiep.HasValue && voucher.GiamTienTrucTiep.Value > 0)
                            {
                                tienGiam = voucher.GiamTienTrucTiep.Value;
                            }
                            else if (voucher.PhanTramGiam.HasValue && voucher.PhanTramGiam.Value > 0)
                            {
                                tienGiam = tongTien * voucher.PhanTramGiam.Value / 100m;
                            }

                            if (tienGiam > tongTien) tienGiam = tongTien;
                            voucher.SoLuong -= 1;
                            voucher.DaSuDung += 1;
                        }
                    }
                }

                decimal tongThanhTien = tongTien - tienGiam;

                var hoaDon = new HoaDon
                {
                    MaKh = maKh.Value,
                    NgayLap = DateTime.Now,
                    TongTien = tongTien,
                    TienGiamGia = tienGiam,
                    ThanhTien = tongThanhTien,
                    MaVoucher = voucher?.MaVoucher,
                    DiaChiGiaoHang = dto.DiaChiGiaoHang,
                    SdtgiaoHang = dto.SdtgiaoHang,
                    GhiChu = dto.GhiChu,
                    TrangThai = 0 // Chờ xử lý
                };

                _context.HoaDons.Add(hoaDon);
                await _context.SaveChangesAsync();

                foreach (var cthd in chiTietList)
                {
                    cthd.MaHd = hoaDon.MaHd;
                    _context.ChiTietHoaDons.Add(cthd);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new
                {
                    success = true,
                    orderId = hoaDon.MaHd,
                    total = tongThanhTien,
                    message = "Đặt hàng thành công! Mã đơn hàng của bạn là #" + hoaDon.MaHd
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "Lỗi khi xử lý đặt hàng: " + ex.Message });
            }
        }
    }
}
