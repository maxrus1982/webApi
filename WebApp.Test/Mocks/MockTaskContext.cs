using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


using WebApp.Domain;
using WebApp.DAL.Mapping;
using WebApp.Domain.Interface;
using WebApp.Service.Interface;

namespace WebApp.Test
{
    public class MockTaskContext : MockBaseContext, ITaskContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new TaskMapper());
        }
    }
}
