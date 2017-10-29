using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
    }
    public class ApplicationUserRepository : BaseRepository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
