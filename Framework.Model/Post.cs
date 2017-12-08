using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("Post")]
    public class Post : Auditable
    {
		public int Id { get; set; }
		public String Id_User { get; set; }
		public String Content { get; set; }
        public int Id_Type { get; set; }
        public int Option { get; set; }
        public String DatePost { get; set; }
        public int Post_Status { get; set; }

    }
    //Post - Post vote
    //Comment - Comment vote
}
