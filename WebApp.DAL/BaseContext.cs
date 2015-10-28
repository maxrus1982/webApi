using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using WebApp.Domain;
using WebApp.DAL.Mapping;
using WebApp.Domain.Interface;
using WebApp.Service.Interface;

namespace WebApp.DAL
{
    public class BaseContext : DbContext
    {
        public virtual IQueryable<TDocument> Query<TDocument>()
            where TDocument : class, IDocument, new()
        {
            return this.Set<TDocument>();
        }
    }
}