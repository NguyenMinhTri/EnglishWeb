using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IDictCacheRepository : IRepository<DictCache>
    {
    }
    public class DictCacheRepository : BaseRepository<DictCache>, IDictCacheRepository
    {
        public DictCacheRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
