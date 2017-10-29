using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ViewData.Control
{
    public class AutoFormAttribute : Attribute
    {
        public String PartialViewName { get; set; }
        public String Group { get; set; }
        public ControlViewData ViewData { get; set; }
        public String ViewDataType { get; set; }
    }
}
