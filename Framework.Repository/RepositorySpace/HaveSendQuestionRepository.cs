using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IHaveSendQuestionRepository : IRepository<HaveSendQuestion>
    {
    }
    public class HaveSendQuestionRepository : BaseRepository<HaveSendQuestion>, IHaveSendQuestionRepository
    {
        public HaveSendQuestionRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
