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
    public interface ICommentVoteDetailService : IQlService<CommentVoteDetail>
    {
    }
    public class CommentVoteDetailService : QlService<CommentVoteDetail>, ICommentVoteDetailService
    {
        ICommentVoteDetailRepository _commentVoteDetailRepository;
        public CommentVoteDetailService(ICommentVoteDetailRepository commentVoteDetailRepository, IUnitOfWork unitOfWork) : base(commentVoteDetailRepository, unitOfWork) 
        {
            this._repository = commentVoteDetailRepository;
        }
    }
}
