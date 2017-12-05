using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IDetailUserTypeRepository : IRepository<DetailUserType>
    {
    }
    public class DetailUserTypeRepository : BaseRepository<DetailUserType>, IDetailUserTypeRepository
    {
        public DetailUserTypeRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
