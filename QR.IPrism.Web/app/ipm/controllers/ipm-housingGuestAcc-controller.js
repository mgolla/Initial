'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.housingGuestAcc.controller', ['$rootScope','$scope', 'lookupDataService', 'housingService', '$state', '$stateParams',
        'sharedDataService', 'appSettings', 'ngDialog', 'analyticsService', 'messages', 'toastr', 'blockUI', '$window', '$filter', 'bundleMessage',
        function ($rootScope,$scope, lookupDataService, housingService, $state, $stateParams, sharedDataService, appSettings, ngDialog,
            analyticsService, messages, toastr, blockUI, $window, $filter,bundleMessage) {

            // private variable declaration
            var ipmhousingNewReqBlockUI = blockUI.instances.get('ipmhousingNewReqBlockUI');
            var currentDate = new Date();

            $scope.model = {};
            $scope.model.BuildingDetails = {};
            $scope.model.Guests = {};

            $scope.GuestList = [];
            $scope.rangeError = false;

            $scope.viewmodel = {};
            $scope.viewmodel.fromMindate = null;
            $scope.viewmodel.fromMaxdate = null;
            $scope.viewmodel.toMindate = null;
            $scope.viewmodel.toMaxdate = null;
            $scope.requestNumber;

            $scope.disableCheckout = true;

            function pageEvents(data) {

                $scope.changeRelation = function (model) {

                    if (model) {
                        $scope.disableGuestName = false;
                        $scope.model.GuestNameObj = model;
                    } else {
                        $scope.disableGuestName = true;
                        $scope.model.GuestNameObj = '';
                    }
                };

                $scope.submitRequest = function (form) {

                    $scope.submitted = true;
                    form.$setValidity('rangeValidity', !$scope.rangeError);

                    if (form.$valid) {
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
                    }
                };

                $scope.$watch('model.ToDate', function (nDate, oDate) {
                    if (nDate) {
                        compareDate(new Date($scope.model.FromDate), new Date($scope.model.ToDate));
                    }
                });

                /* Logic is for From and to date selection
                 * To date is only enabled once from date is selected , Also it cannot be greater than from date
                 * From date starts from current date + 7 days.
                 * If user is entitled for current and next year, Then From date range is current date + 7 days to no of days entitlement available
                 * And Max date is 31st Dec of current calendar year or next calendar year, based on entitlement availability. */
                $scope.$watch('model.FromDate', function (nDate, oDate) {

                    if (nDate) {

                        $scope.disableCheckout = false;
                        $scope.viewmodel.toMindate = $scope.model.FromDate;
                        $scope.model.ToDate = "";
                        $scope.model.Guests.NoOfDays = "";

                        var daysLeft = 0;
                        var fromDate = angular.copy($scope.model.FromDate);

                        if (fromDate.getFullYear() == currentDate.getFullYear()) {
                            daysLeft = $scope.housingEntitlements.Setup_days - $scope.housingEntitlements.Used_No_of_days - 1;

                            if (data.IsEntitledCurrentYear && data.IsEntitledNextYear) {
                                $scope.viewmodel.toMaxdate = new Date(fromDate.setDate(fromDate.getDate() + daysLeft));
                            } else {
                                $scope.viewmodel.toMaxdate = new Date(currentDate.getFullYear(), 11, 31);
                            }
                        } else {
                            daysLeft = $scope.housingEntitlements.Setup_days - $scope.housingEntitlements.Next_No_of_days - 1;

                            var nextYearMax = new Date(fromDate.setDate(fromDate.getDate() + daysLeft));
                            var calendarMax = new Date(currentDate.getFullYear() + 1, 11, 31);

                            if (nextYearMax < calendarMax) {
                                $scope.viewmodel.toMaxdate = nextYearMax;
                            } else {
                                $scope.viewmodel.toMaxdate = calendarMax;
                            }
                        }
                    }
                });

                $scope.clearForm = function (form) {

                    $scope.submitted = false;
                    form.$setPristine();
                    //form.$setValidity();
                    form.$setUntouched();

                    $scope.model = {};
                    $scope.model.StayOut = {};
                    $scope.model.BuildingDetails = {};
                    $scope.model.Guests = {};

                    if ($scope.GuestList && $scope.GuestList.length == 1 ) {
                        $scope.model.RelationObj = $scope.GuestList[0];
                        $scope.changeRelation($scope.model.RelationObj);
                    }
                }
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

            function submitData() {

                ipmhousingNewReqBlockUI.start();
                $scope.model.RequestId = $rootScope.RequestTypeObj ? $rootScope.RequestTypeObj.Value : '';
                $scope.model.RequestType = $rootScope.RequestTypeObj ? $rootScope.RequestTypeObj.Text : '';

                $scope.model.BuildingDetails.BuildingName = $scope.existingAccomm.BuildingDetails ? $scope.existingAccomm.BuildingDetails.BuildingName : '';
                $scope.model.BuildingDetails.FlatNumber = $scope.existingAccomm.BuildingDetails ? $scope.existingAccomm.BuildingDetails.FlatNumber : '';
                $scope.model.Guests.Relationship = $scope.model.RelationObj ? $scope.model.RelationObj.Relationship : '';
                $scope.model.Guests.GuestName = $scope.model.GuestNameObj ? $scope.model.GuestNameObj.GuestName : '';

                $scope.model.Guests.CheckinDate = $scope.model.FromDate ? sharedDataService.getDateOnly($scope.model.FromDate) : '';
                $scope.model.Guests.CheckoutDate = $scope.model.ToDate ? sharedDataService.getDateOnly($scope.model.ToDate) : '';

                housingService.postguestacc($scope.model, function (success) {

                    ipmhousingNewReqBlockUI.stop();

                    if (success.data && success.data.RequestNumber) {

                        $scope.requestNumber = success.data.RequestNumber;
                        toastr.info(bundleMessage.getMessages([messages.HOUGUESTACC05, messages.HOUCREATESUCCESS.replace('@reqno', success.data.RequestNumber)]));
                        $state.go('housing');
                    } else {
                        var msg = success.data.Message ? messages[success.data.Message] : messages.HOUERROR01;
                        toastr.error(msg);
                    }
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            };

            function getGuestDetails() {
                ipmhousingNewReqBlockUI.start();
                housingService.guestdetails(null, function (success) {
                    ipmhousingNewReqBlockUI.stop();
                    $scope.GuestList = success;
                    if (success && success.length == 1) {
                        $scope.model.RelationObj = success[0];
                        $scope.changeRelation($scope.model.RelationObj);
                    }
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            }

            /* If user is entitled to current and next year, then 
                To date max - next year max date.
               If user is entitled to current only, then
                To date max - current year max date.  
            . */
            function initialize() {

                housingService.getHousingEntitlements($scope.messages.HOU1015, function (data) {

                    $scope.housingEntitlements = data;

                    if (data.IsCrewEntitled) {

                        pageEvents(data);
                        $scope.housingEntitlements.currentYear = currentDate.getFullYear();
                        $scope.housingEntitlements.nextYear = currentDate.getFullYear() + 1;


                        if (data.IsEntitledCurrentYear && data.IsEntitledNextYear) {

                            $scope.viewmodel.fromMindate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 7);
                            $scope.viewmodel.fromMaxdate = new Date(currentDate.getFullYear() + 1, 11, 31);

                            $scope.viewmodel.toMindate = $scope.model.FromDate;
                            $scope.viewmodel.toMaxdate = new Date(currentDate.getFullYear() + 1, 11, 31);

                        } else if (data.IsEntitledCurrentYear) {

                            $scope.viewmodel.fromMindate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 7);
                            $scope.viewmodel.fromMaxdate = new Date(currentDate.getFullYear(), 11, 31);

                            $scope.viewmodel.toMindate = $scope.model.FromDate;
                            $scope.viewmodel.toMaxdate = new Date(currentDate.getFullYear(), 11, 31);

                        } else if (data.IsEntitledNextYear) {

                            toastr.info(messages.HOUGUESTACC04);

                            $scope.viewmodel.fromMindate = new Date(currentDate.getFullYear() + 1, 0, 1);
                            $scope.viewmodel.fromMaxdate = new Date(currentDate.getFullYear() + 1, 11, 31);

                            $scope.viewmodel.toMindate = $scope.model.FromDate;
                            $scope.viewmodel.toMaxdate = new Date(currentDate.getFullYear() + 1, 11, 31);
                        }

                        getGuestDetails();
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