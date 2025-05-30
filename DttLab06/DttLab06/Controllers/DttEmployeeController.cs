using Microsoft.AspNetCore.Mvc;
using DttLab06.Models;

namespace DttLab06.Controllers
{
    public class DttEmployeeController : Controller
    {
        public static List<DttEmployee> dttListEmployee = new List<DttEmployee>
        {
            new DttEmployee { DttId = 23109107, DttName = "Đinh Tiến Triển", DttBirthDay = new DateTime(2005,4,12), DttEmail = "Dttstudent@example.com", DttPhone = "0123456789", DttSalary = 1000, DttStatus = "Đang làm" },
            new DttEmployee { DttId = 2, DttName = "Trần Thị B", DttBirthDay = new DateTime(1995,2,2), DttEmail = "b@example.com", DttPhone = "0223456789", DttSalary = 2000, DttStatus = "Nghỉ phép" },
            new DttEmployee { DttId = 3, DttName = "Lê Văn C", DttBirthDay = new DateTime(1990,3,3), DttEmail = "c@example.com", DttPhone = "0323456789", DttSalary = 2500, DttStatus = "Đang làm" },
            new DttEmployee { DttId = 4, DttName = "Phạm Thị D", DttBirthDay = new DateTime(1985,4,4), DttEmail = "d@example.com", DttPhone = "0423456789", DttSalary = 3000, DttStatus = "Thử việc" },
            new DttEmployee { DttId = 5, DttName = "Hoàng Văn E", DttBirthDay = new DateTime(1992,5,5), DttEmail = "e@example.com", DttPhone = "0523456789", DttSalary = 3500, DttStatus = "Đã nghỉ" }
        };

        public IActionResult DttIndex()
        {
            return View(dttListEmployee);
        }

        public IActionResult DttCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DttCreateSubmit(DttEmployee emp)
        {
            emp.DttId = dttListEmployee.Max(e => e.DttId) + 1;
            dttListEmployee.Add(emp);
            return RedirectToAction("DttIndex");
        }

        public IActionResult DttEdit(int id)
        {
            var emp = dttListEmployee.FirstOrDefault(e => e.DttId == id);
            return View(emp);
        }

        [HttpPost]
        public IActionResult DttEdit(DttEmployee emp)
        {
            var existing = dttListEmployee.FirstOrDefault(e => e.DttId == emp.DttId);
            if (existing != null)
            {
                existing.DttName = emp.DttName;
                existing.DttBirthDay = emp.DttBirthDay;
                existing.DttEmail = emp.DttEmail;
                existing.DttPhone = emp.DttPhone;
                existing.DttSalary = emp.DttSalary;
                existing.DttStatus = emp.DttStatus;
            }
            return RedirectToAction("DttIndex");
        }


        public IActionResult DttDelete(int id)
        {
            var emp = dttListEmployee.FirstOrDefault(e => e.DttId == id);
            if (emp != null) dttListEmployee.Remove(emp);
            return RedirectToAction("DttIndex");
        }
    }
}
