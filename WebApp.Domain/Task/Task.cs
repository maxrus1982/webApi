using System;

using WebApp.Domain.Interface;

namespace WebApp.Domain
{
    public class Task : IDocument
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