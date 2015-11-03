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
        public IList<TaskGroupDTO> GetTaskGroupList(TaskRequest request)
        {
            request.Page = 1;
            request.Skip = 0;
            request.Take = Int32.MaxValue;
            var __rawList = this.GetList(request);
            if (__rawList.Count>0)
            {
                var __result = __rawList
                .GroupBy(x => new { State = x.State })
                .Select(x => new TaskGroupDTO() {
                    State = x.Key.State,
                    TasksCount = x.Count(),
                    TasksPercent = Math.Round((decimal)x.Count() * 100 / __rawList.Count(), 2)
                })
                .ToList();
                return __result;
            }
            return new List<TaskGroupDTO>();
        }

        protected override IQueryable<Task> BaseQuery()
        {
            var __expr = base.BaseQuery();
            return __expr.Where(x => x.User == this.UserContext.UserID);
        }

        protected override IQueryable<Task> Where(IQueryable<Task> expr, TaskRequest request)
        {
            var __expr = base.Where(expr, request);
            if (request.IgnoreCompletedTasks)
                __expr = __expr.Where(x => x.Completed == false);
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
            document.Completed = documentDTO.Completed;
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

            documentDTO.TodayTask = !documentDTO.Completed &&
                documentDTO.PlanEndDate != null &&
                documentDTO.PlanEndDate.Value.Date == DateTime.Now.Date;

            documentDTO.LaterTask = !documentDTO.Completed &&
                documentDTO.PlanEndDate != null &&
                documentDTO.PlanEndDate.Value.Date > DateTime.Now.Date;

            documentDTO.OverdueTask = !documentDTO.Completed &&
                documentDTO.PlanEndDate != null &&
                documentDTO.PlanEndDate.Value.Date < DateTime.Now.Date;

            if (documentDTO.Completed)
                documentDTO.State = TaskStateEnum.Completed;
            else if (documentDTO.TodayTask)
                documentDTO.State = TaskStateEnum.Today;
            else if (documentDTO.LaterTask)
                documentDTO.State = TaskStateEnum.Later;
            else if (documentDTO.OverdueTask)
                documentDTO.State = TaskStateEnum.Overdue;
            else
                documentDTO.State = TaskStateEnum.None;
        }
    }
}
