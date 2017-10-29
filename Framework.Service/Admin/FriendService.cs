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
    public interface IFriendService : IQlService<Friend>
    {
    }
    public class FriendService : QlService<Friend>, IFriendService
    {
        IFriendRepository _friendRepository;
        public FriendService(IFriendRepository friendRepository, IUnitOfWork unitOfWork) : base(friendRepository, unitOfWork) 
        {
            this._repository = friendRepository;
        }
    }
}
