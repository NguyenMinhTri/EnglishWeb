namespace Framework.FrameworkContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Routings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FriendlyUrl = c.String(),
                        Controller = c.String(),
                        Action = c.String(),
                        EntityId = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.__MigrationHistory",
                c => new
                    {
                        Migration_ID = c.String(nullable: false, maxLength: 128),
                        ContextKey = c.String(),
                        Model = c.Binary(),
                        ProductVersion = c.String(),
                    })
                .PrimaryKey(t => t.Migration_ID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        Name = c.String(),
                        Avatar = c.String(),
                        AboutUser = c.String(),
                        Address = c.String(),
                        Place = c.String(),
                        Career = c.String(),
                        Subject = c.String(),
                        Logined = c.Boolean(nullable: false),
                        Logined1 = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(),
                        AccessFailedCount = c.Int(),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Id = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.String(maxLength: 256),
                        UserId = c.String(maxLength: 256),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                        Status = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        ApplicationRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetRoles", t => t.ApplicationRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationRole_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                        Status = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.InsertRouting",
                p => new
                    {
                        FriendlyUrl = p.String(),
                        Controller = p.String(),
                        Action = p.String(),
                        EntityId = p.String(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Routings]([FriendlyUrl], [Controller], [Action], [EntityId], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status], [Protected])
                      VALUES (@FriendlyUrl, @Controller, @Action, @EntityId, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @Status, @Protected)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Routings]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Routings] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.UpdateRouting",
                p => new
                    {
                        Id = p.Int(),
                        FriendlyUrl = p.String(),
                        Controller = p.String(),
                        Action = p.String(),
                        EntityId = p.String(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Routings]
                      SET [FriendlyUrl] = @FriendlyUrl, [Controller] = @Controller, [Action] = @Action, [EntityId] = @EntityId, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [UpdatedDate] = @UpdatedDate, [UpdatedBy] = @UpdatedBy, [Status] = @Status, [Protected] = @Protected
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DeleteRouting",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Routings]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.DeleteRouting");
            DropStoredProcedure("dbo.UpdateRouting");
            DropStoredProcedure("dbo.InsertRouting");
            DropForeignKey("dbo.AspNetUserRoles", "ApplicationRole_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserRoles", new[] { "ApplicationRole_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "ApplicationUser_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.__MigrationHistory");
            DropTable("dbo.Routings");
        }
    }
}
