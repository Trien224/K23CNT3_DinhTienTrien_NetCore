using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NemeShop.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace NemeShop.Controllers
{
    public class WishlistController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public WishlistController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // GET: /Wishlist/MyWishlist
        [HttpGet]
        public async Task<IActionResult> MyWishlist()
        {
            var maKh = HttpContext.Session.GetInt32("MaKh");
            if (maKh == null) return RedirectToAction("Login", "Account");

            var wishlistItems = await (
                from d in _context.DanhGiaSanPhams
                join s in _context.SanPhams on d.MaSp equals s.MaSp
                join l in _context.LoaiSanPhams on s.MaLoai equals l.MaLoai
                where d.MaKh == maKh && d.LaYeuThich && s.TrangThai
                orderby d.NgayTao descending
                select new ProductWishlistDto
                {
                    MaDanhGia = d.MaDanhGia,
                    ProductId = s.MaSp,
                    ProductName = s.TenSp,
                    Price = s.DonGia,
                    Image = s.HinhAnh,
                    Category = l.TenLoai,
                    AddedDate = d.NgayTao,
                    Stock = s.SoLuong
                }
            ).ToListAsync();

            return View(wishlistItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleWishlist(int productId)
        {
            try
            {
                var maKh = HttpContext.Session.GetInt32("MaKh");
                if (maKh == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Vui lòng đăng nhập để thêm vào yêu thích",
                        isWishlist = false
                    });
                }

                // Kiểm tra sản phẩm tồn tại
                var productExists = await _context.SanPhams.AnyAsync(p => p.MaSp == productId);
                if (!productExists)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Sản phẩm không tồn tại",
                        isWishlist = false
                    });
                }

                // Kiểm tra xem đã có đánh giá nào của user cho sản phẩm này chưa
                var existingReview = await _context.DanhGiaSanPhams
                    .FirstOrDefaultAsync(r => r.MaSp == productId && r.MaKh == maKh.Value);

                bool isWishlist;
                string message;

                if (existingReview == null)
                {
                    // Tạo mới chỉ với các field cần thiết
                    var newWishlist = new DanhGiaSanPham
                    {
                        MaKh = maKh.Value,
                        MaSp = productId,
                        SoSao = 5,
                        NoiDung = "Yêu thích",
                        LaYeuThich = true,
                        NgayTao = DateTime.Now,
                        TrangThai = true
                    };

                    _context.DanhGiaSanPhams.Add(newWishlist);
                    isWishlist = true;
                    message = "Đã thêm vào yêu thích ❤️";
                }
                else
                {
                    isWishlist = !existingReview.LaYeuThich;
                    existingReview.LaYeuThich = isWishlist;
                    existingReview.NgayTao = DateTime.Now;
                    message = isWishlist ? "Đã thêm vào yêu thích ❤️" : "Đã xóa khỏi yêu thích";
                }

                await _context.SaveChangesAsync();
                return Json(new
                {
                    success = true,
                    isWishlist = isWishlist,
                    message = message
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex.Message}");
                return Json(new
                {
                    success = false,
                    message = "Tính năng tạm thời bảo trì",
                    isWishlist = false
                });
            }
        }

        // POST: /Wishlist/RemoveFromWishlist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromWishlist(int reviewId)
        {
            var maKh = HttpContext.Session.GetInt32("MaKh");
            if (maKh == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập" });

            try
            {
                var wishlistItem = await _context.DanhGiaSanPhams
                    .FirstOrDefaultAsync(r => r.MaDanhGia == reviewId && r.MaKh == maKh.Value);

                if (wishlistItem != null)
                {
                    wishlistItem.LaYeuThich = false;
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true, message = "Đã xóa khỏi yêu thích" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        // POST: /Wishlist/RemoveMultipleFromWishlist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMultipleFromWishlist([FromBody] List<int> productIds)
        {
            var maKh = HttpContext.Session.GetInt32("MaKh");
            if (maKh == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập" });

            try
            {
                var wishlistItems = await _context.DanhGiaSanPhams
                    .Where(r => r.MaKh == maKh.Value && productIds.Contains(r.MaSp) && r.LaYeuThich)
                    .ToListAsync();

                foreach (var item in wishlistItems)
                {
                    item.LaYeuThich = false;
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = $"Đã xóa {wishlistItems.Count} sản phẩm khỏi yêu thích" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        // POST: /Wishlist/CheckoutFromWishlist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutFromWishlist([FromBody] CheckoutRequestDto request)
        {
            try
            {
                var maKh = HttpContext.Session.GetInt32("MaKh");
                if (maKh == null)
                {
                    return Json(new { success = false, message = "Bạn cần đăng nhập để thanh toán." });
                }

                if (request.Items == null || !request.Items.Any())
                {
                    return Json(new { success = false, message = "Không có sản phẩm nào được chọn." });
                }

                // Kiểm tra thông tin bắt buộc
                if (string.IsNullOrWhiteSpace(request.DiaChiGiaoHang))
                {
                    return Json(new { success = false, message = "Vui lòng nhập địa chỉ giao hàng." });
                }

                if (string.IsNullOrWhiteSpace(request.SdtgiaoHang))
                {
                    return Json(new { success = false, message = "Vui lòng nhập số điện thoại." });
                }

                // Lấy danh sách productIds từ items
                var productIds = request.Items.Select(i => i.ProductId).ToList();

                // Lấy thông tin sản phẩm
                var products = await _context.SanPhams
                    .Where(p => productIds.Contains(p.MaSp))
                    .ToListAsync();

                // Kiểm tra tồn kho với số lượng
                foreach (var item in request.Items)
                {
                    var product = products.FirstOrDefault(p => p.MaSp == item.ProductId);
                    if (product == null)
                    {
                        return Json(new { success = false, message = $"Sản phẩm không tồn tại." });
                    }

                    if (product.SoLuong < item.Quantity)
                    {
                        return Json(new { success = false, message = $"Sản phẩm {product.TenSp} chỉ còn {product.SoLuong} sản phẩm." });
                    }
                }

                // Tính tổng tiền
                decimal tongTien = 0;
                decimal thanhTien = 0;
                var chiTietHoaDons = new List<ChiTietHoaDon>();

                foreach (var item in request.Items)
                {
                    var product = products.First(p => p.MaSp == item.ProductId);

                    // Tính giá sau giảm
                    decimal giaSauGiam = product.DonGia;
                    if (product.PhanTramGiam > 0)
                    {
                        giaSauGiam = product.DonGia * (1 - product.PhanTramGiam / 100);
                    }
                    giaSauGiam = Math.Round(giaSauGiam / 1000) * 1000;

                    var thanhTienItem = giaSauGiam * item.Quantity;

                    tongTien += product.DonGia * item.Quantity; // Tổng theo giá gốc
                    thanhTien += thanhTienItem; // Thành tiền theo giá sau giảm

                    // Tạo chi tiết hóa đơn
                    chiTietHoaDons.Add(new ChiTietHoaDon
                    {
                        MaSp = product.MaSp,
                        SoLuong = item.Quantity,
                        DonGia = giaSauGiam,
                        ThanhTien = thanhTienItem
                    });
                }

                // Tạo hóa đơn
                var hoaDon = new HoaDon
                {
                    MaKh = maKh.Value,
                    NgayLap = DateTime.Now,
                    TongTien = tongTien,
                    ThanhTien = thanhTien,
                    DiaChiGiaoHang = request.DiaChiGiaoHang.Trim(),
                    SdtgiaoHang = request.SdtgiaoHang.Trim(),
                    GhiChu = string.IsNullOrEmpty(request.GhiChu) ? "Thanh toán từ wishlist" : request.GhiChu.Trim(),
                    TrangThai = 0 // Chờ xác nhận
                };

                _context.HoaDons.Add(hoaDon);
                await _context.SaveChangesAsync();

                // Tạo chi tiết hóa đơn và trừ kho
                foreach (var item in request.Items)
                {
                    var product = products.First(p => p.MaSp == item.ProductId);

                    // Tính giá sau giảm
                    decimal giaSauGiam = product.DonGia;
                    if (product.PhanTramGiam > 0)
                    {
                        giaSauGiam = product.DonGia * (1 - product.PhanTramGiam / 100);
                    }
                    giaSauGiam = Math.Round(giaSauGiam / 1000) * 1000;

                    var cthd = new ChiTietHoaDon
                    {
                        MaHd = hoaDon.MaHd,
                        MaSp = product.MaSp,
                        SoLuong = item.Quantity,
                        DonGia = giaSauGiam,
                        ThanhTien = giaSauGiam * item.Quantity
                    };
                    _context.ChiTietHoaDons.Add(cthd);

                    // Trừ kho
                    product.SoLuong -= item.Quantity;
                }

                // Xóa các sản phẩm đã thanh toán khỏi wishlist
                var wishlistItems = await _context.DanhGiaSanPhams
                    .Where(r => r.MaKh == maKh.Value && productIds.Contains(r.MaSp) && r.LaYeuThich)
                    .ToListAsync();

                foreach (var item in wishlistItems)
                {
                    item.LaYeuThich = false;
                }

                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = $"Đặt hàng thành công! {request.Items.Sum(i => i.Quantity)} sản phẩm đang chờ xác nhận.",
                    redirectUrl = Url.Action("Details", "HoaDon", new { id = hoaDon.MaHd })
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi xử lý đơn hàng: " + ex.Message });
            }
        }

        // GET: Kiểm tra trạng thái wishlist của sản phẩm
        [HttpGet]
        public async Task<IActionResult> CheckWishlistStatus(int productId)
        {
            var maKh = HttpContext.Session.GetInt32("MaKh");
            if (maKh == null)
                return Json(new { isWishlist = false });

            var isWishlist = await _context.DanhGiaSanPhams
                .AnyAsync(r => r.MaSp == productId && r.MaKh == maKh.Value && r.LaYeuThich);

            return Json(new { isWishlist = isWishlist });
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlistCount()
        {
            var maKh = HttpContext.Session.GetInt32("MaKh");
            if (maKh == null)
                return Json(0);

            var count = await _context.DanhGiaSanPhams
                .CountAsync(r => r.MaKh == maKh.Value && r.LaYeuThich);

            return Json(count);
        }
    }
}