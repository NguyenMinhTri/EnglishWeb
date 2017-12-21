using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IToiecGroupRepository : IRepository<ToiecGroup>
    {
    }
    public class ToiecGroupRepository : BaseRepository<ToiecGroup>, IToiecGroupRepository
    {
        public ToiecGroupRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
