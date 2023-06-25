using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.Controller;
using AVA.UI.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using static AVA.Web.Mvc.Models.BaseViewModel;

namespace AVA.Web.Mvc.Models
{

    #region Index
    public class DefaultViewModel
    {
        public List<SliderViewModel> Slides { get; set; }
        public string ConferenceTitle { get; set; }
        public string ConferenceEnglishTitle { get; set; }
        public string ConferencePlace { get; set; }
        public DateTime? ConferenceEventDate { get; set; }
        public string ConferenceSendStartDate { get; set; }
        public string ConferenceSendEndDate { get; set; }
        public string ConferenceEventDatePersian { get; set; }
        public string PosterImageUrl { get; set; }
        public ElectionViewModel Election { get; set; } = new ElectionViewModel();
        public List<ConferenceCategoryViewModel> ConferenceCategories { get; set; }
        public ConferenceCategoryViewModel SingleCatDataModel { get; set; } = new ConferenceCategoryViewModel();
    }
    public class ElectionViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string StartDateConverted { get; set; }
        public string EndDateConverted { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PosterImageUrl { get; set; }
        public string AttachUrl { get; set; }
    }
    public class ConferenceCategoryViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Explain { get; set; }
        public string IconName { get; set; }
    }
    public class SliderViewModel
    {
        public int ID { get; set; }
        [StringLength(150)]
        public string Link { get; set; }

        [StringLength(150)]
        public string ThumbnailImage { get; set; }

        [StringLength(150)]
        public string Image { get; set; }
        public string Context { get; set; }

        [StringLength(10)]
        public string Width { get; set; }

        [StringLength(10)]
        public string Height { get; set; }
        //public int? Priority { get; set; }
        //public bool Enable { get; set; }

    }
    public class DoctorsAdvertiseViewModel
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Category { get; set; }
    }
    public class FooterViewModel
    {
        public NewsFeedViewModel newsFeedViewModel { get; set; }
        public string Text { get; set; }
        public string SocialMediaLinks { get; set; }
    }
    public class NewsFeedViewModel
    {
        public ObjectState ObjectState { get; set; }
        [Display(Name = "NewsFeedViewModel_NewsFeedEmail", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "NewsFeedViewModel_NewsFeedEmail_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [EmailAddress(ErrorMessageResourceName = "NewsFeedViewModel_NewsFeedEmail_EmailValidation", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string NewsFeedEmail { get; set; }
    }
    public class AboutUsViewModel
    {
        public string AboutUsText { get; set; }
        public string SupportText { get; set; }
    }
    public class MembershipViewModel
    {
        public LoginMembershipViewModel Login { get; set; }
        public PasswordRecoveryMembershipViewModel PasswordRecovery { get; set; }
        public AspUserMembershipViewModel User { get; set; }
    }
    public class LoginMembershipViewModel
    {
        [Display(Name = "LoginMembershipViewModel_Username", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "LoginMembershipViewModel_Username_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Username { get; set; }
        [Display(Name = "LoginMembershipViewModel_Password", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "LoginMembershipViewModel_Password_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Password { get; set; }
        [Display(Name = "LoginMembershipViewModel_RememberMe", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public bool RememberMe { get; set; }
    }
    public class PasswordChangeMembershipViewModel
    {
        public int UserID { get; set; }
        //[Display(Name = "PasswordChangeMembershipViewModel_OldPassword", ResourceType = typeof(AVA.Web.Resources.Resource))]
        //[Required(ErrorMessageResourceName = "PasswordChangeMembershipViewModel_OldPassword_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        //public string OldPassword { get; set; }
        [Display(Name = "PasswordChangeMembershipViewModel_Password", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "PasswordChangeMembershipViewModel_Password_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Password { get; set; }
        [Display(Name = "PasswordChangeMembershipViewModel_ConfirmPassword", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "PasswordChangeMembershipViewModel_ConfirmPassword_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "PasswordChangeMembershipViewModel_ConfirmPassword_Compare", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string UserId { get; set; }
        [Display(Name = "ResetPasswordViewModel_Email", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ResetPasswordViewModel_Email_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [EmailAddress(ErrorMessageResourceName = "ResetPasswordViewModel_Email_EmailValidation", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Email { get; set; }
        [Display(Name = "ResetPasswordViewModel_Password", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ResetPasswordViewModel_Password_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "ResetPasswordViewModel_ConfirmPassword", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "ResetPasswordViewModel_ConfirmPassword_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "ResetPasswordViewModel_ConfirmPassword_Compare", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class PasswordRecoveryMembershipViewModel
    {
        [Display(Name = "PasswordRecoveryMembershipViewModel_Email", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "PasswordRecoveryMembershipViewModel_Email_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [EmailAddress(ErrorMessageResourceName = "PasswordRecoveryMembershipViewModel_Email_EmailValidation", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Email { get; set; }
    }
    public class AspUserMembershipViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public string ID { get; set; }
        [ShowInGridview]
        [Display(Name = "AspUserMembershipViewModel_FirstName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "AspUserMembershipViewModel_FirstName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string FirstName { get; set; }
        [ShowInGridview]
        [Display(Name = "AspUserMembershipViewModel_LastName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "AspUserMembershipViewModel_LastName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "AspUserMembershipViewModel_EnglishFirstName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "AspUserMembershipViewModel_EnglishFirstName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [RegularExpression("(" + ControllerHelper.EnglishONlyRegEx + ")", ErrorMessageResourceName = "AspUserMembershipViewModel_EnglishFirstName_RegEx", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string EnglishFirstName { get; set; }
        [Display(Name = "AspUserMembershipViewModel_EnglishLastName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "AspUserMembershipViewModel_EnglishLastName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [RegularExpression("(" + ControllerHelper.EnglishONlyRegEx + ")", ErrorMessageResourceName = "AspUserMembershipViewModel_EnglishLastName_RegEx", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string EnglishLastName { get; set; }

        [ShowInGridview]
        [Display(Name = "AspUserMembershipViewModel_Email", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "AspUserMembershipViewModel_Email_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [EmailAddress(ErrorMessageResourceName = "AspUserMembershipViewModel_Email_EmailValidation", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string Email { get; set; }
        [ShowInGridview]
        [Display(Name = "AspUserMembershipViewModel_PhoneNumber", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "AspUserMembershipViewModel_Password", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "AspUserMembershipViewModel_Password_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Password { get; set; }

        [Display(Name = "AspUserMembershipViewModel_ConfirmPassword", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "AspUserMembershipViewModel_ConfirmPassword_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "AspUserMembershipViewModel_ConfirmPassword_Compare", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        //[Display(Name = "آدرس")]
        //[Required(ErrorMessage = "آدرس ضروری است")]
        //[StringLength(2000)]
        //public string Address { get; set; }        
        public bool? sex { get; set; }
        public string sexConverted { get; set; }
        [StringLength(200)]
        [ShowInGridview]
        [Display(Name = "AspUserMembershipViewModel_invite", ResourceType = typeof(AVA.Web.Resources.Resource))]

        public string invite { get; set; }
        [ShowInGridview]
        [Display(Name = "AspUserMembershipViewModel_Enable", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public bool? Enable { get; set; }
        [ShowInGridview]
        [Display(Name = "AspUserMembershipViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateConverted { get; set; }
        [Display(Name = "AspUserMembershipViewModel_PersonalImageUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(150)]
        public string PersonalImageUrl { get; set; }
        [StringLength(300)]
        [Display(Name = "AspUserMembershipViewModel_MeliCardUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string MeliCardUrl { get; set; }
        [StringLength(300)]
        [Display(Name = "AspUserMembershipViewModel_UniversityCardUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string UniversityCardUrl { get; set; }
        public ObjectState ObjectState { get; set; }

    }

    #endregion
    #region User Profile
    #region Profile
    public class DashboardViewModel
    {
        public int TotalArticle { get; set; }
        public string ArticleConfirmCount { get; set; }
        public string ArticleRejectCount { get; set; }
        public string UnreadMessageCount { get; set; }

    }
    public class ProfileViewModel
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string ImageUrl { get; set; }
        public string VipEndDate { get; set; }
        public int TotalArticles { get; set; }
        public int AcceptedArticles { get; set; }
        public int RejectedArticles { get; set; }
    }
    public class EditMembershipVm
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public string ID { get; set; }
        [ShowInGridview]
        [Display(Name = "EditMembershipVm_FirstName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "EditMembershipVm_FirstName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string FirstName { get; set; }
        [ShowInGridview]
        [Display(Name = "EditMembershipVm_LastName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "EditMembershipVm_LastName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string LastName { get; set; }
        [Display(Name = "EditMembershipVm_EnglishFirstName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "EditMembershipVm_EnglishFirstName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [RegularExpression("(" + ControllerHelper.EnglishONlyRegEx + ")", ErrorMessageResourceName = "EditMembershipVm_EnglishFirstName_RegEx", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string EnglishFirstName { get; set; }
        [Display(Name = "EditMembershipVm_EnglishLastName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "EditMembershipVm_EnglishLastName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [RegularExpression("(" + ControllerHelper.EnglishONlyRegEx + ")", ErrorMessageResourceName = "EditMembershipVm_EnglishLastName_RegEx", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string EnglishLastName { get; set; }






        [ShowInGridview]
        [Display(Name = "EditMembershipVm_Email", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "EditMembershipVm_Email_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [EmailAddress(ErrorMessageResourceName = "EditMembershipVm_Email_EmailValidation", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]

        [StringLength(50)]
        public string Email { get; set; }
        [ShowInGridview]
        [Display(Name = "EditMembershipVm_PhoneNumber", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "EditMembershipVm_PhoneNumber_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string PhoneNumber { get; set; }
        [Display(Name = "EditMembershipVm_sex", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "EditMembershipVm_sex_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public bool? sex { get; set; }
        [Display(Name = "EditMembershipVm_BornDate", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "EditMembershipVm_BornDate_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string BornDate { get; set; }
        [Display(Name = "EditMembershipVm_University", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "EditMembershipVm_University_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string University { get; set; }
        [Display(Name = "EditMembershipVm_FieldOfStudy", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "EditMembershipVm_FieldOfStudy_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string FieldOfStudy { get; set; }
        [Display(Name = "EditMembershipVm_Degree", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "EditMembershipVm_Degree_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Degree { get; set; }
        [Display(Name = "EditMembershipVm_Melicode", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "EditMembershipVm_Melicode_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Melicode { get; set; }
        public List<DropDownVm> CityFeeds { get; set; }
        public List<DropDownVm> ProvinceFeeds { get; set; }
        [Display(Name = "EditMembershipVm_selectedCityID", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string selectedCityID { get; set; }
        [Display(Name = "EditMembershipVm_selectedProvinceID", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string selectedProvinceID { get; set; }
        [Display(Name = "EditMembershipVm_Address", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Address { get; set; }
        [Display(Name = "EditMembershipVm_PersonalImageUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(300)]
        public string PersonalImageUrl { get; set; }
        [StringLength(300)]
        [Display(Name = "EditMembershipVm_MeliCardUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string MeliCardUrl { get; set; }

        [StringLength(300)]
        [Display(Name = "EditMembershipVm_UniversityCardUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string UniversityCardUrl { get; set; }
        [Display(Name = "EditMembershipVm_MeliCardFileImage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase MeliCardFileImage { get; set; }
        [Display(Name = "EditMembershipVm_PersonalFileImage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase PersonalFileImage { get; set; }
        [Display(Name = "EditMembershipVm_UniversityCardFileImage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase UniversityCardFileImage { get; set; }
        public ObjectState ObjectState { get; set; }

    }

    public class RegisterMembershipVm
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        public string ID { get; set; }
        [ShowInGridview]
        [Display(Name = "RegisterMembershipVm_FirstName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "RegisterMembershipVm_FirstName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string FirstName { get; set; }
        [ShowInGridview]
        [Display(Name = "RegisterMembershipVm_LastName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "RegisterMembershipVm_LastName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "RegisterMembershipVm_EnglishFirstName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "RegisterMembershipVm_EnglishFirstName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [RegularExpression("(" + ControllerHelper.EnglishONlyRegEx + ")", ErrorMessageResourceName = "RegisterMembershipVm_EnglishFirstName_RegEx", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string EnglishFirstName { get; set; }
        [Display(Name = "RegisterMembershipVm_EnglishLastName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "RegisterMembershipVm_EnglishLastName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [RegularExpression("(" + ControllerHelper.EnglishONlyRegEx + ")", ErrorMessageResourceName = "RegisterMembershipVm_EnglishLastName_RegEx", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string EnglishLastName { get; set; }


        [Display(Name = "RegisterMembershipVm_Email", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "RegisterMembershipVm_Email_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [EmailAddress(ErrorMessageResourceName = "RegisterMembershipVm_Email_EmailValidation", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [ShowInGridview]
        [StringLength(50)]
        public string Email { get; set; }
        [ShowInGridview]
        [Display(Name = "RegisterMembershipVm_PhoneNumber", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "RegisterMembershipVm_PhoneNumber_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [RegularExpression("(" + ControllerHelper.MobileRegEx + ")", ErrorMessageResourceName = "RegisterMembershipVm_PhoneNumber_RegEx", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string PhoneNumber { get; set; }
        [Display(Name = "RegisterMembershipVm_sex", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "RegisterMembershipVm_sex_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public bool? sex { get; set; }
        [Display(Name = "RegisterMembershipVm_BornDate", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "RegisterMembershipVm_BornDate_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string BornDate { get; set; }
        [Display(Name = "RegisterMembershipVm_University", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "RegisterMembershipVm_University_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string University { get; set; }
        [Display(Name = "RegisterMembershipVm_FieldOfStudy", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "RegisterMembershipVm_FieldOfStudy_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string FieldOfStudy { get; set; }
        [Display(Name = "RegisterMembershipVm_Degree", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "RegisterMembershipVm_Degree_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Degree { get; set; }
        [Display(Name = "RegisterMembershipVm_Melicode", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "RegisterMembershipVm_Melicode_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Melicode { get; set; }
        [Display(Name = "RegisterMembershipVm_Password", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "RegisterMembershipVm_Password_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "EditMembershipVm_selectedCityID", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string selectedCityID { get; set; }
        [Display(Name = "EditMembershipVm_selectedProvinceID", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string selectedProvinceID { get; set; }
        [Display(Name = "EditMembershipVm_Address", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Address { get; set; }
        [Display(Name = "EditMembershipVm_PersonalImageUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(300)]
        public string PersonalImageUrl { get; set; }
        [StringLength(300)]
        [Display(Name = "EditMembershipVm_MeliCardUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string MeliCardUrl { get; set; }

        [StringLength(300)]
        [Display(Name = "EditMembershipVm_UniversityCardUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string UniversityCardUrl { get; set; }
        [Display(Name = "EditMembershipVm_MeliCardFileImage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase MeliCardFileImage { get; set; }
        [Display(Name = "EditMembershipVm_PersonalFileImage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase PersonalFileImage { get; set; }
        [Display(Name = "EditMembershipVm_UniversityCardFileImage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase UniversityCardFileImage { get; set; }
        public List<DropDownVm> CityFeeds { get; set; }
        public List<DropDownVm> ProvinceFeeds { get; set; }
        public ObjectState ObjectState { get; set; }

    }
    #endregion
    #region Companion
    public class CompanionViewModel
    {
        [Key]
        [ShowInGridview]
        [Display(Name = "ID")]
        [GridColumnWidth(50)]
        public int ID { get; set; }

        [ShowInGridview]
        [Display(Name = "CompanionViewModel_FirstName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "CompanionViewModel_FirstName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string FirstName { get; set; }
        [ShowInGridview]
        [Display(Name = "CompanionViewModel_LastName", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "CompanionViewModel_LastName_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string LastName { get; set; }
        [ShowInGridview]
        [Display(Name = "CompanionViewModel_Email", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "CompanionViewModel_Email_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [EmailAddress(ErrorMessageResourceName = "CompanionViewModel_Email_EmailValidation", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(50)]
        public string Email { get; set; }
        [ShowInGridview]
        [Display(Name = "CompanionViewModel_PhoneNumber", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "CompanionViewModel_PhoneNumber_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        [StringLength(11)]
        [RegularExpression("(" + ControllerHelper.MobileRegEx + ")", ErrorMessageResourceName = "CompanionViewModel_PhoneNumber_RegEx", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Cellphone { get; set; }
        [Display(Name = "CompanionViewModel_BornDate", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "CompanionViewModel_BornDate_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string BornDate { get; set; }
        [Display(Name = "CompanionViewModel_Melicode", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "CompanionViewModel_Melicode_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Melicode { get; set; }
        [Display(Name = "CompanionViewModel_University", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "CompanionViewModel_University_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string University { get; set; }

        [Display(Name = "CompanionViewModel_FieldOfStudy", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "CompanionViewModel_FieldOfStudy_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string FieldOfStudy { get; set; }

        [Display(Name = "CompanionViewModel_Degree", ResourceType = typeof(AVA.Web.Resources.Resource))]
        [Required(ErrorMessageResourceName = "CompanionViewModel_Degree_ReqMsg", ErrorMessageResourceType = typeof(AVA.Web.Resources.Resource))]
        public string Degree { get; set; }
        [Display(Name = "CompanionViewModel_UniversityCardFileImageUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string UniversityCardFileImageUrl { get; set; }
        [Display(Name = "CompanionViewModel_PayReceiptFileImageUrl", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string PayReceiptFileImageUrl { get; set; }

        [Display(Name = "CompanionViewModel_UniversityCardFileImage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase UniversityCardFileImage { get; set; }
        [Display(Name = "CompanionViewModel_PayReceiptFileImage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase PayReceiptFileImage { get; set; }

        [ShowInGridview]
        [Display(Name = "CompanionViewModel_SelectedPackage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string SelectedPackage { get; set; }

        [ShowInGridview]
        [Display(Name = "CompanionViewModel_CreateDateConverted", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public string CreateDateConverted { get; set; }
        [ShowInGridview]
        [Display(Name = "پیوست")]
        [HyperLinkGridviewColumn]
        [HyperLinkGridviewColumnText("AttachText")]
        public string Attach { get; set; }
        public string AttachText { get; set; }
        public bool AllowEdit { get; set; }
        public int ConferenceID { get; set; }
        public ConferencePackageViewModel RegisterUserPackage { get; set; }
        public ConferencePackageViewModel UniversityUserPackage { get; set; }
        public ConferencePackageViewModel RegularUserPackage { get; set; }
        public ConferencePackageViewModel SecondArticlePackage { get; set; }
        //public int RegisterUserPackagePrice { get; set; }
        //public int UniversityUserPackageID { get; set; }
        //public int UniversityUserPackagePrice { get; set; }
        //public int RegularUserPackageID { get; set; }
        //public int RegularUserPackagePrice { get; set; }
        //public int SecondArticlePackageID { get; set; }
        //public int SecondArticlePackagePrice { get; set; }
        public ConferencePackageViewModel SelectedPackageObject { get; set; } = new ConferencePackageViewModel();
        public ObjectState ObjectState { get; set; }

    }
    public class ConferencePackageViewModel
    {
        public string ID { get; set; }
        [StringLength(200)]
        [Required]
        public string Name { get; set; } = "-";
        public int Count { get; set; } = 1;
        public decimal Price { get; set; }

    }
    public class CompanionListViewModel
    {
        public List<CompanionViewModel> DataModel { get; set; }
        public CompanionViewModel SingleDataModel { get; set; } = new CompanionViewModel() { };
        public int pageSize { get; set; }
        public int totalRecords { get; set; }
        public int pageNumber { get; set; }

    }
    #endregion

    #region PayConference
    public class PayConferenceViewModel
    {
        public List<ConferencePackageViewModel> ConferencePackages { get; set; }
        public ConferencePackageViewModel ConferencePackage { get; set; } = new ConferencePackageViewModel();
        public ConferencePackageViewModel CompanionPackage { get; set; } = new ConferencePackageViewModel();
        public bool Enable { get; set; }

    }

    #endregion
    #region PayVip
    public class PayVipViewModel
    {
        public int ID { get; set; } = 0;
        public List<VipPackageNameViewModel> OtherPackages { get; set; }
        public List<VipPackageNameViewModel> RegisterPackages { get; set; }
        public VipPackageNameViewModel PayVipPackage { get; set; } = new VipPackageNameViewModel();
        [Display(Name = "PayVipViewModel_UniversityCardFileImage", ResourceType = typeof(AVA.Web.Resources.Resource))]
        public HttpPostedFileBase UniversityCardFileImage { get; set; }
        public int SelectedRegisterPackageIndex { get; set; } = 0;
        public int SelectedRegisterPackageCount { get; set; } = 1;
        public EditMembershipVm MembershipVm { get; set; }
        public bool AllowEdit { get; set; }
    }
    public class VipPackageNameViewModel
    {
        public int ID { get; set; }
        [StringLength(200)]
        [Required]
        public string Name { get; set; }
        public int Count { get; set; } = 1;
        public int Price { get; set; }

    }
    #endregion
    #endregion
}
