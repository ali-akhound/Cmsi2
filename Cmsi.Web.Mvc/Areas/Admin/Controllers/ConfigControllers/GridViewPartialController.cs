using DevExpress.Web.Mvc;
using AVA.Web.Mvc.Areas.Admin.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DevExpress.Web;
using System.Web;
using System.Collections.Specialized;
using AVA.UI.Helpers.CustomAttribute;
using System.Web.UI;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using AVA.UI.Helpers.Controller;
using System.Web.UI.WebControls;

namespace AVA.Web.Mvc.Controllers.Admin
{
    public class GridViewPartialController : Controller
    {
        // GET: Admin/_GridViewPartial  
     
        #region GridView
        public GridViewColumn[] ColumnGenerator<T>(List<T> records)
        {
            NameValueCollection parameter = new NameValueCollection();
            Type type = records.FirstOrDefault().GetType();
            var fields = type.GetProperties();
            GridViewColumn[] columns = new GridViewColumn[0];
            foreach (var item in fields)
            {
                if (item.GetCustomAttributes(typeof(ShowInGridview), false).Length > 0)
                {
                    Array.Resize<GridViewColumn>(ref columns, columns.Length + 1);
                    if (item.GetCustomAttributes(typeof(ImageGridviewColumn), false).Length > 0)
                    {
                        GridViewDataImageColumn column = new GridViewDataImageColumn();
                        column.Name = item.Name;
                        column.FieldName = item.Name;
                        column.PropertiesImage.ImageUrlFormatString = "{0}";
                        column.PropertiesImage.ImageWidth = 100;
                        column.Caption = item.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false).Cast<System.ComponentModel.DataAnnotations.DisplayAttribute>().Single().GetName();
                        column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                        if (item.GetCustomAttributes(typeof(GridColumnWidth), false).Length > 0)
                        {
                            column.Width = ((GridColumnWidth)item.GetCustomAttributes(typeof(GridColumnWidth), false)[0]).getWidth();
                        }
                        else
                        {
                            column.Width = Unit.Percentage(100);
                        }
                        if (item.GetCustomAttributes(typeof(GridColumnEncodeHtml), false).Length > 0)
                        {
                            column.PropertiesEdit.EncodeHtml = ((GridColumnEncodeHtml)item.GetCustomAttributes(typeof(GridColumnEncodeHtml), false)[0]).getEncodeHtml();
                        }
                        columns[columns.Length - 1] = column;
                    }
                    else
                    {
                        MVCxGridViewColumn column = new MVCxGridViewColumn();
                        if (item.GetCustomAttributes(typeof(GridColumnWidth), false).Length > 0)
                        {
                            column.Width = ((GridColumnWidth)item.GetCustomAttributes(typeof(GridColumnWidth), false)[0]).getWidth();
                        }
                        else
                        {
                            column.Width = Unit.Percentage(100);
                        }
                        if (item.GetCustomAttributes(typeof(GridColumnEncodeHtml), false).Length > 0)
                        {
                            column.PropertiesEdit.EncodeHtml = ((GridColumnEncodeHtml)item.GetCustomAttributes(typeof(GridColumnEncodeHtml), false)[0]).getEncodeHtml();
                        }
                        column.Name = item.Name;
                        column.FieldName = item.Name;
                        //item.GetCustomAttributes().Where(attr=>attr.)
                        column.Caption = item.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false).Cast<System.ComponentModel.DataAnnotations.DisplayAttribute>().Single().GetName();
                        column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                        Type propertyType = item.PropertyType;
                        if (propertyType == typeof(System.Boolean))
                        {
                            column.ColumnType = MVCxGridViewColumnType.CheckBox;
                        }
                        if (propertyType.IsGenericType)
                        {
                            if (item.PropertyType.GetGenericArguments()[0] == typeof(System.Boolean))
                            {
                                column.ColumnType = MVCxGridViewColumnType.CheckBox;
                            }

                        }
                        if (item.GetCustomAttributes(typeof(HyperLinkGridviewColumn), false).Length > 0)
                        {
                            column.ColumnType = MVCxGridViewColumnType.HyperLink;
                            HyperLinkProperties properties = column.PropertiesEdit as HyperLinkProperties;
                            properties.NavigateUrlFormatString = "{0}";
                            properties.TextField = ((HyperLinkGridviewColumnText)item.GetCustomAttributes(typeof(HyperLinkGridviewColumnText), false)[0]).getColumnText();
                        }

                        columns[columns.Length - 1] = column;
                    }
                }
            }
            return columns;
        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial<T>(List<T> records, string ControllerName, string ActionName, string GridViewID, string KeyFieldName, object Params = null)
        {
            Type type = records.FirstOrDefault().GetType();
            GridViewColumn[] columns = ColumnGenerator(records);
            return PartialView("~/Areas/Admin/Views/Shared/GridViewPartial.cshtml", new GridViewViewModel()
            {
                DataModel = records,
                Columns = columns,
                ActionName = ActionName,
                ControllerName = ControllerName,
                GridViewID = GridViewID,
                KeyFieldName = KeyFieldName,
                Type = type,
                Params = Params
            });
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete(System.String ID)
        {
            var model = new object[0];
            if (ID != null)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GridViewPartial", model);
        }
        public ActionResult ExportTo<T>(List<T> records,int ActionType, string GridViewID)
        {
            var settings = new GridViewSettings()
            {
                Name = GridViewID,
                Theme = "Material",
                RightToLeft = DevExpress.Utils.DefaultBoolean.True,
                Width = System.Web.UI.WebControls.Unit.Percentage(100),
            };
            settings.Columns.AddRange(new GridViewPartialController().ColumnGenerator(records));
            if ((int)ControllerHelper.ExportGridviewActionType.Excel == ActionType)
                return GridViewExtension.ExportToXlsx(settings, records);
            if ((int)ControllerHelper.ExportGridviewActionType.Word == ActionType)
                return GridViewExtension.ExportToRtf(settings, records);
            else
                return ControllerHelper.ErrorResult("بروز خطا");
        }
        #endregion
    }
}