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
    public interface IRoutingService : IQlService<Routing>
    {
        Routing GetByUrl(string url);
        Routing GetByActionAndUrl(string controllerName, string action, string url);
    }
    public class RoutingService : QlService<Routing>, IRoutingService
    {
        IRoutingRepository _routingRepository;
        IApplicationUserRepository _userRepository;
        public RoutingService(IRoutingRepository routingRepository,
            IApplicationUserRepository applicationUserRepository,
            IUnitOfWork unitOfWork)
            : base(routingRepository, unitOfWork)
        {
            this._repository = routingRepository;
            _routingRepository = routingRepository;
            _userRepository = applicationUserRepository;
        }

        public Routing GetByUrl(string url)
        {
            return _repository.GetSingleByCondition(x => x.FriendlyUrl == url);
        }


        public Routing GetByActionAndUrl(string controllerName, string action, string url)
        {
            return _repository.GetSingleByCondition(x => x.FriendlyUrl == url && x.Controller == controllerName && x.Action == action);
        }
    }
}
