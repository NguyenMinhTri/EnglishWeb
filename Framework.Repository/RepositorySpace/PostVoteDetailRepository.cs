using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IPostVoteDetailRepository : IRepository<PostVoteDetail>
    {
    }
    public class PostVoteDetailRepository : BaseRepository<PostVoteDetail>, IPostVoteDetailRepository
    {
        public PostVoteDetailRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
