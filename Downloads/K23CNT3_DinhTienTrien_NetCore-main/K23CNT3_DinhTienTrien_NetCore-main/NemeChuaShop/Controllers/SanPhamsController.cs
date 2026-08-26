using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NemeChuaShop.Models;

namespace NemeChuaShop.Controllers
{
    public class SanPhamsController : Controller
    {
        private readonly NemeChuaShopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SanPhamsController(NemeChuaShopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: SanPhams
        public async Task<IActionResult> Index(string? searchString, string? maLoai)
        {
            var query = _context.SanPhams
                .Include(s => s.MaLoaiNavigation)
                .Include(s => s.MaNguoiBanNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.TenSanPham.Contains(searchString) || s.MaSanPham.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(maLoai))
            {
                query = query.Where(s => s.MaLoai == maLoai);
            }

            ViewBag.Categories = new SelectList(await _context.LoaiSanPhams.Where(l => l.TrangThai).ToListAsync(), "MaLoai", "TenLoai", maLoai);
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentCategory = maLoai;

            return View(await query.OrderByDescending(s => s.Id).ToListAsync());
        }

        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaLoaiNavigation)
                .Include(s => s.MaNguoiBanNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: SanPhams/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.MaLoai = new SelectList(await _context.LoaiSanPhams.Where(l => l.TrangThai).ToListAsync(), "MaLoai", "TenLoai");
            ViewBag.MaNguoiBan = new SelectList(await _context.NguoiBans.ToListAsync(), "MaNguoiBan", "MaNguoiBan");
            return View(new SanPham { TrangThai = true, SoLuong = 10, DonGia = 50000 });
        }

        // POST: SanPhams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaSanPham,TenSanPham,SoLuong,DonGia,MaLoai,MaNguoiBan,TrangThai")] SanPham sanPham, IFormFile? uploadHinhAnh)
        {
            if (string.IsNullOrWhiteSpace(sanPham.MaSanPham))
            {
                ModelState.AddModelError("MaSanPham", "Vui lòng nhập mã sản phẩm.");
            }
            else if (await _context.SanPhams.AnyAsync(s => s.MaSanPham == sanPham.MaSanPham))
            {
                ModelState.AddModelError("MaSanPham", "Mã sản phẩm này đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                if (uploadHinhAnh != null && uploadHinhAnh.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString().Substring(0, 8) + "_" + Path.GetFileName(uploadHinhAnh.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadHinhAnh.CopyToAsync(fileStream);
                    }

                    sanPham.HinhAnh = "images/products/" + uniqueFileName;
                }
                else
                {
                    sanPham.HinhAnh = "images/ban3.png"; // Default fallback
                }

                sanPham.NgayTao = DateTime.Now;
                sanPham.NgayCapNhat = DateTime.Now;

                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm sản phẩm mới thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MaLoai = new SelectList(await _context.LoaiSanPhams.Where(l => l.TrangThai).ToListAsync(), "MaLoai", "TenLoai", sanPham.MaLoai);
            ViewBag.MaNguoiBan = new SelectList(await _context.NguoiBans.ToListAsync(), "MaNguoiBan", "MaNguoiBan", sanPham.MaNguoiBan);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
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

            ViewBag.MaLoai = new SelectList(await _context.LoaiSanPhams.Where(l => l.TrangThai).ToListAsync(), "MaLoai", "TenLoai", sanPham.MaLoai);
            ViewBag.MaNguoiBan = new SelectList(await _context.NguoiBans.ToListAsync(), "MaNguoiBan", "MaNguoiBan", sanPham.MaNguoiBan);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaSanPham,TenSanPham,HinhAnh,SoLuong,DonGia,MaLoai,MaNguoiBan,TrangThai")] SanPham sanPham, IFormFile? uploadHinhAnh)
        {
            if (id != sanPham.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.SanPhams.FindAsync(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    if (uploadHinhAnh != null && uploadHinhAnh.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString().Substring(0, 8) + "_" + Path.GetFileName(uploadHinhAnh.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await uploadHinhAnh.CopyToAsync(fileStream);
                        }

                        existing.HinhAnh = "images/products/" + uniqueFileName;
                    }

                    existing.TenSanPham = sanPham.TenSanPham;
                    existing.DonGia = sanPham.DonGia;
                    existing.SoLuong = sanPham.SoLuong;
                    existing.MaLoai = sanPham.MaLoai;
                    existing.MaNguoiBan = sanPham.MaNguoiBan;
                    existing.TrangThai = sanPham.TrangThai;
                    existing.NgayCapNhat = DateTime.Now;

                    _context.Update(existing);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật thông tin sản phẩm thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.Id))
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

            ViewBag.MaLoai = new SelectList(await _context.LoaiSanPhams.Where(l => l.TrangThai).ToListAsync(), "MaLoai", "TenLoai", sanPham.MaLoai);
            ViewBag.MaNguoiBan = new SelectList(await _context.NguoiBans.ToListAsync(), "MaNguoiBan", "MaNguoiBan", sanPham.MaNguoiBan);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaLoaiNavigation)
                .Include(s => s.MaNguoiBanNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPhams.Remove(sanPham);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.Id == id);
        }
    }
}
