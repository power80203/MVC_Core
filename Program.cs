using Microsoft.EntityFrameworkCore;
using MVC_Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using DotNetEnv;

// 加載 .env 文件
Env.Load();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//#DB測試環境作法#####################################################################

// 測試環境作法
// 添加 連接字串 builder ~ app
// 作法 2 採用 讀取 appseting的資訊

//var confiqureBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
//IConfiguration config = confiqureBuilder.Build();
//string DBconn = config["ConnectionStrings:DefaultConnection"];
//builder.Services.AddDbContext<MVC_UserDBContext>(options => options.UseSqlServer(DBconn));

//#DB測試環境作法#####################################################################

//DB正式環境作法#####################################################################
// 新作法 避免被看到
// 讀取環境變數中的資料庫連線字串

var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // 如果您還有其他配置文件，也可以將它們加載到配置中
    .AddEnvironmentVariables(); // 這將添加環境變數到配置中

var configuration = configBuilder.Build();
string dbConnection = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

// 判斷說是否env有DB變數
if (string.IsNullOrEmpty(dbConnection))
{
    throw new Exception("Database connection string not found in environment variables.");
}
// 注冊 DbContext 並使用從環境變數中讀取的資料庫連線字串
builder.Services.AddDbContext<MVC_UserDBContext>(options => options.UseSqlServer(dbConnection));
//#DB正式環境作法#####################################################################



// Configure the HTTP request pipeline.
//############################################################################

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 路由設定 可以設定很多組
// 第一組優先順序最高
// 像是這邊沒輸入預設就是home 控制器
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
