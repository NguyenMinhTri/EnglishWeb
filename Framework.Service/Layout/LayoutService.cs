using Framework.Model;
using Framework.Repository.RepositorySpace;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.ViewData;
using Framework.Common;
using System.Web.Mvc;
using System.Web;
namespace Framework.Service
{
    public interface ILayoutService
    {
        ApplicationUser GetUserById(string userId);
        ApplicationUser GetUserByUserName(string userName);
        List<String> GetRolesOfUser(string userId);
        List<ApplicationUser> listUserID();
        List<String> GetAllFriend(string id_user);
        List<Friend> GetRelationship(string id_user);

    }
    public class LayoutService : ILayoutService
    {
        IUnitOfWork _unitOfWork;
        IApplicationUserRepository _userRepository;
        IApplicationUserRoleRepository _userRoleRepository;
        IFriendRepository _friendRepository;

        public LayoutService(
            IApplicationUserRepository userRepository,
            IApplicationUserRoleRepository userRoleRepository,
            IFriendRepository friendRepository,
            IUnitOfWork unitOfWork

            )
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _friendRepository = friendRepository;
            _unitOfWork = unitOfWork;

        }


        public ApplicationUser GetUserById(string userId)
        {
            return _userRepository.GetSingleByCondition(x => x.Id == userId);
        }

        public ApplicationUser GetUserByUserName(string userName)
        {
            return _userRepository.GetSingleByCondition(x => x.UserName == userName);
        }

        public List<string> GetRolesOfUser(string userId)
        {
            return _userRoleRepository.GetRolesOfUser(userId);
        }
        public List<ApplicationUser> listUserID()
        {
            return _userRepository.GetAll().ToList();
        }
        public List<String> GetAllFriend(string id_user)
        {
            List<String> listFriend = new List<String>();
            listFriend = _friendRepository.GetMulti(x => x.Id_User == id_user && x.CodeRelationshipId == 1).Select(x => x.Id_Friend).ToList();
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
    }
}
