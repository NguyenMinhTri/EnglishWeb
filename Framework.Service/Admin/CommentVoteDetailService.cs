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
        CommentVoteDetail getVoteByIdUser(string id_user, int id_comment);
    }
    public class CommentVoteDetailService : QlService<CommentVoteDetail>, ICommentVoteDetailService
    {
        ICommentVoteDetailRepository _commentVoteDetailRepository;
        public CommentVoteDetailService(ICommentVoteDetailRepository commentVoteDetailRepository, IUnitOfWork unitOfWork) : base(commentVoteDetailRepository, unitOfWork) 
        {
            this._repository = commentVoteDetailRepository;
            _commentVoteDetailRepository = commentVoteDetailRepository;
        }
        public CommentVoteDetail getVoteByIdUser(string id_user, int id_comment)
        {
            return _commentVoteDetailRepository.GetSingleByCondition(x => x.Id_User == id_user && x.Id_Comment == id_comment);
        }
    }
}
