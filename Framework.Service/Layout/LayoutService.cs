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
    }
    public class LayoutService : ILayoutService
    {
        IUnitOfWork _unitOfWork;
        IApplicationUserRepository _userRepository;
        IApplicationUserRoleRepository _userRoleRepository;
        public LayoutService(
            IApplicationUserRepository userRepository,
            IApplicationUserRoleRepository userRoleRepository,
            IUnitOfWork unitOfWork

            )
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
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
    }
}
