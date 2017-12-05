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
    public interface IDetailUserTypeService : IQlService<DetailUserType>
    {
    }
    public class DetailUserTypeService : QlService<DetailUserType>, IDetailUserTypeService
    {
        IDetailUserTypeRepository _detailUserTypeRepository;
        public DetailUserTypeService(IDetailUserTypeRepository detailUserTypeRepository, IUnitOfWork unitOfWork) : base(detailUserTypeRepository, unitOfWork) 
        {
            this._repository = detailUserTypeRepository;
        }
    }
}
