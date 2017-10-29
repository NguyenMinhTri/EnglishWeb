using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model.TableJoin
{
    public class ApplicationUserRoleGetAll 
    {
        public String RoleDisplay { get; set; }
        public String UserDisplay { get; set; }
        public int Id { get; set; }
        public String ApplicationRole_Id { get; set; }
        public String ApplicationUser_Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool Status { get; set; }
    }
}
