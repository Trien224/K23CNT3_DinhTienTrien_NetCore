using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NemeShop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace NemeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuanLyTapHoaContext _context;
        public HomeController(QuanLyTapHoaContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var products = await _context.SanPhams
                .Include(p => p.MaLoaiNavigation)
                .Where(p => p.TrangThai)
                .OrderByDescending(p => p.MaSp)
                .Take(24)
                .ToListAsync();

            // Lấy danh sách categories để truyền sang view
            var categories = await _context.LoaiSanPhams
                .Where(l => l.TrangThai)
                .OrderBy(l => l.TenLoai)
                .ToListAsync();

            ViewBag.Categories = categories;

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.SanPhams
                .Include(p => p.MaLoaiNavigation)
                .FirstOrDefaultAsync(p => p.MaSp == id && p.TrangThai);

            if (product == null) return NotFound();

            var giaSauGiam = product.PhanTramGiam > 0 ?
      product.DonGia * (1 - product.PhanTramGiam / 100) : product.DonGia;
            var dangGiamGia = product.PhanTramGiam > 0;
            var tietKiem = product.DonGia - giaSauGiam;

            ViewBag.GiaSauGiam = giaSauGiam;
            ViewBag.DangGiamGia = dangGiamGia;
            ViewBag.TietKiem = tietKiem;
            // Lấy đánh giá đã được duyệt (loại trừ wishlist items)
            var reviews = await _context.DanhGiaSanPhams
                .Where(r => r.MaSp == id && r.TrangThai && r.SoSao > 0) // Chỉ lấy đánh giá thực
                .OrderByDescending(r => r.NgayTao)
                .Select(r => new DanhGiaSanPhamDto
                {
                    MaDanhGia = r.MaDanhGia,
                    MaKh = r.MaKh,
                    MaSp = r.MaSp,
                    SoSao = r.SoSao,
                    NoiDung = r.NoiDung,
                    LaYeuThich = r.LaYeuThich,
                    NgayTao = r.NgayTao,
                    TrangThai = r.TrangThai,
                    TenKhachHang = r.MaKhNavigation.HoTen,
                    TenSanPham = r.MaSpNavigation.TenSp
                })
                .ToListAsync();

            // Kiểm tra xem user hiện tại có yêu thích sản phẩm này không
            var isWishlist = false;
            var maKh = HttpContext.Session.GetInt32("MaKh");
            if (maKh != null)
            {
                var wishlistItem = await _context.DanhGiaSanPhams
                    .FirstOrDefaultAsync(r => r.MaSp == id && r.MaKh == maKh.Value && r.LaYeuThich);
                isWishlist = wishlistItem != null;
            }

            ViewBag.Reviews = reviews;
            ViewBag.IsWishlist = isWishlist;

            // Tính rating trung bình (chỉ tính đánh giá thực)
            if (reviews.Any(r => r.SoSao > 0))
            {
                ViewBag.AverageRating = reviews.Where(r => r.SoSao > 0).Average(r => r.SoSao ?? 0);
            }
            else
            {
                ViewBag.AverageRating = 0;
            }

            return View(product);
        }
        [HttpGet]
        public IActionResult CheckLoginStatus()
        {
            var isLoggedIn = HttpContext.Session.GetInt32("MaKh") != null;
            return Json(new { isLoggedIn = isLoggedIn });
        }
        [HttpGet]
        public async Task<IActionResult> AddReview(int spId)
        {
            // Lấy sản phẩm để hiển thị tên
            var product = await _context.SanPhams.FindAsync(spId);
            if (product == null) return NotFound();

            var dto = new DanhGiaSanPhamDto
            {
                MaSp = spId,
                TenSanPham = product.TenSp,
                NgayTao = DateTime.Now,
                TrangThai = true
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(DanhGiaSanPhamDto dto)
        {
            // Kiểm tra đăng nhập - lấy từ session
            var maKh = HttpContext.Session.GetInt32("MaKh");
            if (maKh == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để đánh giá sản phẩm." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            var entity = new DanhGiaSanPham
            {
                MaKh = maKh.Value, // Lấy từ session
                MaSp = dto.MaSp ?? 0,
                SoSao = dto.SoSao ?? 0,
                NoiDung = dto.NoiDung?.Trim(),
                LaYeuThich = false, // Đã bỏ tính năng yêu thích
                NgayTao = DateTime.Now,
                TrangThai = true // Chờ duyệt
            };

            try
            {
                _context.DanhGiaSanPhams.Add(entity);
                await _context.SaveChangesAsync();

                // Load lại danh sách đánh giá (chỉ hiển thị những cái đã được duyệt)
                var reviews = await _context.DanhGiaSanPhams
                    .Where(r => r.MaSp == entity.MaSp && r.TrangThai) // Chỉ lấy đánh giá đã duyệt
                    .OrderByDescending(r => r.NgayTao)
                    .Select(r => new DanhGiaSanPhamDto
                    {
                        MaDanhGia = r.MaDanhGia,
                        MaKh = r.MaKh,
                        MaSp = r.MaSp,
                        SoSao = r.SoSao,
                        NoiDung = r.NoiDung,
                        LaYeuThich = r.LaYeuThich,
                        NgayTao = r.NgayTao,
                        TrangThai = r.TrangThai,
                        TenKhachHang = r.MaKhNavigation.HoTen,
                        TenSanPham = r.MaSpNavigation.TenSp
                    })
                    .ToListAsync();

                return PartialView("_ReviewListPartial", reviews);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi lưu đánh giá: " + ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> SearchProducts(string q, int? cat, decimal? min, decimal? max, string sort = "popular", int page = 1, int pageSize = 24)
        {
            var query = _context.SanPhams.Include(p => p.MaLoaiNavigation).AsQueryable();
            query = query.Where(p => p.TrangThai);

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(p => EF.Functions.Like(p.TenSp, $"%{q}%"));

            if (cat.HasValue) query = query.Where(p => p.MaLoai == cat.Value);
            if (min.HasValue) query = query.Where(p => p.DonGia >= min.Value);
            if (max.HasValue && max.Value > 0) query = query.Where(p => p.DonGia <= max.Value);

            query = sort switch
            {
                "priceAsc" => query.OrderBy(p => p.DonGia),
                "priceDesc" => query.OrderByDescending(p => p.DonGia),
                "newest" => query.OrderByDescending(p => p.MaSp),
                _ => query.OrderByDescending(p => p.PhanTramGiam)
            };

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return PartialView("_ProductGridPartial", items);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductsByCategory(int? categoryId, string sort = "newest", int page = 1, int pageSize = 24)
        {
            var query = _context.SanPhams
                .Include(p => p.MaLoaiNavigation)
                .Where(p => p.TrangThai)
                .AsQueryable();

            // Lọc theo category
            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(p => p.MaLoai == categoryId.Value);
            }

            // Sắp xếp
            query = sort switch
            {
                "priceAsc" => query.OrderBy(p => p.DonGia),
                "priceDesc" => query.OrderByDescending(p => p.DonGia),
                "name" => query.OrderBy(p => p.TenSp),
                _ => query.OrderByDescending(p => p.MaSp) // newest
            };

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return PartialView("_ProductGridPartial", products);
        }
        [HttpGet]
        public async Task<IActionResult> Category(int id)
        {
            // Lấy thông tin category
            var category = await _context.LoaiSanPhams
                .FirstOrDefaultAsync(c => c.MaLoai == id && c.TrangThai);

            if (category == null)
            {
                return NotFound();
            }

            // Lấy sản phẩm theo category
            var products = await _context.SanPhams
                .Include(p => p.MaLoaiNavigation)
                .Where(p => p.MaLoai == id && p.TrangThai)
                .OrderByDescending(p => p.MaSp)
                .ToListAsync();

            // Lấy danh sách categories để hiển thị trong filter
            var categories = await _context.LoaiSanPhams
                .Where(l => l.TrangThai)
                .OrderBy(l => l.TenLoai)
                .ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.CurrentCategory = category;
            ViewBag.CategoryId = id;

            return View("Category", products);
        }
        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
            {
                return Json(new { success = false, suggestions = new List<string>() });
            }

            try
            {
                var suggestions = await _context.SanPhams
                    .Where(p => p.TrangThai && p.TenSp.Contains(term))
                    .Select(p => new {
                        p.TenSp,
                        p.MaSp,
                        p.HinhAnh,
                        p.DonGia
                    })
                    .Take(8)
                    .ToListAsync();

                return Json(new { success = true, suggestions });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, suggestions = new List<string>() });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Search(string q, int? cat, decimal? min, decimal? max, string sort = "popular", int page = 1)
        {
            var query = _context.SanPhams.Include(p => p.MaLoaiNavigation).AsQueryable();
            query = query.Where(p => p.TrangThai);

            if (!string.IsNullOrWhiteSpace(q))
            {
                ViewBag.SearchQuery = q;
                query = query.Where(p => EF.Functions.Like(p.TenSp, $"%{q}%"));
            }

            if (cat.HasValue) query = query.Where(p => p.MaLoai == cat.Value);
            if (min.HasValue) query = query.Where(p => p.DonGia >= min.Value);
            if (max.HasValue && max.Value > 0) query = query.Where(p => p.DonGia <= max.Value);

            query = sort switch
            {
                "priceAsc" => query.OrderBy(p => p.DonGia),
                "priceDesc" => query.OrderByDescending(p => p.DonGia),
                "newest" => query.OrderByDescending(p => p.MaSp),
                _ => query.OrderByDescending(p => p.PhanTramGiam)
            };

            var products = await query.Skip((page - 1) * 24).Take(24).ToListAsync();

            // Lấy danh sách categories
            var categories = await _context.LoaiSanPhams
                .Where(l => l.TrangThai)
                .OrderBy(l => l.TenLoai)
                .ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.CurrentCategory = cat;
            ViewBag.MinPrice = min;
            ViewBag.MaxPrice = max;
            ViewBag.Sort = sort;

            return View("Index", products);
        }
    }
}
