using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    public class OxfordDict
    {
        public OxfordDict()
        {
            m_Explanation = new List<Explanation>();
        }
        public string m_Voca { set; get; }
        public string m_Type { set; get; }
        public string m_Pron { set; get; }
        public string m_SoundUrl { set; get; }
        public List<Explanation> m_Explanation;
        public ExampleTraCau m_ExTraCau;
    }
    //
    public class Explanation
    {
        public Explanation()
        {
            m_ListExample = new List<string>();
        }
        public string m_UseCase { set; get; }
        public List<string> m_ListExample;
    }

    //Json tracau
    public class Fields
    {
        public string en { get; set; }
        public string vi { get; set; }
    }

    public class Sentence
    {
        public string _id { get; set; }
        public Fields fields { get; set; }
    }

    public class ExampleTraCau
    {
        public string language { get; set; }
        public List<Sentence> sentences { get; set; }
        public List<object> suggestions { get; set; }
        public List<object> tratu { get; set; }
    }
}
