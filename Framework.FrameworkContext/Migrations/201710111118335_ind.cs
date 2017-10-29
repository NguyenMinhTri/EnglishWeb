namespace Framework.FrameworkContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ind : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Post", new[] { "Status", "CreatedDate" }, name: "IX_Post");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Post", "IX_Post");
        }
    }
}
