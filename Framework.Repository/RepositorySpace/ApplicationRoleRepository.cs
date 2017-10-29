using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IApplicationRoleRepository : IRepository<ApplicationRole>
    {
    }
    public class ApplicationRoleRepository : BaseRepository<ApplicationRole>, IApplicationRoleRepository
    {
        public ApplicationRoleRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
