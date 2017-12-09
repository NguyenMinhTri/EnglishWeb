using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ViewData.Admin.GetData
{
    public class ListUserNofity
    {
        public ListUserNofity()
        {
            reminduser = new List<RemindUser>();
        }
        public List<RemindUser> reminduser { get; set; }
    }
    public class RemindUser
    {
        public RemindUser()
        {
            vocainfo = new List<VocaInfo>();
        }
        public string UrlStudy { set; get; }
        public string IdMess { get; set; }
        public List<VocaInfo> vocainfo { get; set; }
    }
    public class VocaInfo
    {
        public string voca { get; set; }
        public string pron { get; set; }
        public string usecase { get; set; }
        public string meanVN { get; set; }
        public string meanEN { get; set; }
    }
}
