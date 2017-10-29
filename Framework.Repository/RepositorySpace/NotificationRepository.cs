using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface INotificationRepository : IRepository<Notification>
    {
    }
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
