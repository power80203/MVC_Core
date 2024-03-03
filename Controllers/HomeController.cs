using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Core.Models;
using System.Diagnostics;

namespace MVC_Core.Controllers
{
    public class HomeController : Controller
    {

        #region
        // 初始化
        private readonly MVC_UserDBContext _db = new MVC_UserDBContext();

        #endregion


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MVC_UserDBContext context)
        {
            _logger = logger;

            // 真正的把 context放入 才有DB
            _db = context;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //=================================================
        //== 以上都是預設。原本 Home控制器就有的動作
        //=================================================

        // IActionResult 的 I 就是 interface

        public  async Task<IActionResult> List()



        {
            if (_db.UserTables == null)
            {
                return Content("Error");
            }
            else
            {
                return View(await _db.UserTables.ToListAsync()); // 直接列出來all
            }
        }





    }
}
