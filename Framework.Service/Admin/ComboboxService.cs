using Framework.Model;
using Framework.Repository.RepositorySpace;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.ViewData.Admin.GetData;
using Framework.ViewData;
using Framework.ViewData.Admin.GetData;
namespace Framework.Service.Admin
{
    public interface IComboboxService
    {
        List<ApplicationRoleComboboxViewData> GetRolesExceptInUserIdCombobox(String userId);
    }
    public class ComboboxService : IComboboxService
    {
        IApplicationUserRepository _userRepository;
        IApplicationUserRoleRepository _userRoleRepository;
        public ComboboxService(
            IApplicationUserRepository userRepository, 
            IApplicationUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }
        public List<UserComboboxViewData> GetUsersCombobox()
        {
            return ViewDataMapper<UserComboboxViewData, ApplicationUser>.MapObjects(_userRepository.GetMulti(x => x.Status).ToList());
        }

        public List<ApplicationRoleComboboxViewData> GetRolesExceptInUserIdCombobox(String userId)
        {
            return ViewDataMapper<ApplicationRoleComboboxViewData, ApplicationRole>.MapObjects(_userRoleRepository.GetRolesExceptInUserIdCombobox(userId).Where(x => x.Status).ToList());
        }
    }
}
