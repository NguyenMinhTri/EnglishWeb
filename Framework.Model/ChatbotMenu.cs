using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    public class CallToAction2
    {
        public string title { get; set; }
        public string type { get; set; }
        public string payload { get; set; }
    }

    public class CallToAction
    {
        public CallToAction()
        {
         //   call_to_actions = new List<CallToAction2>();
        }
        public string title { get; set; }
        public string type { get; set; }
        public string payload { get; set; }
        public List<CallToAction2> call_to_actions { get; set; }
    }

    public class PersistentMenu
    {
        public PersistentMenu()
        {
          //  call_to_actions = new List<CallToAction>();
        }
        public string locale { get; set; }
        public List<CallToAction> call_to_actions { get; set; }
    }

    public class ChatbotMenu
    {
        public ChatbotMenu()
        {
            persistent_menu = new List<PersistentMenu>();
        }
        public List<PersistentMenu> persistent_menu { get; set; }
    }
}
