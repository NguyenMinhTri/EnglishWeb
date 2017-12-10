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
		public String Id_User { get; set; }
        public String Content { get; set; }
        public String DateComment { get; set; }
        public int UpVote { get; set; }
        public int DownVote { get; set; }
        public bool Corrected { get; set; }
    }
}
