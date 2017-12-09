using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    public class TracNghiem
    {
        public TracNghiem ()
        {
            ABCD = new List<DapAn>();
        }
        public string Question { set; get; }
        public List<DapAn> ABCD { set; get; }
    }
    public class DapAn
    {
        public DapAn ()
        {
            Checked = false;
        }
        public string Contain { set; get; }
        public bool Checked { set; get; }
    }
        
}
