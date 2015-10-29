using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using WebApp.Domain;
using WebApp.DAL.Mapping;
using WebApp.Domain.Interface;
using WebApp.Service.Interface;

namespace WebApp.Test
{
    public class MockBaseContext : DbContext, IBaseContext
    {
        public MockBaseContext() : base()
        {

        }

        public virtual IQueryable<TDocument> Query<TDocument>()
            where TDocument : class, IDocument, new()
        {
            return this.Set<TDocument>();
        }

        public virtual TDocument Add<TDocument>(TDocument document)
            where TDocument : class, IDocument, new()
        {
            return this.Set<TDocument>().Add(document);
        }

        public virtual TDocument Remove<TDocument>(TDocument document)
            where TDocument : class, IDocument, new()
        {
            return this.Set<TDocument>().Remove(document);
        }
    }
}
