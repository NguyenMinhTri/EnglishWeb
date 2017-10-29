using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IRoutingRepository : IRepository<Routing>
    {
    }
    public class RoutingRepository : BaseRepository<Routing>, IRoutingRepository
    {
        public RoutingRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
