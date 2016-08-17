'use strict'
angular.module('app.ipm.module').controller('ipm.home.controller', ['$scope', '$http', 'sharedDataService', 'analyticsService',
    '$state', '$rootScope', '$stateParams', 'messages','ngDialog',
    function ($scope, $http, sharedDataService, analyticsService, $state, $rootScope, $stateParams, messages, ngDialog) {
        $rootScope.IsRefreshAlerts = false;
        $rootScope.isMobile = false;
        //$rootScope.isMobileHeader = function () {
        //    var vpwM = $(window).width();
        //    if (vpwM < 769) {
        //        return true;
        //    } else {
        //        return false;
        //    }
        //}

        $rootScope.isNotNullOrEmptyOrUndefined = function (value) {
            return (value != null && value.toString().trim().length > 0);
        };
        var Initialize = function () {

            if (sharedDataService.notificationDetails) {

                $scope.dialogTitle = "Assessment Notifications";
                $scope.dialogMessage = messages.ASSESSMENTACK;//"An assessment has been done on you. Kindly click on view to acknowledge the assessment.";
                ngDialog.open({
                    template: '/app/ipm/partials/ipmModalConfirmation.html',
                    showClose: false,
                    closeByEscape: false,
                    scope: $scope,
                    preCloseCallback: function (value) {
                        if (value == 'Post') {
                            $state.go('idpack', { reqno: sharedDataService.notificationDetails.Id, reqtype: sharedDataService.notificationDetails.Type });
                        }
                    }
                });
            }

            if ($stateParams.param && $stateParams.param.IsRefreshAlerts && $stateParams.param.IsRefreshAlerts != null &&
                $stateParams.param.IsRefreshAlerts.toString().trim().length > 0) {
                $rootScope.IsRefreshAlerts = true;
            } else {
                $rootScope.IsRefreshAlerts = false;
            }
            //$state.go("home.weeklyRoaster");
            analyticsService.trackEvent('Load', 'Add', 'HomePage', 'Home Page');
        }
        Initialize();
    }]);

