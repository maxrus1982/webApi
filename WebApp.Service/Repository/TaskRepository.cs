using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

using WebApp.Domain;
using WebApp.Domain.Interface;
using WebApp.Service.Interface;
using WebApp.Core;
using WebApp.Service.TaskRequests;

namespace WebApp.Service
{
    public class TaskRepository : BaseDocumentRepository<Task, TaskDTO, TaskRequest, CreateTaskRequest>
    {
        protected override IQueryable<Task> BaseQuery()
        {
            var __expr = base.BaseQuery();
            return __expr.Where(x => x.User == this.UserContext.UserID);
        }

        protected override IQueryable<Task> Where(IQueryable<Task> expr, TaskRequest request)
        {
            var __expr = base.Where(expr, request);
            if (request.IngnoreCompletedTasks)
                __expr = __expr.Where(x => x.IsCompleted == false);
            return __expr;
        }

        protected override void BeforeSaveDocument(Task document, TaskDTO documentDTO, bool isNew)
        {
            base.BeforeSaveDocument(document, documentDTO, isNew);
            if (isNew)
            {
                document.User = UserContext.UserID;
                document.CreateDate = DateTime.Now;
            }
            document.Name = documentDTO.Name;
            document.BeginDate = documentDTO.BeginDate;
            document.EndDate = documentDTO.EndDate;
            document.PlanBeginDate = documentDTO.PlanBeginDate;
            document.PlanEndDate = documentDTO.PlanEndDate;
            document.IsCompleted = documentDTO.IsCompleted;
        }

        protected override void OnNewDocument(TaskDTO documentDTO, CreateTaskRequest request)
        {
            base.OnNewDocument(documentDTO, request);
            documentDTO.PlanBeginDate = request.PlanBeginDate;
            documentDTO.PlanEndDate = request.PlanEndDate;
        }

        protected override void OnGetDocument(TaskDTO documentDTO, TaskRequest request)
        {
            base.OnGetDocument(documentDTO, request);
            documentDTO.IsOverdue = documentDTO.IsCompleted && documentDTO.PlanEndDate < DateTime.Now;
        }
    }
}
