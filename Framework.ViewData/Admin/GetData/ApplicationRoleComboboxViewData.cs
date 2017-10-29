using Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ViewData.Admin.GetData
{
    public class ApplicationRoleComboboxViewData : BaseViewData<ApplicationRole>
    {
        public String Id { get; set; }
        public String Name { get; set; }
    }
}
