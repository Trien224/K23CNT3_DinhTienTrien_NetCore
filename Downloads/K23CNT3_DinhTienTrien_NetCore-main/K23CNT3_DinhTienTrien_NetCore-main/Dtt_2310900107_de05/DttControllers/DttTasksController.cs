using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dtt_2310900107_de05.Models;

namespace Dtt_2310900107_de05.DttControllers
{
    public class DttTasksController : Controller
    {
        private readonly Dinhtientrien2310900107De05Context _context;

        public DttTasksController(Dinhtientrien2310900107De05Context context)
        {
            _context = context;
        }

        // GET: DttTasks
        public async Task<IActionResult> DttIndex()
        {
            return View(await _context.DttTasks.ToListAsync());
        }

        // GET: DttTasks/DttDetails/5
        public async Task<IActionResult> DttDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dttTask = await _context.DttTasks
                .FirstOrDefaultAsync(m => m.DttTaskId == id);
            if (dttTask == null)
            {
                return NotFound();
            }

            return View(dttTask);
        }

        // GET: DttTasks/DttCreate
        public IActionResult DttCreate()
        {
            return View();
        }

        // POST: DttTasks/DttCreate
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more DttDetails, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DttCreate([Bind("DttTaskId,DttTaskName,DttTaskLevel,DttStartDate,DttTaskStatus")] DttTask dttTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dttTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DttIndex));
            }
            return View(dttTask);
        }

        // GET: DttTasks/DttEdit/5
        public async Task<IActionResult> DttEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dttTask = await _context.DttTasks.FindAsync(id);
            if (dttTask == null)
            {
                return NotFound();
            }
            return View(dttTask);
        }

        // POST: DttTasks/DttEdit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more DttDetails, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DttEdit(int id, [Bind("DttTaskId,DttTaskName,DttTaskLevel,DttStartDate,DttTaskStatus")] DttTask dttTask)
        {
            if (id != dttTask.DttTaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dttTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DttTaskExists(dttTask.DttTaskId))
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
            return View(dttTask);
        }

        // GET: DttTasks/DttDelete/5
        public async Task<IActionResult> DttDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dttTask = await _context.DttTasks
                .FirstOrDefaultAsync(m => m.DttTaskId == id);
            if (dttTask == null)
            {
                return NotFound();
            }

            return View(dttTask);
        }

        // POST: DttTasks/DttDelete/5
        [HttpPost, ActionName("DttDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DttDeleteConfirmed(int id)
        {
            var dttTask = await _context.DttTasks.FindAsync(id);
            if (dttTask != null)
            {
                _context.DttTasks.Remove(dttTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DttIndex));
        }

        private bool DttTaskExists(int id)
        {
            return _context.DttTasks.Any(e => e.DttTaskId == id);
        }
    }
}
