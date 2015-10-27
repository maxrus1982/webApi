using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using WebApp.Domain;
using WebApp.DAL.Mapping;

namespace WebApp.DAL
{
    public class MainContext : DbContext
    {
        public IDbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new TaskMapper());
        }
    }
}