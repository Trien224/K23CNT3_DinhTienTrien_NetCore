using System.Diagnostics;
using DttLesson05.Models;
using DttLesson05.Models.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace DttLesson05.Controllers
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
            var dttMember = new DttMember();

            dttMember.DttMemberId = Guid.NewGuid().ToString();
            dttMember.DttUsername = " Triển";
            dttMember.DttFullName = "Đinh Tiến Triển";
            dttMember.DttPassword = "123098 ";
            dttMember.DttEmail = "dinhtientrien@gmail.com";
            ViewBag.dttmember = dttMember;

            return View(dttMember);
        }
        



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
