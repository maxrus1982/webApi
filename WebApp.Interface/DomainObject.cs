using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Interface
{
    public interface DomainObject
    {
        Guid ID { get; set; }
    }
}
