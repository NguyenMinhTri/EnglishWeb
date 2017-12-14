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
    public interface ICommentService : IQlService<Comment>
    {
        List<Comment> getCommentOfPost(int id_Post);
        List<Comment> getChildOfComment(int id_Post, int id_Comment);
        Comment getCorrectComment(int id_Comment);
    }
    public class CommentService : QlService<Comment>, ICommentService
    {
        ICommentRepository _postCommentDetailRepository;
        public CommentService(ICommentRepository postCommentDetailRepository, IUnitOfWork unitOfWork) : base(postCommentDetailRepository, unitOfWork) 
        {
            this._repository = postCommentDetailRepository;
            _postCommentDetailRepository = postCommentDetailRepository;
        }
        public List<Comment> getCommentOfPost(int id_Post){
            return _postCommentDetailRepository.GetMulti(x => x.Id_Post == id_Post && x.Id_Comment == 0).ToList();
        }
        public List<Comment> getChildOfComment(int id_Post, int id_Comment)
        {
            return _postCommentDetailRepository.GetMulti(x => x.Id_Post == id_Post && x.Id_Comment == id_Comment).ToList();
        }
        public Comment getCorrectComment(int id_Post)
        {
            return _postCommentDetailRepository.GetSingleByCondition(x => x.Id_Post == id_Post && x.Corrected == true);
        }
    }
}
