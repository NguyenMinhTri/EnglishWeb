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
    public class MessJson : MessageObject
    {
        public string text { get; set; }
    }
    public class ChatfuelJson
    {
        public ChatfuelJson()
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
    //Google giai thich tu 

    public class Entry
    {
        public Entry()
        {
            reverse_translation = new List<string>();
        }
        public string word { get; set; }
        public List<string> reverse_translation { get; set; }
    }

    public class Dict
    {
        public Dict()
        {
            entry = new List<Entry>();
            terms = new List<string>();
        }
        public string pos { get; set; }
        public List<string> terms { get; set; }
        public List<Entry> entry { get; set; }
        public string base_form { get; set; }
        public int pos_enum { get; set; }
    }

    public class GoogleTrans
    {
        public GoogleTrans()
        {
            sentences = new List<Sentence>();
            dict = new List<Dict>();
        }
        public List<Sentence> sentences { get; set; }
        public List<Dict> dict { get; set; }
        public string src { get; set; }
    }
}