using DttLesson04.Models;
using Microsoft.AspNetCore.Mvc;

namespace DttLesson04.Controllers
{
    public class DttBookController : Controller
    {
        public static List<DttBook> DttBooks = new List<DttBook>
        {
                new DttBook
                {
                    DttId = "BK001",
                    DttTitle = "Lập Trình C# Cơ Bản",
                    DttDescription = "Cuốn sách giới thiệu các khái niệm cơ bản về lập trình C#.",
                    DttImage = "../imge/03.png",
                    DttPrice = 150000,
                    DttPage = 300
                },
                new DttBook
                {
                    DttId = "BK002",
                    DttTitle = "Hướng Dẫn ASP.NET Core",
                    DttDescription = "Tìm hiểu cách xây dựng ứng dụng web với ASP.NET Core.",
                    DttImage = "../imge/02.png",
                    DttPrice = 200000,
                    DttPage = 450
                },
                new DttBook
                {
                    DttId = "BK003",
                    DttTitle = "SQL Server Cho Người Mới",
                    DttDescription = "Hướng dẫn quản lý và truy vấn cơ sở dữ liệu SQL Server.",
                    DttImage = "../imge/03.png",
                    DttPrice = 180000,
                    DttPage = 380
                },
                new DttBook
                {
                    DttId = "BK004",
                    DttTitle = "Lập Trình Web Với Blazor",
                    DttDescription = "Xây dựng ứng dụng web tương tác bằng công nghệ Blazor.",
                    DttImage = "../imge/02.png",
                    DttPrice = 220000,
                    DttPage = 420
                },
                new DttBook
                {
                    DttId = "BK005",
                    DttTitle = "Thiết Kế Hệ Thống Phần Mềm",
                    DttDescription = "Cách phân tích và thiết kế hệ thống phần mềm theo mô hình hướng đối tượng.",
                    DttImage = "../imge/01.png",
                    DttPrice = 250000,
                    DttPage = 500
                }
        };
        //Get all data Dttbooks
        public IActionResult DttIndex()
        {
            //Hien view
            return View(DttBooks);
        }

        // GET
        [HttpGet]
        public IActionResult DttTao()
        {
            return View(new DttBook());
        }

        // POST
        [HttpPost]
        public IActionResult DttTao(DttBook newBook)
        {
            if (ModelState.IsValid)
            {
                DttBooks.Add(newBook);
                return RedirectToAction("DttIndex");
            }
            return View(newBook);
        }

        [HttpPost]
        public IActionResult DttGuiTao(DttBook newBook)
        {
            if (ModelState.IsValid)
            {
                DttBooks.Add(newBook);
                return RedirectToAction("DttIndex");
            }
            return View("DttTao", newBook); // nếu có lỗi nhập liệu
        }

        // GET: DttSua
        public IActionResult DttSua(string id)
        {
            var book = DttBooks.FirstOrDefault(b => b.DttId == id);
            if (book == null)
            {
                return NotFound(); // Không tìm thấy sách
            }
            return View(book); // Trả về view sửa với dữ liệu sách
        }

        // POST: DttGuiSua
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DttGuiSua(DttBook updatedBook)
        {
            if (ModelState.IsValid)
            {
                var book = DttBooks.FirstOrDefault(b => b.DttId == updatedBook.DttId);
                if (book != null)
                {
                    // Cập nhật thông tin sách
                    book.DttTitle = updatedBook.DttTitle;
                    book.DttDescription = updatedBook.DttDescription;
                    book.DttImage = updatedBook.DttImage;
                    book.DttPrice = updatedBook.DttPrice;
                    book.DttPage = updatedBook.DttPage;

                    TempData["Message"] = "✅ Đã cập nhật sách thành công!";
                    return RedirectToAction("DttIndex");
                }
                return NotFound(); // Không tìm thấy sách để sửa
            }

            // Dữ liệu không hợp lệ -> quay lại form
            return View("DttSua", updatedBook);
        }

    }
}
