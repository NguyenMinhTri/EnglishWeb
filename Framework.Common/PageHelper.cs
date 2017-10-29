using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common
{
    public class PageHelper
    {
        public static int GetPageCount(int total, int itemShowCount)
        {
            return (total + itemShowCount - 1) / itemShowCount;
        }
    }
}
