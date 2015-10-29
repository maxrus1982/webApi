using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using WebApp.Domain;
using WebApp.DAL.Mapping;
using WebApp.Domain.Interface;
using WebApp.Service.Interface;

namespace WebApp.DAL.Context
{
    public class TaskContext : BaseContext, ITaskContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new TaskMapper());
        }
    }
}