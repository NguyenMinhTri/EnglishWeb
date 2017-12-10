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
    public interface ISubTypeService : IQlService<SubType>
    {
    }
    public class SubTypeService : QlService<SubType>, ISubTypeService
    {
        ISubTypeRepository _subTypeRepository;
        public SubTypeService(ISubTypeRepository subTypeRepository, IUnitOfWork unitOfWork) : base(subTypeRepository, unitOfWork) 
        {
            this._repository = subTypeRepository;
        }
    }
}
