using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IFriendRepository : IRepository<Friend>
    {
    }
    public class FriendRepository : BaseRepository<Friend>, IFriendRepository
    {
        public FriendRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
