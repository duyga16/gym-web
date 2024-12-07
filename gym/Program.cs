using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using gym.Models;
using gym.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
// Cấu hình EmailSettings từ appsettings.json
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Đăng ký dịch vụ EmailService và IEmailService
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddDbContext<GymContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));


// Add services to the container.
builder.Services.AddControllersWithViews();

// xác thực người dùng
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/dang-nhap";
        options.LogoutPath = "/dang-xuat";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Cookie xác thực hết hạn
        options.Cookie.HttpOnly = true;   // Đảm bảo cookie chỉ có thể truy cập qua HTTP
    });

// Thêm Session vào ứng dụng
builder.Services.AddDistributedMemoryCache();  // Cấu hình bộ nhớ đệm cho session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session
    options.Cookie.HttpOnly = true; // Cookie chỉ có thể truy cập thông qua HTTP
    options.Cookie.IsEssential = true; // Yêu cầu cookie là cần thiết
});
// Cấu hình Cookie Antiforgery (nếu có)
builder.Services.AddAntiforgery(options =>
{
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
var app = builder.Build();

// Thêm các endpoint xử lý của CKFinder
app.UseStaticFiles(); // Đảm bảo phục vụ tệp tĩnh
app.UseCors("AllowAll");  // Sử dụng chính sách CORS

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

// tính chỉ số BMI
app.MapControllerRoute(
    name: "BMI",
    pattern: "bmi",
    defaults: new { controller = "Accounts", action = "bmiCalculator" });

// lịch tập của người dùng
app.MapControllerRoute(
    name: "timetable",
    pattern: "lich-tap",
    defaults: new { controller = "Accounts", action = "classTimetable" });

// đăng nhập 
app.MapControllerRoute(
    name: "login",
    pattern: "dang-nhap",
    defaults: new { controller = "Accounts", action = "Login" });

// đăng ký
app.MapControllerRoute(
    name: "register",
    pattern: "dang-ky",
    defaults: new { controller = "Accounts", action = "Register" });

// quên mật khẩu
app.MapControllerRoute(
    name: "forgotPW",
    pattern: "quen-mat-khau",
    defaults: new { controller = "Accounts", action = "ForgotPassword" });

// tài khoản
app.MapControllerRoute(
    name: "accountUser",
    pattern: "tai-khoan",
    defaults: new { controller = "Accounts", action = "account" });

// cho trang lớp tập chi tiết 
app.MapControllerRoute(
    name: "classDetails",
    pattern: "lop-tap/{meta}",
    defaults: new { controller = "Classes", action = "Details" });

// cho trang lớp tập trên menu
app.MapControllerRoute(
        name: "classDefault",
        pattern: "lop-tap",
        defaults: new { controller = "Classes", action = "DefaultClass" });

// cho trang Blogs
app.MapControllerRoute(
    name: "BlogSuKien",
    pattern: "{meta}/{subMeta?}",
    defaults: new { controller = "Blogs", action = "Details"});


// cho trang PT
app.MapControllerRoute(
    name: "PtsDetails",
    pattern: "doi-ngu/{meta}",
    defaults: new { controller = "PTs", action = "Details" });


// admin 
app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// admin-menu


// index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// cho thanh menu
app.MapControllerRoute(
        name: "dynamic",
        pattern: "{meta}",
        defaults: new { controller = "Home", action = "DynamicPage" });


app.Run();
