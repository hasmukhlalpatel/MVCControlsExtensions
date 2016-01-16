using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcExtensions.Demo.Models;

namespace MvcExtensions.Demo.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new HomeViewModel
            {
                NewsList = new List<NewsViewModel>
                {
                    new NewsViewModel
                    {
                        Description = "Test",
                        Id = 1,
                        PublishedOn = DateTime.Now,
                        Summary = "test summary"
                    },
                    new NewsViewModel
                    {
                        Description = "Test2",
                        Id = 2,
                        PublishedOn = DateTime.Now,
                        Summary = "test summary2"
                    },
                    new NewsViewModel
                    {
                        Description = "Test3",
                        Id = 3,
                        PublishedOn = DateTime.Now,
                        Summary = "test summary3"
                    }
                }
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(HomeViewModel model)
        {
            return View(model);
        }
    }
}
