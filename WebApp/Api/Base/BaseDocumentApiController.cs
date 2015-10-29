using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using WebApp.Service.Interface;
using WebApp.Service;
using WebApp.Domain.Interface;
using WebApp.Core;

namespace WebApp.Api
{
    /// <summary>
    /// Базовы ApiController для реализации REST по документу
    /// </summary>
    [RoutePrefix(AreaConsts.ApiArea + "/BaseDocument")]
    [ResourceAccessType(AccessType.Search)]
    public abstract class BaseDocumentApiController<TDocument, TDocumentDTO, TRequest, TCreateDocumentRequest, TRepository, TValidator> : BaseListApiController<TDocumentDTO, TRequest, TRepository>
        where TDocument : class, IDocument, new()
        where TDocumentDTO : class, IDocumentDTO, new()
        where TRequest : class, IRequest, new()
        where TCreateDocumentRequest : class, ICreateDocumentRequest, new()
        where TRepository : BaseDocumentRepository<TDocument, TDocumentDTO, TRequest, TCreateDocumentRequest>, new()
        where TValidator : BaseDocumentValidator<TDocumentDTO>, new()
    {
        [HttpGet]
        [Route("Get")]
        [ResourceAccessType(AccessType.View)]
        public virtual IHttpActionResult Get(Guid id)
        {
            var __documentDTO = Repository.Get(id);
            return OkResult(__documentDTO);
        }

        [HttpPost]
        [Route("New")]
        [ResourceAccessType(AccessType.Create)]
        public virtual IHttpActionResult New(TCreateDocumentRequest request)
        {
            var __documentDTO = Repository.New(request);
            return OkResult(__documentDTO);
        }

        [HttpPost]
        [Route("Post")]
        [ResourceAccessType(AccessType.ModifyAccess | AccessType.Create)]
        public virtual IHttpActionResult Post(TDocumentDTO documentDTO)
        {
            ModelState.Clear();
            if (new TValidator().Validate(documentDTO, ModelState))
            {
                var __documentDTO = Repository.Post(documentDTO);
                return OkResult(__documentDTO);
            }
            else
            {
                var __result = Response.CreateErrorResponse(documentDTO, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList());
                return UnProcessableDataResult(__result);
            }
        }

        [HttpPost]
        [Route("Remove")]
        [ResourceAccessType(AccessType.Remove)]
        public virtual IHttpActionResult Remove(TRequest request)
        {
            var __resultOper = Repository.Remove(request.ID.GetValueOrDefault());
            var __response = new Response() {
                Success = __resultOper
            }; ;
            return OkResult(__response);
        }
    }
}