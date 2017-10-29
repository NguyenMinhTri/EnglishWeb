using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ViewData.Control
{
    public class VideoFieldViewData : ControlViewData
    {
        public VideoFieldViewData() : base()
        {
            FormatterFunct = "videoFormatter";
        }
    }
}
