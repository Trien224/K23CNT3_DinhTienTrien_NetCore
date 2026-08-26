using Microsoft.AspNetCore.Mvc;
using Project_02.Data;

namespace Project_02.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public HomeController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.SanPhams
                                   .Where(p => p.TrangThai == true)
                                   .ToList();
            return View(products);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var product = _context.SanPhams
                                  .FirstOrDefault(p => p.MaSp == id && p.TrangThai == true);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
