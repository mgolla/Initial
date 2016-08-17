/*********************************************************************
* File Name     : ipm-housingAcknowledge-controller.js
* Description   : Controller for acknowledge Housing Request.
* Create Date   : 25th Jan 2016
* Modified Date : 18th Jul 2016
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.housingAcknowledge.controller', ['$rootScope', '$scope', 'lookupDataService', 'housingService', '$state', '$stateParams',
        'sharedDataService', 'appSettings', 'ngDialog', 'analyticsService', 'messages', 'toastr', 'blockUI', '$window', '$filter',
        function ($rootScope, $scope, lookupDataService, housingService, $state, $stateParams, sharedDataService, appSettings, ngDialog,
            analyticsService, messages, toastr, blockUI, $window, $filter) {

            var ipmhousingReadonlyBlockUI = blockUI.instances.get('ipmhousingReadonlyBlockUI');

            /* scope namespace */
            $scope.model = {};
            $scope.requestDetails = {};
            $scope.notificationModel = {};
            $scope.submitted = false;
            $scope.messages = messages;
            $scope.statusType = sharedDataService.getStatusType();

            function pageEvents() {

                /* acknowledge the housing request */
                $scope.acknowledge = function () {
                    
                    $scope.dialogTitle = "Confirmation";
                    $scope.dialogMessage = messages.HOUCONFIRMACK;
                    ngDialog.open({
                        scope: $scope,
                        preCloseCallback: function (value) {
                            if (value == 'Post') {

                                var model = {
                                    Id: $stateParams.reqno,
                                    Status: 'D'
                                }
                                
                                ipmhousingReadonlyBlockUI.start();
                                // Update notification 
                                sharedDataService.updateNotifications(model, function (result) {
                                    ipmhousingReadonlyBlockUI.stop();
                                    if (result) {
                                        toastr.info(messages.HOUACCEPTNOTICE);
                                        $state.go('home');
                                    } else {
                                        toastr.error(messages.HOUERROR01);
                                    }
                                }, function (error) {
                                    ipmhousingReadonlyBlockUI.stop();
                                    toastr.error(messages.HOUERROR01);
                                });
                            }
                        }
                    });
                };

                /* Approve housing request */
                $scope.approve = function (form, status) {

                    $scope.submitted = true;

                    if (form.$valid) {

                        $scope.dialogTitle = "Confirmation";
                        $scope.dialogMessage = messages.HOUCONFIRMACK;
                        ngDialog.open({
                            scope: $scope,
                            preCloseCallback: function (value) {
                                if (value == 'Post') {

                                    var model = {
                                        RequestId: $scope.model.RequestDetails.RequestNumber,
                                        RequestGuid: $scope.model.RequestDetails.RequestId,
                                        Id: $stateParams.reqno,
                                        Message: $scope.model.Message,
                                        Status: status
                                    }

                                    ipmhousingReadonlyBlockUI.start();
                                    // Update notification 
                                    housingService.updateSwapNotification(model, function (result) {
                                        ipmhousingReadonlyBlockUI.stop();
                                        toastr.info(messages.NOTFSWAP);
                                        $state.go('home', { param: { IsRefreshAlerts: true } });

                                    }, function (error) {
                                        ipmhousingReadonlyBlockUI.stop();
                                        toastr.error(messages.HOUERROR01);
                                    });
                                }
                            }
                        });
                    }
                };
            }

            function initialize() {

                ipmhousingReadonlyBlockUI.start();

                /* Gets notification details and based on notification type specific page is loaded */
                housingService.getNotifications($stateParams.reqno, function (success) {

                    ipmhousingReadonlyBlockUI.stop();
                    $scope.model = success;

                    if (success.Notification) {

                        pageEvents();
                        if ($stateParams.reqtype == "Guest Approval" || $stateParams.reqtype == messages.HOU1015) {

                            $scope.templateUrl = "/app/ipm/partials/ipmHousingGuestAckPartial.html";
                        } else if ($stateParams.reqtype == "Swap Rooms" || $stateParams.reqtype == messages.HOU1019) {

                            $scope.templateUrl = "/app/ipm/partials/ipmHousingSwapAckPartial.html";
                        } else {

                            $scope.templateUrl = "/app/ipm/partials/ipmHousingAckPartial.html";
                            $scope.notificationModel.Message = messages.HOUACK
                                .replace('$id', '<b>' + success.Notification.Type + '</b>')
                                .replace('$date', '<b>' + $filter('date')(success.Notification.Date, "dd-MMM-yyyy") + '</b>')
                                .replace('$reqid', '<b>' + success.Notification.RequestId + '</b>')
                                .replace('$status', '<b>' + $scope.statusType[success.Notification.Status] + '</b>');
                        }
                    } else {
                        toastr.error(messages.HOUERROR01);
                    }

                }, function (error) {
                    ipmhousingReadonlyBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            };

            initialize();
        }]);