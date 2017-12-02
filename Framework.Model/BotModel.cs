using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model.Bot
{
    public class Payload
    {
        public string url { get; set; }
    }

    public class Attachment
    {
        public string type { get; set; }
        public Payload payload { get; set; }
    }

    public class Messages
    {
        public Attachment attachment { get; set; }
    }

    public class RootObject
    {
        public List<Messages> messages { get; set; }
    }
}
