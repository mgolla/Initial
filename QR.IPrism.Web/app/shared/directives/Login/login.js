"use strict"
var loginModule = angular.module('app.login',['ngAnimate', 'toastr', 'ui.bootstrap', 'angular-loading-bar','ngDialog', 'blockUI', 'ui.router']);

loginModule.controller('srd.login.controller', ['$scope', '$http', 'toastr', '$location', '$window', function ($scope, $http, toastr, $location, $window) {
    var Initialize = function () {
        $scope.loginModel = {"StaffNumber":"","Password":""};
    }
    Initialize();
    $scope.login=function()
    {
        var model = $scope.loginModel;
        var location = $location;
        $http.post("/login/post", $scope.loginModel).success(function (data, status) {
            if (data.IsSuccess) {
                var url = "";
                if (data.IsCaptchaEnabled)
                    url = "http://" + $window.location.host + "/Main/cta";
                else
                    url = "http://" + $window.location.host + "/Main/hm";

                $window.location.href = url;
            }
            else
                toastr.error('Invalid user name and password.');
        })
    }
}]);

loginModule.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/login');
    $stateProvider
    .state('login', {
        url: '',
        views: {
            "loginview": {
                templateUrl: '/app/shared/directives/Login/login.html',
                controller: 'srd.login.controller'
            }
        }
    })
});


loginModule.config(function (toastrConfig) {
    angular.extend(toastrConfig, {
        autoDismiss: false,
        containerId: 'toast-container',
        maxOpened: 1,
        newestOnTop: true,
        positionClass: 'toast-bottom-right',
        preventDuplicates: true,
        preventOpenDuplicates: true,
        target: 'body',
        allowHtml: false,
        closeButton: false,
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
            progressbar: 'app/shared/templates/progressbar.html'
        },
        timeOut: 5000,
        titleClass: 'toast-title',
        toastClass: 'toast'
    });
});

loginModule.config(['ngDialogProvider', function (ngDialogProvider) {
    ngDialogProvider.setDefaults({
        className: 'ngdialog-theme-default ngdialog-theme-custom',
        plain: false,
        closeByDocument: false,
        closeByEscape: true,
        template: '/app/shared/directives/srd-modal-confirmation.html'
    });
}]);

loginModule.directive('autoFocus', function ($timeout) {
    return {
        restrict: 'AC',
        link: function (_scope, _element) {
            $timeout(function () {
                _element[0].focus();
            }, 0);
        }
    };
});
loginModule.config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeSpinner = false;
    cfpLoadingBarProvider.latencyThreshold = 200;
}]);

loginModule.config(function (blockUIConfig) {
    blockUIConfig.templateUrl = '/app/shared/templates/srd-spiner-template.html';
    blockUIConfig.autoBlock = false;
    blockUIConfig.resetOnException = false;

});

