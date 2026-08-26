using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_02.Data;
using Project_02.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Project_02.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class DanhGiaSanPhamsController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public DanhGiaSanPhamsController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // GET: Customer/DanhGiaSanPhams
        public async Task<IActionResult> Index()
        {
            var data = _context.DanhGiaSanPhams
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaSpNavigation);
            return View(await data.ToListAsync());
        }

        // GET: Customer/DanhGiaSanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var danhGia = await _context.DanhGiaSanPhams
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.MaDanhGia == id);

            if (danhGia == null) return NotFound();

            return View(danhGia);
        }

        // GET: Customer/DanhGiaSanPhams/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "HoTen");
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "TenSp");
            return View();
        }

        // POST: Customer/DanhGiaSanPhams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DanhGiaSanPhamDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "HoTen", dto.MaKh);
                ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "TenSp", dto.MaSp);

                // map DTO -> Model để View không lỗi
                var model = new DanhGiaSanPham
                {
                    MaKh = dto.MaKh,
                    MaSp = dto.MaSp,
                    SoSao = dto.SoSao,
                    NoiDung = dto.NoiDung,
                    LaYeuThich = dto.LaYeuThich,
                    TrangThai = dto.TrangThai
                };

                return View(model);
            }

            var entity = new DanhGiaSanPham
            {
                MaKh = dto.MaKh,
                MaSp = dto.MaSp,
                SoSao = dto.SoSao,
                NoiDung = dto.NoiDung,
                LaYeuThich = dto.LaYeuThich,
                NgayTao = DateTime.Now,
                TrangThai = true
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Customer/DanhGiaSanPhams/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var danhGia = await _context.DanhGiaSanPhams.FindAsync(id);
            if (danhGia == null) return NotFound();

            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "HoTen", danhGia.MaKh);
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "TenSp", danhGia.MaSp);

            // Trả về Model thay vì DTO
            return View(danhGia);
        }
        // POST: Customer/DanhGiaSanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DanhGiaSanPhamDto dto)
        {
            if (id != dto.MaDanhGia) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "HoTen", dto.MaKh);
                ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "TenSp", dto.MaSp);

                // map DTO -> Model để View không lỗi
                var model = new DanhGiaSanPham
                {
                    MaDanhGia = dto.MaDanhGia,
                    MaKh = dto.MaKh,
                    MaSp = dto.MaSp,
                    SoSao = dto.SoSao,
                    NoiDung = dto.NoiDung,
                    LaYeuThich = dto.LaYeuThich,
                    TrangThai = dto.TrangThai
                };

                return View(model);
            }

            var entity = await _context.DanhGiaSanPhams.FindAsync(id);
            if (entity == null) return NotFound();

            entity.MaKh = dto.MaKh;
            entity.MaSp = dto.MaSp;
            entity.SoSao = dto.SoSao;
            entity.NoiDung = dto.NoiDung;
            entity.LaYeuThich = dto.LaYeuThich;
            entity.TrangThai = dto.TrangThai;

            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DanhGiaSanPhamExists(dto.MaDanhGia))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Customer/DanhGiaSanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var danhGia = await _context.DanhGiaSanPhams
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.MaDanhGia == id);

            if (danhGia == null) return NotFound();

            return View(danhGia);
        }

        // POST: Customer/DanhGiaSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhGia = await _context.DanhGiaSanPhams.FindAsync(id);
            if (danhGia != null)
            {
                _context.DanhGiaSanPhams.Remove(danhGia);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DanhGiaSanPhamExists(int id)
        {
            return _context.DanhGiaSanPhams.Any(e => e.MaDanhGia == id);
        }
    }
}
