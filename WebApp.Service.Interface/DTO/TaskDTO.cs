using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;

using WebApp.Core;

namespace WebApp.Service.Interface
{
    [DataContract]
    [JsonObject(MemberSerialization.OptOut)]
    public class TaskDTO : BaseDocumentDTO
    {
        //автомаппинг
        public virtual String User { get; set; }
        public virtual String Name { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual DateTime? BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime? PlanBeginDate { get; set; }
        public virtual DateTime? PlanEndDate { get; set; }
        public virtual Boolean Completed { get; set; }

        [ProjectionIgnore]
        public virtual Boolean TodayTask { get; set; }

        [ProjectionIgnore]
        public virtual Boolean LaterTask { get; set; }

        [ProjectionIgnore]
        public virtual Boolean OverdueTask { get; set; }

        [ProjectionIgnore]
        public virtual TaskStateEnum State { get; set; }

        [ProjectionIgnore]
        public virtual String StateName { get { return EnumUtils<TaskStateEnum>.GetDescription(this.State); } }
    }

    public enum TaskStateEnum
    {
        [Description("Неизвестен")]
        None = 0,

        [Description("Завершен")]
        Completed = 1,

        [Description("На сегодня")]
        Today = 2,

        [Description("На завтра и позже")]
        Later = 3,

        [Description("Просрочен")]
        Overdue = 4
    }
}
