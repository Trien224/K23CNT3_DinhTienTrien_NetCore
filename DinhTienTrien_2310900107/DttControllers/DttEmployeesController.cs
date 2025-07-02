using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DinhTienTrien_2310900107.Models;

namespace DinhTienTrien_2310900107.Controllers
{
    public class DttEmployeesController : Controller
    {
        private readonly DinhTienTrien2310900107Context _context;

        public DttEmployeesController(DinhTienTrien2310900107Context context)
        {
            _context = context;
        }

        // GET: DttEmployees
        public async Task<IActionResult> DttIndex()
        {
            return View(await _context.DttEmployees.ToListAsync());
        }

        // GET: DttEmployees/Details/5
        public async Task<IActionResult> DttDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dttEmployee = await _context.DttEmployees
                .FirstOrDefaultAsync(m => m.DttEmpId == id);
            if (dttEmployee == null)
            {
                return NotFound();
            }

            return View(dttEmployee);
        }

        // GET: DttEmployees/Create
        public IActionResult DttCreate()
        {
            return View();
        }

        // POST: DttEmployees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DttCreate([Bind("DttEmpId,DttEmpName,DttEmpLevel,DttStartDate,DttEmpStatus")] DttEmployee dttEmployee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dttEmployee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DttIndex));
            }
            return View(dttEmployee);
        }

        // GET: DttEmployees/Edit/5
        public async Task<IActionResult> DttEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dttEmployee = await _context.DttEmployees.FindAsync(id);
            if (dttEmployee == null)
            {
                return NotFound();
            }
            return View(dttEmployee);
        }

        // POST: DttEmployees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DttEdit(int id, [Bind("DttEmpId,DttEmpName,DttEmpLevel,DttStartDate,DttEmpStatus")] DttEmployee dttEmployee)
        {
            if (id != dttEmployee.DttEmpId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dttEmployee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DttEmployeeExists(dttEmployee.DttEmpId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DttIndex));
            }
            return View(dttEmployee);
        }

        // GET: DttEmployees/Delete/5
        public async Task<IActionResult> DttDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dttEmployee = await _context.DttEmployees
                .FirstOrDefaultAsync(m => m.DttEmpId == id);
            if (dttEmployee == null)
            {
                return NotFound();
            }

            return View(dttEmployee);
        }

        // POST: DttEmployees/Delete/5
        [HttpPost, ActionName("DttDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dttEmployee = await _context.DttEmployees.FindAsync(id);
            if (dttEmployee != null)
            {
                _context.DttEmployees.Remove(dttEmployee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DttIndex));
        }

        private bool DttEmployeeExists(int id)
        {
            return _context.DttEmployees.Any(e => e.DttEmpId == id);
        }
    }
}
