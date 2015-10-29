using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using System.Data.Entity.Infrastructure.Interception;
using Microsoft.Practices.Unity;

using WebApp;
using WebApp.Core;
using WebApp.DAL;
using WebApp.Service;
using WebApp.Service.Interface;
using WebApp.DAL.Context;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            WebApp.Core.IoC.Container.RegisterType<IBaseContext, BaseContext>();
            WebApp.Core.IoC.Container.RegisterType<ITaskContext, TaskContext>();
        }
    }
}
