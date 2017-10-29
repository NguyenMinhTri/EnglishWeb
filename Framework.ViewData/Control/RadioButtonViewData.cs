using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Framework.ViewData.Control
{
    public class RadioButtonViewData : ControlViewData
    {
        public String DisplayMember { get; set; }
        public String ValueMember { get; set; }
        public String Url { get; set; }
        public String CheckedMember { get; set; }
    }
}
