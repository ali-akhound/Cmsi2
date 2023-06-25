using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AVA.Web.Mvc.Areas.Admin.Models.Base
{
    public class DropDownVm
    {
        public string Value { get; set; }
        public string Text { get; set; }

    }
    public class GridViewExportViewModel
    {
        public GridViewExportViewModel(string _ActionName = "ExportTo")
        {
            ActionName = _ActionName;
        }
        public string ModuleName { get; set; }
        public string ActionName { get; set; }
        public object Parameters { get; set; }
    }
    public class GridViewViewModel
    {
        public string KeyFieldName { get; set; }
        public string ControllerName { get; set; }
        public string GridViewID { get; set; }
        public string ActionName { get; set; }
        public object DataModel { get; set; }
        public object Params { get; set; }
        public Type Type { get; set; }
        public GridViewColumn[] Columns { get; set; }
    }
    public class HtmlEditorViewModel
    {
        public string InitEventName { get; set; }
        public string HtmlChangedEventName { get; set; }
        public string HtmlEditorID { get; set; }
    }
}