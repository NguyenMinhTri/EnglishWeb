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
    public interface IDictCacheService : IQlService<DictCache>
    {
    }
    public class DictCacheService : QlService<DictCache>, IDictCacheService
    {
        IDictCacheRepository _dictCacheRepository;
        public DictCacheService(IDictCacheRepository dictCacheRepository, IUnitOfWork unitOfWork) : base(dictCacheRepository, unitOfWork) 
        {
            this._repository = dictCacheRepository;
        }
    }
}
