		public DbSet<HaveSendQuestion> HaveSendQuestions { set; get; }
protected override void OnModelCreating(DbModelBuilder builder)
{
builder.Entity<HaveSendQuestion>().MapToStoredProcedures(s => s.Insert(u => u.HasName("InsertHaveSendQuestion", "dbo"))
.Update(u => u.HasName("UpdateHaveSendQuestion", "dbo"))
.Delete(u => u.HasName("DeleteHaveSendQuestion", "dbo")));
}
