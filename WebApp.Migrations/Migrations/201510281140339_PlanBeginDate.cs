namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlanBeginDate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        User = c.String(),
                        Name = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        BeginDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        PlanBeginDate = c.DateTime(nullable: false),
                        PlanEndDate = c.DateTime(nullable: false),
                        IsCompleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tasks");
        }
    }
}
