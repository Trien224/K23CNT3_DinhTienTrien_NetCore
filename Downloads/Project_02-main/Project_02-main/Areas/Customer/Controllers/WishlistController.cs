    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Project_02.Models;
    using Project_02.Data;

    namespace Project_02.Areas.Customer.Controllers
    {
        [Area("Customer")]
        public class WishlistController : Controller
        {
            private readonly QuanLyTapHoaContext _context;

            public WishlistController(QuanLyTapHoaContext context)
            {
                _context = context;
            }

            // ✅ Hiển thị danh sách sản phẩm yêu thích
            [HttpGet]
            public async Task<IActionResult> MyWishlist()
            {
                var maKh = HttpContext.Session.GetInt32("MaKh");
                if (maKh == null)
                    return RedirectToAction("Login", "Account");

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

            // ✅ Thêm hoặc gỡ sản phẩm khỏi yêu thích (AJAX)
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> ToggleWishlist(int productId)
            {
                try
                {
                    var maKh = HttpContext.Session.GetInt32("MaKh");
                    if (maKh == null)
                    {
                        return Json(new { success = false, message = "Vui lòng đăng nhập để thêm vào yêu thích", isWishlist = false });
                    }

                    var productExists = await _context.SanPhams.AnyAsync(p => p.MaSp == productId);
                    if (!productExists)
                    {
                        return Json(new { success = false, message = "Sản phẩm không tồn tại", isWishlist = false });
                    }

                    var existingReview = await _context.DanhGiaSanPhams
                        .FirstOrDefaultAsync(r => r.MaSp == productId && r.MaKh == maKh.Value);

                    bool isWishlist;
                    string message;

                    if (existingReview == null)
                    {
                        _context.DanhGiaSanPhams.Add(new DanhGiaSanPham
                        {
                            MaKh = maKh.Value,
                            MaSp = productId,
                            SoSao = 5,
                            NoiDung = "Yêu thích",
                            LaYeuThich = true,
                            NgayTao = DateTime.Now,
                            TrangThai = true
                        });

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
                    return Json(new { success = true, isWishlist, message });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ ERROR: {ex.Message}");
                    return Json(new { success = false, message = "Tính năng đang bảo trì", isWishlist = false });
                }
            }

            // ✅ Xóa 1 sản phẩm khỏi yêu thích
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> RemoveFromWishlist(int reviewId)
            {
                var maKh = HttpContext.Session.GetInt32("MaKh");
                if (maKh == null)
                    return Json(new { success = false, message = "Vui lòng đăng nhập" });

                var wishlistItem = await _context.DanhGiaSanPhams
                    .FirstOrDefaultAsync(r => r.MaDanhGia == reviewId && r.MaKh == maKh.Value);

                if (wishlistItem != null)
                {
                    wishlistItem.LaYeuThich = false;
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true, message = "Đã xóa khỏi yêu thích" });
            }

            // ✅ Xóa nhiều sản phẩm cùng lúc
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> RemoveMultipleFromWishlist([FromBody] List<int> productIds)
            {
                var maKh = HttpContext.Session.GetInt32("MaKh");
                if (maKh == null)
                    return Json(new { success = false, message = "Vui lòng đăng nhập" });

                var wishlistItems = await _context.DanhGiaSanPhams
                    .Where(r => r.MaKh == maKh.Value && productIds.Contains(r.MaSp) && r.LaYeuThich)
                    .ToListAsync();

                foreach (var item in wishlistItems)
                    item.LaYeuThich = false;

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = $"Đã xóa {wishlistItems.Count} sản phẩm khỏi yêu thích" });
            }

            // ✅ Kiểm tra trạng thái yêu thích của sản phẩm
            [HttpGet]
            public async Task<IActionResult> CheckWishlistStatus(int productId)
            {
                var maKh = HttpContext.Session.GetInt32("MaKh");
                if (maKh == null)
                    return Json(new { isWishlist = false });

                var isWishlist = await _context.DanhGiaSanPhams
                    .AnyAsync(r => r.MaSp == productId && r.MaKh == maKh.Value && r.LaYeuThich);

                return Json(new { isWishlist });
            }

            // ✅ Đếm tổng số sản phẩm trong wishlist
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
