using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model.Google
{
    public class Sentence
    {
        public string trans { get; set; }
        public string orig { get; set; }
        public int backend { get; set; }
    }

    public class GoogleTransJson
    {
        public List<Sentence> sentences { get; set; }
        public string src { get; set; }
    }
}