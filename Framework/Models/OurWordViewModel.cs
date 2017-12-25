using Framework.Controllers;
using Framework.Model;
using Framework.Model.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.ViewModels
{

    public class OurWordViewModel : LayoutViewModel, IRef<DictionaryController>
    {
        public String VocaID { get; set; }
        public String Pron { get; set; }
        public String MeanVi { get; set; }
        public String MeanEn { get; set; }
        public String SoundUrl { set; get; }
    }

}