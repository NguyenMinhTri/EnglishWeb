using Framework.Model;
using Framework.ViewData.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ViewData.Admin.Input
{
    public class BaseInputViewData<S> : BaseViewData<S> where S : class, IAuditable
    {
        
        //public String CreatedDateStr { get; set; }
        [DateFieldViewData(Label = "Created Date", RowIndex = 100, Visible = false, NotChangeWhenUpdate = true, HideOnTable=true , IsRender = false)]
        public DateTime? CreatedDate { set; get; }
        [TextFieldViewData(Label = "Created By", RowIndex = 101, Visible = false, NotChangeWhenUpdate = true, HideOnTable = true, IsRender = false)]
        public String CreatedBy { get; set; }
        [TextFieldViewData(Label = "Updated By", RowIndex = 102, Visible = false, NotChangeWhenUpdate = true, HideOnTable = true, IsRender = false)]
        public String UpdatedBy { get; set; }
        [DateFieldViewData(Label = "Updated Date", RowIndex = 103, Visible = false, NotChangeWhenUpdate = true, HideOnTable = true, IsRender = false)]
        public DateTime? UpdatedDate { set; get; }
        [SwitchViewData(Label = "Status", RowIndex = 104, ShowWhenAdd = false)]
        virtual public bool Status { get; set; }

    }
}
