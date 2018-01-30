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
        List<DetailOurWord> listOutWord(string iduser, int timeNotify);
        DetailOurWord findDetailOurWord(string id_user, int id_word);
        void resetLearningTime(string iduser);
        void SetNextTime(int idWord, int NextTime, int NumberOfNT);
        List<int> remindVoca(string userid);
    }
    public class DetailOurWordService : QlService<DetailOurWord>, IDetailOurWordService
    {
        IDetailOurWordRepository _detailOurWordRepository;
        public DetailOurWordService(IDetailOurWordRepository detailOurWordRepository, IUnitOfWork unitOfWork) : base(detailOurWordRepository, unitOfWork) 
        {
            this._repository = detailOurWordRepository;
            this._detailOurWordRepository = detailOurWordRepository;
        }

        public DetailOurWord findDetailOurWord(string id_user, int id_word)
        {
            DetailOurWord detailOurWord = _detailOurWordRepository.GetSingleByCondition(x => x.Id_User == id_user && x.Id_OurWord == id_word);
            if (detailOurWord != null)
            {
                return detailOurWord;
            }
            return null;
        }

        public List<String> listUserToNofity()
        {
            int nowDay = DateTime.Now.Day;
            int maxTime = 1;
            int minTime = maxTime - 1;
            List <string> result =  _detailOurWordRepository.GetMulti(x => x.Schedule.Hour <= maxTime && x.Schedule.Hour > minTime && x.UpdatedDate.Value.Day != nowDay).Select(x => x.Id_Messenger).ToList();
            return result;
        }
        public List<DetailOurWord> listOutWord(string iduser, int timeNotify)
        {
            var listID = _detailOurWordRepository.GetMulti(x => x.Id_User == iduser).ToList();
            return listID.ToList();
        }
        public List<int> listIdOutWord(string iduser, int timeNotify)
        {
            //Check đúng ngày đúng giờ hay ko
            DateTime currentVNTime = DateTime.Now.AddHours(7);
            int nowDay = currentVNTime.Day;
            if(timeNotify ==-1)
            {
                var listID = _detailOurWordRepository.GetMulti(x => x.Id_User == iduser && x.Learned == 1).ToList();
                return listID.Select(x => x.Id_OurWord).ToList();
            }
            if (timeNotify == 0)
            {
                //Update ngay gui nhac nho
                //&& x.UpdatedDate.Value.Day != nowDay
                
                var listID = _detailOurWordRepository.GetMulti(x => x.Id_User == iduser && x.Schedule.Day != nowDay && x.Learned == 1).ToList();
                foreach (var id in listID)
                {
                    //Reset all properties
                    var temp = GetById(id.Id);
                    temp.Status = false;
                    temp.Schedule = currentVNTime.AddMinutes(temp.NextTime);
                    Update(temp);
                    Save();
                }
                //
                return listID.Select(x => x.Id_OurWord).ToList();
            }
            return null;
        }
        public void resetLearningTime(string userId)
        {
            var listID = _detailOurWordRepository.GetMulti(x => x.Id_User == userId).ToList();
            foreach (var id in listID)
            {
                var temp = GetById(id.Id);
                temp.Status = true;
                temp.Learned = 1;
                //Hom nay da gui
                temp.Schedule = DateTime.Now.AddDays(-1);
                Update(temp);
                Save();
            }
        }
         public void SetNextTime(int idWord, int NextTime, int NmumberOfNT)
        {
            DateTime currentVNTime = DateTime.Now.AddHours(7);
            var Word = _detailOurWordRepository.GetSingleByCondition(x => x.Id_OurWord == idWord);
            Word.NextTime = NextTime;
            Word.NumberAgain = NmumberOfNT;
            Word.Schedule = currentVNTime.AddMinutes(Word.NextTime);
            Update(Word);
            Save();
        }

        public  List<int> remindVoca(string userid)
        {
            //Check đúng ngày đúng giờ hay ko
            DateTime currentVNTime = DateTime.Now.AddHours(7);
            int nowDay = currentVNTime.Day;
            int nowMinue = currentVNTime.Minute;
            var listID = _detailOurWordRepository.GetMulti(x => x.Id_User == userid && x.Schedule.Day == nowDay && x.Schedule.Minute <= nowMinue && x.NumberAgain > 0).ToList();
            foreach (var id in listID)
            {
                //Reset all properties
                var temp = GetById(id.Id);
                temp.NumberAgain--;
                temp.Status = false;
                temp.Schedule = currentVNTime.AddMinutes(temp.NextTime);
                Update(temp);
                Save();
            }
            //
            return listID.Select(x => x.Id_OurWord).ToList();
        }
    }
}
