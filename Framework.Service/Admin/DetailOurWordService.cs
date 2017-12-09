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
        List<String> listUserToNofity();
        List<int> listIdOutWord(string iduser, int timeNotify);
    }
    public class DetailOurWordService : QlService<DetailOurWord>, IDetailOurWordService
    {
        IDetailOurWordRepository _detailOurWordRepository;
        public DetailOurWordService(IDetailOurWordRepository detailOurWordRepository, IUnitOfWork unitOfWork) : base(detailOurWordRepository, unitOfWork) 
        {
            this._repository = detailOurWordRepository;
            this._detailOurWordRepository = detailOurWordRepository;
        }

        public List<String> listUserToNofity()
        {
            int nowDay = DateTime.Now.Day;
            int maxTime = 1;
            int minTime = maxTime - 1;
            List <string> result =  _detailOurWordRepository.GetMulti(x => x.Schedule.Hour <= maxTime && x.Schedule.Hour > minTime && x.UpdatedDate.Value.Day != nowDay).Select(x => x.Id_Messenger).ToList();
            return result;
        }
        public List<int> listIdOutWord(string iduser, int timeNotify)
        {
            int nowDay = DateTime.Now.Day;
            int maxTime = timeNotify;
            int minTime = maxTime - 1;
            //Update ngay gui nhac nho
            var listID = _detailOurWordRepository.GetMulti(x => x.Id_User == iduser && x.Status == true && x.Schedule.Hour <= maxTime && x.Schedule.Hour > minTime && x.UpdatedDate.Value.Day != nowDay).Select(x => x.Id).ToList();
            foreach (var id in listID)
            {
                var temp = GetById(id);
                temp.UpdatedDate = DateTime.Now;
                Update(temp);
                Save();
            }
            //
            return _detailOurWordRepository.GetMulti(x => x.Id_User == iduser && x.Status == true).Select(x=>x.Id_OurWord).ToList();
        }
    }
}
