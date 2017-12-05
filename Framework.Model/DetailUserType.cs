using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("DetailUserType")]
    public class DetailUserType : Auditable
    {
		public int Id { get; set; }
		public string UserID { get; set; }
		public int Type { get; set; }
		public float Level { get; set; }
		public int WrongAnsw { get; set; }
		public int CorrectAnsw { get; set; }
	}
}
