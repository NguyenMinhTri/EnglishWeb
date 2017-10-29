using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ViewData.Control
{
    public class AutoForm<VM> where VM : new()
    {
        public List<ControlViewData> GetControls()
        {
            List<ControlViewData> controls = new List<ControlViewData>();

            VM instance = new VM();
            var propertiesString = instance.GetType().GetProperties().Select(propety => propety.Name).ToList();

            foreach (var propertyString in propertiesString)
            {
                var property = instance.GetType().GetProperty(propertyString);
                var attrType = typeof(ControlViewData);

                var controlAnotations = property.GetCustomAttributes(attrType, false);
                if (controlAnotations.Count()==0)
                    continue;
                var control = (ControlViewData)controlAnotations[0];
                control.FieldName = propertyString;
                controls.Add(control);
                if (control.Placehoder == null || control.Placehoder == "")
                {
                    control.Placehoder = control.Label;
                }
            }

            controls = controls.OrderBy(x => x.RowIndex).ToList();

            return controls;
        }
    }
}
