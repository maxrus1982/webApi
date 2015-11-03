using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using System.Web.Mvc;

using WebApp.Service;
using WebApp.Service.Interface;
using WebApp.Service.TaskRequests;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ///этот код инициализации должен быть ну максмимум в сиде/миграции, а вообще он не нужен
            ///а здесь чисто для демо
            var __rep = new TaskRepository();
            __rep.UserContext = new UserContext();
            __rep.UserContext.UserID = "Foo";
            __rep.DbContext = WebApp.Core.IoC.Container.Resolve<ITaskContext>();
            if (__rep.GetList(new TaskRequest()).Select(x => x.ID).Count() == 0)
            {
                AddTestRows(__rep);
            }

            return View();
        }        

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }

        private void AddTestRows(TaskRepository repository)
        {
            for (int i = 1; i <= 20; i++)
                AddTask("Foo_" + i, true, repository);
            for (int i = 1; i <= 20; i++)
                AddTask("Foo_" + i, false, repository);
        }

        private void AddTask(string name, bool completed, TaskRepository repository)
        {
            var __id = Guid.NewGuid();
            var __documentDTO = repository.New(new CreateTaskRequest()
            {
                PlanBeginDate = DateTime.Now,
                PlanEndDate = DateTime.Now.AddDays(100)
            });
            __documentDTO.Name = name;
            __documentDTO.Completed = completed;
            __documentDTO = repository.Post(__documentDTO);
        }
    }
}