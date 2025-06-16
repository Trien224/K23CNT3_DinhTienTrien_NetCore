using System.Diagnostics;
using Lesson09.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lesson09.Controllers
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

        public IActionResult DttAoubt()
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
