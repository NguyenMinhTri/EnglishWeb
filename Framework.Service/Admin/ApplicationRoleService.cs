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
    public interface IApplicationRoleService : IQlService<ApplicationRole>
    {
        void DeleteByStringId(String id);
    }
    public class ApplicationRoleService : QlService<ApplicationRole>, IApplicationRoleService
    {
        IApplicationRoleRepository _applicationRoleRepository;
        public ApplicationRoleService(IApplicationRoleRepository applicationRoleRepository, IUnitOfWork unitOfWork) : base(applicationRoleRepository, unitOfWork) 
        {
            this._repository = applicationRoleRepository;
            _applicationRoleRepository = applicationRoleRepository;
        }

        public void DeleteByStringId(string id)
        {
            _repository.Delete(_applicationRoleRepository.GetSingleByCondition(x => x.Id == id));
        }
    }
}
