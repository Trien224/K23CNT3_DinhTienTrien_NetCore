using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2_QuanLyTapHoa.Models;

namespace Project2_QuanLyTapHoa.Controllers
{
    public class ShopController : Controller
    {
        private readonly QuanLyTapHoaContext _context;
        public ShopController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // Trang shop chính
        public IActionResult Index()
        {
            return View();
        }

        // API trả về danh sách sản phẩm đang bán
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.SanPhams
                .Where(p => p.TrangThai) // chỉ hiện sản phẩm đang bán
                .Select(p => new {
                    p.MaSp,
                    p.TenSp,
                    p.DonGia,
                    p.HinhAnh,
                    p.MoTa,
                    p.MaLoai
                })
                .ToListAsync();

            return Json(products);
        }
        public IActionResult Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return View(new List<SanPham>());
        }

        // Tìm sản phẩm chứa từ khóa
        var results = _context.SanPhams
                              .Where(p => p.TenSp.Contains(query))
                              .OrderBy(p => p.TenSp)
                              .ToList();

        return View(results);
    }
        // Thanh toán (tạm thời chỉ return success)
        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] List<CartItem> cart)
        {
            // TODO: Xử lý đơn hàng, lưu vào DB
            return Json(new { success = true });
        }
    }

    // Class cho giỏ hàng JS gửi về
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
    }
}
