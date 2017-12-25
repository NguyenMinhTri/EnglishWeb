using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("DictCache")]
    public class DictCache : Auditable
    {
		public int Id { get; set; }
		public string VocaID { get; set; }
		public string Pron { get; set; }
		public string MeanVi { get; set; }
		public string MeanEn { get; set; }
		public string SoundUrl { get; set; }
	}
}
