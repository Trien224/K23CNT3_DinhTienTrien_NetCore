using System.Diagnostics;
using Dtt_2310900107_de05.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dtt_2310900107_de05.Controllers
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

        public IActionResult DttPrivacy()
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
