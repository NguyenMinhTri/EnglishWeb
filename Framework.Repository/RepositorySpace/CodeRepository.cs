using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface ICodeRepository : IRepository<Code>
    {
    }
    public class CodeRepository : BaseRepository<Code>, ICodeRepository
    {
        public CodeRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
