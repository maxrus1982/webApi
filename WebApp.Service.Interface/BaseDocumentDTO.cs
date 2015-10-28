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
    [DataContract]
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class BaseDocumentDTO : IDocumentDTO
    {
        public virtual Guid ID { get; set; }
    }
}
