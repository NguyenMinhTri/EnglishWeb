using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.FrameworkContext
{
    public class FrameworkHistoryContext : HistoryContext
    {
        public FrameworkHistoryContext(DbConnection dbConnection, string defaultSchema)
                : base(dbConnection, defaultSchema)
            {
            }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.Entity<HistoryRow>().ToTable(tableName: "MigrationHistory", schemaName: FrameworkDbContext.schema);
                modelBuilder.Entity<HistoryRow>().Property(p => p.MigrationId).HasColumnName("Migration_ID");
            }
    }
}
