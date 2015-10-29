using System;
using System.Web.Http;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

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

        [TestMethod]
        public void TasksListPassed()
        {
            //простой список
            var __request = new TaskRequest();
            __request.Page = 1;
            __request.Skip = 0;
            __request.Take = 20;
            var __dataList = Repository.GetList(__request);
            Assert.IsNotNull(__dataList);

            //простой список
            __request = new TaskRequest();
            __request.Page = 1;
            __request.Skip = 0;
            __request.Take = 20;
            __request.IngnoreCompletedTasks = true;
            __dataList = Repository.GetList(__request);
            Assert.IsNotNull(__dataList);

            //пагинация
            __request = new TaskRequest();
            __request.Page = 2;
            __request.Skip = 21;
            __request.Take = 21;
            __dataList = Repository.GetList(__request);
            Assert.IsNotNull(__dataList);

            //кастомный фильтр по ID-у
            __request = new TaskRequest();
            __request.Page = 1;
            __request.Skip = 0;
            __request.Take = 20;
            __request.ID = Guid.NewGuid();
            __dataList = Repository.GetList(__request);
            Assert.IsNotNull(__dataList);

            //search
            __request = new TaskRequest();
            __request.Page = 1;
            __request.Skip = 0;
            __request.Take = 20;
            __request.SearchData = new SearchData();
            __request.SearchData.Search = "SearchText";
            __request.SearchData.Fields.Add("Name");
            __dataList = Repository.GetList(__request);
            Assert.IsNotNull(__dataList);

            //order by
            __request = new TaskRequest();
            __request.Page = 1;
            __request.Skip = 0;
            __request.Take = 20;
            __request.Sort.Add(new Sort() { Dir = "desc", Field = "Name" });
            __dataList = Repository.GetList(__request);
            Assert.IsNotNull(__dataList);

            //filter
            __request = new TaskRequest();
            __request.Page = 1;
            __request.Skip = 0;
            __request.Take = 20;
            __request.Filter = new Filter();
            __request.Filter.Logic = "and";
            __request.Filter.Filters.Add(new Filter() { Field = "Name", Operator = "contains", Value = "FilterText" });
            __dataList = Repository.GetList(__request);
            Assert.IsNotNull(__dataList);
        }

        [TestMethod]
        public void TaskGroupListPassed()
        {
            //простой список
            var __request = new TaskRequest();
            var __dataList = Repository.GetTaskGroupList(__request);
            Assert.IsNotNull(__dataList);
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

        protected void AddTask()
        {
            var __id = Guid.NewGuid();
            var __documentDTO = Repository.New(new CreateTaskRequest()
            {
                PlanBeginDate = DateTime.Now,
                PlanEndDate = DateTime.Now.AddDays(100)
            });
            __documentDTO.Name = "Foo";
            __documentDTO = Repository.Post(__documentDTO);
        }

        [TestMethod]
        public void TasksAdded()
        {
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
            AddTask();
        }
    }
}
