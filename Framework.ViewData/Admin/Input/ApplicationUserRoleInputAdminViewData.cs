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

    public class ApplicationUserRoleInputAdminViewData : BaseViewData<ApplicationUserRole>
    {
        [NumberFieldViewData(Label = "Id", RowIndex = 0, Visible = false)]
        public int Id { get; set; }
        [ComboboxViewData(Label = "Role", RowIndex = 1, PlaceOnTable=false,ValueMember="Id",DisplayMember="Name",Url="/AdminApplicationUserRole/GetApplicationRoles")]
        public String RoleId { get; set; }
        [ControlViewData(Label="Role",IsRender=false,RowIndex=2)]
        public String RoleDisplay { get; set; }
        [ControlViewData(Label = "User Id", PlaceOnTable=false, NotChangeWhenUpdate=true, IsRender = false, RowIndex = 3)]
        public String UserId { get; set; }
    }
}
