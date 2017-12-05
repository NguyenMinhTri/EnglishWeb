namespace Framework.FrameworkContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ah : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PostCommentDetail", "Id_Comment", c => c.Int(nullable: false));
            AddColumn("dbo.PostCommentDetail", "Content", c => c.String());
            AddColumn("dbo.Post", "Id_Type", c => c.Int(nullable: false));
            AddColumn("dbo.Post", "Option", c => c.Int(nullable: false));
            DropColumn("dbo.PostCommentDetail", "Comment");
            DropColumn("dbo.Post", "CodePostTypeId");
            DropColumn("dbo.Post", "Link");
            AlterStoredProcedure(
                "dbo.InsertPostCommentDetail",
                p => new
                    {
                        Id_Post = p.Int(),
                        Id_Comment = p.Int(),
                        Id_Friend = p.String(),
                        Content = p.String(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[PostCommentDetail]([Id_Post], [Id_Comment], [Id_Friend], [Content], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status], [Protected])
                      VALUES (@Id_Post, @Id_Comment, @Id_Friend, @Content, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @Status, @Protected)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PostCommentDetail]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PostCommentDetail] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.UpdatePostCommentDetail",
                p => new
                    {
                        Id = p.Int(),
                        Id_Post = p.Int(),
                        Id_Comment = p.Int(),
                        Id_Friend = p.String(),
                        Content = p.String(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[PostCommentDetail]
                      SET [Id_Post] = @Id_Post, [Id_Comment] = @Id_Comment, [Id_Friend] = @Id_Friend, [Content] = @Content, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [UpdatedDate] = @UpdatedDate, [UpdatedBy] = @UpdatedBy, [Status] = @Status, [Protected] = @Protected
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.InsertPost",
                p => new
                    {
                        Id_User = p.String(),
                        Content = p.String(),
                        Id_Type = p.Int(),
                        Option = p.Int(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Post]([Id_User], [Content], [Id_Type], [Option], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status], [Protected])
                      VALUES (@Id_User, @Content, @Id_Type, @Option, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @Status, @Protected)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Post]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Post] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.UpdatePost",
                p => new
                    {
                        Id = p.Int(),
                        Id_User = p.String(),
                        Content = p.String(),
                        Id_Type = p.Int(),
                        Option = p.Int(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Post]
                      SET [Id_User] = @Id_User, [Content] = @Content, [Id_Type] = @Id_Type, [Option] = @Option, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [UpdatedDate] = @UpdatedDate, [UpdatedBy] = @UpdatedBy, [Status] = @Status, [Protected] = @Protected
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Post", "Link", c => c.String());
            AddColumn("dbo.Post", "CodePostTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.PostCommentDetail", "Comment", c => c.String());
            DropColumn("dbo.Post", "Option");
            DropColumn("dbo.Post", "Id_Type");
            DropColumn("dbo.PostCommentDetail", "Content");
            DropColumn("dbo.PostCommentDetail", "Id_Comment");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
