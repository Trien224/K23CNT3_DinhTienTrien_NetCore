using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neme.Filters;
using Neme.Models;

namespace Neme.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeAdmin]
    public class LoaiSanPhamsController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public LoaiSanPhamsController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // GET: LoaiSanPhams
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoaiSanPhams.ToListAsync());
        }

        // GET: LoaiSanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var loaiSanPham = await _context.LoaiSanPhams
                .FirstOrDefaultAsync(m => m.MaLoai == id);
            if (loaiSanPham == null) return NotFound();

            return View(loaiSanPham);
        }

        // GET: LoaiSanPhams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoaiSanPhams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoai,TenLoai,MoTa,TrangThai")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiSanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiSanPham);
        }

        // GET: LoaiSanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);
            if (loaiSanPham == null) return NotFound();

            return View(loaiSanPham);
        }

        // POST: LoaiSanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLoai,TenLoai,MoTa,TrangThai")] LoaiSanPham loaiSanPham)
        {
            if (id != loaiSanPham.MaLoai) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiSanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiSanPhamExists(loaiSanPham.MaLoai))
                        return NotFound();
                    else
                    {
                        ModelState.AddModelError("", "Đã có người khác sửa dữ liệu này. Vui lòng thử lại.");
                        return View(loaiSanPham);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(loaiSanPham);
        }

        // GET: LoaiSanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var loaiSanPham = await _context.LoaiSanPhams
                .FirstOrDefaultAsync(m => m.MaLoai == id);
            if (loaiSanPham == null) return NotFound();

            return View(loaiSanPham);
        }

        // POST: LoaiSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loai = await _context.LoaiSanPhams
                .Include(l => l.SanPhams)
                .FirstOrDefaultAsync(l => l.MaLoai == id);

            if (loai != null)
            {
                if (loai.SanPhams.Any())
                {
                    ModelState.AddModelError("", "Không thể xóa loại sản phẩm vì còn sản phẩm đang sử dụng.");
                    return View(loai);
                }

                _context.LoaiSanPhams.Remove(loai);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LoaiSanPhamExists(int id)
        {
            return _context.LoaiSanPhams.Any(e => e.MaLoai == id);
        }
    }
}
