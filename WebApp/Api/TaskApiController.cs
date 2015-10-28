﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using WebApp.Core;
using WebApp.Domain;
using WebApp.Domain.Interface;
using WebApp.Service;
using WebApp.Service.Interface;

namespace WebApp.Api
{
    [RoutePrefix(AreaConsts.ApiArea + "/Task")]
    [ResourceAccessType(AccessType.Search)]
    public class TaskApiController : BaseDocumentApiController<Task, TaskDTO, TaskRequest, CreateTaskRequest, TaskRepository, TaskValidator>
    {
        public override string ResourceID { get { return "Foo"; } }

        [HttpGet]
        [Route("Foo")]
        [ResourceAccessType(AccessType.View)]
        public TaskDTO Foo()
        {
            return new TaskDTO() { Name = "Foo" };
        }
    }
}