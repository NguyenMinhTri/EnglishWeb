using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.ViewData.Control
{
    public class DateOfWeekViewData : ControlViewData
    {
        public DateOfWeekViewData()
        {
            FormatterFunct = "comboboxFormatter";
        }
    }
}