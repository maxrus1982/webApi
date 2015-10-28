using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WebApp.Domain;
using WebApp.Domain.Interface;
using WebApp.Service.Interface;

namespace WebApp.Service
{
    public class TaskRepository : BaseDocumentRepository<Task, TaskDTO, TaskRequest, CreateTaskRequest>
    {
        public TaskRepository()
            : base(new DAL.MainContext())
        {

        }

        protected override void BeforeSaveDocument(Task document, TaskDTO documentDTO, bool isNew)
        {
            base.BeforeSaveDocument(document, documentDTO, isNew);
            if (isNew)
            {
                if (UserContext != null)
                    document.User = UserContext.UserID;
            }
            document.Name = documentDTO.Name;
            document.CreateDate = documentDTO.CreateDate;
            document.BeginDate = documentDTO.BeginDate;
            document.EndDate = documentDTO.EndDate;
            document.PlanBeginDate = documentDTO.PlanBeginDate;
            document.PlanEndDate = documentDTO.PlanEndDate;
            document.IsCompleted = documentDTO.IsCompleted;
        }

        protected override void OnGetDocument(TaskDTO documentDTO, TaskRequest request)
        {
            base.OnGetDocument(documentDTO, request);
            documentDTO.IsOverdue = documentDTO.IsCompleted && documentDTO.PlanEndDate < DateTime.Now;
        }
    }
}
