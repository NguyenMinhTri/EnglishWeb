using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("ToiecGroup")]
    public class ToiecGroup : Auditable
    {
		public int Id { get; set; }
		public string Id_Post { get; set; }
		public string Content { get; set; }
		public string ImageUrl { get; set; }
		public string DapAn { get; set; }
		public string GiaiThich { get; set; }
	}
}
