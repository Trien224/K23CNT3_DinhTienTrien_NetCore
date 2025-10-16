using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neme.Filters;
using Neme.Models;
using System.Diagnostics;

namespace Neme.Areas.Admin.Controllers
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

        // Dashboard Index
        public async Task<IActionResult> Index()
        {
            var totalSanPhamTask = _context.SanPhams.CountAsync();
            var totalKhachHangTask = _context.KhachHangs.CountAsync();
            var totalHoaDonTask = _context.HoaDons.CountAsync();
            var totalQTVTask = _context.QuanTriViens.CountAsync();

            await Task.WhenAll(totalSanPhamTask, totalKhachHangTask, totalHoaDonTask, totalQTVTask);

            var model = new DashboardViewModel
            {
                TotalSanPham = totalSanPhamTask.Result,
                TotalKhachHang = totalKhachHangTask.Result,
                TotalHoaDon = totalHoaDonTask.Result,
                TotalQTV = totalQTVTask.Result
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            _logger.LogError("Lỗi xảy ra với RequestId: {RequestId}", requestId);

            var model = new ErrorViewModel { RequestId = requestId };
            return View(model);
        }
    }

    // ViewModel cho Dashboard
    public class DashboardViewModel
    {
        public int TotalSanPham { get; set; }
        public int TotalKhachHang { get; set; }
        public int TotalHoaDon { get; set; }
        public int TotalQTV { get; set; }
    }
}
