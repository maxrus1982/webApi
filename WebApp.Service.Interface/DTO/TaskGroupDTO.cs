using System;
using System.ComponentModel;
using System.Reflection;
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
    [DataContract]
    [JsonObject(MemberSerialization.OptOut)]
    public class TaskGroupDTO
    {
        public virtual TaskStateEnum State { get; set; }
        public virtual Int64 TasksCount { get; set; }
        public virtual Decimal TasksPercent { get; set; }

        public virtual String StateName { get { return EnumUtils<TaskStateEnum>.GetDescription(this.State); } }
    }    
}
