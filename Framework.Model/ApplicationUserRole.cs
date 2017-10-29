using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    [Table("AspNetUserRoles")]
    public class ApplicationUserRole : IdentityUserRole, IAuditable
    {

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public int Id { get; set; }
        [MaxLength(256)]
        public override String RoleId { get; set; }
        [MaxLength(256)]
        public override String UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool Status { get; set; }
        public bool Protected { get; set; }
     
    }
}
