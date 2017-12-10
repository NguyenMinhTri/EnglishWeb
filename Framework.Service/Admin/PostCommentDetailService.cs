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
        List<PostCommentDetail> getCommentOfPost(int id_Post);
        List<PostCommentDetail> getChildOfComment(int id_Post, int id_Comment);
    }
    public class PostCommentDetailService : QlService<PostCommentDetail>, IPostCommentDetailService
    {
        IPostCommentDetailRepository _postCommentDetailRepository;
        public PostCommentDetailService(IPostCommentDetailRepository postCommentDetailRepository, IUnitOfWork unitOfWork) : base(postCommentDetailRepository, unitOfWork) 
        {
            this._repository = postCommentDetailRepository;
            _postCommentDetailRepository = postCommentDetailRepository;
        }
        public List<PostCommentDetail> getCommentOfPost(int id_Post){
            return _postCommentDetailRepository.GetMulti(x => x.Id_Post == id_Post && x.Id_Comment == 0).ToList();
        }
        public List<PostCommentDetail> getChildOfComment(int id_Post, int id_Comment)
        {
            return _postCommentDetailRepository.GetMulti(x => x.Id_Post == id_Post && x.Id_Comment == id_Comment).ToList();
        }

    }
}
