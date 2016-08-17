/*********************************************************************
* File Name     : ipm-housing-controller.js
* Description   : Controller for Housing Request module.
* Create Date   : 25th Jan 2016
* Modified Date : 25th Jan 2016
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.flightdelayentertabcause.controller', ['$scope', '$state', '$stateParams', 'flightDelayService', 'lookupDataService', 'messages', 'blockUI', 'toastr', 'ngDialog',
        function ($scope, $state, $stateParams, flightDelayService, lookupDataService, messages, blockUI, toastr, ngDialog) {

            // private variable declaration
            var ipmFlightDelayTabBlockUI = blockUI.instances.get('ipmFlightDelayTabBlockUI');

            $scope.model = {};
            // $scope.ArrivalDelayReason = [];
            // $scope.DepartureDelayReason = [];
            $scope.Reason = [];           
            $scope.submitted = false;

            $scope.submittedData = [];
            $scope.showDeptDelay = false;
            $scope.showArrDelay = false;

            $scope.readonly = true;
            $scope.IsFromEvr = false;

            function getEnterFlightDelayResults() {

                $scope.IsFromEvr = $stateParams.IsFromEvr;

                flightDelayService.getEnterFlightDelayResults({}, function (success) {
                    angular.forEach(success.data, function (data) {
                        if (data.FlightDetsID == $stateParams.FlightDetsID) {
                            $scope.model = data;
                            initialize();
                        }
                    });
                    ipmFlightDelayTabBlockUI.stop();
                }, function (error) {
                    ipmFlightDelayTabBlockUI.stop();
                }, null, false);
            }

            function initialize() {

                $scope.model.DepartureDelayObj = [];
                $scope.model.ArrivalDelayObj = [];

                $scope.submitRequest = function (form) {
                    $scope.submitted = true;
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

                if ($scope.model.FlightDelay.DepartureDelay > "00:00") {
                    $scope.showDeptDelay = true;
                }
                // Tested the string comparision with 00:04 as well. its acting like INT comparision
                if ($scope.model.FlightDelay.ArrivalDelay > "00:00") {
                    $scope.showArrDelay = true;
                }

                flightDelayService.getdelayReason($scope.model.FlightDetsID, function (result) {

                    var arrSelected = [], deptSelected = [];

                    angular.forEach(result, function (obj) {

                        if (obj.IsSelected && obj.DelayType == "Arrival") {

                            arrSelected.push({ Value: obj.FlightDelayCatId, Text: obj.FlightDelayCatName });

                            if (obj.DelayComment) {
                                $scope.model.ArrivalDelayComment = obj.DelayComment;
                            }

                        } else if (obj.IsSelected && obj.DelayType == "Departure") {

                            deptSelected.push({ Value: obj.FlightDelayCatId, Text: obj.FlightDelayCatName });

                            if (obj.DelayComment) {
                                $scope.model.DepartureDelayComment = obj.DelayComment;
                            }

                        } else if (!obj.IsSelected) {

                            $scope.Reason.push({ Value: obj.FlightDelayCatId, Text: obj.FlightDelayCatName });
                        }
                    });

                    angular.forEach(arrSelected, function (selected) {
                        angular.forEach($scope.Reason, function (item) {
                            if (selected.Text == item.Text) {
                                $scope.model.ArrivalDelayObj.push(item);
                            }
                        });
                    });

                    angular.forEach(deptSelected, function (selected) {
                        angular.forEach($scope.Reason, function (item) {
                            if (selected.Text == item.Text) {
                                $scope.model.DepartureDelayObj.push(item);
                            }
                        });
                    });

                }, function (error) {
                    toastr.error(messages.HOUERROR01);
                });

                flightDelayService.isEnterDelayForFlight($scope.model.FlightDetsID, function (result) {

                    if (result == "Y" || $scope.model.DelayReportStatus != "Not Submitted") {

                        if (!$scope.model.ActualArrTime) {
                            toastr.error(messages.FLIGHTDELAYMSG01);
                        } else if (result == "Y" || $scope.model.DelayReportStatus == "Not Submitted") {
                            toastr.error(messages.FLIGHTDELAYMSG02);
                        }
                    } else {
                        $scope.readonly = false;
                    }
                    ipmFlightDelayTabBlockUI.stop();
                }, function (error) {
                    toastr.error(messages.HOUERROR01);
                    ipmFlightDelayTabBlockUI.stop();
                });

            }

            function submitData() {

                var DelayType = "";
                if ($scope.model.ArrivalDelayObj.length > 0) {

                    for (var i = 0; i < $scope.model.ArrivalDelayObj.length; i++) {
                        DelayType = DelayType + $scope.model.ArrivalDelayObj[i].Value + "|";
                    }

                    $scope.model.DelayType = DelayType.substr(0, DelayType.length - 1);
                    $scope.submittedData.push({
                        DelayType: "Arrival",
                        DelayComment: $scope.model.ArrivalDelayComment,
                        FlightDetsID: $scope.model.FlightDetsID,
                        FlightDelayCatId: $scope.model.DelayType
                    });
                }

                DelayType = "";
                if ($scope.model.DepartureDelayObj.length > 0) {

                    for (var i = 0; i < $scope.model.DepartureDelayObj.length; i++) {
                        DelayType = DelayType + $scope.model.DepartureDelayObj[i].Value + "|";
                    }

                    $scope.model.DelayType = DelayType.substr(0, DelayType.length - 1);
                    $scope.submittedData.push({
                        DelayType: "Departure",
                        DelayComment: $scope.model.DepartureDelayComment,
                        FlightDetsID: $scope.model.FlightDetsID,
                        FlightDelayCatId: $scope.model.DelayType
                    });
                }

                ipmFlightDelayTabBlockUI.start();

                flightDelayService.setFlightDelayDetails($scope.submittedData, function (result) {
                    ipmFlightDelayTabBlockUI.stop();

                    if ($stateParams.IsFromEvr == 'true' || $stateParams.IsFromEvr == true) {
                        $scope.model.DelayReportStatus = "Submitted";
                        $state.go('evrlstsState');
                    } else {
                        $state.go('enterFlightDelay');
                    }
                }, function (result) {
                    ipmFlightDelayTabBlockUI.stop();
                });
            }

            getEnterFlightDelayResults();
        }]);