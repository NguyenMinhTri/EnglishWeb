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
    public interface IClientNotificationService : IQlService<Notification>
    {
        List<Notification> getAllNotification(string id_user);
    }
    class NotificationService : QlService<Notification>, IClientNotificationService
    {
        INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
            : base(notificationRepository, unitOfWork)
        {
            this._repository = notificationRepository;
            _notificationRepository = notificationRepository;
        }

        public List<Notification> getAllNotification(string id_user)
        {
            List<Notification> listNotification = new List<Notification>();
            listNotification = _notificationRepository.GetMulti(x => x.Id_Friend == id_user).ToList();
            return listNotification;
        }
    }
}
