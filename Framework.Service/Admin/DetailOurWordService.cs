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
    public interface IDetailOurWordService : IQlService<DetailOurWord>
    {
    }
    public class DetailOurWordService : QlService<DetailOurWord>, IDetailOurWordService
    {
        IDetailOurWordRepository _detailOurWordRepository;
        public DetailOurWordService(IDetailOurWordRepository detailOurWordRepository, IUnitOfWork unitOfWork) : base(detailOurWordRepository, unitOfWork) 
        {
            this._repository = detailOurWordRepository;
        }
    }
}
