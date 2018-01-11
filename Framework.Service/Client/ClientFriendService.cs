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
        List<String> GetAllFriend(string id_user);
        List<Friend> GetAllFriends(string id_user);
        List<Friend> GetRelationship(string id_user);

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
            Friend friend = _friendRepository.GetSingleByCondition(x => x.Id_User == id_user && x.Id_Friend == id_friend);
            if (friend == null)
            {
                friend = _friendRepository.GetSingleByCondition(x => x.Id_User == id_friend && x.Id_Friend == id_user);
            }
            return friend;
        }

        public List<String> GetAllFriend(string id_user)
        {
            List<String> listFriend = new List<String>();
            listFriend = _friendRepository.GetMulti(x => x.Id_User == id_user && x.CodeRelationshipId == 1).Select(x=>x.Id_Friend).ToList();
            listFriend.AddRange(_friendRepository.GetMulti(x => x.Id_Friend == id_user && x.CodeRelationshipId == 1).Select(x => x.Id_User).ToList());
            return listFriend;
        }

        public List<Friend> GetRelationship(string id_user)
        {
            List<Friend> listRelationship = new List<Friend>();
            listRelationship = _friendRepository.GetMulti(x => x.Id_Friend == id_user && x.CodeRelationshipId == -1).ToList();
            listRelationship.Add(null);
            listRelationship.AddRange(_friendRepository.GetMulti(x => x.Id_User == id_user && x.CodeRelationshipId == -1).ToList());
            return listRelationship;
        }

        public List<Friend> GetAllFriends(string id_user)
        {
            List<Friend> listFriend = new List<Friend>();
            listFriend = _friendRepository.GetMulti(x => x.Id_User == id_user && x.CodeRelationshipId == 1).ToList();
            listFriend.AddRange(_friendRepository.GetMulti(x => x.Id_Friend == id_user && x.CodeRelationshipId == 1).ToList());
            return listFriend;
        }
    }
}
