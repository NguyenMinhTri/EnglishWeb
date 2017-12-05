using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IPostTypeRepository : IRepository<PostType>
    {
    }
    public class PostTypeRepository : BaseRepository<PostType>, IPostTypeRepository
    {
        public PostTypeRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
