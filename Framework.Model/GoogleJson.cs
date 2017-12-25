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
    public class Recipient
    {
        public string id { get; set; }
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
            type = "audio";
        }
        public string type { get; set; }
        public Payload2 payload { get; set; }
    }

    public class AttachmentJson : MessageObject
    {
        public AttachmentJson()
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
            recipient = new Recipient();
            messages = new List<MessageObject>();
        }
        public Recipient recipient { get; set; }
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

    public class QuickReplyMess
    {
        public QuickReplyMess()
        {
            content_type = "text";
        }
        public string content_type { get; set; }
        public string title { get; set; }
        public string payload { get; set; }
    }

    public class MessageQuick : MessageObject
    {
        public MessageQuick()
        {
            quick_replies = new List<QuickReplyMess>();
        }
        public string text { get; set; }
        public List<QuickReplyMess> quick_replies { get; set; }
    }

    public class JsonMessengerText 
    {
        public JsonMessengerText()
        {
            recipient = new Recipient();
        }
        public Recipient recipient { get; set; }
        public MessageObject message { get; set; }
    }
}