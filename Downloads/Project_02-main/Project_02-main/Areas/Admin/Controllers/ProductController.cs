using Microsoft.AspNetCore.Mvc;
using Project_02.Data;
using Project_02.Models;

namespace Project_02.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly QuanLyTapHoaContext _context;

        public ProductController(QuanLyTapHoaContext context)
        {
            _context = context;
        }

        // GET: Admin/Product
        public IActionResult Index()
        {
            var products = _context.SanPhams.ToList();
            return View(products);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SanPham product, IFormFile? HinhAnhFile)
        {
            if (ModelState.IsValid)
            {
                if (HinhAnhFile != null)
                {
                    string fileName = Path.GetFileName(HinhAnhFile.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        HinhAnhFile.CopyTo(stream);
                    }

                    product.HinhAnh = fileName;
                }

                _context.SanPhams.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public IActionResult Edit(int id)
        {
            var product = _context.SanPhams.Find(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SanPham product, IFormFile? HinhAnhFile)
        {
            if (ModelState.IsValid)
            {
                if (HinhAnhFile != null)
                {
                    string fileName = Path.GetFileName(HinhAnhFile.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        HinhAnhFile.CopyTo(stream);
                    }

                    product.HinhAnh = fileName;
                }

                _context.SanPhams.Update(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public IActionResult Delete(int id)
        {
            var product = _context.SanPhams.Find(id);
            if (product == null) return NotFound();

            _context.SanPhams.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
