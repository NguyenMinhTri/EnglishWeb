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
    public interface IOurWordService : IQlService<OurWord>
    {
    }
    public class OurWordService : QlService<OurWord>, IOurWordService
    {
        IOurWordRepository _ourWordRepository;
        public OurWordService(IOurWordRepository ourWordRepository, IUnitOfWork unitOfWork) : base(ourWordRepository, unitOfWork) 
        {
            this._repository = ourWordRepository;
        }
    }
}
