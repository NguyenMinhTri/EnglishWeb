using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common
{
    public class QuestionModel
    {
        string m_Ques { set; get; }
        string m_awnA { set; get; }
        string m_awnB { set; get; }
        string m_awnC { set; get; }
        string m_awnD { set; get; }
    }
    public class AnalyzeQuestion
    {
        public bool alazyeQues(string inQues)
        {
            int countAnw = 0;
            if(inQues.IndexOf("(A)") > -1 || inQues.IndexOf("A.") >-1)
            {
                countAnw++;
            }
            if (inQues.IndexOf("(B)") > -1 || inQues.IndexOf("C.") > -1)
            {
                countAnw++;
            }

            return false;
        }
    }
}
