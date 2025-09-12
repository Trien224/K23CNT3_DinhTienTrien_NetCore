using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2_QuanLyTapHoa.Models;

public class CustomerAccountController : Controller
{
    private readonly QuanLyTapHoaContext _context;
    public CustomerAccountController(QuanLyTapHoaContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string matKhau)
    {
        var kh = await _context.KhachHangs
            .FirstOrDefaultAsync(x => x.Email == email && x.MatKhau == matKhau);

        if (kh != null)
        {
            HttpContext.Session.SetString("CustomerEmail", kh.Email);
            return RedirectToAction("Index", "Shop");
        }

        ViewBag.Error = "Sai email hoặc mật khẩu!";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("CustomerEmail");
        return RedirectToAction("Login");
    }
}
