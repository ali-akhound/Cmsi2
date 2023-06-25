using System.Web;
using System.Web.Optimization;

namespace AVA.Web.Mvc
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            #region Admin Css& Scripts
            #region Css
            bundles.Add(new StyleBundle("~/AdminBundles/core-css")
                .Include("~/Areas/Admin/assets/css/bootstrap.min.css")
                //   < !--Main Inspinia CSS files-- >
                .Include("~/Areas/Admin/assets/css/animate.css")
                .Include("~/Areas/Admin/assets/css/plugins/angular-block-ui/angular-block-ui.min.css")
                .Include("~/Areas/Admin/assets/css/plugins/toastr/toastr.min.css")
                .Include("~/Areas/Admin/assets/css/plugins/sweetalert/sweetalert.css")
                .Include("~/Areas/Admin/assets/css/plugins/iCheck/custom.css", new CssRewriteUrlTransform())
            );
            #endregion
            #region Js
            bundles.Add(new ScriptBundle("~/AdminBundles/core-js")
               //< !--jQuery and Bootstrap-- >
               .Include("~/Areas/Admin/assets/js/jquery/jquery-3.1.1.min.js")
               .Include("~/Areas/Admin/assets/js/plugins/jquery-ui/jquery-ui.min.js")
               //< !--jQuery and validation-- >
               .Include("~/Areas/Admin/assets/js/jquery.validate.min.js")
               .Include("~/Areas/Admin/assets/js/jquery.validate.unobtrusive.js")

               .Include("~/Areas/Admin/assets/js/bootstrap/bootstrap.min.js")
               //< !--MetsiMenu-- >
               .Include("~/Areas/Admin/assets/js/plugins/metisMenu/jquery.metisMenu.js")
               //< !--SlimScroll-- >
               .Include("~/Areas/Admin/assets/js/plugins/slimscroll/jquery.slimscroll.min.js")
               //< !--Peace JS-- >
               .Include("~/Areas/Admin/assets/js/plugins/pace/pace.min.js")
               //< !--Custom and plugin javascript-- >
               .Include("~/Areas/Admin/assets/js/inspinia.js")
               //< !--Main Angular scripts-- >
               .Include("~/Areas/Admin/assets/js/angular/angular.min.js")
               .Include("~/Areas/Admin/assets/js/angular/angular-sanitize.js")
               .Include("~/Areas/Admin/assets/js/plugins/oclazyload/dist/ocLazyLoad.min.js")
               .Include("~/Areas/Admin/assets/js/angular-translate/angular-translate.min.js")
               .Include("~/Areas/Admin/assets/js/ui-router/angular-ui-router.min.js")
               .Include("~/Areas/Admin/assets/js/bootstrap/ui-bootstrap-tpls-1.1.2.min.js")
               .Include("~/Areas/Admin/assets/js/plugins/angular-idle/angular-idle.js")
               .Include("~/Areas/Admin/assets/js/plugins/angular-block-ui/angular-block-ui.min.js")
               .Include("~/Areas/Admin/assets/js/plugins/toastr/toastr.min.js")
               .Include("~/Areas/Admin/assets/js/plugins/sweetalert/sweetalert.min.js")
               .Include("~/Areas/Admin/assets/js/plugins/sweetalert/angular-sweetalert.min.js")
               .Include("~/Areas/Admin/assets/js/plugins/iCheck/icheck.min.js")
               .Include("~/Areas/Admin/assets/js/plugins/jasny/jasny-bootstrap.min.js")
               /*< !--
              You need to include this script on any page that has a Google Map.
              When using Google Maps on your own site you MUST signup for your own API key at:

              https://developers.google.com/maps/documentation/javascript/tutorial#api_key

              After your sign up replace the key in the URL below..
             -- >*///.Include("https://maps.googleapis.com/maps/api/js?key=AIzaSyDQTpXj82d8UpCi97wzo_nKXL7nYrd4G70")
                   //  < !--Anglar App Script-- >
               .Include("~/Areas/Admin/assets/js/app.js")
             //.Include("~/Areas/Admin/assets/js/translations.js")
             //.Include("~/Areas/Admin/assets/js/config.js")
             //.Include("~/Areas/Admin/assets/js/directives.js")
             //.Include("~/Areas/Admin/assets/js/controllers.js")
             );
            #endregion

            #endregion
            #region Csmi Css& Scripts
            #region Css
            bundles.Add(new StyleBundle("~/CsmiBundles/core-css")
            .Include("~/Areas/Admin/assets/css/plugins/angular-block-ui/angular-block-ui.min.css")
            .Include("~/Areas/Admin/assets/css/plugins/sweetalert/sweetalert.css")
            .Include("~/Areas/Admin/assets/css/plugins/toastr/toastr.min.css")
            //< !--Bootstrap v4.0.0 - beta CSS via CDN-- >
            .Include("~/assets/css/bootstrap.min.css")
            //< !--Theme style-- >
            .Include("~/assets/css/theme-style.min.css", new CssRewriteUrlTransform())
            .Include("~/assets/css/colour-green-bright.min.css")
            //.Include("~/assets/css/animate.min.css")
            //< link href = "~/assets/plugins/_overrides/plugin-owl-carousel.min.css" rel = "stylesheet" />
            //< link href = "~/assets/plugins/carousel/owl.carousel.min.css" rel = "stylesheet" />
            //< link href = "~/assets/plugins/carousel/animate.min.css" rel = "stylesheet" />
            );
            #endregion
            #region Js
            //bundles.Add(new ScriptBundle("~/CsmiBundles/bootstrap-js")
            // // .Include("~/assets/js/bootstrap.min.js")
            //  );
            bundles.Add(new ScriptBundle("~/CsmiBundles/core-js")
              .Include("~/assets/js/jquery.min.js")
              .Include("~/assets/js/popper.min.js")
              .Include("~/assets/js/retina.min.js")
              .Include("~/assets/plugins/jPanelMenu/jquery.jpanelmenu.min.js")
              .Include("~/assets/plugins/fixto/fixto.js")
              .Include("~/assets/plugins/carousel/owl.carousel.min.js")
              .Include("~/assets/js/jquery.mask.min.js")
           );
            bundles.Add(new ScriptBundle("~/CsmiBundles/app-js")
              .Include("~/assets/js/script.min.js")
              .Include("~/assets/js/custom-script.js")
              );
            bundles.Add(new ScriptBundle("~/CsmiBundles/core-Myangular")
               //< !--jQuery and validation-- >
               .Include("~/Areas/Admin/assets/js/jquery.validate.min.js")
               .Include("~/Areas/Admin/assets/js/jquery.validate.unobtrusive.js")
               //< !--jQuery and Bootstrap-- >")
               //.Include("~/Areas/Admin/assets/js/plugins/jquery-ui/jquery-ui.min.js")
               //.Include("~/Areas/Admin/assets/js/plugins/slimscroll/jquery.slimscroll.min.js")
               //< !--Peace JS-- >
               .Include("~/Areas/Admin/assets/js/plugins/pace/pace.min.js")
               //  //< !--Main Angular scripts-- >
               .Include("~/Areas/Admin/assets/js/angular/angular.min.js")
               .Include("~/Areas/Admin/assets/js/angular/angular-sanitize.js")
               .Include("~/Areas/Admin/assets/js/plugins/oclazyload/dist/ocLazyLoad.min.js")
               .Include("~/Areas/Admin/assets/js/angular-translate/angular-translate.min.js")
               .Include("~/Areas/Admin/assets/js/ui-router/angular-ui-router.min.js")
               //.Include("~/Areas/Admin/assets/js/bootstrap/ui-bootstrap-tpls-1.1.2.min.js")
               .Include("~/Areas/Admin/assets/js/plugins/angular-idle/angular-idle.js")
               .Include("~/Areas/Admin/assets/js/plugins/angular-block-ui/angular-block-ui.min.js")
               .Include("~/Areas/Admin/assets/js/plugins/sweetalert/sweetalert.min.js")
               .Include("~/Areas/Admin/assets/js/plugins/sweetalert/angular-sweetalert.min.js")
               .Include("~/Areas/Admin/assets/js/plugins/toastr/toastr.min.js")
               .Include("~/assets/js/Csmi/update-meta.min.js")
                //.Include("~/assets/js/Csmi/translations.js")
                //.Include("~/assets/js/Csmi/config.js")
                //.Include("~/assets/js/Csmi/directives.js")
                //.Include("~/assets/js/Csmi/controllers.js")
                //.Include("~/assets/js/Csmi/dirPagination.js")
                );
            #endregion
            #endregion
        }
    }
}
