using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("NewLetterRequest")]
    public class NewLetterRequest : Auditable
    {
		public String Name { get; set; }
		public String Email { get; set; }
	}
}
