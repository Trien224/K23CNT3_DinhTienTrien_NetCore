using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NemeChuaShop.Models;

namespace NemeChuaShop.Controllers
{
    public class LoaiSanPhamsController : Controller
    {
        private readonly NemeChuaShopContext _context;

        public LoaiSanPhamsController(NemeChuaShopContext context)
        {
            _context = context;
        }

        // GET: LoaiSanPhams
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoaiSanPhams.Include(l => l.SanPhams).ToListAsync());
        }

        // GET: LoaiSanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiSanPham = await _context.LoaiSanPhams
                .Include(l => l.SanPhams)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }

            return View(loaiSanPham);
        }

        // GET: LoaiSanPhams/Create
        public IActionResult Create()
        {
            return View(new LoaiSanPham { TrangThai = true });
        }

        // POST: LoaiSanPhams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaLoai,TenLoai,TrangThai")] LoaiSanPham loaiSanPham)
        {
            if (string.IsNullOrWhiteSpace(loaiSanPham.MaLoai))
            {
                ModelState.AddModelError("MaLoai", "Mã loại không được để trống.");
            }
            else if (await _context.LoaiSanPhams.AnyAsync(l => l.MaLoai == loaiSanPham.MaLoai))
            {
                ModelState.AddModelError("MaLoai", "Mã loại này đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                loaiSanPham.NgayTao = DateTime.Now;
                loaiSanPham.NgayCapNhat = DateTime.Now;
                _context.Add(loaiSanPham);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm loại sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(loaiSanPham);
        }

        // GET: LoaiSanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }
            return View(loaiSanPham);
        }

        // POST: LoaiSanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaLoai,TenLoai,TrangThai")] LoaiSanPham loaiSanPham)
        {
            if (id != loaiSanPham.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.LoaiSanPhams.FindAsync(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    existing.TenLoai = loaiSanPham.TenLoai;
                    existing.TrangThai = loaiSanPham.TrangThai;
                    existing.NgayCapNhat = DateTime.Now;

                    _context.Update(existing);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật loại sản phẩm thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiSanPhamExists(loaiSanPham.Id))
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
            return View(loaiSanPham);
        }

        // GET: LoaiSanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiSanPham = await _context.LoaiSanPhams
                .Include(l => l.SanPhams)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }

            return View(loaiSanPham);
        }

        // POST: LoaiSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiSanPham = await _context.LoaiSanPhams.Include(l => l.SanPhams).FirstOrDefaultAsync(l => l.Id == id);
            if (loaiSanPham != null)
            {
                if (loaiSanPham.SanPhams.Any())
                {
                    TempData["ErrorMessage"] = "Không thể xóa loại sản phẩm này vì vẫn còn sản phẩm thuộc loại này!";
                    return RedirectToAction(nameof(Index));
                }

                _context.LoaiSanPhams.Remove(loaiSanPham);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa loại sản phẩm thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LoaiSanPhamExists(int id)
        {
            return _context.LoaiSanPhams.Any(e => e.Id == id);
        }
    }
}
