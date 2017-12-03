using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IOurWordRepository : IRepository<OurWord>
    {
    }
    public class OurWordRepository : BaseRepository<OurWord>, IOurWordRepository
    {
        public OurWordRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
