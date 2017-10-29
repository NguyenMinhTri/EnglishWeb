using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model.TableJoin
{
    public class DoctorWithName
    {
        public int Id { get; set; }
        public String UserId { get; set; }
        public String DoctorName { get; set; }
        public bool Status { get; set; }
    }
}
