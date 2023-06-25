using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace AVA.Web.Mvc.Models.Admin
{

    #region Role
    public class RoleViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public string ID { get; set; }
        [Display(Name = "نام نقش")]
        [ShowInGridview]
        [Required(ErrorMessage = "نام نقش ضروری است")]
        public string Name { get; set; }
        public ObjectState ObjectState { get; set; }
        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }

        //public string Code { get; set; }
    }

    #endregion
    #region UserRole
    public class RoleUserViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class Select2InputViewModel
    {
        public string Name { get; set; }
        public ICollection<RoleUserViewModel> Roles { get; set; }

    }
    public class UserRoleViewModel
    {
        [ShowInGridview]
        [Display(Name = "ID")]
        public string ID { get; set; }
        [ShowInGridview]
        [Display(Name = "UserID")]
        public string UserID { get; set; }
        [Display(Name = "نقش ها")]
        public ICollection<RoleUserViewModel> Roles { get; set; }
        public ICollection<RoleUserViewModel> RolesFeed { get; set; }
        public ObjectState ObjectState { get; set; }
    }
    #endregion
    #region SysRoleModule
    public class SysRoleModuleViewModel
    {
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Display(Name = "نقش")]
        public string RoleName { get; set; }
        public string RoleID { get; set; }
        [Display(Name = "بخش ها")]
        public ICollection<SysRoleModule> Modules { get; set; }
        public ObjectState ObjectState { get; set; }
    }
    public class SysRoleModule
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }

    #endregion
    #region ContactUs
    public class ContactUsViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public long ID { get; set; }
        [Display(Name = "ContactUsViewModel_Name", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ContactUsViewModel_Name_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        [StringLength(500)]
        public string Name { get; set; }
        [Display(Name = "ContactUsViewModel_Family", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ContactUsViewModel_Family_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        [StringLength(500)]
        public string Family { get; set; }
        [Display(Name = "ContactUsViewModel_TelNumber", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ContactUsViewModel_TelNumber_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        [StringLength(50)]
        public string TelNumber { get; set; }

        [Display(Name = "ContactUsViewModel_Email", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ContactUsViewModel_Email_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [EmailAddress(ErrorMessageResourceName = "ContactUsViewModel_Email_NotValidMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        [StringLength(50)]
        public string Email { get; set; }
        [ShowInGridview]
        [Display(Name = "ContactUsViewModel_Subject", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ContactUsViewModel_Subject_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Subject { get; set; }
        [Display(Name = "ContactUsViewModel_Text", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ContactUsViewModel_Text_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        public string Text { get; set; }
        public DateTime? ContactDate { get; set; }
        [Display(Name = "ContactUsViewModel_ContactDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        public string ContactDateConverted { get; set; }
        public ObjectState ObjectState { get; set; }
    }
    #endregion
    #region DynamicPage
    public class DynamicPageViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [ShowInGridview]
        [Display(Name = "عنوان")]
        [StringLength(100)]
        [Required(ErrorMessage = "عنوان ضروری است")]
        public string Title { get; set; }
        [Display(Name = "کلمات کلیدی")]
        [StringLength(300)]
        public string Keyword { get; set; }
        [Display(Name = "توضیحات")]
        [StringLength(300)]
        public string Description { get; set; }
        [ShowInGridview]
        [Display(Name = "فعال")]
        public bool? Enable { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ")]
        public string DynamicPageDateConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "اولویت")]
        public int? Priority { get; set; }
        public string DefaultContext { get; set; }
        [AllowHtml]
        [Display(Name = "محتوا")]
        public string Context { get; set; }
        [AllowHtml]
        [Display(Name = "محتوا انگلیسی")]
        public string EnglishContext { get; set; }
        public ObjectState ObjectState { get; set; }
    }
    #endregion
    #region User
    public class AspUserViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public string ID { get; set; }
        [ShowInGridview]
        [Display(Name = "نام")]
        [Required(ErrorMessage = "نام ضروری است")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [ShowInGridview]
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "نام خانوادگی ضروری است")]
        [StringLength(50)]
        public string LastName { get; set; }
        [ShowInGridview]
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "نام کاربری ضروری است")]
        [StringLength(50)]
        public string UserName { get; set; }
        [ShowInGridview]
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "ایمیل ضروری است")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست")]
        [StringLength(50)]
        public string Email { get; set; }
        [ShowInGridview]
        [Display(Name = "همراه")]
        public string PhoneNumber { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "رمز عبور ضروری است")]
        public string Password { get; set; }


        //[Display(Name = "آدرس")]
        //[Required(ErrorMessage = "آدرس ضروری است")]
        //[StringLength(2000)]
        //public string Address { get; set; }        
        public bool? sex { get; set; }
        public string sexConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "نقش های کاربر")]
        public string UserRoles { get; set; }
        [StringLength(200)]
        [ShowInGridview]
        [Display(Name = "نحوه آشنایی")]
        public string invite { get; set; }
        [ShowInGridview]
        [Display(Name = "فعال")]
        public bool? Enable { get; set; }
        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }
        [StringLength(150)]
        public string ImageUrl { get; set; }
        public ObjectState ObjectState { get; set; }

    }

    #endregion
    #region MailTemplate
    public partial class MailTemplateVmItem
    {
        [Key]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Display(Name = "پارامترها")]
        public JsonResult Params { get; set; }
        [Display(Name = "قالب")]
        public string Template { get; set; }
        [Display(Name = "عنوان")]
        [StringLength(500)]
        public string Subject { get; set; }
        [Display(Name = "متن پیام کوتاه")]
        public string SMS { get; set; }
        public string Description { get; set; }
        public ObjectState ObjectState { get; set; }
    }
    public partial class MailTemplateViewModel
    {
        public List<MailTemplateVmItem> TotalMailTemplate { get; set; }
        public MailTemplateVmItem SingleMailTemplate { get; set; } = new MailTemplateVmItem();
        [Display(Name = "نوع قالب")]
        public SelectListItem[] DprItems { get; set; }
        public int CurrentID { get; set; }
        public string SelectedID { get; set; }

    }
    #endregion
    #region Membership
    public class MembershipViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "نام کاربری ضروری است")]
        public string Username { get; set; }
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "رمز عبور ضروری است")]
        public string Password { get; set; }
        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
    public class PasswordChangeMembershipViewModel
    {
        public int UserID { get; set; }
        [Display(Name = "رمز عبور قدیم")]
        [Required(ErrorMessage = "رمز عبور قدیم ضروری است")]
        public string OldPassword { get; set; }
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "رمز عبور ضروری است")]
        public string Password { get; set; }
        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "تکرار رمز عبور ضروری است")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "رمز عبور با تکرار آن مطابقت ندارد")]
        public string ConfirmPassword { get; set; }
    }
    public class PasswordRecoveryMembershipViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "ایمیل ضروری است")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
        public string Email { get; set; }
    }
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "ایمیل ضروری است")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
        public string Email { get; set; }
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "رمز عبور ضروری است")]
        public string Password { get; set; }
        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "تکرار رمز عبور ضروری است")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "رمز عبور با تکرار آن مطابقت ندارد")]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
    #endregion
    #region Layout
    public class LayoutViewModel
    {
        [Key]
        [Display(Name = "ID")]
        public string ID { get; set; }
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        public ICollection<RoleViewModel> Roles { get; set; }
    }
    #endregion
    #region Categories
    public class CategoryViewModel
    {
        [Key]
        [Display(Name = "ID")]
        [ShowInGridview]
        public int ID { get; set; }
        [Display(Name = "نام")]
        [StringLength(500)]
        [ShowInGridview]
        [Required(ErrorMessage = "نام ضروری است")]
        public string Name { get; set; }
        [Display(Name = "اولویت")]
        public int? priority { get; set; }
        [Display(Name = "فعال")]
        [ShowInGridview]
        public bool? Enable { get; set; }
        [Display(Name = "نام گره پدر")]
        [StringLength(500)]
        [ShowInGridview]
        public string ParentName { get; set; }
        [Display(Name = "نام گره پدر")]
        public int? ParentID { get; set; }
        [Display(Name = "تاریخ ثبت ")]
        [ShowInGridview]
        public string CreateDateConverted { get; set; }
        public ObjectState ObjectState { get; set; }
        public string CreateDate { get; set; }
        public string TableName { get; set; }
    }
    public class CategoryGridParams
    {
        public string TableName { get; set; }
    }
    #endregion
    #region CityZone
    public partial class CityZonesViewModel
    {
        [Key]
        [Display(Name = "ID")]
        [ShowInGridview]
        public int ID { get; set; }
        [Display(Name = "نام")]
        [StringLength(500)]
        [ShowInGridview]
        [Required(ErrorMessage = "نام ضروری است")]
        public string Name { get; set; }
        [Display(Name = "تاریخ ثبت")]
        [ShowInGridview]
        public string CreateDateConverted { get; set; }
        public ObjectState ObjectState { get; set; }
        public string CreateDate { get; set; }
    }
    #endregion
    #region Feature
    public enum DataType
    {
        [Description("دارد/ندارد")]
        Bool = 1,
        [Description("متن")]
        Text = 2
    }
    public partial class FeaturesViewModel
    {
        [Key]
        [Display(Name = "ID")]
        [ShowInGridview]
        public int ID { get; set; }
        [Display(Name = "نام")]
        [StringLength(500)]
        [ShowInGridview]
        [Required(ErrorMessage = "نام ضروری است")]
        public string Name { get; set; }
        [ShowInGridview]
        [Display(Name = "رسته")]
        public string CatNames { get; set; }
        [StringLength(50)]
        [Display(Name = "نوع")]
        public string SelectedDataType { get; set; }

        [Display(Name = "تاریخ ثبت")]
        [ShowInGridview]
        public string CreateDateConverted { get; set; }

        public string DataType { get; set; }
        [Display(Name = "رسته")]
        public ICollection<CategoryFeedVm> Categories { get; set; }
        public ICollection<CategoryFeedVm> CategoriesFeed { get; set; }
        public ObjectState ObjectState { get; set; }

    }
    public class CategoryFeedVm
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class Select2CategoriesViewModel
    {
        public string Name { get; set; }
        public ICollection<CategoryFeedVm> Categories { get; set; }

    }
    #endregion


}
