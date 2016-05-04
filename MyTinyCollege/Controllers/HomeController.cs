using MyTinyCollege.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyTinyCollege.Controllers
{
    public class HomeController : Controller
    {
        private SchoolContext db = new SchoolContext();
        public ActionResult Index()
        {
            //jkhalack: added department model to this view
            //          for display on home page
            //return View();

            var departments = db.Departments
                .OrderBy(d => d.Name).ToList();
            return View(departments);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test()
        {
            return View();
            //return Content("<h1>This is just a test</h1>");
        }
    }
}