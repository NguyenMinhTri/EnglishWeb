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
    public interface ICodeService : IQlService<Code>
    {
    }
    public class CodeService : QlService<Code>, ICodeService
    {
        ICodeRepository _codeRepository;
        public CodeService(ICodeRepository codeRepository, IUnitOfWork unitOfWork) : base(codeRepository, unitOfWork) 
        {
            this._repository = codeRepository;
        }
    }
}
