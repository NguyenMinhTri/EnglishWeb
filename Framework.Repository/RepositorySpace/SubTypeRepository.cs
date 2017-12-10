using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface ISubTypeRepository : IRepository<SubType>
    {
    }
    public class SubTypeRepository : BaseRepository<SubType>, ISubTypeRepository
    {
        public SubTypeRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
