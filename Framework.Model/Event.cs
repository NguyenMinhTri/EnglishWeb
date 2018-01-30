using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("Event")]
    public class Event : Auditable
    {
		public int id { get; set; }
		public string userID { get; set; }
		public string text { get; set; }
		public DateTime eventstart { get; set; }
		public DateTime eventend { get; set; }
	}
}
