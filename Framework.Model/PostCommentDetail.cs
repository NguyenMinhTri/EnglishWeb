using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("PostCommentDetail")]
    public class PostCommentDetail : Auditable
    {
		public int Id { get; set; }
		public int Id_Post { get; set; }
        public int Id_Comment { get; set; }
		public String Id_Friend { get; set; }
        public String Content { get; set; }
        public String DateComment { get; set; }
	}
}
