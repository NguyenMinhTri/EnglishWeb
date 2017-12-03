using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IDetailOurWordRepository : IRepository<DetailOurWord>
    {
    }
    public class DetailOurWordRepository : BaseRepository<DetailOurWord>, IDetailOurWordRepository
    {
        public DetailOurWordRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
