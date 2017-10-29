using Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.ViewModels
{

    public class DictionariesViewModel : LayoutViewModel, IRef<DictionaryController>
    {
        public string Word { get; set; }
        public string Mp3 { get; set; }


    }

    public class OldWordsViewModel : LayoutViewModel, IRef<DictionaryController>
    {
    }
}