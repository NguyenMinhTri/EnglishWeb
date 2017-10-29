using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ViewData.Control
{
    public class ComboboxViewData : ControlViewData
    {
        public ComboboxViewData()
        {
            FormatterFunct = "comboboxFormatter";
        }
        public String DisplayMember;
        public String ValueMember;
        public String Url;
    }
}
