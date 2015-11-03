using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class TaskController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TaskPage1()
        {
            return View();
        }

        public ActionResult TaskPage2()
        {
            return View();
        }

        public ActionResult TaskPage3()
        {
            return View();
        }

        public ActionResult StaticFilter()
        {
            return View();
        }

        public ActionResult DynamicFilter()
        {
            return View();
        }

        public ActionResult SortByID()
        {
            return View();
        }

        public ActionResult SortByName()
        {
            return View();
        }

        public ActionResult Typehead()
        {
            return View();
        }

        public ActionResult GroupedList()
        {
            return View();
        }
    }
}