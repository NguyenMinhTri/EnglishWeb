namespace Framework.FrameworkContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Avatar");
            DropColumn("dbo.AspNetUsers", "AboutUser");
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "Place");
            DropColumn("dbo.AspNetUsers", "Career");
            DropColumn("dbo.AspNetUsers", "Subject");
            DropColumn("dbo.AspNetUsers", "Logined1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Logined1", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Subject", c => c.String());
            AddColumn("dbo.AspNetUsers", "Career", c => c.String());
            AddColumn("dbo.AspNetUsers", "Place", c => c.String());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "AboutUser", c => c.String());
            AddColumn("dbo.AspNetUsers", "Avatar", c => c.String());
        }
    }
}
