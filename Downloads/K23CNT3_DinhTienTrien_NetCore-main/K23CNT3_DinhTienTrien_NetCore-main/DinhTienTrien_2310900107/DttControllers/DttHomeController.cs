using System.Diagnostics;
using DinhTienTrien_2310900107.Models;
using Microsoft.AspNetCore.Mvc;

namespace DinhTienTrien_2310900107.Controllers
{
    public class DttHomeController : Controller
    {
        private readonly ILogger<DttHomeController> _logger;

        public DttHomeController(ILogger<DttHomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult DttIndex()
        {
            return View();
        }

        public IActionResult DttAbout()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
