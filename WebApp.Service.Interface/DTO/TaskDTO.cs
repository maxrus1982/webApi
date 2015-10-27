using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Service.Interface
{
    public class TaskDTO : IDocumentDTO
    {
        public virtual Guid ID { get; set; }
        public virtual String User { get; set; }
        public virtual String Name { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual DateTime? BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual Boolean IsCompleted { get; set; }
    }
}
