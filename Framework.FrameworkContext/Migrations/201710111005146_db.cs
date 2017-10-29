namespace Framework.FrameworkContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Code",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CommentVoteDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_Comment = c.Int(nullable: false),
                        Id_Friend = c.String(),
                        VoteAmout = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Friend",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_User = c.String(),
                        Id_Friend = c.String(),
                        CodeRelationshipId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_User = c.String(),
                        Id_Friend = c.String(),
                        Content = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_User = c.String(),
                        Id_Friend = c.String(),
                        CodeNotificationTypeId = c.Int(nullable: false),
                        Id_Post = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PostCommentDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_Post = c.Int(nullable: false),
                        Id_Friend = c.String(),
                        Comment = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Post",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_User = c.String(),
                        CodePostTypeId = c.Int(nullable: false),
                        Content = c.String(),
                        Link = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PostVoteDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_Post = c.Int(nullable: false),
                        Id_Friend = c.String(),
                        VoteAmout = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "About", c => c.String());
            AddColumn("dbo.AspNetUsers", "Level", c => c.String());
            AddColumn("dbo.AspNetUsers", "Birthday", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "Career", c => c.String());
            AddColumn("dbo.AspNetUsers", "Organization", c => c.String());
            AddColumn("dbo.AspNetUsers", "Sex", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "UserStatus", c => c.String());
            AddColumn("dbo.AspNetUsers", "Phone", c => c.String());
            AddColumn("dbo.AspNetUsers", "Facebook", c => c.String());
            AddColumn("dbo.AspNetUsers", "Google", c => c.String());
            AddColumn("dbo.AspNetUsers", "Hobby", c => c.String());
            AddColumn("dbo.AspNetUsers", "FavoriteShow", c => c.String());
            AddColumn("dbo.AspNetUsers", "FavoriteFilm", c => c.String());
            AddColumn("dbo.AspNetUsers", "FavoriteGame", c => c.String());
            AddColumn("dbo.AspNetUsers", "FavoriteArtist", c => c.String());
            AddColumn("dbo.AspNetUsers", "FavoriteBook", c => c.String());
            AddColumn("dbo.AspNetUsers", "FavoriteAuthor", c => c.String());
            AddColumn("dbo.AspNetUsers", "Avatar", c => c.String());
            AddColumn("dbo.AspNetUsers", "Score", c => c.Single(nullable: false));
            CreateStoredProcedure(
                "dbo.InsertCode",
                p => new
                    {
                        Name = p.String(),
                        Value = p.String(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Code]([Name], [Value], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status], [Protected])
                      VALUES (@Name, @Value, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @Status, @Protected)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Code]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Code] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.UpdateCode",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(),
                        Value = p.String(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Code]
                      SET [Name] = @Name, [Value] = @Value, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [UpdatedDate] = @UpdatedDate, [UpdatedBy] = @UpdatedBy, [Status] = @Status, [Protected] = @Protected
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DeleteCode",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Code]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.InsertCommentVoteDetail",
                p => new
                    {
                        Id_Comment = p.Int(),
                        Id_Friend = p.String(),
                        VoteAmout = p.Int(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[CommentVoteDetail]([Id_Comment], [Id_Friend], [VoteAmout], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status], [Protected])
                      VALUES (@Id_Comment, @Id_Friend, @VoteAmout, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @Status, @Protected)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CommentVoteDetail]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CommentVoteDetail] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.UpdateCommentVoteDetail",
                p => new
                    {
                        Id = p.Int(),
                        Id_Comment = p.Int(),
                        Id_Friend = p.String(),
                        VoteAmout = p.Int(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[CommentVoteDetail]
                      SET [Id_Comment] = @Id_Comment, [Id_Friend] = @Id_Friend, [VoteAmout] = @VoteAmout, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [UpdatedDate] = @UpdatedDate, [UpdatedBy] = @UpdatedBy, [Status] = @Status, [Protected] = @Protected
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DeleteCommentVoteDetail",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CommentVoteDetail]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.InsertFriend",
                p => new
                    {
                        Id_User = p.String(),
                        Id_Friend = p.String(),
                        CodeRelationshipId = p.Int(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Friend]([Id_User], [Id_Friend], [CodeRelationshipId], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status], [Protected])
                      VALUES (@Id_User, @Id_Friend, @CodeRelationshipId, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @Status, @Protected)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Friend]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Friend] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.UpdateFriend",
                p => new
                    {
                        Id = p.Int(),
                        Id_User = p.String(),
                        Id_Friend = p.String(),
                        CodeRelationshipId = p.Int(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Friend]
                      SET [Id_User] = @Id_User, [Id_Friend] = @Id_Friend, [CodeRelationshipId] = @CodeRelationshipId, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [UpdatedDate] = @UpdatedDate, [UpdatedBy] = @UpdatedBy, [Status] = @Status, [Protected] = @Protected
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DeleteFriend",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Friend]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.InsertMessage",
                p => new
                    {
                        Id_User = p.String(),
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
                    @"INSERT [dbo].[Message]([Id_User], [Id_Friend], [Content], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status], [Protected])
                      VALUES (@Id_User, @Id_Friend, @Content, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @Status, @Protected)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Message]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Message] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.UpdateMessage",
                p => new
                    {
                        Id = p.Int(),
                        Id_User = p.String(),
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
                    @"UPDATE [dbo].[Message]
                      SET [Id_User] = @Id_User, [Id_Friend] = @Id_Friend, [Content] = @Content, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [UpdatedDate] = @UpdatedDate, [UpdatedBy] = @UpdatedBy, [Status] = @Status, [Protected] = @Protected
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DeleteMessage",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Message]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.InsertNotification",
                p => new
                    {
                        Id_User = p.String(),
                        Id_Friend = p.String(),
                        CodeNotificationTypeId = p.Int(),
                        Id_Post = p.Int(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Notification]([Id_User], [Id_Friend], [CodeNotificationTypeId], [Id_Post], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status], [Protected])
                      VALUES (@Id_User, @Id_Friend, @CodeNotificationTypeId, @Id_Post, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @Status, @Protected)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Notification]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Notification] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.UpdateNotification",
                p => new
                    {
                        Id = p.Int(),
                        Id_User = p.String(),
                        Id_Friend = p.String(),
                        CodeNotificationTypeId = p.Int(),
                        Id_Post = p.Int(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Notification]
                      SET [Id_User] = @Id_User, [Id_Friend] = @Id_Friend, [CodeNotificationTypeId] = @CodeNotificationTypeId, [Id_Post] = @Id_Post, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [UpdatedDate] = @UpdatedDate, [UpdatedBy] = @UpdatedBy, [Status] = @Status, [Protected] = @Protected
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DeleteNotification",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Notification]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.InsertPostCommentDetail",
                p => new
                    {
                        Id_Post = p.Int(),
                        Id_Friend = p.String(),
                        Comment = p.String(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[PostCommentDetail]([Id_Post], [Id_Friend], [Comment], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status], [Protected])
                      VALUES (@Id_Post, @Id_Friend, @Comment, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @Status, @Protected)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PostCommentDetail]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PostCommentDetail] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.UpdatePostCommentDetail",
                p => new
                    {
                        Id = p.Int(),
                        Id_Post = p.Int(),
                        Id_Friend = p.String(),
                        Comment = p.String(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[PostCommentDetail]
                      SET [Id_Post] = @Id_Post, [Id_Friend] = @Id_Friend, [Comment] = @Comment, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [UpdatedDate] = @UpdatedDate, [UpdatedBy] = @UpdatedBy, [Status] = @Status, [Protected] = @Protected
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DeletePostCommentDetail",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[PostCommentDetail]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.InsertPost",
                p => new
                    {
                        Id_User = p.String(),
                        CodePostTypeId = p.Int(),
                        Content = p.String(),
                        Link = p.String(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Post]([Id_User], [CodePostTypeId], [Content], [Link], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status], [Protected])
                      VALUES (@Id_User, @CodePostTypeId, @Content, @Link, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @Status, @Protected)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Post]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Post] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.UpdatePost",
                p => new
                    {
                        Id = p.Int(),
                        Id_User = p.String(),
                        CodePostTypeId = p.Int(),
                        Content = p.String(),
                        Link = p.String(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Post]
                      SET [Id_User] = @Id_User, [CodePostTypeId] = @CodePostTypeId, [Content] = @Content, [Link] = @Link, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [UpdatedDate] = @UpdatedDate, [UpdatedBy] = @UpdatedBy, [Status] = @Status, [Protected] = @Protected
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DeletePost",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Post]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.InsertPostVoteDetail",
                p => new
                    {
                        Id_Post = p.Int(),
                        Id_Friend = p.String(),
                        VoteAmout = p.Int(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[PostVoteDetail]([Id_Post], [Id_Friend], [VoteAmout], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status], [Protected])
                      VALUES (@Id_Post, @Id_Friend, @VoteAmout, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @Status, @Protected)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PostVoteDetail]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PostVoteDetail] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.UpdatePostVoteDetail",
                p => new
                    {
                        Id = p.Int(),
                        Id_Post = p.Int(),
                        Id_Friend = p.String(),
                        VoteAmout = p.Int(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[PostVoteDetail]
                      SET [Id_Post] = @Id_Post, [Id_Friend] = @Id_Friend, [VoteAmout] = @VoteAmout, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [UpdatedDate] = @UpdatedDate, [UpdatedBy] = @UpdatedBy, [Status] = @Status, [Protected] = @Protected
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DeletePostVoteDetail",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[PostVoteDetail]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.DeletePostVoteDetail");
            DropStoredProcedure("dbo.UpdatePostVoteDetail");
            DropStoredProcedure("dbo.InsertPostVoteDetail");
            DropStoredProcedure("dbo.DeletePost");
            DropStoredProcedure("dbo.UpdatePost");
            DropStoredProcedure("dbo.InsertPost");
            DropStoredProcedure("dbo.DeletePostCommentDetail");
            DropStoredProcedure("dbo.UpdatePostCommentDetail");
            DropStoredProcedure("dbo.InsertPostCommentDetail");
            DropStoredProcedure("dbo.DeleteNotification");
            DropStoredProcedure("dbo.UpdateNotification");
            DropStoredProcedure("dbo.InsertNotification");
            DropStoredProcedure("dbo.DeleteMessage");
            DropStoredProcedure("dbo.UpdateMessage");
            DropStoredProcedure("dbo.InsertMessage");
            DropStoredProcedure("dbo.DeleteFriend");
            DropStoredProcedure("dbo.UpdateFriend");
            DropStoredProcedure("dbo.InsertFriend");
            DropStoredProcedure("dbo.DeleteCommentVoteDetail");
            DropStoredProcedure("dbo.UpdateCommentVoteDetail");
            DropStoredProcedure("dbo.InsertCommentVoteDetail");
            DropStoredProcedure("dbo.DeleteCode");
            DropStoredProcedure("dbo.UpdateCode");
            DropStoredProcedure("dbo.InsertCode");
            DropColumn("dbo.AspNetUsers", "Score");
            DropColumn("dbo.AspNetUsers", "Avatar");
            DropColumn("dbo.AspNetUsers", "FavoriteAuthor");
            DropColumn("dbo.AspNetUsers", "FavoriteBook");
            DropColumn("dbo.AspNetUsers", "FavoriteArtist");
            DropColumn("dbo.AspNetUsers", "FavoriteGame");
            DropColumn("dbo.AspNetUsers", "FavoriteFilm");
            DropColumn("dbo.AspNetUsers", "FavoriteShow");
            DropColumn("dbo.AspNetUsers", "Hobby");
            DropColumn("dbo.AspNetUsers", "Google");
            DropColumn("dbo.AspNetUsers", "Facebook");
            DropColumn("dbo.AspNetUsers", "Phone");
            DropColumn("dbo.AspNetUsers", "UserStatus");
            DropColumn("dbo.AspNetUsers", "Sex");
            DropColumn("dbo.AspNetUsers", "Organization");
            DropColumn("dbo.AspNetUsers", "Career");
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "Birthday");
            DropColumn("dbo.AspNetUsers", "Level");
            DropColumn("dbo.AspNetUsers", "About");
            DropTable("dbo.PostVoteDetail");
            DropTable("dbo.Post");
            DropTable("dbo.PostCommentDetail");
            DropTable("dbo.Notification");
            DropTable("dbo.Message");
            DropTable("dbo.Friend");
            DropTable("dbo.CommentVoteDetail");
            DropTable("dbo.Code");
        }
    }
}
