using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebApp.Domain.Interface;
using WebApp.Service.Interface;
using WebApp.DAL;

namespace WebApp.Service.Base
{
    public abstract class BaseListRepository<TDocumentDTO, TRequest>
        where TDocumentDTO : class, IDocumentDTO, new()
        where TRequest : Request, new()
    {
        protected virtual BaseContext Context { get; set; }

        public BaseListRepository(BaseContext context)
        {
            Context = context;
        }

        public virtual IList<TDocumentDTO> GetList(TRequest request)
        {
            var __result = this.Query(request).ToList();
            this.OnGetDocumentList(__result, request);
            return __result;
        }

        public virtual IList<TDocumentDTO> Search(TRequest request)
        {
            return GetList(request);
        }

        protected abstract IQueryable<TDocumentDTO> Query(TRequest request);
        protected virtual void OnGetDocumentList(IList<TDocumentDTO> documentDTOList, TRequest request) { }
    }
}
