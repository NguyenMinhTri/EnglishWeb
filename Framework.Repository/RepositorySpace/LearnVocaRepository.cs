using Framework.Model;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface ILearnVocaRepository : IRepository<LearnVoca>
    {
    }
    public class LearnVocaRepository : BaseRepository<LearnVoca>, ILearnVocaRepository
    {
        public LearnVocaRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
   }
}
