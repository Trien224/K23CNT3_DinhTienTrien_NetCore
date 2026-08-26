using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_02.Data;
using Project_02.Models;

namespace Project_02.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamsController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public SanPhamsController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // GET: Admin/SanPhams
        public async Task<IActionResult> Index()
        {
            var quanLyTapHoaContext = _context.SanPhams.Include(s => s.MaLoaiNavigation);
            return View(await quanLyTapHoaContext.ToListAsync());
        }

        // GET: Admin/SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaLoaiNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
        public IActionResult Create()
        {
            ViewData["MaLoai"] = new SelectList(_context.LoaiSanPhams, "MaLoai", "TenLoai");
            return View();
        }

        // ✅ POST: Admin/SanPhams/Create (Upload ảnh)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPham sanPham)
        {
            if (sanPham.UploadImage != null)
            {
                // 🗂️ Tạo thư mục lưu ảnh
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "sanpham");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // 📸 Đặt tên file duy nhất (tránh trùng)
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(sanPham.UploadImage.FileName);
                string filePath = Path.Combine(uploadDir, fileName);

                // 💾 Lưu file ảnh lên server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await sanPham.UploadImage.CopyToAsync(stream);
                }

                // 🧩 Lưu tên file vào DB
                sanPham.HinhAnh = fileName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                TempData["Success"] = "✅ Thêm sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaLoai"] = new SelectList(_context.LoaiSanPhams, "MaLoai", "TenLoai", sanPham.MaLoai);
            return View(sanPham);
        }


        // GET: Admin/SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["MaLoai"] = new SelectList(_context.LoaiSanPhams, "MaLoai", "MaLoai", sanPham.MaLoai);
            return View(sanPham);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SanPham sanPham)
        {
            if (id != sanPham.MaSp)
            {
                return NotFound();
            }

            var existingSp = await _context.SanPhams.FindAsync(id);
            if (existingSp == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin cơ bản
                    existingSp.TenSp = sanPham.TenSp;
                    existingSp.MoTa = sanPham.MoTa;
                    existingSp.DonGia = sanPham.DonGia;
                    existingSp.SoLuong = sanPham.SoLuong;
                    existingSp.MaLoai = sanPham.MaLoai;
                    existingSp.TrangThai = sanPham.TrangThai;
                    existingSp.PhanTramGiam = sanPham.PhanTramGiam;

                    // 🖼️ Nếu có upload ảnh mới
                    if (sanPham.UploadImage != null)
                    {
                        string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "sanpham");

                        // Xóa ảnh cũ nếu tồn tại
                        if (!string.IsNullOrEmpty(existingSp.HinhAnh))
                        {
                            string oldFilePath = Path.Combine(uploadDir, existingSp.HinhAnh);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Lưu ảnh mới
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(sanPham.UploadImage.FileName);
                        string filePath = Path.Combine(uploadDir, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await sanPham.UploadImage.CopyToAsync(stream);
                        }

                        existingSp.HinhAnh = fileName; // cập nhật tên file mới
                    }

                    _context.Update(existingSp);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "✅ Cập nhật sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "❌ Lỗi khi cập nhật sản phẩm: " + ex.Message;
                }
            }

            ViewData["MaLoai"] = new SelectList(_context.LoaiSanPhams, "MaLoai", "TenLoai", sanPham.MaLoai);
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaLoaiNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sp = await _context.SanPhams.FindAsync(id);

            if (sp == null)
                return NotFound();

            try
            {
                _context.SanPhams.Remove(sp);
                await _context.SaveChangesAsync();

                TempData["Success"] = "✅ Xóa sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "❌ Không thể xóa sản phẩm này vì còn đánh giá hoặc hóa đơn liên quan!";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }


        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.MaSp == id);
        }
    }
}
