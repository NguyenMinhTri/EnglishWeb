using Framework.Controllers;
using Framework.Model;
using Framework.Model.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.ViewModels
{

    public class DictionariesViewModel : LayoutViewModel, IRef<DictionaryController>
    {
        public DictionariesViewModel()
        {
            m_Explanation = new List<Explanation>();
            m_ExaTraCau = new ExampleTraCau();
            isGoogleTrans = false;
        }
        public bool isGoogleTrans { set; get; }
        public string m_Voca { set; get; }
        public string m_Type { set; get; }
        public string m_Pron { set; get; }
        public string m_SoundUrl { set; get; }
        public string m_MeanVn { set; get; }
        public List<Explanation> m_Explanation;
        public ExampleTraCau m_ExaTraCau;
        public GoogleTrans m_GoogleTrans;
    }
    public class OldWordViewModelItem
    {
        public int Id { get; set; }
        public string m_Voca { set; get; }
        public int Learned { get; set; }
    }
    public class OldWordsViewModel : LayoutViewModel, IRef<DictionaryController>
    {
        public OldWordsViewModel()
        {
            ListOldWords = new List<OldWordViewModelItem>();
        }
        public List<OldWordViewModelItem> ListOldWords { set; get; }
    }
}