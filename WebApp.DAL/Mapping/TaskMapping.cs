using System;
using System.Data.Entity.ModelConfiguration;

using WebApp.Domain;

namespace WebApp.DAL.Mapping
{
    public class TaskMapper : EntityTypeConfiguration<Task>
    {
        public TaskMapper()
        {
            ToTable("Tasks");
            Property(p => p.Name);
            Property(p => p.User);
            Property(p => p.CreateDate);
            Property(p => p.BeginDate);
            Property(p => p.EndDate);
            Property(p => p.Completed);
        }
    }
}
