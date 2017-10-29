namespace Framework.FrameworkContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aeeeee : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "UserStatus", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "UserStatus", c => c.Boolean(nullable: false));
        }
    }
}
