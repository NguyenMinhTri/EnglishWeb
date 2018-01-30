using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    public class Recipient
    {
        public string id { get; set; }
    }

    public class NotiButton
    {
        public NotiButton()
        {
            type = "postback";
            title = "Trả lời";
        }
        public string type { get; set; }
        public string title { get; set; }
        public string payload { get; set; }
    }

    public class Payload
    {
        public Payload()
        {
            buttons = new List<NotiButton>();
            template_type = "button";
        }
        public string template_type { get; set; }
        public string text { get; set; }
        public List<NotiButton> buttons { get; set; }
    }

    public class Attachment
    {
        public Attachment()
        {
            payload = new Payload();
            type = "template";
        }
        public string type { get; set; }
        public Payload payload { get; set; }
    }

    public class Message
    {
        public Message()
        {
            attachment = new Attachment();
        }
        public Attachment attachment { get; set; }
    }

    public class FBPostNoti
    {
        public FBPostNoti()
        {
            recipient = new Recipient();
            message = new Message();
        }
        public Recipient recipient { get; set; }
        public Message message { get; set; }
    }
}
