using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model.TableJoin
{
    public class DoctorGetAll : Auditable
    {
        public int Id { get; set; }
        public String UserId { get; set; }
        public String UserName { get; set; }
        public String LinkFacebook { get; set; }
        public String LinkTwistter { get; set; }
        public String LinkTumblr { get; set; }
        public String LinkInstagram { get; set; }
        public String Name { get; set; }
        public int DoctorTypeId { get; set; }
        public String DoctorTypeName { get; set; }
    }
}
