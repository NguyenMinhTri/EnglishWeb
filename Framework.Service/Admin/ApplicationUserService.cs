using Framework.Model;
using Framework.Repository.RepositorySpace;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.ViewData.Admin.Input;
using Framework.ViewData;
namespace Framework.Service.Admin
{
    public interface IApplicationUserService : IQlService<ApplicationUser>
    {
        ApplicationUser GetUserById(String id);
        List<String> GetRolesByUserId(string id);
    }
    public class ApplicationUserService : QlService<ApplicationUser>, IApplicationUserService
    {
        IApplicationUserRepository _applicationUserRepository;
        IApplicationUserRoleRepository _userRoleRepository;
        public ApplicationUserService(IApplicationUserRepository applicationUserRepository,
            IApplicationUserRoleRepository userRoleRepository,
            IUnitOfWork unitOfWork)
            : base(applicationUserRepository, unitOfWork)
        {
            this._repository = applicationUserRepository;
            _applicationUserRepository = applicationUserRepository;
            _userRoleRepository = userRoleRepository;
        }

        public ApplicationUser GetUserById(string id)
        {
            return _applicationUserRepository.GetSingleByCondition(x => x.Id == id);
        }

        public List<string> GetRolesByUserId(string id)
        {
            return _userRoleRepository.GetMulti(x => x.UserId == id).Select(x => x.RoleId).ToList();
        }
    }
}
