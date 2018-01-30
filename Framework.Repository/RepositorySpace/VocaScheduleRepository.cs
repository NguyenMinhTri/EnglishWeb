using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IVocaScheduleRepository : IRepository<VocaSchedule>
    {
    }
    public class VocaScheduleRepository : BaseRepository<VocaSchedule>, IVocaScheduleRepository
    {
        public VocaScheduleRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
