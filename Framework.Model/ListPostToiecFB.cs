using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model.Feeds
{
    public class Datum
    {
        public string message { get; set; }
        public DateTime updated_time { get; set; }
        public string id { get; set; }
        public string DapAn { set; get; }
        public string imageURL { set; get; }
        public string GiaiThich { set; get; }
    }

    public class Paging
    {
        public string previous { get; set; }
        public string next { get; set; }
    }

    public class ListPostFB
    {
        public ListPostFB()
        {
            data = new List<Datum>();
        }
        public List<Datum> data { get; set; }
        public Paging paging { get; set; }
        public Error error { get; set; }
    }

    public class Error
    {
        public string message { get; set; }
        public string type { get; set; }
        public int code { get; set; }
        public int error_subcode { get; set; }
        public string fbtrace_id { get; set; }
    }

}
