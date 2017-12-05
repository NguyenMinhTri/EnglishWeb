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
    public interface IOurWordService : IQlService<OurWord>
    {
        //  OurWord addDictfavorite(OurWord newWord, ApplicationUser User);
        
    }
    public class OurWordService : QlService<OurWord>, IOurWordService
    {
        IOurWordRepository _ourWordRepository;
        IDetailOurWordRepository _detailOurWord;
        public OurWordService(IOurWordRepository ourWordRepository,IDetailOurWordRepository detailOurWord, IUnitOfWork unitOfWork) : base(ourWordRepository, unitOfWork) 
        {
            this._repository = ourWordRepository;
            this._detailOurWord = detailOurWord;
            this._ourWordRepository = ourWordRepository;
        }

        //public OurWord addDictfavorite(OurWord newWord, ApplicationUser User)
        //{
        //    Add(newWord);
        //    Save();
        //    return newWord;
        //    DetailOurWord detailWord =newWord new DetailOurWord();
        //    detailWord.Id_User = User.Id;
        //    detailWord.Id_Messenger = User.Id_Messenger;
        //    detailWord.Id_OurWord = newWord.Id;
        //    detailWord.Schedule = new DateTime(2017, 12, 31, 1, 0, 0);
        //    detailWord.Learned = 1;
        //    _detailOurWord.Add(detailWord);
        //}

    }
}
