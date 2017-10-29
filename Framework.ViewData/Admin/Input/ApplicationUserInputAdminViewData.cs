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

    public class ApplicationUserInputAdminViewData : BaseInputViewData<ApplicationUser>
    {
        [TextFieldViewData(Label = "Id" , HideOnTable=true, RowIndex = 0, Visible = false)]
        public String Id { get; set; }
        [TextFieldViewData(Label = "Name", RowIndex = 1)]
        public String Name { get; set; }
        [ControlViewData(Label = "UserName",RowIndex=2, IsRender = false, NotChangeWhenUpdate = true)]
        public string UserName { get; set; }
        [ImageFileViewData(Label="Avatar",RowIndex=3)]
        public String Avatar { get; set; }
        [TextFieldViewData(Label = "Address", RowIndex = 4)]
        public String Address { get; set; }
        [TextFieldViewData(Label = "Place", RowIndex = 5)]
        public String Place { get; set; }
        [EmailFieldViewData(Label = "Email", RowIndex = 7)]
        public string Email { get; set; }
        [TelFieldViewData(Label = "Phone Number", RowIndex = 8)]
        public string PhoneNumber { get; set; }
        [ControlViewData(PlaceOnTable=false,IsRender=false,NotChangeWhenUpdate=true)]
        public bool Logined { get; set; }
        // Summary:
        //     Used to record failures for the purposes of lockout
        [ControlViewData(PlaceOnTable = false, IsRender = false, NotChangeWhenUpdate = true)]
        public int AccessFailedCount { get; set; }
        
        [ControlViewData(PlaceOnTable = false, IsRender = false, NotChangeWhenUpdate = true)]
        public bool EmailConfirmed { get; set; }
        [ControlViewData(PlaceOnTable = false, IsRender = false, NotChangeWhenUpdate = true)]
        public bool LockoutEnabled { get; set; }
        [ControlViewData(PlaceOnTable = false, IsRender = false, NotChangeWhenUpdate = true)]
        public string PasswordHash { get; set; }
        [ControlViewData(PlaceOnTable = false, IsRender = false, NotChangeWhenUpdate = true)]
        public bool PhoneNumberConfirmed { get; set; }
        [ControlViewData(PlaceOnTable = false, IsRender = false, NotChangeWhenUpdate = true)]
        public string SecurityStamp { get; set; }
        [ControlViewData(PlaceOnTable = false, IsRender = false, NotChangeWhenUpdate = true)]
        public bool TwoFactorEnabled { get; set; }
        [ControlViewData(PlaceOnTable = false, IsRender = false)]
        public bool IsDoctor { get; set; }
    }
}
