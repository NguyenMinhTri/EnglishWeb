using Framework.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Framework.FrameworkContext
{
    public class FrameworkDbContext : DbContext
    {
        public FrameworkDbContext()
            : base("DbConnection")
        {
        }
        #region Table in database

        //TODO 1_ThemBang: st2. Thêm DbSet<Lớp mới tạo ở bước 1> vào đây
        // Ví dụ ở đây là public DbSet<Comment> Comments { set; get; }
        public DbSet<Friend> Friends { set; get; }
        public DbSet<Notification> Notifications { set; get; }
        public DbSet<Post> Posts { set; get; }
        public DbSet<Comment> Comments { set; get; }
        public DbSet<PostVoteDetail> PostVoteDetails { set; get; }
        public DbSet<CommentVoteDetail> CommentVoteDetails { set; get; }
        public DbSet<Routing> Routings { set; get; }
        public DbSet<DetailOurWord> DetailOurWords { set; get; }
        public DbSet<PostType> PostTypes { set; get; }
        public DbSet<DetailUserType> DetailUserTypes { set; get; }
        public DbSet<HaveSendQuestion> HaveSendQuestions { set; get; }
        public DbSet<DictCache> DictCaches { set; get; }
        public DbSet<SubType> SubTypes { set; get; }
        public DbSet<LearnVoca> LearnVocas { set; get; }
        public DbSet<ToiecGroup> ToiecGroups { set; get; }
        #endregion
        public static FrameworkDbContext Create()
        {
            return new FrameworkDbContext();
        }

        public static string schema
        {
            get
            {
                return "dbo";
            }
        }


        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema(schema);
            builder.Entity<HistoryRow>().HasKey(i => new { i.MigrationId }).ToTable(tableName: "__MigrationHistory", schemaName: schema);
            builder.Entity<HistoryRow>().Property(p => p.MigrationId).HasColumnName("Migration_ID");
            builder.Entity<ApplicationUser>().HasKey(i => new { i.Id }).ToTable("AspNetUsers");
            builder.Entity<ApplicationUser>().Property(x => x.AccessFailedCount).IsOptional();
            builder.Entity<ApplicationUser>().Property(x => x.LockoutEnabled).IsOptional();
            builder.Entity<ApplicationUser>().Property(x => x.EmailConfirmed).IsOptional();
            builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("AspNetUserLogins");
            builder.Entity<ApplicationRole>().ToTable("AspNetRoles");
            builder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("AspNetUserClaims");
            builder.Entity<ApplicationUserRole>().ToTable("AspNetUserRoles");

            // TODO 1_ThemBang: st3. Thêm đoạn code tương tự như sau để thêm store procedure
            //builder.Entity<Topic>().MapToStoredProcedures(s => s.Insert(u => u.HasName("InsertTopic", schema))
            //                                       .Update(u => u.HasName("UpdateTopic", schema))
            //                                       .Delete(u => u.HasName("DeleteTopic", schema)));

            builder.Entity<Routing>().MapToStoredProcedures(s => s
           .Insert(u => u.HasName("InsertRouting", schema))
           .Update(u => u.HasName("UpdateRouting", schema))
           .Delete(u => u.HasName("DeleteRouting", schema)));
            builder.Entity<Friend>().MapToStoredProcedures(s => s
                .Insert(u => u.HasName("InsertFriend", "dbo"))
                .Update(u => u.HasName("UpdateFriend", "dbo"))
                .Delete(u => u.HasName("DeleteFriend", "dbo")));
            builder.Entity<Notification>().MapToStoredProcedures(s => s
                .Insert(u => u.HasName("InsertNotification", "dbo"))
                .Update(u => u.HasName("UpdateNotification", "dbo"))
                .Delete(u => u.HasName("DeleteNotification", "dbo")));
            builder.Entity<Post>().MapToStoredProcedures(s => s
                .Insert(u => u.HasName("InsertPost", "dbo"))
                .Update(u => u.HasName("UpdatePost", "dbo"))
                .Delete(u => u.HasName("DeletePost", "dbo")));
            builder.Entity<Post>().HasIndex("IX_Post",
                       e => e.Property(x => x.Status),
                       e => e.Property(x => x.CreatedDate));
            builder.Entity<Comment>().MapToStoredProcedures(s => s
                .Insert(u => u.HasName("InsertComment", "dbo"))
                .Update(u => u.HasName("UpdateComment", "dbo"))
                .Delete(u => u.HasName("DeleteComment", "dbo")));
            builder.Entity<PostVoteDetail>().MapToStoredProcedures(s => s
                .Insert(u => u.HasName("InsertPostVoteDetail", "dbo"))
                .Update(u => u.HasName("UpdatePostVoteDetail", "dbo"))
                .Delete(u => u.HasName("DeletePostVoteDetail", "dbo")));
            builder.Entity<CommentVoteDetail>().MapToStoredProcedures(s => s
                .Insert(u => u.HasName("InsertCommentVoteDetail", "dbo"))
                .Update(u => u.HasName("UpdateCommentVoteDetail", "dbo"))
                .Delete(u => u.HasName("DeleteCommentVoteDetail", "dbo")));
            builder.Entity<DetailOurWord>().MapToStoredProcedures(s => s
                .Insert(u => u.HasName("InsertDetailOurWord", "dbo"))
                .Update(u => u.HasName("UpdateDetailOurWord", "dbo"))
                .Delete(u => u.HasName("DeleteDetailOurWord", "dbo")));
            builder.Entity<PostType>().MapToStoredProcedures(s => s
                .Insert(u => u.HasName("InsertPostType", "dbo"))
                .Update(u => u.HasName("UpdatePostType", "dbo"))
                .Delete(u => u.HasName("DeletePostType", "dbo")));
            builder.Entity<DetailUserType>().MapToStoredProcedures(s => s
                .Insert(u => u.HasName("InsertDetailUserType", "dbo"))
                .Update(u => u.HasName("UpdateDetailUserType", "dbo"))
                .Delete(u => u.HasName("DeleteDetailUserType", "dbo")));
            builder.Entity<HaveSendQuestion>().MapToStoredProcedures(s => s
                .Insert(u => u.HasName("InsertHaveSendQuestion", "dbo"))
                .Update(u => u.HasName("UpdateHaveSendQuestion", "dbo"))
                .Delete(u => u.HasName("DeleteHaveSendQuestion", "dbo")));
            builder.Entity<DictCache>().MapToStoredProcedures(s => s
                .Insert(u => u.HasName("InsertDictCache", "dbo"))
                .Update(u => u.HasName("UpdateDictCache", "dbo"))
                .Delete(u => u.HasName("DeleteDictCache", "dbo")));
            builder.Entity<SubType>().MapToStoredProcedures(s => s
                .Insert(u => u.HasName("InsertSubType", "dbo"))
                .Update(u => u.HasName("UpdateSubType", "dbo"))
                .Delete(u => u.HasName("DeleteSubType", "dbo")));
            builder.Entity<LearnVoca>().MapToStoredProcedures(s => s
                .Insert(u => u.HasName("InsertLearnVoca", "dbo"))
                .Update(u => u.HasName("UpdateLearnVoca", "dbo"))
                .Delete(u => u.HasName("DeleteLearnVoca", "dbo")));
            builder.Entity<ToiecGroup>().MapToStoredProcedures(s => s
                .Insert(u => u.HasName("InsertToiecGroup", "dbo"))
                .Update(u => u.HasName("UpdateToiecGroup", "dbo"))
                .Delete(u => u.HasName("DeleteToiecGroup", "dbo")));
        }
        //st3. -- END


    }

    //TODO 1_ThemBang: st4. vào package manager console gõ Add_Migration [Tên_migration tên gì cũng được miễn sao dễ hiểu]
    //tiếp tục gõ Update_Database
    //TODO 1_ThemBang: st5....................................................................................End 

    //TODO 2_CSCot: st2. vào package manager console gõ Add_Migration [Tên_migration tên gì cũng được miễn sao dễ hiểu]
    //tiếp tục gõ Update_Database
    //TODO 2_CSCot: st3....................................................................................End

}
