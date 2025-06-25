using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lesson10.Models;

namespace Lesson10.DttControllers
{
    public class DttLessonsController : Controller
    {
        private readonly DttK23cnt3Lesson10dbContext _context;

        public DttLessonsController(DttK23cnt3Lesson10dbContext context)
        {
            _context = context;
        }

        // GET: DttLessons
        public async Task<IActionResult> DttIndex()
        {
            return View(await _context.DttLessons.ToListAsync());
        }

        // GET: DttLessons/DttDetails/5
        public async Task<IActionResult> DttDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dttLesson = await _context.DttLessons.FirstOrDefaultAsync(m => m.DttId == id);
            if (dttLesson == null)
            {
                return NotFound();
            }

            return View(dttLesson);
        }

        // GET: DttLessons/DttCreate
        public IActionResult DttCreate()
        {
            return View();
        }

        // POST: DttLessons/DttCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DttCreate(DttLesson dttLesson, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                dttLesson.DttImage = "images/" + fileName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(dttLesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DttIndex));
            }

            return View(dttLesson);
        }

        // GET: DttLessons/DttEdit/5
        public async Task<IActionResult> DttEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dttLesson = await _context.DttLessons.FindAsync(id);
            if (dttLesson == null)
            {
                return NotFound();
            }
            return View(dttLesson);
        }

        // POST: DttLessons/DttEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DttEdit(int id, DttLesson dttLesson, IFormFile ImageFile)
        {
            if (id != dttLesson.DttId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingLesson = await _context.DttLessons.AsNoTracking().FirstOrDefaultAsync(x => x.DttId == id);
                    if (existingLesson == null)
                    {
                        return NotFound();
                    }

                    // Nếu có ảnh mới được chọn => xử lý upload ảnh mới
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(ImageFile.FileName);
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        var filePath = Path.Combine(folderPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        dttLesson.DttImage = "images/" + fileName;
                    }
                    else
                    {
                        // Nếu không chọn ảnh mới => giữ nguyên ảnh cũ
                        dttLesson.DttImage = existingLesson.DttImage;
                    }

                    _context.Update(dttLesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DttLessonExists(dttLesson.DttId))
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

            return View(dttLesson);
        }

        // GET: DttLessons/DttDelete/5
        public async Task<IActionResult> DttDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dttLesson = await _context.DttLessons.FirstOrDefaultAsync(m => m.DttId == id);
            if (dttLesson == null)
            {
                return NotFound();
            }

            return View(dttLesson);
        }

        // POST: DttLessons/DttDelete/5
        [HttpPost, ActionName("DttDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DttDeleteConfirmed(int id)
        {
            var dttLesson = await _context.DttLessons.FindAsync(id);
            if (dttLesson != null)
            {
                _context.DttLessons.Remove(dttLesson);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(DttIndex));
        }

        private bool DttLessonExists(int id)
        {
            return _context.DttLessons.Any(e => e.DttId == id);
        }
    }
}
