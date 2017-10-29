using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IPostCommentDetailRepository : IRepository<PostCommentDetail>
    {
    }
    public class PostCommentDetailRepository : BaseRepository<PostCommentDetail>, IPostCommentDetailRepository
    {
        public PostCommentDetailRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
