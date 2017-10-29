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
    public interface INotificationService : IQlService<Notification>
    {
    }
    public class NotificationService : QlService<Notification>, INotificationService
    {
        INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork) : base(notificationRepository, unitOfWork) 
        {
            this._repository = notificationRepository;
        }
    }
}
