using Microsoft.EntityFrameworkCore;
using Project2_QuanLyTapHoa.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Kết nối Database
var connectionString = builder.Configuration.GetConnectionString("TapHoa");
builder.Services.AddDbContext<QuanLyTapHoaContext>(options =>
    options.UseSqlServer(connectionString));

// Thêm HttpContextAccessor nếu cần
builder.Services.AddHttpContextAccessor();

// Thêm Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();          // ← phải đặt trước UseAuthorization
app.UseAuthorization();

// Area routing
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
