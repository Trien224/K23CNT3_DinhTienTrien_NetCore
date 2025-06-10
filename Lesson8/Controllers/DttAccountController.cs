using Lesson8.Models;
using Microsoft.AspNetCore.Mvc;
namespace Lesson8.Controllers {
    public class DttAccountController : Controller
    {
        private static List<DttAccount> dttListAccount = new List<DttAccount>
        {
            new DttAccount { DttId = 231090107, DttName = "Nguyễn Văn A", DttBirthDay = new DateTime(1990, 5, 10), DttEmail = "vana@example.com", DttPhone = "0912345678", DttPassword = "MatKhau123", DttStatus = "facebook.com/vana", DttAvatar = "avatar1.jpg", DttAddress = "Hà Nội" },
            new DttAccount { DttId = 2, DttName = "Trần Thị B", DttBirthDay = new DateTime(1992, 8, 15), DttEmail = "thib@example.com", DttPhone = "0934567890", DttPassword = "Password456", DttStatus = "facebook.com/thib", DttAvatar = "avatar2.png", DttAddress = "TP.HCM" },
            new DttAccount { DttId = 3, DttName = "Lê Văn C", DttBirthDay = new DateTime(1988, 1, 20), DttEmail = "vanc@example.com", DttPhone = "0909876543", DttPassword = "LeVanC2023", DttStatus = "facebook.com/vanc", DttAvatar = "avatar3.jpg", DttAddress = "Đà Nẵng" },
            new DttAccount { DttId = 4, DttName = "Phạm Thị D", DttBirthDay = new DateTime(1995, 3, 30), DttEmail = "thid@example.com", DttPhone = "0943216547", DttPassword = "PhamThiD#1", DttStatus = "facebook.com/thid", DttAvatar = "avatar4.jpg", DttAddress = "Cần Thơ" },
            new DttAccount { DttId = 5, DttName = "Hoàng Văn E", DttBirthDay = new DateTime(1993, 11, 5), DttEmail = "vane@example.com", DttPhone = "0976543210", DttPassword = "HoangVanE!", DttStatus = "facebook.com/vane", DttAvatar = "avatar5.jpg", DttAddress = "Hải Phòng" }
        };
        public ActionResult DttIndex()
        {
            return View(dttListAccount);
        }

        public ActionResult DttCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DttCreate(DttAccount account)
        {
            try
            {
                account.DttId = dttListAccount.Count > 0 ? dttListAccount.Max(a => a.DttId) + 1 : 1;
                dttListAccount.Add(account);
                return RedirectToAction(nameof(DttIndex));
            }
            catch
            {
                return View(account);
            }
        }


        public ActionResult DttEdit(int id)
        {
            var account = dttListAccount.FirstOrDefault(a => a.DttId == id);
            if (account == null) return NotFound();
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DttEdit(int id, DttAccount updatedAccount)
        {
            try
            {
                var account = dttListAccount.FirstOrDefault(a => a.DttId == id);
                if (account == null) return NotFound();

                account.DttName = updatedAccount.DttName;
                account.DttEmail = updatedAccount.DttEmail;
                account.DttPassword = updatedAccount.DttPassword;
                account.DttStatus = updatedAccount.DttStatus;
                return RedirectToAction(nameof(DttIndex));
            }
            catch
            {
                return View(updatedAccount);
            }
        }

        public ActionResult DttDetails(int id)
        {
            var account = dttListAccount.FirstOrDefault(a => a.DttId == id);
            if (account == null) return NotFound();
            return View(account);
        }

        public ActionResult DttDelete(int id)
        {
            var account = dttListAccount.FirstOrDefault(a => a.DttId == id);
            if (account == null) return NotFound();
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DttDeleteConfirmed(int id)
        {
            var account = dttListAccount.FirstOrDefault(a => a.DttId == id);
            if (account == null)
            {
                TempData["Error"] = "Tài khoản không tồn tại.";
                return RedirectToAction(nameof(DttIndex));
            }

            try
            {
                dttListAccount.Remove(account);
                TempData["Success"] = "Xóa tài khoản thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa tài khoản: " + ex.Message;
            }

            return RedirectToAction(nameof(DttIndex));
        }

    }
}
