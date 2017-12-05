namespace Framework.FrameworkContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.InsertPostType",
                p => new
                    {
                        Name = p.String(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[PostType]([Name], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status], [Protected])
                      VALUES (@Name, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @Status, @Protected)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PostType]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PostType] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.UpdatePostType",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[PostType]
                      SET [Name] = @Name, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [UpdatedDate] = @UpdatedDate, [UpdatedBy] = @UpdatedBy, [Status] = @Status, [Protected] = @Protected
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DeletePostType",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[PostType]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.DeletePostType");
            DropStoredProcedure("dbo.UpdatePostType");
            DropStoredProcedure("dbo.InsertPostType");
            DropTable("dbo.PostType");
        }
    }
}
