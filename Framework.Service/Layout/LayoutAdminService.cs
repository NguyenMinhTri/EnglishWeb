using Framework.Model;
using Framework.Repository.RepositorySpace;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.ViewData;
using Framework.Common;
namespace Framework.Service.Admin
{
    public interface ILayoutAdminService
    {
        ApplicationUser GetUserById(String userId);
    }
    public class LayoutAdminService : ILayoutAdminService
    {
        IApplicationUserRepository _userRepository;
        IUnitOfWork _unitOfWork;
        public LayoutAdminService(
            IApplicationUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public ApplicationUser GetUserById(string userId)
        {
            return _userRepository.GetSingleByCondition(x => x.Id == userId);
        }
    }
}
