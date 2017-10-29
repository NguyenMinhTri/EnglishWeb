using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace  Framework.ViewData.Control
{
    public class DateFieldViewData : ControlViewData
    {
        public DateFieldViewData() : base()
        {
            FormatterFunct = "dateFormatter";
        }
    }
}