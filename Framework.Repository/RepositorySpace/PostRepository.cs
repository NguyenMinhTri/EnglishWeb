using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IPostRepository : IRepository<Post>
    {
    }
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
