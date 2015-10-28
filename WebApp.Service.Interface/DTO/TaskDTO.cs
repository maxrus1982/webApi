using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Runtime.Serialization;
using Newtonsoft.Json;

using WebApp.Core;

namespace WebApp.Service.Interface
{
    public class TaskDTO : BaseDocumentDTO
    {
        //автомаппинг
        public virtual String User { get; set; }
        public virtual String Name { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual DateTime? BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime PlanBeginDate { get; set; }
        public virtual DateTime PlanEndDate { get; set; }
        public virtual Boolean IsCompleted { get; set; }

        //расчетное поле
        [ProjectionIgnore]
        public virtual Boolean IsOverdue { get; set; }
    }
}
