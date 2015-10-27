using System;

using WebApp.Domain.Interface;

namespace WebApp.Domain
{
    public class Task : IDocument
    {
        public Guid ID { get; set; }
        public String Name { get; set; }
    }
}