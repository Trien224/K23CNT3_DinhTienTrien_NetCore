using System.Diagnostics;
using System.Runtime.Serialization.DataContracts;
using DttLesson05.Models;
using DttLesson05.Models.DataModels;
using Microsoft.AspNetCore.Mvc;
namespace DttLesson05.Controllers
{
    public class DttMemberController : Controller
    {
        public IActionResult DttGetMember()
        {
            var members = new List<DttLesson05.Models.DataModels.DttMember>
    {
        new DttLesson05.Models.DataModels.DttMember
        {
            DttMemberId ="2310900107",
            DttUsername = "Triển",
            DttFullName = "Đinh Tiến Triển",
            DttPassword = "Trien2005",
            DttEmail = "Tientriendinh244@mail.com"
        },
        new DttLesson05.Models.DataModels.DttMember
        {
            DttMemberId = Guid.NewGuid().ToString(),
            DttUsername = "user2",
            DttFullName = "Bob Smith",
            DttPassword = "password2",
            DttEmail = "bob@example.com"
        },
        new DttLesson05.Models.DataModels.DttMember
        {
            DttMemberId = Guid.NewGuid().ToString(),
            DttUsername = "user3",
            DttFullName = "Charlie Brown",
            DttPassword = "password3",
            DttEmail = "charlie@example.com"
        },
        new DttLesson05.Models.DataModels.DttMember
        {
            DttMemberId = Guid.NewGuid().ToString(),
            DttUsername = "user4",
            DttFullName = "Diana Prince",
            DttPassword = "password4",
            DttEmail = "diana@example.com"
        },
        new DttLesson05.Models.DataModels.DttMember
        {
            DttMemberId = Guid.NewGuid().ToString(),
            DttUsername = "user5",
            DttFullName = "Ethan Hunt",
            DttPassword = "password5",
            DttEmail = "ethan@example.com"
        }
    };

            return View(members);
        }

    }

}   
