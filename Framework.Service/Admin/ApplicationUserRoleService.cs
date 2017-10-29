using Framework.Model;
using Framework.Repository.RepositorySpace;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Model.TableJoin;
namespace Framework.Service.Admin
{
    public interface IApplicationUserRoleService : IQlService<ApplicationUserRole>
    {
        List<ApplicationUserRoleGetAll> GetAllWithDisplay();
        List<ApplicationUserRoleGetAll> GetAllWithDisplayByUserId(String userId);
        String GetUserName(String userId);

    }
    public class ApplicationUserRoleService : QlService<ApplicationUserRole>, IApplicationUserRoleService
    {
        IApplicationUserRoleRepository _applicationUserRoleRepository;
        
        public ApplicationUserRoleService(IApplicationUserRoleRepository applicationUserRoleRepository, IUnitOfWork unitOfWork) : base(applicationUserRoleRepository, unitOfWork) 
        {
            this._repository = applicationUserRoleRepository;
            _applicationUserRoleRepository = applicationUserRoleRepository;
        }

        public List<ApplicationUserRoleGetAll> GetAllWithDisplay()
        {
            return _applicationUserRoleRepository.GetAllWithDisplay().ToList();
        }


        public List<ApplicationUserRoleGetAll> GetAllWithDisplayByUserId(string userId)
        {
            return _applicationUserRoleRepository.GetAllWithDisplayByUserId(userId).ToList();
        }


        public string GetUserName(string userId)
        {
            return "";
        }
    }
}
