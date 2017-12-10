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
            //Check đúng ngày đúng giờ hay ko
            int nowDay = DateTime.Now.Day;
            if(timeNotify ==-1)
            {
                var listID = _detailOurWordRepository.GetMulti(x => x.Id_User == iduser && x.Learned == 1).ToList();
                return listID.Select(x => x.Id_OurWord).ToList();
            }
            if (DateTime.Now.Hour == timeNotify)
            {
                //Update ngay gui nhac nho
                //&& x.UpdatedDate.Value.Day != nowDay
                
                var listID = _detailOurWordRepository.GetMulti(x => x.Id_User == iduser && x.Schedule.Day != nowDay).ToList();
                foreach (var id in listID)
                {
                    var temp = GetById(id.Id);
                    temp.Status = false;
                    //Hom nay da gui
                    temp.Schedule = DateTime.Now;
                    Update(temp);
                    Save();
                }
                //
                return listID.Select(x => x.Id_OurWord).ToList();
            }
            return null;
        }
    }
}
