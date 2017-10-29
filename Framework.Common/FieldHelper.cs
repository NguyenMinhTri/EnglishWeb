using System.Linq;
using System.Reflection;

namespace Framework.Common
{
    public static class FieldHelper
    {
        public static D CopyNotNullValue<D, S>(D dest, S source)
            where S : class
            where D : class
        {
            PropertyInfo[] props = typeof(D).GetProperties();
            PropertyInfo[] propsSource = typeof(S).GetProperties();
            var nProp = props.Count();

            for (int iProp = 0; iProp < nProp; iProp++)
            {
                var p = props[iProp];
                if (!(propsSource.Count(x => x.Name == p.Name && x.PropertyType == p.PropertyType) > 0))
                {
                    continue;
                }

                if (source.GetType().GetProperty(p.Name) == null)
                    continue;

                var value = source.GetType().GetProperty(p.Name).GetValue(source, null);

                if (value != null)
                {
                    PropertyInfo destPropertyInfo = dest.GetType().GetProperty(p.Name);
                    destPropertyInfo.SetValue(dest, value);

                }
            }

            return dest;

        }

    }
}
