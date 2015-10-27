using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Service.Interface
{
    public class TaskDTO : IDocumentDTO
    {
        public Guid ID { get; set; }
        public String Name { get; set; }
    }
}
