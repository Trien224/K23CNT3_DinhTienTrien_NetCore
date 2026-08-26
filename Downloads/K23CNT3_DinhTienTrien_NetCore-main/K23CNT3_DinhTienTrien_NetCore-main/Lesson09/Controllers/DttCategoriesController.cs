using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lesson09.Models;

namespace Lesson09.Controllers
{
    public class DttCategoriesController : Controller
    {
        private readonly DttBookStoreContext _context;

        public DttCategoriesController(DttBookStoreContext context)
        {
            _context = context;
        }

        // GET: DttCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.DttCategories.ToListAsync());
        }

        // GET: DttCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dttCategory = await _context.DttCategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (dttCategory == null)
            {
                return NotFound();
            }

            return View(dttCategory);
        }

        // GET: DttCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DttCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] DttCategory dttCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dttCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dttCategory);
        }

        // GET: DttCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dttCategory = await _context.DttCategories.FindAsync(id);
            if (dttCategory == null)
            {
                return NotFound();
            }
            return View(dttCategory);
        }

        // POST: DttCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] DttCategory dttCategory)
        {
            if (id != dttCategory.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dttCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DttCategoryExists(dttCategory.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dttCategory);
        }

        // GET: DttCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dttCategory = await _context.DttCategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (dttCategory == null)
            {
                return NotFound();
            }

            return View(dttCategory);
        }

        // POST: DttCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dttCategory = await _context.DttCategories.FindAsync(id);
            if (dttCategory != null)
            {
                _context.DttCategories.Remove(dttCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DttCategoryExists(int id)
        {
            return _context.DttCategories.Any(e => e.CategoryId == id);
        }
    }
}
