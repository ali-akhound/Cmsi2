using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models.Admin
{
    #region GalleryViewModel
    public class GalleryImageViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "GalleryImageViewModel_Name", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "GalleryImageViewModel_Name_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(200)]
        [ShowInGridview]
        public string Name { get; set; }
        [Display(Name = "GalleryImageViewModel_Explain", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(500)]
        public string Explain { get; set; }

        [Display(Name = "GalleryImageViewModel_LongImageUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(200)]
        public string LongImageUrl { get; set; }
        [Display(Name = "GalleryImageViewModel_ShortImageUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(200)]
        [ImageGridviewColumn]
        [ShowInGridview]
        [GridColumnWidth(200)]
        public string ShortImageUrl { get; set; }
        [ShowInGridview]
        [Display(Name = "GalleryImageViewModel_Priority", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "GalleryImageViewModel_Priority_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [GridColumnWidth(80)]
        public int Priority { get; set; }
        [Display(Name = "GalleryImageViewModel_ImageLink", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(500)]
        public string ImageLink { get; set; }
        [Display(Name = "GalleryImageViewModel_Height", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public int? Height { get; set; }
        [Display(Name = "GalleryImageViewModel_Width", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public int? Width { get; set; }

        public int GalleryAlbumID { get; set; }
        [Display(Name = "GalleryImageViewModel_GalleryAlbumName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        public string GalleryAlbumName { get; set; }

        [Display(Name = "GalleryImageViewModel_LongImageFile", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "GalleryImageViewModel_LongImageFile_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase LongImage { get; set; }

        [Display(Name = "GalleryImageViewModel_ShortImage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "GalleryImageViewModel_ShortImage_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase SmallImage { get; set; }

        [ShowInGridview]
        [Display(Name = "GalleryImageViewModel_Enable", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public bool? Enable { get; set; }
        [ShowInGridview]
        [Display(Name = "GalleryImageViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateConverted { get; set; }
    
        public ObjectState ObjectState { get; set; }

    }
    public class GalleryAlbumViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "GalleryAlbumViewModel_Name", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "GalleryAlbumViewModel_Name_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(200)]
        [ShowInGridview]
        public string Name { get; set; }
        [Display(Name = "GalleryAlbumViewModel_Description", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(500)]
        [ShowInGridview]
        public string Description { get; set; }
        [ShowInGridview]
        [Display(Name = "GalleryAlbumViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateConverted { get; set; }
        public ObjectState ObjectState { get; set; }
    }
    public class GalleryImageListViewModel
    {
        public List<GalleryImageViewModel> GalleryImages { get; set; }
        public List<GalleryAlbumViewModel> GalleryAlbums { get; set; }
        public GalleryImageViewModel SingleGalleryImage { get; set; } = new GalleryImageViewModel();
    }
    #endregion
}
