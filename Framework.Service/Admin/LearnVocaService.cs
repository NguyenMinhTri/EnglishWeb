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
    public interface ILearnVocaService : IQlService<LearnVoca>
    {
    }
    public class LearnVocaService : QlService<LearnVoca>, ILearnVocaService
    {
        ILearnVocaRepository _learnVocaRepository;
        public LearnVocaService(ILearnVocaRepository learnVocaRepository, IUnitOfWork unitOfWork) : base(learnVocaRepository, unitOfWork) 
        {
            this._repository = learnVocaRepository;
        }
    }
}
