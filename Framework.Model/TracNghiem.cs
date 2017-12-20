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
            ABCD = new DapAn[4];
            ABCD[0] = new DapAn();
            ABCD[1] = new DapAn();
            ABCD[2] = new DapAn();
            ABCD[3] = new DapAn();
            Question = "";
            UrlImage = "";
        }
        public string Question { set; get; }
        public string UrlImage { set; get; }
        public DapAn [] ABCD { set; get; }
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
