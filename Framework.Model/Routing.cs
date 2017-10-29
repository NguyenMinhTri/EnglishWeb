using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    public class Routing : Auditable
    {
        public int Id { get; set; }
        public String FriendlyUrl { get; set; }
        public String Controller { get; set; }
        public String Action { get; set; }
        public String EntityId { get; set; }
    }
}
