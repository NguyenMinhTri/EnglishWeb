using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.FrameworkContext
{
    public class FrameworkConfiguration : DbConfiguration
    {
        public FrameworkConfiguration()
        {
            this.SetHistoryContext("System.Data.SqlClient",
                (connection, defaultSchema) => new FrameworkHistoryContext(connection, defaultSchema));
        }
    }
}
