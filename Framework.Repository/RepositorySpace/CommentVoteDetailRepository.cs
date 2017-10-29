using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface ICommentVoteDetailRepository : IRepository<CommentVoteDetail>
    {
    }
    public class CommentVoteDetailRepository : BaseRepository<CommentVoteDetail>, ICommentVoteDetailRepository
    {
        public CommentVoteDetailRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
