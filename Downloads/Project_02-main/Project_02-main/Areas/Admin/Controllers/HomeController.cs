using Microsoft.AspNetCore.Mvc;
using Project_02.Data;
using Project_02.Models;
using System.Linq;

namespace Project_02.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public HomeController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new
            {
                TongKhachHang = _context.KhachHangs.Count(),
                TongSanPham = _context.SanPhams.Count(),
                TongHoaDon = _context.HoaDons.Count(),
                DoanhThu = _context.HoaDons.Sum(h => (decimal?)h.ThanhTien) ?? 0M
            };

            return View(model);
        }
    }
}
