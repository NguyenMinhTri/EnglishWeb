using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("OurWord")]
    public class OurWord : Auditable
    {
		public int Id { get; set; }
		public String Word { get; set; }
		public String Pronounciation { get; set; }
		public String MeanVi { get; set; }
		public String MeanEn { get; set; }
	}
}
