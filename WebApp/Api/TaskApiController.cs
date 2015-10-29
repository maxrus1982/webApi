using System;
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
using WebApp.Service.TasRequests;

namespace WebApp.Api
{
    [RoutePrefix(AreaConsts.ApiArea + "/Task")] // URL
    [ResourceAccessType(AccessType.Search)] //мин уровень доступа
    public class TaskApiController : BaseDocumentApiController<Task, TaskDTO, TaskRequest, CreateTaskRequest, TaskRepository, TaskValidator> //БО, DTO, Клиентский запрос, Клиентский запрос для нового документа, репозиторий, валидатор
    {
        public override string ResourceID { get { return "Foo"; } } //определяет ресурс для контроля доступа
    }
}