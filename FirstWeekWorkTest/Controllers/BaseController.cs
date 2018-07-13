using FirstWeekWorkTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstWeekWorkTest.Controllers
{
    public class BaseController : Controller
    {
        protected 客戶資料Entities1 db = new 客戶資料Entities1();
        protected override void HandleUnknownAction(string actionName)
        {
            this.RedirectToAction("Index").ExecuteResult(this.ControllerContext);
        }
    }
}