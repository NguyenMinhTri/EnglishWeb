using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.ViewData.Control
{
    public class ControlViewData : Attribute
    {
        public ControlViewData()
        {
            ShowWhenAdd = true;
            IsRender = true;
            NotChangeWhenUpdate = false;
            PlaceOnTable = true;
            Visible = true;
            Require = true;
            HideOnTable = false;
            Width = 10;
            Sortable = true;
            RowIndex = 10000;
            FormatterFunct = "defaultFormatter";
        }
        public bool PlaceOnTable { get; set; }
        public float Width { get; set; }
        public bool HideOnTable { get; set; }
        public bool NotChangeWhenUpdate { get; set; }
        public int RowIndex;
        public String Label { get; set; }
        public String FieldName { get; set; }
        public String Placehoder { get; set; }
        public bool ShowWhenAdd { get; set; }
        public bool Require { get; set; }
        public bool IsRender { get; set; }
        public bool Visible { get; set; }
        public bool Sortable { get; set; }
        public String FormatterFunct { get; set; }
        public String PartialViewName
        {
            get
            {
                String modelName = this.GetType().Name;
                modelName = modelName.Substring(0, modelName.Length - 8);
                return "~/Views/Control/" + modelName + ".cshtml";
            }
        }
    }
}