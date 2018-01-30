using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("VocaSchedule")]
    public class VocaSchedule : Auditable
    {
		public int id { get; set; }
		public string UserID { get; set; }
		public int IdWord { get; set; }
		public int NextTime { get; set; }
		public bool Status { get; set; }
	}
}
