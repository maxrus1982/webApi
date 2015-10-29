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

namespace WebApp.Test
{
    [TestClass]
    public class TaskRepositoryTest
    {
        public TaskRepositoryTest()
        {
            WebApp.Core.IoC.Container.RegisterType<IBaseContext, MockBaseContext>();
            WebApp.Core.IoC.Container.RegisterType<ITaskContext, MockTaskContext>();
        }

        [TestMethod]
        public void GetList()
        {
            
        }

        [TestMethod]
        public void Get()
        {

        }

        [TestMethod]
        public void New()
        {

        }

        [TestMethod]
        public void Post()
        {

        }

        [TestMethod]
        public void PostNew()
        {

        }

        [TestMethod]
        public void Remove()
        {

        }
    }
}
