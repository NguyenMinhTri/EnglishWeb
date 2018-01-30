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
    public interface IVocaScheduleService : IQlService<VocaSchedule>
    {
    }
    public class VocaScheduleService : QlService<VocaSchedule>, IVocaScheduleService
    {
        IVocaScheduleRepository _vocaScheduleRepository;
        public VocaScheduleService(IVocaScheduleRepository vocaScheduleRepository, IUnitOfWork unitOfWork) : base(vocaScheduleRepository, unitOfWork) 
        {
            this._repository = vocaScheduleRepository;
        }
    }
}
