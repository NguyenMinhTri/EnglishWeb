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
    public interface IPostService : IQlService<Post>
    {
        List<Post> getPostByUserType(string id_user);
        List<Post> getPostByType(int type);
    }
    public class PostService : QlService<Post>, IPostService
    {
        IPostRepository _postRepository;
        IPostTypeRepository _postTypeRepository;
        IDetailUserTypeRepository _detailUserTypeRepository;

        public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork, IDetailUserTypeRepository detailUserTypeRepository, IPostTypeRepository postTypeRepository)
            : base(postRepository, unitOfWork)
        {
            this._repository = postRepository;
            this._postRepository = postRepository;
            this._postTypeRepository = postTypeRepository;
            this._detailUserTypeRepository = detailUserTypeRepository;
        }

        public List<Post> getPostByUserType(string id_user)
        {
            List<Post> listPost = new List<Post>();
            List<int> listPostType = _detailUserTypeRepository.GetMulti(x => x.UserID == id_user).Select(x => x.Type).ToList();
            if (listPostType == null)
            {
                listPostType = _postTypeRepository.GetAll().Select(x => x.Id).ToList();
            }
            foreach (int i in listPostType)
            {
                List<Post> postType = new List<Post>();
                postType = _postRepository.GetMulti(x => x.Id_Type == i).OrderByDescending(x => x.DatePost).ToList();
                listPost.AddRange(postType);
            }
            return listPost.OrderByDescending(x => x.DatePost).ToList();
        }

        public List<Post> getPostByType(int type)
        {
            return  _postRepository.GetMulti(x => x.Id_Type == type).OrderByDescending(x => x.DatePost).ToList();
        }
    }
}
