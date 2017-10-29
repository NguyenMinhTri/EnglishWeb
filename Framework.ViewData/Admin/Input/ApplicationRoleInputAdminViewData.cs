using Framework.Model;
using Framework.Model.TableJoin;
using Framework.ViewData.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Framework.ViewData.Admin.Input
{

    public class ApplicationRoleInputAdminViewData : BaseInputViewData<ApplicationRole>
    {
        [TextFieldViewData(Label = "Id", RowIndex = 0, Visible = false)]
        public String Id { get; set; }
        [TextFieldViewData(Label = "Role Name", RowIndex = 1)]
        public String Name { get; set; }

    }
}
