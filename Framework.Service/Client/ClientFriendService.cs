using Framework.Repository.RepositorySpace;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Model;
namespace Framework.Service
{
    public interface IClientFriendService : IQlService<Friend>
    {
        Friend FindRelationship(string id_user, string id_friend);
    }
    class FriendService : QlService<Friend>, IClientFriendService
    {
        IFriendRepository _friendRepository;
        public FriendService(IFriendRepository friendRepository, IUnitOfWork unitOfWork)
            : base(friendRepository, unitOfWork)
        {
            this._repository = friendRepository;
            _friendRepository = friendRepository;
        }

        public Friend FindRelationship(string id_user, string id_friend)
        {
            return _friendRepository.GetSingleByCondition(x => x.Id_User == id_user && x.Id_Friend == id_friend);
        }
    }
}
