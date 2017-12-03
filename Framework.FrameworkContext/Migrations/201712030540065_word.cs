namespace Framework.FrameworkContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class word : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DetailOurWord",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_OurWord = c.Int(nullable: false),
                        Id_User = c.String(),
                        Id_Messenger = c.String(),
                        Learned = c.Int(nullable: false),
                        Schedule = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OurWord",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Word = c.String(),
                        Pronounciation = c.String(),
                        MeanVi = c.String(),
                        MeanEn = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        Protected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.InsertDetailOurWord",
                p => new
                    {
                        Id_OurWord = p.Int(),
                        Id_User = p.String(),
                        Id_Messenger = p.String(),
                        Learned = p.Int(),
                        Schedule = p.DateTime(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[DetailOurWord]([Id_OurWord], [Id_User], [Id_Messenger], [Learned], [Schedule], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status], [Protected])
                      VALUES (@Id_OurWord, @Id_User, @Id_Messenger, @Learned, @Schedule, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @Status, @Protected)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[DetailOurWord]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[DetailOurWord] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.UpdateDetailOurWord",
                p => new
                    {
                        Id = p.Int(),
                        Id_OurWord = p.Int(),
                        Id_User = p.String(),
                        Id_Messenger = p.String(),
                        Learned = p.Int(),
                        Schedule = p.DateTime(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[DetailOurWord]
                      SET [Id_OurWord] = @Id_OurWord, [Id_User] = @Id_User, [Id_Messenger] = @Id_Messenger, [Learned] = @Learned, [Schedule] = @Schedule, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [UpdatedDate] = @UpdatedDate, [UpdatedBy] = @UpdatedBy, [Status] = @Status, [Protected] = @Protected
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DeleteDetailOurWord",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[DetailOurWord]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.InsertOurWord",
                p => new
                    {
                        Word = p.String(),
                        Pronounciation = p.String(),
                        MeanVi = p.String(),
                        MeanEn = p.String(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[OurWord]([Word], [Pronounciation], [MeanVi], [MeanEn], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status], [Protected])
                      VALUES (@Word, @Pronounciation, @MeanVi, @MeanEn, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @Status, @Protected)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[OurWord]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[OurWord] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.UpdateOurWord",
                p => new
                    {
                        Id = p.Int(),
                        Word = p.String(),
                        Pronounciation = p.String(),
                        MeanVi = p.String(),
                        MeanEn = p.String(),
                        CreatedDate = p.DateTime(),
                        CreatedBy = p.String(maxLength: 256),
                        UpdatedDate = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 256),
                        Status = p.Boolean(),
                        Protected = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[OurWord]
                      SET [Word] = @Word, [Pronounciation] = @Pronounciation, [MeanVi] = @MeanVi, [MeanEn] = @MeanEn, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [UpdatedDate] = @UpdatedDate, [UpdatedBy] = @UpdatedBy, [Status] = @Status, [Protected] = @Protected
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DeleteOurWord",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[OurWord]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.DeleteOurWord");
            DropStoredProcedure("dbo.UpdateOurWord");
            DropStoredProcedure("dbo.InsertOurWord");
            DropStoredProcedure("dbo.DeleteDetailOurWord");
            DropStoredProcedure("dbo.UpdateDetailOurWord");
            DropStoredProcedure("dbo.InsertDetailOurWord");
            DropTable("dbo.OurWord");
            DropTable("dbo.DetailOurWord");
        }
    }
}
