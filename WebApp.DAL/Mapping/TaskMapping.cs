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
        }
    }
}
