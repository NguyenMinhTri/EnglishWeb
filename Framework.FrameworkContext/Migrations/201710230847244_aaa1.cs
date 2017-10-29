namespace Framework.FrameworkContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaa1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "UserStatus", c => c.Boolean(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "UserStatus", c => c.String());
        }
    }
}
