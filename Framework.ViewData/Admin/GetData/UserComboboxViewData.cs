using Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ViewData.Admin.GetData
{
    public class UserComboboxViewData  : BaseViewData<ApplicationUser>
    {
        public String Id { get; set; }
        public String UserName { get; set; }
    }
}
