using Microsoft.EntityFrameworkCore;
using MVC_Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using DotNetEnv;

// �[�� .env ���
Env.Load();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//#DB�������ҧ@�k#####################################################################

// �������ҧ@�k
// �K�[ �s���r�� builder ~ app
// �@�k 2 �ĥ� Ū�� appseting����T

//var confiqureBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
//IConfiguration config = confiqureBuilder.Build();
//string DBconn = config["ConnectionStrings:DefaultConnection"];
//builder.Services.AddDbContext<MVC_UserDBContext>(options => options.UseSqlServer(DBconn));

//#DB�������ҧ@�k#####################################################################

//DB�������ҧ@�k#####################################################################
// �s�@�k �קK�Q�ݨ�
// Ū�������ܼƤ�����Ʈw�s�u�r��

var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // �p�G�z�٦���L�t�m���A�]�i�H�N���̥[����t�m��
    .AddEnvironmentVariables(); // �o�N�K�[�����ܼƨ�t�m��

var configuration = configBuilder.Build();
string dbConnection = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

// �P�_���O�_env��DB�ܼ�
if (string.IsNullOrEmpty(dbConnection))
{
    throw new Exception("Database connection string not found in environment variables.");
}
// �`�U DbContext �èϥαq�����ܼƤ�Ū������Ʈw�s�u�r��
builder.Services.AddDbContext<MVC_UserDBContext>(options => options.UseSqlServer(dbConnection));
//#DB�������ҧ@�k#####################################################################



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

// ���ѳ]�w �i�H�]�w�ܦh��
// �Ĥ@���u�����ǳ̰�
// ���O�o��S��J�w�]�N�Ohome ���
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
