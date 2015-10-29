using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Domain.Interface
{
    /// <summary>
    /// Entity Document
    /// </summary>
    public interface IDocument
    {
        Guid ID { get; set; }
    }
}
