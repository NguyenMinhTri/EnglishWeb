using Framework.Repository.RepositorySpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Service.Client
{
    public interface IClientDictionaryService
    {
    }
    class ClientDictionaryService : IClientDictionaryService
    {
        ICodeRepository _codeRepository;
        public ClientDictionaryService(
            )
        {

        }

    }
}
