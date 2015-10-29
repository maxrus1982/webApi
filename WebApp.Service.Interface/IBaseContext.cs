using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebApp.Domain.Interface;

namespace WebApp.Service.Interface
{
    public interface IBaseContext : IDisposable
    {
        IQueryable<TDocument> Query<TDocument>() where TDocument : class, IDocument, new();
        TDocument Add<TDocument>(TDocument document) where TDocument : class, IDocument, new();
        TDocument Remove<TDocument>(TDocument document) where TDocument : class, IDocument, new();
        int SaveChanges();
    }
}
