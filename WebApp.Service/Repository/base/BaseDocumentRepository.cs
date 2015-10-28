using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebApp.Domain.Interface;
using WebApp.Service.Interface;
using WebApp.DAL;
using WebApp.Core;

namespace WebApp.Service
{
    //базовый репозиторий для полного REST по документу
    public abstract class BaseDocumentRepository<TDocument, TDocumentDTO, TRequest, TCreateDocumentRequest> : BaseListRepository<TDocumentDTO, TRequest>
        where TDocument : class, IDocument, new()
        where TDocumentDTO : class, IDocumentDTO, new()
        where TRequest : Request, new()
        where TCreateDocumentRequest : CreateDocumentRequest, new()
    {
        public BaseDocumentRepository(BaseContext dbContext)
            : base(dbContext)
        {

        }

        //REST
        public override IList<TDocumentDTO> GetList(TRequest request)
        {
            var __result = base.GetList(request);
            __result.ForEach(x =>
            {
                this.OnGetDocument(x, request);
            });
            return __result;
        }

        public virtual TDocumentDTO Get(Guid documentID)
        {
            if (documentID == Guid.Empty)
                return default(TDocumentDTO);
            var __documentDTO = this.BaseQuery().Where(o => o.ID == documentID).Map<TDocument, TDocumentDTO>().FirstOrDefault();
            if (__documentDTO != null)
            {
                this.OnGetDocument(__documentDTO, null);
                this.OnGetDocumentList(new List<TDocumentDTO> { __documentDTO }, null);
            }
            return __documentDTO;
        }

        public virtual TDocumentDTO New(TCreateDocumentRequest request)
        {
            var __documentDTO = new TDocumentDTO();
            OnNewDocument(__documentDTO, request);
            return __documentDTO;
        }

        public virtual TDocumentDTO Post(TDocumentDTO documentDTO)
        {
            if (documentDTO == null) return null;
            TDocument __document = this.BaseQuery().Where(x => x.ID == documentDTO.ID).FirstOrDefault();
            bool __isNewRecord = __document == null;
            if (__isNewRecord)
            {
                __document = new TDocument()
                {
                    ID = documentDTO.ID
                };
            }
            BeforeSaveDocument(__document, documentDTO, __isNewRecord);
            __document = __isNewRecord ? DbContext.Set<TDocument>().Add(__document) : null;
            AfterSaveDocument(__document, documentDTO, __isNewRecord);
            return this.Get(__document.ID);
        }

        public virtual bool Remove(Guid documentID)
        {
            var __doc = this.GetByID(documentID);
            var __docDTO = Get(documentID);
            BeforeDeleteDocument(__doc, __docDTO);
            DbContext.Set<TDocument>().Remove(__doc);
            AfterDeleteDocument(__doc, __docDTO);
            return true;
        }

        //to be override
        protected virtual IQueryable<TDocument> BaseQuery()
        {
            return DbContext.Query<TDocument>();
        }

        protected virtual TDocument GetByID(Guid documentID)
        {
            return this.BaseQuery().Where(o => o.ID == documentID).FirstOrDefault(); ;
        }

        protected override IQueryable<TDocumentDTO> Query(TRequest request)
        {
            var __queryExpr = BaseQuery();
            __queryExpr = this.Where(__queryExpr, request);
            if (request.Sort == null || request.Sort.Count == 0)
            {
                __queryExpr = __queryExpr.OrderBy(x => x.ID);
            }

            var __result = __queryExpr.Map<TDocument, TDocumentDTO>();
            __result = FilteringService.ByRequest<TDocumentDTO, TRequest>(__result, request);

            return __result;
        }

        protected virtual IQueryable<TDocument> Where(IQueryable<TDocument> expr, TRequest request)
        {
            var __result = expr;
            if (request != null)
            {
                if (request.ID.GetValueOrDefault() != Guid.Empty)
                    __result = __result.Where(x => x.ID == request.ID.Value);
            }
            return __result;
        }        

        //HANDLERS
        protected virtual void OnGetDocument(TDocumentDTO documentDTO, TRequest request)
        {

        }

        protected virtual void OnNewDocument(TDocumentDTO documentDTO, TCreateDocumentRequest request)
        {

        }

        protected virtual void BeforeSaveDocument(TDocument document, TDocumentDTO documentDTO, bool isNew)
        {

        }

        protected virtual void AfterSaveDocument(TDocument document, TDocumentDTO documentDTO, bool isNew)
        {

        }

        protected virtual void BeforeDeleteDocument(TDocument document, TDocumentDTO documentDTO)
        {

        }

        protected virtual void AfterDeleteDocument(TDocument document, TDocumentDTO documentDTO)
        {

        }
    }
}
