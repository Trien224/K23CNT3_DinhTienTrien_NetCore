using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project2_QuanLyTapHoa.Filters;
using Project2_QuanLyTapHoa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_QuanLyTapHoa.Areas.Admin.Controllers
{
    [Area("Admin")] // Bắt buộc để MVC biết controller này thuộc Admin area
    [AuthorizeAdmin] // Kiểm tra quyền Admin
    public class QuanTriViensController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public QuanTriViensController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // GET: Admin/QuanTriViens
        public async Task<IActionResult> Index()
        {
            return View(await _context.QuanTriViens.ToListAsync());
        }

        // GET: Admin/QuanTriViens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var quanTriVien = await _context.QuanTriViens
                .FirstOrDefaultAsync(m => m.MaQtv == id);

            if (quanTriVien == null)
                return NotFound();

            return View(quanTriVien);
        }

        // GET: Admin/QuanTriViens/Create
        public IActionResult Create()
        {
            var model = new QuanTriVien
            {
                TrangThai = true // mặc định là Active
            };
            return View(model);
        }

        // POST: Admin/QuanTriViens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaQtv,HoTen,Email,MatKhau,DienThoai,DiaChi,TrangThai")] QuanTriVien quanTriVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quanTriVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quanTriVien);
        }

        // GET: Admin/QuanTriViens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var quanTriVien = await _context.QuanTriViens.FindAsync(id);
            if (quanTriVien == null)
                return NotFound();

            return View(quanTriVien);
        }

        // POST: Admin/QuanTriViens/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaQtv,HoTen,Email,MatKhau,DienThoai,DiaChi,TrangThai")] QuanTriVien quanTriVien)
        {
            if (id != quanTriVien.MaQtv)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quanTriVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuanTriVienExists(quanTriVien.MaQtv))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(quanTriVien);
        }

        // GET: Admin/QuanTriViens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var quanTriVien = await _context.QuanTriViens
                .FirstOrDefaultAsync(m => m.MaQtv == id);

            if (quanTriVien == null)
                return NotFound();

            return View(quanTriVien);
        }

        // POST: Admin/QuanTriViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quanTriVien = await _context.QuanTriViens.FindAsync(id);
            if (quanTriVien != null)
            {
                _context.QuanTriViens.Remove(quanTriVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuanTriVienExists(int id)
        {
            return _context.QuanTriViens.Any(e => e.MaQtv == id);
        }
    }
}
