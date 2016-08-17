var sharedModule = angular.module('app.shared.components');
sharedModule.config(['toastrConfig',function (toastrConfig) {
    angular.extend(toastrConfig, {
        autoDismiss: false,
        containerId: 'toast-container',
        maxOpened: 1,
        newestOnTop: true,
        positionClass: 'toast-bottom-right',
        preventDuplicates: false,
        preventOpenDuplicates: true,
        target: 'body',
        allowHtml: true,
        closeButton: true,
        closeHtml: '<button>&times;</button>',
        extendedTimeOut: 1000,
        iconClasses: {
            error: 'toast-error',
            info: 'toast-info',
            success: 'toast-success',
            warning: 'toast-warning'
        },
        messageClass: 'toast-message',
        onHidden: null,
        onShown: null,
        onTap: null,
        progressBar: false,
        tapToDismiss: true,
        templates: {
            toast: '/app/shared/templates/toast.html',
            progressbar: '/app/shared/templates/progressbar.html',
            controller:''
        },
        timeOut: 5000,
        titleClass: 'toast-title',
        toastClass: 'toast'
    });
}]);

sharedModule.config(['ngDialogProvider', function (ngDialogProvider) {
    ngDialogProvider.setDefaults({
        className: 'ngdialog-theme-default ngdialog-theme-custom',
        plain: false,
        closeByDocument: false,
        closeByEscape: true,
        template: '/app/shared/directives/srd-modal-confirmation.html'
    });
}]);

sharedModule.config(['KeepaliveProvider', 'IdleProvider',  function (KeepaliveProvider, IdleProvider) {
    IdleProvider.idle(1*60);
    IdleProvider.timeout(20);
    KeepaliveProvider.interval(10);
}]);

sharedModule.directive('autoFocus',['$timeout', function ($timeout) {
    return {
        restrict: 'AC',
        link: function (_scope, _element) {
            $timeout(function () {
                _element[0].focus();
            }, 0);
        }
    };
}]);
sharedModule.config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeSpinner = false;
    cfpLoadingBarProvider.latencyThreshold = 200;
}]);
sharedModule.config(['blockUIConfig',function (blockUIConfig) {
    blockUIConfig.templateUrl = '/app/shared/templates/srd-spiner-template.html';
    blockUIConfig.autoBlock = false;
    blockUIConfig.resetOnException = false;

}]);

sharedModule.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
}]);

sharedModule.run(['$templateCache', function ($templateCache) {
    $templateCache.put("template/tabs/tabset.html", "<div><ul class=\"nav nav-{{type || 'tabs'}}\" ng-class=\"{'nav-stacked': vertical, 'nav-justified': justified}\" ng-transclude></ul><div class=\"tab-content\"><div ng-repeat-start=\"tab in tabs\" ng-click=\"tab.active=true\" class=\"tab-accordion-header\" ng-class=\"{'active': tab.active}\">{{tab.heading}}</div><div class=\"tab-pane\"ng-repeat-end ng-class=\"{active: tab.active}\" tab-content-transclude=\"tab\"> </div> </div> </div>");
}]);

sharedModule.run(["$templateCache", function ($templateCache) {
    $templateCache.put("template/tabs/tab.html",
      "<li ng-click=\"select()\" ng-class=\"{active: active, disabled: disabled}\">\n" +
      "<div  uib-tab-heading-transclude><i></i> {{heading}}</div>\n" +
      "</li>\n" +
      "");
}]);

sharedModule.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.imgSrcSanitizationWhitelist(/^\s*(https?|ftp|file|chrome-extension):|data:image\//);
    $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|file|chrome-extension|tel|data|blob):/);
}]);

sharedModule.filter('slice', function () {
    return function (arr, start, end) {
        if (arr) {
            return arr.slice(start, end);
        }
        return arr;
    };
});



//sharedModule.config(['$cordovaInAppBrowserProvider',function ($cordovaInAppBrowserProvider) {

//    var defaultOptions = {
//        location: 'no',
//        clearcache: 'no',
//        toolbar: 'no'
//    };

//    document.addEventListener("deviceready", function () {

//        $cordovaInAppBrowserProvider.setDefaultOptions(defaultOptions)

//    }, false);
//}]);

sharedModule.config(['fileManagerConfigProvider', function (config) {
    var pathArray = window.location.href.split('/');
    var protocol = pathArray[0];
    var host = pathArray[2];
    var baseURL = protocol + '//' + host;

    var defaults = config.$get();
    config.set({
        appName: '',
        allowedActions: angular.extend(defaults.allowedActions, {
            remove: false
        }),
        listUrl: baseURL + '/api/filemg',
        downloadFileUrl: baseURL + '/api/FileMgDownload/post',
        permissionsUrl: baseURL + '/api/filemg',
        extractUrl: baseURL + '/api/filemg',
        compressUrl: baseURL + '/api/filemg',
        createFolderUrl: baseURL + '/api/filemg',
        getContentUrl: baseURL + '/api/FileMgDownload/post',
        removeUrl: baseURL + '/api/filemg',
        copyUrl: baseURL + '/api/filemg',
        renameUrl: baseURL + '/api/filemg',
        uploadUrl: baseURL + '/api/filemg'

    });
}]);

sharedModule.filter('spaceless', function () {
    return function (input) {
        if (input) {
            return input.replace(/\s+/g, '-');
        }
    }
});

sharedModule.config(['uiSelectConfig',function (uiSelectConfig) {
    uiSelectConfig.theme = 'bootstrap';
}]);

sharedModule.run(['tokensManagerService', '$rootScope', '$state', '$timeout', 'ngDialog', 'sharedDataService', 'messages', function (tokensManagerService, $rootScope, $state, $timeout, ngDialog, sharedDataService, messages) {
    $rootScope.$state = $state;
}]);

var appSettings = {};
appSettings.API = '/';
appSettings.isMobile = false;
appSettings.DocumentId = "";
appSettings.SecondDocumentId = "";
appSettings.DocumentPath = "";
appSettings.DocumentFolders = [];
appSettings.CrewPhotos = "/ProcessedImages/CrewPhotos/";
appSettings.BackGroundImages = "/ProcessedImages/BackGroundImages/";
appSettings.isNotDepNewsLoad = true;
appSettings.isNotAirlineNewsLoad = true;
appSettings.isNotIFEGuideLoad = true;


appSettings.isMobileHeader = function () {
    var vpwM = window.innerWidth ;
    if (vpwM < 769) {
        return true;
    } else {
        return false;
    }
}
sharedModule.value("appSettings", appSettings);



