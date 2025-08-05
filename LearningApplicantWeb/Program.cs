using LearningApplicantWeb; // Pastikan using ini ada untuk mengakses DBClass
using LearningApplicantWeb.Models.EF;
using Microsoft.EntityFrameworkCore; // Tambahkan using untuk ModelContext Anda


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new ArgumentNullException("Connection string 'DefaultConnection' is not configured in appsettings.json.");
}

DBClass._ConnString = connectionString;

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "MyCookieAuth";
        options.LoginPath = "/Account/Login"; // Halaman login jika user belum terautentikasi
        options.AccessDeniedPath = "/Home/AccessDenied"; // Halaman jika user tidak punya izin
    });

builder.Services.AddDbContext<ModelContext>(options =>
    options.UseMySql(DBClass._ConnString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.4.3-mysql")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();