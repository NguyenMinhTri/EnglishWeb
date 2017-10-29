using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common
{
    public class StringHelper
    {
        public static string FirstLetterLower(string name)
        {
            return String.Format("{0}{1}", name.First().ToString().ToLowerInvariant(), name.Substring(1));
        }


        /// <summary>
        /// get random string
        /// </summary>
        /// <returns></returns>
        public static string GetRandomText()
        {
            StringBuilder randomText = new StringBuilder();
            string alphabets = "012345679ACEFGHKLMNPRSWXZabcdefghijkhlmnopqrstuvwxyz";
            Random r = new Random();
            for (int j = 0; j <= 5; j++)
            {
                randomText.Append(alphabets[r.Next(alphabets.Length)]);
            }
            return randomText.ToString();
        }

    }
}
