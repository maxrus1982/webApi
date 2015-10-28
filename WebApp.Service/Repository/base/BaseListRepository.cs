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
    //базовый репозиторий для простого списка
    public abstract class BaseListRepository<TDocumentDTO, TRequest> : IDisposable
        where TDocumentDTO : class, IDocumentDTO, new()
        where TRequest : Request, new()
    {
        protected virtual BaseContext DbContext { get; set; }
        public virtual UserContext UserContext { get; set; }

        public BaseListRepository(BaseContext dbContext)
        {
            DbContext = dbContext;
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

        #region Dispose

        private bool _disposed = false;
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (DbContext != null)
                    DbContext.Dispose();
            }

            _disposed = true;
        }

        ~BaseListRepository()
        {
            Dispose(false);
        }
        #endregion
    }
}
