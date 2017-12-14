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
    public interface IPostVoteDetailService : IQlService<PostVoteDetail>
    {
        PostVoteDetail getVoteByIdUser(string id_Use, int id_Post);
    }
    public class PostVoteDetailService : QlService<PostVoteDetail>, IPostVoteDetailService
    {
        IPostVoteDetailRepository _postVoteDetailRepository;
        public PostVoteDetailService(IPostVoteDetailRepository postVoteDetailRepository, IUnitOfWork unitOfWork)
            : base(postVoteDetailRepository, unitOfWork)
        {
            this._repository = postVoteDetailRepository;
            _postVoteDetailRepository = postVoteDetailRepository;
        }

        public PostVoteDetail getVoteByIdUser(string id_User, int id_Post)
        {
            return _postVoteDetailRepository.GetSingleByCondition(x => x.Id_User == id_User && x.Id_Post == id_Post);
        }
    }
}
