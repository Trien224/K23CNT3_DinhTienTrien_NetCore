using DttLesson02.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DttLesson02.Controllers
{
    public class DttAccountController : Controller
    {
        private static List<DttAccount> DttAccounts = new List<DttAccount>
        {
            new DttAccount
            {
                Id = 1,
                Name = "Dinh tien Trien",
                Email = "Tientrien224@gmail.com",
                Phone = "0968212516",
                Address = "Ninh binh",
                Avatar = "/images/Avatar/01.png",
                Gender = 1,
                Bio = "Hi everyone",
                Birthday = new DateTime(2005, 4, 12)
            },
            new DttAccount
            {
                Id = 2,
                Name = "Nguyen Van A",
                Email = "nguyenvana@example.com",
                Phone = "0912345678",
                Address = "Hanoi",
                Avatar = "/images/Avatar/02.png",
                Gender = 1,
                Bio = "Welcome!",
                Birthday = new DateTime(2000, 1, 1)
            }
        };

        public IActionResult DttIndex()
        {
            ViewBag.DttAccounts = DttAccounts;
            return View();
        }

        public IActionResult Details(int id)
        {
            var account = DttAccounts.FirstOrDefault(a => a.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }
    }
}
