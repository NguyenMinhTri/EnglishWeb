using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("Friend")]
    public class Friend : Auditable
    {
		public int Id { get; set; }
		public String Id_User { get; set; }
		public String Id_Friend { get; set; }
		public int CodeRelationshipId { get; set; }
	}
}
