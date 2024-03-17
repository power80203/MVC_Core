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

        public async Task<IActionResult> List()



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

        [HttpGet]
        public IActionResult Details(int?  _ID = 1) // int? 表示 允許null

        {
            if (_ID == null||_ID.HasValue == false)
            {
                // 沒有ID被輸入就會報錯
                return new StatusCodeResult((int)System.Net.HttpStatusCode.BadRequest);
            }

            UserTable ut = _db.UserTables.Find(_ID);


            if (ut == null)
            {
                return NotFound();
            }
            else
            {
                return View(ut);
            }

        }

        public IActionResult Create()
        {
            return View();
        }



        // 因為這個是http post 所以會進過這個處理
        [HttpPost]
        public IActionResult Create(UserTable _userTable)
        {
            // 檢查是否回傳值有問題

            if (_userTable != null)
            {
                _db.UserTables.Add(_userTable);
                _db.SaveChanges();


                return Content("新增一紀錄成功!!!");
            }
            else
            {
                return View();
                //return RedirectToAction("List","Home");
            }

        }

        public IActionResult Delete(int? _ID=1)


        {
            if (_ID == null)
            {
                return Content("沒有輸入文章ID");
            }


            UserTable ut = _db.UserTables.Find(_ID);

            if (ut == null)
            {
                return Content("沒有輸入這篇文章");
            }



            return View(ut);

        }

        // 用不同的函數名稱 但是一樣去跑delete
        [HttpPost, ActionName("Delete")]

        public IActionResult DeleteConfrim(int _ID = 1)
        {


            //ModelState 就是表單傳過來透過 ModelState.IsValid 檢查是否必要的欄位都有輸入
            if(ModelState.IsValid)
                {

                    UserTable ut = _db.UserTables.Find(_ID);
                    if (ut == null)
                    {
                        return Content("找不到這筆紀錄");
                    }

                    _db.Entry(ut).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    // save change 才是真的開始
                    _db.SaveChanges(true);

                    



            }
            return Content("成功刪除囉");

        }




    }
}
