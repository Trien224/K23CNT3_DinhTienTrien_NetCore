using Microsoft.EntityFrameworkCore;
using Project_02.Data;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Thêm Session service
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 2️⃣ Add MVC
builder.Services.AddControllersWithViews();

// 3️⃣ Kết nối DB
var connectionString = builder.Configuration.GetConnectionString("TapHoa");
builder.Services.AddDbContext<QuanLyTapHoaContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// 4️⃣ Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ⚠️ BẮT BUỘC phải gọi UseSession trước UseAuthorization
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
