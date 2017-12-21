using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    public class From
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Datum
    {
        public DateTime created_time { get; set; }
        public From from { get; set; }
        public string message { get; set; }
        public string id { get; set; }
    }

    public class Cursors
    {
        public string before { get; set; }
        public string after { get; set; }
    }

    public class Paging
    {
        public Cursors cursors { get; set; }
        public string next { get; set; }
    }

    public class ListCommentOfPost
    {
        public List<Datum> data { get; set; }
        public Paging paging { get; set; }
    }
}
