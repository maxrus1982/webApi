using System;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using System.Data.SqlTypes;

using WebApp;
using WebApp.Core;
using WebApp.DAL;
using WebApp.Service;
using WebApp.Service.Interface;
using WebApp.DAL.Context;
using WebApp.Service.TaskRequests;

namespace WebApp.Test
{
    [TestClass]
    public class TaskRepositoryTest
    {
        protected TaskRepository Repository { get; set; }

        public TaskRepositoryTest()
        {
            WebApp.Core.IoC.Container.RegisterType<IBaseContext, MockBaseContext>();
            WebApp.Core.IoC.Container.RegisterType<ITaskContext, MockTaskContext>();            

            Repository = new TaskRepository();
            Repository.DbContext = WebApp.Core.IoC.Container.Resolve<ITaskContext>();
            Repository.UserContext = this.UserContext();
        }

        protected IUserContext UserContext()
        {
            var __userContext = new MockUserContext();
            __userContext.UserID = "Foo";
            return __userContext;
        }

        private void AddTestRows()
        {
            for (int i = 1; i <= 20; i++)
                AddTask("Foo_"+i, true);
            for (int i = 1; i <= 20; i++)
                AddTask("Foo_" + i, false);
        }

        private void RemoveAllTestRows()
        {
            foreach (var __itemID in Repository.GetList(new TaskRequest()).Select(x => x.ID))
                Repository.Remove(__itemID);
        }

        [TestMethod]
        public void TasksListPassed()
        {
            RemoveAllTestRows();
            AddTestRows();

            //простой список
            var __request = new TaskRequest();
            __request.Page = 1;
            __request.Skip = 0;
            __request.Take = 10;
            var __dataList = Repository.GetList(__request);
            //размер страницы 20, а общее количество > 10
            Assert.IsTrue(__dataList.Count > 0 && __dataList.Count == 10 && __request.TotalRows>10);

            //простой список
            __request = new TaskRequest();
            __request.Page = 1;
            __request.Skip = 0;
            __request.Take = 10;
            __request.IgnoreCompletedTasks = true; //!!!
            __dataList = Repository.GetList(__request);
            //размер страницы 10, а общее количество > 10, с учетом кастомного фильтра через Request.IgnoreCompletedTasks
            Assert.IsTrue(__dataList.Count > 0 && __dataList.Count == 10 && __dataList.Where(x => x.State == TaskStateEnum.Completed).Count() == 0);

            //пагинация по 21 запись
            __request = new TaskRequest();
            __request.Page = 2;//!!!
            __request.Skip = 10;//!!!
            __request.Take = 10;//!!!
            __dataList = Repository.GetList(__request);
            //вторая страница, по 10 записей
            Assert.IsTrue(__dataList.Count == 10 && __request.TotalRows > 20);

            //кастомный фильтр по ID-у
            __request = new TaskRequest();
            __request.Page = 1;
            __request.Skip = 0;
            __request.Take = 10;
            __request.ID = Repository.GetList(new TaskRequest()).Select(x => x.ID).FirstOrDefault();//!!!
            __dataList = Repository.GetList(__request);
            //запрос должен вернуть только одну запись
            Assert.IsTrue(__dataList.Count == 1);

            //search none
            __request = new TaskRequest();
            __request.Page = 1;
            __request.Skip = 0;
            __request.Take = 10;
            __request.SearchData = new SearchData();//!!!
            __request.SearchData.Search = "SearchTextZZZZZZZZZZZZZZZZZZ";//!!!
            __request.SearchData.Fields.Add("Name");//!!!
            __dataList = Repository.GetList(__request);
            //запрос не должен вернуть результат
            Assert.IsTrue(__dataList.Count == 0);

            //search Foo
            __request = new TaskRequest();
            __request.Page = 1;
            __request.Skip = 0;
            __request.Take = 10;
            __request.SearchData = new SearchData();//!!!
            __request.SearchData.Search = "Foo";//!!!
            __request.SearchData.Fields.Add("Name");//!!!
            __dataList = Repository.GetList(__request);
            //запрос должен вернуть результат
            Assert.IsTrue(__dataList.Count == 10);

            //order by
            __request = new TaskRequest();
            __request.Page = 1;
            __request.Skip = 0;
            __request.Take = 10000000;
            __request.Sort.Add(new Sort() { Dir = "desc", Field = "ID" });//!!!
            __dataList = Repository.GetList(__request); // сортируем на СУБД
            var __orderedDataList = Repository.GetList(new TaskRequest() { Take = 10000000 }).OrderByDescending(x => new SqlGuid(x.ID)).ToList(); // сортируем в LINQ
            //результат должен быть отсортирован правильно - опорная точка - первая и последняя запись
            Assert.IsTrue(
                __dataList.FirstOrDefault().ID == __orderedDataList.FirstOrDefault().ID &&
                __dataList.LastOrDefault().ID == __orderedDataList.LastOrDefault().ID
            );

            //filter none
            __request = new TaskRequest();
            __request.Page = 1;
            __request.Skip = 0;
            __request.Take = 10;
            __request.Filter = new Filter();//!!!
            __request.Filter.Logic = "and";//!!!
            __request.Filter.Filters.Add(new Filter() { Field = "Name", Operator = "contains", Value = "FilterTextЧЧЧЧЧЧЧЧЧЧЧЧЧЧЧЧЧЧЧЧЧЧЧ" });//!!!
            __dataList = Repository.GetList(__request);
            //запрос не должен вернуть результат
            Assert.IsTrue(__dataList.Count == 0);

            //filter Foo
            __request = new TaskRequest();
            __request.Page = 1;
            __request.Skip = 0;
            __request.Take = 10;
            __request.Filter = new Filter();//!!!
            __request.Filter.Logic = "and";//!!!
            __request.Filter.Filters.Add(new Filter() { Field = "Name", Operator = "contains", Value = "Foo" });//!!!
            __dataList = Repository.GetList(__request);
            //запрос должен вернуть результат
            Assert.IsTrue(__dataList.Count == 10);
            
            RemoveAllTestRows();
            AddTestRows();
        }

        [TestMethod]
        public void TaskGroupListPassed()
        {
            //простой список
            var __request = new TaskRequest();
            var __dataList = Repository.GetTaskGroupList(__request);
            //запрос должен вернуть результат
            Assert.IsNotNull(__dataList.Count>0);
        }

        [TestMethod]
        public void TaskPassed()
        {
            var __documentDTO = Repository.Get(Guid.NewGuid());
            Assert.IsNull(__documentDTO);
        }

        [TestMethod]
        public void NewTaskPassed()
        {
            var __documentDTO = Repository.New(new CreateTaskRequest() {
                PlanBeginDate = DateTime.Now,
                PlanEndDate = DateTime.Now.AddDays(100)
            });
            Assert.IsNotNull(__documentDTO);
        }

        [TestMethod]
        public void NewTaskPosted()
        {
            var __id = Guid.NewGuid();
            var __documentDTO = Repository.New(new CreateTaskRequest() {
                PlanBeginDate = DateTime.Now,
                PlanEndDate = DateTime.Now.AddDays(100)
            });
            __documentDTO.Name = "Foo";
            __documentDTO = Repository.Post(__documentDTO);
            Repository.Remove(__documentDTO.ID);
            Assert.IsNotNull(__documentDTO);
        }

        [TestMethod]
        public void TaskPosted()
        {
            var __id = Guid.NewGuid();
            var __documentDTO = Repository.New(new CreateTaskRequest()
            {
                PlanBeginDate = DateTime.Now,
                PlanEndDate = DateTime.Now.AddDays(100)
            });
            __documentDTO.Name = "Foo";
            __documentDTO = Repository.Post(__documentDTO);
            __documentDTO = Repository.Get(__documentDTO.ID);

            __documentDTO.Name = "Foo2";
            __documentDTO = Repository.Post(__documentDTO);
            Repository.Remove(__documentDTO.ID);
            Assert.IsNotNull(__documentDTO);
        }

        [TestMethod]
        public void RemoveTask()
        {
            var __id = Guid.NewGuid();
            var __documentDTO = Repository.New(new CreateTaskRequest()
            {
                PlanBeginDate = DateTime.Now,
                PlanEndDate = DateTime.Now.AddDays(100)
            });
            __documentDTO.Name = "Foo";
            __documentDTO = Repository.Post(__documentDTO);
            Repository.Remove(__documentDTO.ID);
            Assert.IsNotNull(__documentDTO);
        }

        protected void AddTask(string name, bool completed)
        {
            var __id = Guid.NewGuid();
            var __documentDTO = Repository.New(new CreateTaskRequest()
            {
                PlanBeginDate = DateTime.Now,
                PlanEndDate = DateTime.Now.AddDays(100)
            });
            __documentDTO.Name = name;
            __documentDTO.Completed = completed;
            __documentDTO = Repository.Post(__documentDTO);
        }
    }
}
