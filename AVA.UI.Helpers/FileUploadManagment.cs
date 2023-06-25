using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
/// <summary>
/// Summary description for ImageManagment
/// </summary>
/// 
namespace AVA.UI.Helpers.FileUploadManagment
{
    public class FileUploadManagment
    {
        public FileUploadManagment()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //مدرک کاربر را اپلود می کند و ان را به فولد Temp منتقل می کند و نام ان را در ارایه ای در Session  ذخیره می کند.
        public static string UploadFile(string FolderPath, string TempFolder, HttpPostedFileBase File, AppFileType FileType, string SpecificFileType = "", int MaxSize = 512000)
        {
            var extension = Path.GetExtension(File.FileName);
            string FileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
            string FilePath = FolderPath + FileName + extension;
            string thumpFile = TempFolder + FileName + extension;
            var path = HttpContext.Current.Server.MapPath(FilePath);
            if (Convert.ToInt64(File.ContentLength) <= MaxSize)
            {
                if (FileType == AppFileType.Image)
                {
                    if (CheckAppImageFileType(File.FileName) && (extension.ToLower() == SpecificFileType.ToLower().Trim() || SpecificFileType == ""))
                    {
                        File.SaveAs(path);
                        return FilePath;
                    }
                }
                else if (FileType == AppFileType.Document)
                {
                    if (CheckAppDocumentFileType(File.FileName) && (extension.ToLower() == SpecificFileType.ToLower().Trim() || SpecificFileType == ""))
                    {
                        File.SaveAs(path);
                        return FilePath;
                    }
                }
                else if (FileType == AppFileType.Compress && (extension.ToLower() == SpecificFileType.ToLower().Trim() || SpecificFileType == ""))
                {
                    if (CheckAppZipFileType(File.FileName))
                    {
                        File.SaveAs(path);
                        return FilePath;
                    }
                }
                else if (FileType == AppFileType.Video && (extension.ToLower() == SpecificFileType.ToLower().Trim() || SpecificFileType == ""))
                {
                    if (CheckAppVideoFileType(File.FileName))
                    {
                        File.SaveAs(path);
                        return FilePath;
                    }
                }
                return "";
            }
            return "";
        }
        public enum AppFileType
        {
            Image = 0,
            Document = 1,
            Compress = 2,
            Video=3
        }
        static bool CheckAppImageFileType(string FileName)
        {
            string ext = Path.GetExtension(FileName).ToLower();
            switch (ext.ToLower())
            {
                case ".JPG":
                    return true;
                case ".JPEG":
                    return true;

                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;

                case ".png":
                    return true;

                default:
                    return false;
            }
        }
        static bool CheckAppDocumentFileType(string FileName)
        {
            string ext = Path.GetExtension(FileName).ToLower();
            switch (ext.ToLower())
            {
                case ".doc":
                    return true;
                case ".docx":
                    return true;
                case ".pdf":
                    return true;
                case ".pub":
                    return true;
                case ".pptx":
                    return true;
                case ".ppt":
                    return true;
                default:
                    return false;
            }
        }
        static bool CheckAppZipFileType(string FileName)
        {
            string ext = Path.GetExtension(FileName).ToLower();
            switch (ext.ToLower())
            {
                case ".ZIP":
                    return true;
                case ".zip":
                    return true;
                default:
                    return false;
            }
        }
        static bool CheckAppVideoFileType(string FileName)
        {
            string ext = Path.GetExtension(FileName).ToLower();
            switch (ext.ToLower())
            {
                case ".mp4":
                    return true;
                default:
                    return false;
            }
        }
    }
}