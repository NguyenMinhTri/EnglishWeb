using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("SubType")]
    public class SubType : Auditable
    {
		public int Id { get; set; }
		public string Id_User { get; set; }
		public int Id_Type { get; set; }
	}
}
