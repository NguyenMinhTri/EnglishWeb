using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IMessageRepository : IRepository<Message>
    {
    }
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
