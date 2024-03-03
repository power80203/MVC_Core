using Microsoft.EntityFrameworkCore;
using MVC_Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//#############################################################################

// 添加 連接字串 builder ~ app
// 作法 2 採用 讀取 appseting的資訊

var confiqureBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
IConfiguration config = confiqureBuilder.Build();
string DBconn = config["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<MVC_UserDBContext>(options => options.UseSqlServer(DBconn));

//#############################################################################


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
