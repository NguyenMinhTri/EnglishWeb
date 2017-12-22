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
        bool getRegisterPostType(string id_User, int type);
        void removeByUserId(string id_User);
    }
    public class DetailUserTypeService : QlService<DetailUserType>, IDetailUserTypeService
    {
        IDetailUserTypeRepository _detailUserTypeRepository;
        public DetailUserTypeService(IDetailUserTypeRepository detailUserTypeRepository, IUnitOfWork unitOfWork)
            : base(detailUserTypeRepository, unitOfWork)
        {
            this._repository = detailUserTypeRepository;
            _detailUserTypeRepository = detailUserTypeRepository;
        }

        public bool getRegisterPostType(string id_User, int type)
        {
            if (_detailUserTypeRepository.GetSingleByCondition(x => x.UserID == id_User && x.Type == type) != null)
            {
                return true;
            }
            return false;
        }

        public void removeByUserId(string id_User)
        {
            List<DetailUserType> list = _detailUserTypeRepository.GetMulti(x => x.UserID == id_User).ToList();
            if (list != null)
            {
                foreach (DetailUserType item in list)
                {
                    _detailUserTypeRepository.Delete(item);
                }
            }
        }
    }
}
