using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("Notification")]
    public class Notification : Auditable
    {
		public int Id { get; set; }
		public String Id_User { get; set; }
		public String Id_Friend { get; set; }
		public int Id_Post { get; set; }
        public int Id_Comment { get; set; }
        public String DateComment { get; set; }
	}
}
