using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Framework.ViewData.Control
{
    public class NumberFieldViewData : ControlViewData
    {
        public NumberFieldViewData()
        {
            MaxValue = int.MaxValue;
            MinValue = -int.MaxValue;
            Visible = true;
        }
        public bool Visible { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }
}
