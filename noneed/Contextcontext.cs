		public DbSet<OurWord> OurWords { set; get; }
		public DbSet<DetailOurWord> DetailOurWords { set; get; }
protected override void OnModelCreating(DbModelBuilder builder)
{
builder.Entity<OurWord>().MapToStoredProcedures(s => s.Insert(u => u.HasName("InsertOurWord", "dbo"))
.Update(u => u.HasName("UpdateOurWord", "dbo"))
.Delete(u => u.HasName("DeleteOurWord", "dbo")));
builder.Entity<DetailOurWord>().MapToStoredProcedures(s => s.Insert(u => u.HasName("InsertDetailOurWord", "dbo"))
.Update(u => u.HasName("UpdateDetailOurWord", "dbo"))
.Delete(u => u.HasName("DeleteDetailOurWord", "dbo")));
}
