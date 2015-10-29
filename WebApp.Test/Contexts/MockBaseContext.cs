using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebApp.Domain;
using WebApp.DAL.Mapping;
using WebApp.Domain.Interface;
using WebApp.Service.Interface;

namespace WebApp.Test
{
    public class MockBaseContext : IBaseContext
    {
        public virtual IQueryable<TDocument> Query<TDocument>()
            where TDocument : class, IDocument, new()
        {
            return null;
        }

        public virtual TDocument Add<TDocument>(TDocument document)
            where TDocument : class, IDocument, new()
        {
            return null;
        }

        public virtual TDocument Remove<TDocument>(TDocument document)
            where TDocument : class, IDocument, new()
        {
            return null;
        }

        public virtual int SaveChanges()
        {
            return 0;
        }

        public virtual void Dispose()
        {

        }
    }
}
