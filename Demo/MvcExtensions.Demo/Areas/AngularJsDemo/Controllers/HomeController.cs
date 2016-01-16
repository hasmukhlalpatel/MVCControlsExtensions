using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcExtensions.Demo.Areas.AngularJsDemo.Models;

namespace MvcExtensions.Demo.Areas.AngularJsDemo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /AngulerJsDemo/Home/

        public ActionResult Index()
        {
            var model = new DemoModel() { Name = "Angular Js Demo model" };
            return View(model);
        }

    }
}
