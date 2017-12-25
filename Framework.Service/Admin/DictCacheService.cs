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
        DictCache findWordCache(string word);
    }
    public class DictCacheService : QlService<DictCache>, IDictCacheService
    {
        IDictCacheRepository _dictCacheRepository;
        public DictCacheService(IDictCacheRepository dictCacheRepository, IUnitOfWork unitOfWork) : base(dictCacheRepository, unitOfWork) 
        {
            this._repository = dictCacheRepository;
            _dictCacheRepository = dictCacheRepository;
        }

        public DictCache findWordCache(string word)
        {
            DictCache dictCache = _dictCacheRepository.GetSingleByCondition(x => x.VocaID == word);
            if (dictCache != null)
            {
                return dictCache;
            }
            return null;
        }
    }
}
