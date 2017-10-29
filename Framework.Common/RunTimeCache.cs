using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Framework.Common
{
    public class RunTimeCache<T> where T : class
    {
        public delegate T GetCacheDataFunct();
        public static Dictionary<string, bool> Refreshs = new Dictionary<string, bool>();
        public static T GetCacheData(String cacheName, GetCacheDataFunct getCacheFunct)
        {
            T cache = HttpRuntime.Cache[cacheName] as T;
            if (cache == null || Refreshs[cacheName] == false)
            {
                cache = getCacheFunct();
                HttpRuntime.Cache[cacheName] = cache;
                Refreshs[cacheName] = true;
            }
            return cache;
        }
    }
}
