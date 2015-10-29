using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

using WebApp.DAL;

namespace WebApp.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<WebApp.DAL.Context.BaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebApp.DAL.Context.BaseContext context)
        {

        }
    }
}
