using Microsoft.AspNetCore.Mvc;
using Project_02.Data;
using System.Linq;

namespace Project_02.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public ProductController(QuanLyTapHoaContext context)
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

        public IActionResult Details(int id)
        {
            var product = _context.SanPhams.FirstOrDefault(p => p.MaSp == id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}
