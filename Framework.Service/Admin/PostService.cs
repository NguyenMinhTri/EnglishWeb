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
    }
    public class PostService : QlService<Post>, IPostService
    {
        IPostRepository _postRepository;
        public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork) : base(postRepository, unitOfWork) 
        {
            this._repository = postRepository;
        }
    }
}
