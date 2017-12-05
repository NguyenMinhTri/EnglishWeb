		public DbSet<PostType> PostTypes { set; get; }
protected override void OnModelCreating(DbModelBuilder builder)
{
builder.Entity<PostType>().MapToStoredProcedures(s => s.Insert(u => u.HasName("InsertPostType", "dbo"))
.Update(u => u.HasName("UpdatePostType", "dbo"))
.Delete(u => u.HasName("DeletePostType", "dbo")));
}
