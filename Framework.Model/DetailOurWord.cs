using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("DetailOurWord")]
    public class DetailOurWord : Auditable
    {
        public DetailOurWord()
        {
            NextTime = 0;
            NumberAgain = 0;
        }
        public int Id { get; set; }
		public int Id_OurWord { get; set; }
		public String Id_User { get; set; }
		public String Id_Messenger { get; set; }
		public int Learned { get; set; }
		public DateTime Schedule { get; set; }

        public int NextTime { set; get; }
        public int NumberAgain { set; get; }
	}
}
