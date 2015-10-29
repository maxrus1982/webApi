using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebApp.Service.Interface;

namespace WebApp.Service.TaskRequests
{
    public class TaskRequest : Request
    {
        public bool IngnoreCompletedTasks { get; set; }
    }

    public class CreateTaskRequest : CreateDocumentRequest
    {
        public DateTime PlanBeginDate { get; set; }
        public DateTime PlanEndDate { get; set; }
    }
}
