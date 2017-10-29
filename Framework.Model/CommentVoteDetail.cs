using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("CommentVoteDetail")]
    public class CommentVoteDetail : Auditable
    {
		public int Id { get; set; }
		public int Id_Comment { get; set; }
		public String Id_Friend { get; set; }
		public int VoteAmout { get; set; }
	}
}
