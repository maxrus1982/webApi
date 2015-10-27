using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.DAL;


namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private MainContext db = new MainContext();

        public ActionResult Index()
        {
            return View();
        }        

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}