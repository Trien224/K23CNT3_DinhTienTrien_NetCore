using System.Diagnostics;
using DttLesson07.Models;
using Microsoft.AspNetCore.Mvc;

namespace DttLesson07.Controllers
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
            ViewBag.Name = "Đinh Tiến Triển";
            ViewBag.MSSV = "2310900107";
            ViewBag.Email = "dtt@student.university.edu";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
