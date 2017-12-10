using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("LearnVoca")]
    public class LearnVoca : Auditable
    {
		public int Id { get; set; }
		public string Id_User { get; set; }
		public DateTime Learn_Day { get; set; }
		public int Wrong { get; set; }
		public int NumbSentences { get; set; }
	}
}
