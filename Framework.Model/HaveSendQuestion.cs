using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("HaveSendQuestion")]
    public class HaveSendQuestion : Auditable
    {
		public int Id { get; set; }
		public string UserID { get; set; }
		public int QuesID { get; set; }
	}
}
