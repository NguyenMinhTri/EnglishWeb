using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Framework.Model
{

    [Table("Relationship")]
    public class Relationship : Auditable
    {
        public int Id{get;set;}
        
	}
}
