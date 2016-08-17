'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.housingStayOut.controller', ['$rootScope', '$scope', 'lookupDataService', 'housingService', '$state', 'FileUploader', '$stateParams',
        'sharedDataService', 'appSettings', 'ngDialog', 'analyticsService', 'messages', 'toastr', 'blockUI', '$window', '$filter',
        function ($rootScope, $scope, lookupDataService, housingService, $state, FileUploader, $stateParams, sharedDataService, appSettings, ngDialog,
            analyticsService, messages, toastr, blockUI, $window, $filter) {

            // private variable declaration
            var ipmhousingNewReqBlockUI = blockUI.instances.get('ipmhousingNewReqBlockUI');

            $scope.model = {};
            $scope.model.StayOut = {};
            $scope.model.Guests = {};

            $scope.rangeError = false;
            $scope.stayOutRequestType = [];
            $scope.GuestList = [];

            $scope.viewmodel = {};
            $scope.uploader;
            $scope.requestNumber;

            function pageEvents() {

                $scope.submitRequest = function (form) {

                    $scope.submitted = true;
                    form.$setValidity('rangeValidity', !$scope.rangeError);

                    if (form.$valid && $scope.uploader.queue.length > 0) {
                        $scope.dialogTitle = "Confirmation";
                        $scope.dialogMessage = messages.HOUCONFIRMNEW;
                        ngDialog.open({
                            scope: $scope,
                            preCloseCallback: function (value) {
                                if (value == 'Post') {
                                    submitData();
                                    //analyticsService.trackEvent('Action', 'Add', 'View', 'Create Housing Request');
                                }
                            }
                        });
                    } else if ($scope.uploader.queue.length < 1) {
                        toastr.error($scope.messages.HOUATTACHMENTSO);
                       // $scope.invalidAttachment = true;
                    }
                }

                $scope.changeStayoutReqDetails = function (model) {

                    if (model) {
                        switch (model.Text.toLocaleLowerCase()) {

                            case messages.HOUSWAP03.toLocaleLowerCase():
                            case messages.HOUSWAP05.toLocaleLowerCase():
                                $scope.model.FromDate = '';
                                $scope.model.ToDate = '';

                                $scope.disableSpecificDates = true;
                                $scope.disabledFromDate = false;
                                $scope.disabledToDate = true;

                                break;
                            case messages.HOUSWAP04.toLocaleLowerCase():

                                $scope.model.FromDate = '';
                                $scope.model.ToDate = '';

                                $scope.disabledFromDate = true;
                                $scope.disabledToDate = true;
                                break;

                            default:
                                $scope.disableSpecificDates = false;
                                $scope.disabledFromDate = false;
                                $scope.disabledToDate = false;
                                break;
                        }
                    }
                };

                $scope.changeStayoutReason = function (model) {

                    if (model && model.Value == "STAYOUT_REASON2") {
                        $scope.attachmentNote = messages.HOUSWAP06;
                    } else {
                        $scope.attachmentNote = messages.HOUSWAP07;
                    }
                };

                // Based on selection of request details type
                // If Type is rostered days off for whole month , then auto populate to date to last day of month
                $scope.$watch('model.FromDate', function (nDate, oDate) {
                   
                    if (nDate) {

                        var reason = $scope.model.stayoutRequestdetObj ? $scope.model.stayoutRequestdetObj.Text : '';

                        if (reason && reason.toLocaleLowerCase() == messages.HOUSWAP03.toLocaleLowerCase()) {
                            $scope.model.ToDate = new Date(nDate.getFullYear(), nDate.getMonth() + 1, 0);
                        };

                        $scope.viewmodel.toMindate = nDate;
                        compareDate(new Date($scope.model.FromDate), new Date($scope.model.ToDate));
                    }
                });

                $scope.$watch('model.ToDate', function (nDate, oDate) {
                    compareDate(new Date($scope.model.FromDate), new Date($scope.model.ToDate))
                });

                $scope.clearForm = function (form) {

                    $scope.submitted = false;
                    form.$setPristine();
                    //form.$setValidity();
                    form.$setUntouched();
                    //$scope.invalidAttachment = false;

                    $scope.model = {};
                    $scope.model.StayOut = {};
                    $scope.model.BuildingDetails = {};
                    $scope.model.Guests = {};

                    $scope.uploader.clearQueue();
                }
              
            };

            function submitData() {

                ipmhousingNewReqBlockUI.start();
                $scope.model.RequestId = $rootScope.RequestTypeObj ? $rootScope.RequestTypeObj.Value : '';
                $scope.model.RequestType = $rootScope.RequestTypeObj ? $rootScope.RequestTypeObj.Text : '';

                $scope.model.RequestReason = $scope.model.ReasonObj ? $scope.model.ReasonObj.Text : '';
                $scope.model.StayOut.StayOutRequestTypeId = $scope.model.stayoutRequestdetObj ? $scope.model.stayoutRequestdetObj.Value : '';

                $scope.model.StayOut.StayOutRequestFromDate= $scope.model.FromDate ? sharedDataService.getDateOnly($scope.model.FromDate) : '';
                $scope.model.StayOut.StayOutRequestToDate = $scope.model.ToDate ? sharedDataService.getDateOnly($scope.model.ToDate) : '';

                if ($scope.model.StayOutCrewRelationObj) {

                    var relativesNames = "";
                    var relativesIds = "";
                    for (var i = 0; i < $scope.model.StayOutCrewRelationObj.length; i++) {
                        relativesIds = relativesIds + $scope.model.StayOutCrewRelationObj[i].RelationType + "|";
                        relativesNames = relativesNames + $scope.model.StayOutCrewRelationObj[i].GuestName + "|";
                    }

                    $scope.model.StayOut.StayOutCrewRelationId = relativesIds.substr(0, relativesIds.length - 1);
                    $scope.model.StayOut.StayOutCrewRelationName = relativesNames.substr(0, relativesNames.length - 1);
                }

                housingService.poststayout($scope.model, function (success) {

                  
                    var data = success.data;
                    if (data && data.RequestNumber && data.ResponseId) {

                        $scope.requestNumber = data.RequestNumber;
                        if ($scope.uploader.queue.length > 0) {
                            $scope.requestId = data.ResponseId;
                            $scope.uploader.uploadAll();
                        } else {
                            ipmhousingNewReqBlockUI.stop();
                            toastr.info(messages.HOUCREATESUCCESS.replace('@reqno', data.RequestNumber));
                            $state.go('housing');
                        }
                    } else {
                        ipmhousingNewReqBlockUI.stop();
                        var msg = data.Message ? messages[data.Message] : messages.HOUERROR01;
                        toastr.error(msg);
                    }
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            };

            function compareDate(todate, fromdate) {
                if (fromdate && todate) {
                    if (fromdate < todate) {
                        $scope.rangeError = true;
                        toastr.error("From date cannot exceed to date");
                    } else {
                        $scope.rangeError = false;
                        $scope.model.Guests.NoOfDays = (new Date(fromdate) - new Date(todate)) / (1000 * 60 * 60 * 24) + 1;
                    }
                }
            }

            function getStayOutRequestType() {
                // Gets reason list for move in type
                housingService.getStayOutRequestType(null, function (result) {
                    $scope.stayOutRequestType = result;
                    $scope.model.stayoutRequestdetObj = $scope.stayOutRequestType[0];
                    $scope.changeStayoutReqDetails($scope.model.stayoutRequestdetObj);
                }, function (error) {

                });
            };

            function getStayOutReason() {
                // Gets reason list for move in type
                sharedDataService.getCommonInfo('HR_StayOutReason', function (result) {
                    $scope.ReasonList = result;
                    $scope.model.ReasonObj = $scope.ReasonList[0];
                    $scope.changeStayoutReason($scope.model.ReasonObj);
                }, function (error) {

                });
            };

            function getCrewRelations() {

                ipmhousingNewReqBlockUI.start();

                housingService.getCrewRelations(null, function (success) {
                    ipmhousingNewReqBlockUI.stop();
                    $scope.GuestList = success;
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            }

            function getSwapRoomsRequestType() {

                ipmhousingNewReqBlockUI.start();
                housingService.getSwapRoomsRequestType(null, function (success) {
                    ipmhousingNewReqBlockUI.stop();
                    $scope.SwapRoomsRequest = success;

                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            };

            function initialize() {

                //fileuploader();
                housingService.getHousingEntitlements($scope.messages.HOU1018, function (data) {

                    $scope.housingEntitlements = data;
                    var currentDate = new Date();

                    if (data.IsCrewEntitled) {

                        pageEvents();
                        getStayOutRequestType();
                        getStayOutReason();
                        getCrewRelations();

                        $scope.disableSpecificDates = false;
                        $scope.viewmodel.fromMindate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 7);
                        $scope.viewmodel.toMindate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 7);
                        $scope.attachmentNote = messages.HOUSWAP06;
                    } else {
                        toastr.info(data.Message);
                    }

                    ipmhousingNewReqBlockUI.stop();
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });

            };

            ipmhousingNewReqBlockUI.start();
            initialize();
        }]);