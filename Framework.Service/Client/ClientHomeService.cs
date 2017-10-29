using Framework.Repository.RepositorySpace;
using Framework.ViewData.ClientHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Service.Client
{
    public interface IClientHomeService
    {
        List<CodeViewData> GetCodes();
    }
    class ClientHomeService : IClientHomeService
    {
        ICodeRepository _codeRepository;
        public ClientHomeService(
            ICodeRepository codeRepository
            )
        {
            _codeRepository=codeRepository;
        }

        public List<CodeViewData> GetCodes()
        {
            var query = _codeRepository.GetMulti(x => x.Status);
            return _codeRepository.OptimizeSelect<CodeViewData>(query).ToList();
        }
    }
}
