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
    public interface IMessageService : IQlService<Message>
    {
    }
    public class MessageService : QlService<Message>, IMessageService
    {
        IMessageRepository _messageRepository;
        public MessageService(IMessageRepository messageRepository, IUnitOfWork unitOfWork) : base(messageRepository, unitOfWork) 
        {
            this._repository = messageRepository;
        }
    }
}
