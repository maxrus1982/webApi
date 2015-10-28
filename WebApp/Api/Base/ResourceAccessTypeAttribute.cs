using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using WebApp.Core;

namespace WebApp.Api.Base
{
    public class ResourceAccessTypeAttribute : ActionFilterAttribute
    {
        public AccessType AccessType { get; set; }

        public override void OnActionExecuting(HttpActionContext filterContext)
        {

        }

        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {

        }

        public ResourceAccessTypeAttribute(AccessType accessType)
        {
            AccessType = accessType;
        }
    }
}