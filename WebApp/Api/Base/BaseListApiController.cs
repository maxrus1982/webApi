using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net;

using WebApp.Service.Interface;
using WebApp.Service;
using WebApp.Core;

namespace WebApp.Api
{
    //базовый API-контроллер для списка
    [RoutePrefix(AreaConsts.ApiArea + "/BaseList")]
    [ResourceAccessType(AccessType.Search)]
    public abstract class BaseListApiController<TDocumentDTO, TRequest, TRepository> : ApiController
        where TDocumentDTO : class, IDocumentDTO, new()
        where TRequest : Request, new()
        where TRepository : BaseListRepository<TDocumentDTO, TRequest>, new()
    {
        public abstract string ResourceID { get; }
        public TRepository Repository { get; set; }

        public BaseListApiController()
        {
            Repository = new TRepository();
            Repository.UserContext = new UserContext();
            Repository.UserContext.UserID = this.User.Identity.Name;
            if (String.IsNullOrWhiteSpace(Repository.UserContext.UserID))
                Repository.UserContext.UserID = "Foo";
        }

        [HttpPost]
        [Route("GetList")]
        [ResourceAccessType(AccessType.View)]
        public virtual Response GetList(TRequest request)
        {
            if (request == null)
                request = new TRequest();
            var __data = Repository.GetList(request);
            var __result = new Response
            {
                Data = __data,
                Total = request.TotalRows
            };
            return __result;
        }

        [HttpPost]
        [Route("Search")]
        [ResourceAccessType(AccessType.Search)]
        public virtual Response Search(TRequest request)
        {
            if (request == null)
                request = new TRequest();
            var __data = Repository.Search(request);
            var __result = new Response
            {
                Data = __data,
                Total = request.TotalRows
            };
            return __result;
        }

        [HttpGet]
        [Route("GetAccessType")]
        public virtual AccessTypeResponse GetAccessType()
        {
            throw new NotImplementedException();
        }

        public IHttpActionResult OkResult(object data)
        {
            var __response = new Response() { Data = data };
            return this.Content<Response>(HttpStatusCode.OK, __response);
        }

        public IHttpActionResult OkResult(Response response)
        {
            return this.Content<Response>(HttpStatusCode.OK, response);
        }

        public IHttpActionResult UnProcessableDataResult(Response response)
        {
            return this.Content<Response>((HttpStatusCode)422, response);
        }
    }
}