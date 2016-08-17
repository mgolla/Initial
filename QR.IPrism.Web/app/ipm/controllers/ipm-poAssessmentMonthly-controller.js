'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.poAssessmentMonthly.controller', ['$scope', '$http', 'assessmentsearchService', 'analyticsService',
        'messages', 'sharedDataService', '$state', '$rootScope', '$timeout', '$compile', 'blockUI', '$stateParams', 'appSettings', 'ngDialog',
        'toastr', '$window', function ($scope, $http, assessmentsearchService, analyticsService, messages,
                                      sharedDataService, $state, $rootScope, $timeout, $compile, blockUI, $stateParams, appSettings,
                                      ngDialog, toastr, $window) {

            var ipmAsmtMonthlyBlockUI = blockUI.instances.get('ipmAsmtMonthlyBlockUI');
            var startDate;

            $scope.AsmntDetails = {};
            $scope.model = {};

            //Controller Scope Initialization
            function Initialize() {

                $scope.selectedRoster = -1;
                $scope.clickedRosterId = -1;
                setStyleFormatting();
                $scope.filter = {
                    StaffID: $stateParams.staffID,
                    StartDate: null,
                    EndDate: null,
                    RosterTypeID: 0,
                    TimeFormat: 3,
                    IsLanding: true,
                    LeftValue: -10,
                    RightValue: 10
                }
                loadRosterList();
            }

            function setStyleFormatting() {

                var vpw = $(window).width();
                if (vpw < 769) {
                    $rootScope.isWideScreen = false;
                }
                else {
                    $rootScope.isWideScreen = true;
                }
            }

            function loadRosterList() {
                // $scope.StaffNum = filter.staffID;
                ipmAsmtMonthlyBlockUI.start();
                assessmentsearchService.getAsmntRoster($scope.filter, function (result) {

                    ipmAsmtMonthlyBlockUI.stop();
                    startDate = new Date(result.data.StartDate);

                    $scope.model.rosterViewModelPO = result.data;
                    $scope.model.rosterViewModelPO.TimeFormat = $scope.filter.TimeFormat;
                    $scope.extraRosters = [];

                    for (var i = 0; i < (32 - $scope.model.rosterViewModelPO.Rosters.length) ; i++) {
                        $scope.extraRosters.push(i);
                    }
                }, function (error) {
                    ipmAsmtMonthlyBlockUI.stop();
                });
            }                     
            //$scope.goBack = function () {

            //    $state.go('poasmnt');
            //}

            $scope.leftClick = function () {

                startDate.setMonth(startDate.getMonth() - 1);
                $scope.filter.StartDate = startDate;
                $scope.filter.IsLanding = false;
                $scope.filter.IsNext = 2;

                loadRosterList();
            }

            $scope.rightClick = function () {

                startDate.setMonth(startDate.getMonth() + 1);
                $scope.filter.StartDate = startDate;
                $scope.filter.IsLanding = false;
                $scope.filter.IsNext = 1;

                loadRosterList();
            }

            $scope.rosterClick = function (item, flightNo) {
                if ((item && item.Flight && item.AssessmentAllowed) || (flightNo && item.AssessmentAllowed)) {
                    var _item = item;
                    $scope.AsmntDetails = {
                        AssesseeStaffNo: $scope.filter.StaffID,
                        FlightNumber: item.Flight,
                        SectorFrom: item.Departure,
                        SectorTo: item.Arrival,
                        FlightDate: item.DutyDate,
                    };

                    var msg = messages.SCHEDULEASMNT + ' QR ' + item.Flight + ' from ' + item.Departure + ' to ' +
                                item.Arrival + ' on ' + item.StandardTimeDepartureUtc + " (UTC)";
                    $scope.dialogTitle = "Confirmation";
                    $scope.dialogMessage = msg;
                    ngDialog.open({
                        scope: $scope,
                        preCloseCallback: function (value) {
                            if (value == 'Post') {
                                scheduleAssessment($scope.AsmntDetails);
                            }
                        }
                    });
                }
            };

            function scheduleAssessment(AsmntDetails) {

                ipmAsmtMonthlyBlockUI.start();
                assessmentsearchService.scheduledassessment(AsmntDetails, function (success) {
                    ipmAsmtMonthlyBlockUI.stop();

                    if (!success.data.IsSuccess) {
                        toastr.error(success.data.Message);
                    }
                    else {
                        toastr.info(messages.SCHEDULEASMNTCONFIRM);
                        $state.go('poasmnt', { myparam: true });
                    }
                }, function (error) {
                    ipmAsmtMonthlyBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            };

            Initialize();

        }]);
