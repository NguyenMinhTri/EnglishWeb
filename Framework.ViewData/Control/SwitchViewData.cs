using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ViewData.Control
{
    public class SwitchViewData : ControlViewData
    {
        public SwitchViewData() : base()
        {
            FormatterFunct = "switchFormatter";
        }
        public bool Value { get; set; }
    }
}
