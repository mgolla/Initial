using System.Web;
using System.Web.Optimization;
using QR.IPrism.Web.Helper;

namespace QR.IPrism.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                       "~/Scripts/ie10-viewport-bug-workaround.js"));

            bundles.Add(new StyleBundle("~/bundles/css").Include("~/Content/bootstrap.css")
                      .Include("~/Content/bootstrap-theme.css")
                      .Include("~/Content/angular-toastr.css")
                      .Include("~/Content/jquery.rondell.css")
                      .Include("~/Content/css/styles/fonts/font-awesome.min.css", new CssRewriteUrlTransform())
                      .Include("~/Content/select.css")
                      .Include("~/Content/select2.css")
                      .Include("~/Content/ngDialog-theme-default.css")
                      .Include("~/Content/ngDialog.css")
                      .Include("~/Content/loading-bar.css")
                      .Include("~/Content/selectize.css")
                      .Include("~/Content/angular-block-ui.css")
                      .Include("~/Content/angular-carousel.css")
                      .Include("~/Content/videoplayer.css")
                // .Include("~/Content/angular-filemanager.min.css")
                      .Include("~/Content/ui-grid.css")
                      .Include("~/Content/ng-scrollbar.css")

                // Custom CSS
                      .Include("~/Content/css/styles/custom.css", new CssRewriteUrlTransform())
                      .Include("~/Content/css/styles/ie10-viewport-bug-workaround.css")
                      .Include("~/Content/css/styles/media-quiries.css")
                      .Include("~/Content/css/styles/rosterTable.css")
                      .Include("~/Content/css/styles/animate.css")
                      .Include("~/Content/css/styles/dashboard.css")
                      .Include("~/Content/css/styles/profileTabs.css")
                     .Include("~/Content/viewer.css")
                      );


            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                    "~/Scripts/pdfjs/build/pdf.js",
                    "~/Scripts/pdfjs/build/pdf.worker.js",
                    "~/Scripts/angular.js",
                    "~/Scripts/angular-ui/ui-bootstrap.js",
                     "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                    "~/Scripts/angular-animate.js",
                    "~/Scripts/angular-block-ui.js",
                   "~/Scripts/angular-route.js",
                   "~/Scripts/angular-scroll-animate.js",
                   "~/Scripts/angular-sanitize.js",
                    "~/Scripts/angular-ui-router.js",
                     "~/Scripts/ct-ui-router-extras.js",
                     "~/Scripts/ui-grid.js",
                     "~/Scripts/angular-toastr.js",
                     "~/Scripts/angular-toastr.tpls.js",
                     "~/Scripts/ngDialog.js",
                     "~/Scripts/ng-device-detector.js",
                     "~/Scripts/re-tree.js",
                     "~/Scripts/angular-cache.js",
                     "~/Scripts/loading-bar.js",
                     "~/Scripts/angular-local-storage.js",
                      "~/Scripts/custom.js",
                      "~/Scripts/ng-map.min.js",
                      "~/Scripts/angular-carousel.js",
                      "~/Scripts/videogular.js",
                    "~/Scripts/vg-controls.js",
                    "~/Scripts/vg-overlay-play.js",
                    "~/Scripts/vg-poster.js",
                    "~/Scripts/vg-buffering.js",
                  "~/Scripts/angular-cookies.min.js",
                  "~/Scripts/angular-translate.min.js",
                  "~/Scripts/angular-filemanager.min.js",
                  "~/Scripts/moment.js",
                  "~/Scripts/bootstrap-datetimepicker.min.js",
                  "~/Scripts/bootstrap-datetimepicker-directive.js",
                  "~/Scripts/angular-touch.js",
                  "~/Scripts/angular-file-upload.min.js",
                  "~/Scripts/select.js",
                  "~/Scripts/custom.js",
                  "~/Scripts/angular-idle.min.js",
                   "~/Scripts/typedarray.js",
                  "~/Scripts/Blob.js",
                  "~/Scripts/ng-scrollbar.js",
                 //"~/Scripts/angular-pdf.js",
                 "~/Scripts/jquery.media.js"
                 // "~/Scripts/angular-pdfjs-viewer.js"
                
                // ,
                //"~/Scripts/scrollbars.js"


                     ));


            bundles.Add(new ScriptBundle("~/bundles/appdependency").Include(
                  "~/app/app.js",
                  "~/app/shared/services/srd-auth-interceptor-service.js",
                  "~/app/shared/services/srd-loading-bar-service.js",
                  "~/app/shared/services/srd-spinner-service.js",
                  "~/app/shared/services/srd-web-api-service.js",
                  "~/app/shared/services/srd-lookup-service.js",
                  "~/app/shared/services/srd-auth-service.js",
                  "~/app/shared/services/srd-analytics-service.js",
                  "~/app/shared/services/srd-session-storage.js",
                  "~/app/shared/services/srd-shared-service.js",
                  "~/app/shared/services/srd-tok-manager-service.js",
                  "~/app/shared/services/srd-bundle-message-service.js",
                   "~/app/shared/services/srd-notificationalert-services.js",
                  "~/app/shared/directives/srd-directive-footer.js",
                  "~/app/shared/directives/srd-directive-header.js",
                  "~/app/shared/directives/ModalConfirmation/srd-modal-confirmation.js",
                  "~/app/shared/directives/srd-config.js",
                  "~/app/shared/directives/error-message.js",
                  "~/app/shared/directives/CheckAllcheckboxList/srd-checkAllCheckBoxList.js",
                  "~/app/shared/directives/Datepicker/srd-Datepicker.js",
                  "~/app/shared/directives/Timepicker/srd-Timepicker.js",
                  "~/app/shared/directives/DatepickerRange/srd-Datepicker-range.js",
                  "~/app/shared/directives/Dropdown/srd-Dropdown.js",
                  "~/app/shared/directives/Grid/srd-grid.js",
                  "~/app/shared/directives/Input/srd-input.js",
                  "~/app/shared/directives/MultiselectCheckbox/srd-Multiselect-Checkbox.js",
                  "~/app/shared/directives/MultiselectMapping/srd-Multiselect-Mapping.js",
                  "~/app/shared/directives/Fileupload/srd-fileupload.js",
                  "~/app/shared/directives/Select/srd-select.js",
                  "~/app/shared/directives/Textarea/srd-TextArea.js",
                  "~/app/shared/filters/srd-track-event-filter.js",
                  "~/app/shared/filters/srd-split-filter.js",
                  "~/app/shared/directives/Images/srd-image.js",
                  "~/app/shared/controllers/srd-menu-controller.js"
                  

                  ));

            bundles.Add(new ScriptBundle("~/bundles/ioscodova").Include(
               // "~/Scripts/ng-cordova.js",
          "~/Scripts/ios/cordova.js",
           "~/Scripts/ios/plugins/phonegap-plugin-push/www/push.js",
            "~/Scripts/ios/plugins/cordova-plugin-device/www/device.js",
             "~/Scripts/ios/plugins/cordova-plugin-app-version/www/AppVersionPlugin.js",
              
                "~/Scripts/ios/plugins/cordova-plugin-inappbrowser/www/inappbrowser.js"
          // "~/Scripts/ios/inappbrowser.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/androidcodova").Include(
         // "~/Scripts/ng-cordova.js",
         "~/Scripts/android/cordova.js",
            "~/Scripts/android/plugins/phonegap-plugin-push/www/push.js",
            "~/Scripts/android/plugins/cordova-plugin-device/www/device.js",
             "~/Scripts/android/plugins/cordova-plugin-app-version/www/AppVersionPlugin.js",
              

             //"~/Scripts/ios/inappbrowser.js"
                //"~/Scripts/android/cordova_plugins.js",
                "~/Scripts/android/plugins/cordova-plugin-inappbrowser/www/inappbrowser.js"
          ));



            bundles.Add(new ScriptBundle("~/bundles/ipmdependency").Include(
         "~/app/ipm/app.js",
                //Directives 
         "~/app/ipm/directives/ipm-directive-roster.js",
                //Services
          "~/app/shared/controllers/srd-documentviewer-controller.js",
         "~/app/ipm/services/ipm-request-services.js",
         "~/app/ipm/services/ipm-roster-services.js",
         "~/app/ipm/services/ipm-crewinfo-services.js",
         "~/app/ipm/services/ipm-hotelinfo-services.js",
         "~/app/ipm/services/ipm-overview-services.js",
         "~/app/ipm/services/ipm-stationinfo-services.js",
         "~/app/ipm/services/ipm-summaryservice-services.js",
         "~/app/ipm/services/ipm-housing-services.js",
         "~/app/ipm/services/ipm-assessmentlist-services.js",
         "~/app/ipm/services/ipm-assessment-services.js",
         "~/app/ipm/services/ipm-search-services.js",
         "~/app/ipm/services/ipm-departmentnews-services.js",
         "~/app/ipm/services/ipm-document-services.js",
         "~/app/ipm/services/ipm-file-services.js",
         "~/app/ipm/services/ipm-ifeguide-services.js",
         "~/app/ipm/services/ipm-link-services.js",
         "~/app/ipm/services/ipm-myrequest-services.js",
         "~/app/ipm/services/ipm-assessmentsearch-services.js",

         "~/app/ipm/services/ipm-svpmessage-services.js",
         "~/app/ipm/services/ipm-visionmission-services.js",
         "~/app/ipm/services/ipm-crewprofile-service.js",
         "~/app/ipm/services/ipm-evrsearch-service.js",
         "~/app/ipm/services/ipm-flightdelay-services.js",
         "~/app/ipm/services/ipm-flightdetailsaddedit-services.js",
         "~/app/ipm/services/ipm-kafou-service.js",

         //Models
         "~/app/ipm/models/ipm-housing-model.js",
          "~/app/ipm/models/ipm-assessment-model.js",

         //Controllers
         "~/app/ipm/controllers/ipm-home-controller.js",
         "~/app/ipm/controllers/ipm-request-controller.js",
         "~/app/ipm/controllers/ipm-roster-controller.js",
         "~/app/ipm/controllers/ipm-monthlyRoster-controller.js",
         "~/app/ipm/controllers/ipm-weeklyRoster-controller.js",
         "~/app/ipm/controllers/ipm-rosterDetailTabs-controller.js",
         "~/app/ipm/controllers/ipm-overviewTab-controller.js",
         "~/app/ipm/controllers/ipm-stationTab-controller.js",
         "~/app/ipm/controllers/ipm-crewinfo-controller.js",
         "~/app/ipm/controllers/ipm-hotelinfo-controller.js",
         "~/app/ipm/controllers/ipm-summaryservice-controller.js",
         "~/app/ipm/controllers/ipm-alertTabs-controller.js",
         "~/app/ipm/controllers/ipm-departmentnews-controller.js",
         "~/app/ipm/controllers/ipm-document-controller.js",
         "~/app/ipm/controllers/ipm-file-controller.js",
         "~/app/ipm/controllers/ipm-ifeguide-controller.js",
         "~/app/ipm/controllers/ipm-myrequest-controller.js",
         "~/app/ipm/controllers/ipm-notificationalert-controller.js",
         "~/app/ipm/controllers/ipm-notificationAll-controller.js",
         "~/app/ipm/controllers/ipm-svpmessage-controller.js",
         "~/app/ipm/controllers/ipm-link-controller.js",
          "~/app/ipm/controllers/ipm-newsTabs-controller.js",
         "~/app/ipm/controllers/ipm-visionmission-controller.js",
         "~/app/ipm/controllers/ipm-housing-controller.js",
         "~/app/ipm/controllers/ipm-housingNewRequest-controller.js",
         "~/app/ipm/controllers/ipm-filefolder-controller.js",
         "~/app/ipm/controllers/ipm-documentfilemg-controller.js",
         "~/app/ipm/controllers/ipm-crewprofile-controller.js",
         "~/app/ipm/controllers/ipm-crewpersonaldetails-controller.js",
         "~/app/ipm/controllers/ipm-crewTrainingHistory-controller.js",
         "~/app/ipm/controllers/ipm-crewQualnVisa-controller.js",
         "~/app/ipm/controllers/ipm-crewCareerPath-controller.js",
         "~/app/ipm/controllers/ipm-crewIDP-controller.js",
         "~/app/ipm/controllers/ipm-crewMyDoc-controller.js",
         "~/app/ipm/controllers/ipm-crewDstVstd-controller.js",
         "~/app/ipm/controllers/ipm-housingAcknowledge-controller.js",
         "~/app/ipm/controllers/ipm-housingReadonly-controller.js",
         "~/app/ipm/controllers/ipm-search-controller.js",
         "~/app/ipm/controllers/ipm-searchcrew-controller.js",
         "~/app/ipm/controllers/ipm-searchdocuments-controller.js",
          "~/app/ipm/controllers/ipm-searchtransport-controller.js",
           "~/app/ipm/controllers/ipm-searchweather-controller.js",
            "~/app/ipm/controllers/ipm-searchstationinfo-controller.js",
         "~/app/ipm/controllers/ipm-assessmentsearch-controller.js",
         "~/app/ipm/controllers/ipm-poassessment-controller.js",
         "~/app/ipm/controllers/ipm-crewOnBoardAssessmentSearch-controller.js",

         //  "~/app/ipm/controllers/ipm-searchweather-controller.js",
                //   "~/app/ipm/controllers/ipm-searchstationinfo-controller.js",
            "~/app/ipm/controllers/ipm-loading-controller.js",
                //"~/app/ipm/controllers/ipm-transport-controller.js",
           "~/app/ipm/controllers/ipm-weatherinfo-controller.js",
            "~/app/ipm/controllers/ipm-currencydetail-controller.js",
         "~/app/ipm/controllers/ipm-housingChangeAcc-controller.js",
         "~/app/ipm/controllers/ipm-housingGuestAcc-controller.js",
         "~/app/ipm/controllers/ipm-housingMoveOut-controller.js",
         "~/app/ipm/controllers/ipm-housingStayOut-controller.js",
         "~/app/ipm/controllers/ipm-housingSwapRoom-controller.js",
         "~/app/ipm/controllers/ipm-flightAddEditFilter-controller.js",
         "~/app/ipm/controllers/ipm-flightAddEditResult-controller.js",
         "~/app/ipm/controllers/ipm-flightDelayFilter-controller.js",
         "~/app/ipm/controllers/ipm-flightDelayResult-controller.js",
         "~/app/ipm/controllers/ipm-flightDelayEnterFilter-controller.js",
         "~/app/ipm/controllers/ipm-flightDelayEnterTab-controller.js",
         "~/app/ipm/controllers/ipm-flightDelayEnterTabCause-controller.js",
         "~/app/ipm/controllers/ipm-evrsearch-controller.js",
         "~/app/ipm/controllers/ipm-evrmain-controller.js",
         "~/app/ipm/controllers/ipm-evrtabs-controller.js",
         "~/app/ipm/controllers/ipm-evrdraft-controller.js",
         "~/app/ipm/controllers/ipm-evrlists-controller.js",
          "~/app/ipm/controllers/ipm-crewprofiletabs-controller.js",
          "~/app/ipm/controllers/ipm-assessmentList-controller.js",
          "~/app/ipm/controllers/ipm-flightDetails-controller.js",
          "~/app/ipm/controllers/ipm-evrviewmain-controller.js",
          "~/app/ipm/controllers/ipm-evrsubmitted-controller.js",
          "~/app/ipm/controllers/ipm-evrdetails-controller.js",
          "~/app/ipm/controllers/ipm-evrlisttopgrid-controller.js",
          "~/app/ipm/controllers/ipm-evrlisttoptengrid-controller.js",

         //"~/Scripts/jsapi.js",
                //"~/app/ipm/controllers/ipm-airlinenews-controller.js",

         "~/app/ipm/filters/ipm-dateformate-filter.js",
         "~/app/ipm/controllers/ipm-assessment-controller.js",
          "~/app/ipm/controllers/ipm-poAssessmentMonthly-controller.js",
           "~/app/ipm/controllers/ipm-training-controller.js",
          "~/app/ipm/controllers/ipm-poAssessmentMonthly-controller.js",
          "~/app/ipm/controllers/ipm-unscheduledassessment-controller.js",
         "~/app/ipm/controllers/ipm-assessmentPrevious-controller.js",
          "~/app/ipm/controllers/ipm-assessmentPrevDetails-controller.js",
          "~/app/ipm/controllers/ipm-poAssessmentMonthly-controller.js",
           "~/app/ipm/controllers/ipm-crewlocator-controller.js",
              "~/app/ipm/controllers/ipm-crewlocatorsearch-controller.js",
           "~/app/ipm/controllers/ipm-training-controller.js",
           "~/app/ipm/controllers/ipm-acknowledge-controller.js",
            "~/app/ipm/controllers/ipm-assessmentReadonly-controller.js",
            "~/app/ipm/controllers/ipm-evrprevious-controller.js",
             "~/app/ipm/controllers/ipm-assessmentPreviousAll-controller.js",
             "~/app/ipm/controllers/ipm-ivrkeycontacts-controller.js",
             "~/app/ipm/controllers/ipm-kafouenterList-controller.js",
             "~/app/ipm/controllers/ipm-kafouAddEdit-controller.js",
             "~/app/ipm/controllers/ipm-kafouSearch-controller.js",
             "~/app/ipm/controllers/ipm-kafouMy-controller.js"

         ));
            bundles.Add(new SharedBundle(Constants.IpmModule, string.Empty, "~/bundles/messages"));
            bundles.Add(new ScriptBundle("~/bundles/cache").Include(
           "~/app/shared/services/srd-shared-service.js"
           ));
        }
    }
}
