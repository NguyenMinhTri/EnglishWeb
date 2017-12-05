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
    public interface IFriendService : IQlService<Friend>
    {

    }
    class FriendService : QlService<Friend>, IFriendService
    {
        IFriendRepository _friendRepository;
        public FriendService(IFriendRepository friendRepository, IUnitOfWork unitOfWork) : base(friendRepository, unitOfWork)
        {
            this._repository = friendRepository;
        }
    }
}
