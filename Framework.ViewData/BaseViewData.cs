using Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.ViewData
{
    public delegate void OnConvertDelegate<D, S>(D dest, S source);
    public class BaseViewData<S> where S : class
    {
        public virtual void OnConvert(S source, params object[] values)
        {
            dynamic dest = this;
            PropertyCopy.Copy(source, dest);
        }
    }


    public class ViewDataMapper<D, S>
        where D : BaseViewData<S>, new()
        where S : class, new()
    {
        public static D Map(S source, params object[] values)
        {
            D dest = new D();
            dest.OnConvert(source, values);
            return dest;
        }
        public static List<D> MapObjects(List<S> sources, params object[] values) 
        {
            List<D> dests = new List<D>();
            foreach (S source in sources)
            {
                D dest = new D();
                dest.OnConvert(source, values);

                dests.Add(dest);
            }
            return dests;
        }

        public static List<D> MapObjectWithCondition(List<S> sources, OnConvertDelegate<D,S> onConvert)
        {
            List<D> dests = new List<D>();
            foreach (S source in sources)
            {
                D dest = new D();
                dest.OnConvert(source, null);
                if (onConvert!=null)
                {
                    onConvert(dest, source);
                }

                dests.Add(dest);
            }
            return dests;
        }
    }
}