		public DbSet<Friend> Friends { set; get; }
		public DbSet<Code> Codes { set; get; }
		public DbSet<Message> Messages { set; get; }
		public DbSet<Notification> Notifications { set; get; }
		public DbSet<Post> Posts { set; get; }
		public DbSet<PostCommentDetail> PostCommentDetails { set; get; }
		public DbSet<PostVoteDetail> PostVoteDetails { set; get; }
		public DbSet<CommentVoteDetail> CommentVoteDetails { set; get; }
protected override void OnModelCreating(DbModelBuilder builder)
{
builder.Entity<Friend>().MapToStoredProcedures(s => s.Insert(u => u.HasName("InsertFriend", "dbo"))
.Update(u => u.HasName("UpdateFriend", "dbo"))
.Delete(u => u.HasName("DeleteFriend", "dbo")));
builder.Entity<Code>().MapToStoredProcedures(s => s.Insert(u => u.HasName("InsertCode", "dbo"))
.Update(u => u.HasName("UpdateCode", "dbo"))
.Delete(u => u.HasName("DeleteCode", "dbo")));
builder.Entity<Message>().MapToStoredProcedures(s => s.Insert(u => u.HasName("InsertMessage", "dbo"))
.Update(u => u.HasName("UpdateMessage", "dbo"))
.Delete(u => u.HasName("DeleteMessage", "dbo")));
builder.Entity<Notification>().MapToStoredProcedures(s => s.Insert(u => u.HasName("InsertNotification", "dbo"))
.Update(u => u.HasName("UpdateNotification", "dbo"))
.Delete(u => u.HasName("DeleteNotification", "dbo")));
builder.Entity<Post>().MapToStoredProcedures(s => s.Insert(u => u.HasName("InsertPost", "dbo"))
.Update(u => u.HasName("UpdatePost", "dbo"))
.Delete(u => u.HasName("DeletePost", "dbo")));
builder.Entity<PostCommentDetail>().MapToStoredProcedures(s => s.Insert(u => u.HasName("InsertPostCommentDetail", "dbo"))
.Update(u => u.HasName("UpdatePostCommentDetail", "dbo"))
.Delete(u => u.HasName("DeletePostCommentDetail", "dbo")));
builder.Entity<PostVoteDetail>().MapToStoredProcedures(s => s.Insert(u => u.HasName("InsertPostVoteDetail", "dbo"))
.Update(u => u.HasName("UpdatePostVoteDetail", "dbo"))
.Delete(u => u.HasName("DeletePostVoteDetail", "dbo")));
builder.Entity<CommentVoteDetail>().MapToStoredProcedures(s => s.Insert(u => u.HasName("InsertCommentVoteDetail", "dbo"))
.Update(u => u.HasName("UpdateCommentVoteDetail", "dbo"))
.Delete(u => u.HasName("DeleteCommentVoteDetail", "dbo")));
}
