using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model.Google
{
    public interface MessageObject
    {

    }
    public class Payload2
    {
        public string url { get; set; }
    }

    public class Attachment2
    {
        public Attachment2()
        {
            payload = new Payload2();
        }
        public string type { get; set; }
        public Payload2 payload { get; set; }
    }

    public class Message2 : MessageObject
    {
        public Message2()
        {
            attachment = new Attachment2();
        }
        
        public Attachment2 attachment { get; set; }
    }
    public class Message3 : MessageObject
    {
        public string text { get; set; }
    }
    public class RootObject2
    {
        public RootObject2()
        {
            messages = new List<MessageObject>();
        }
        public List<MessageObject> messages { get; set; }
    }
    public class Sentence
    {
        public string trans { get; set; }
        public string orig { get; set; }
        public int backend { get; set; }
    }

    public class GoogleTransJson
    {
        public List<Sentence> sentences { get; set; }
        public string src { get; set; }
    }
}