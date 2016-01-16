using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcExtensions.Demo.Areas.AngularJsDemo.Models;

namespace MvcExtensions.Demo.Areas.KnockoutJsDemo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /KnockoutJsDemo/Home/

        public ActionResult Index()
        {
            var model = new DemoModel {Name = "Knockout Test"};
            return View();
        }

    }
}
