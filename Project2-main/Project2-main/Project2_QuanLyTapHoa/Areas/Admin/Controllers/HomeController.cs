using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2_QuanLyTapHoa.Filters;
using Project2_QuanLyTapHoa.Models;
using System.Diagnostics;

namespace Project2_QuanLyTapHoa.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeAdmin]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QuanLyTapHoaContext _context;

        public HomeController(ILogger<HomeController> logger, QuanLyTapHoaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalSanPham = await _context.SanPhams.CountAsync();
            ViewBag.TotalKhachHang = await _context.KhachHangs.CountAsync();
            ViewBag.TotalHoaDon = await _context.HoaDons.CountAsync();
            ViewBag.TotalQTV = await _context.QuanTriViens.CountAsync();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(model);
        }
    }
}
