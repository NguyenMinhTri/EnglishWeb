using Framework.Model;
using Framework.Repository.RepositorySpace;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Service.Admin
{
    public interface IPostCommentDetailService : IQlService<PostCommentDetail>
    {
    }
    public class PostCommentDetailService : QlService<PostCommentDetail>, IPostCommentDetailService
    {
        IPostCommentDetailRepository _postCommentDetailRepository;
        public PostCommentDetailService(IPostCommentDetailRepository postCommentDetailRepository, IUnitOfWork unitOfWork) : base(postCommentDetailRepository, unitOfWork) 
        {
            this._repository = postCommentDetailRepository;
        }
    }
}
