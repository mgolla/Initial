'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.kafouSearch.controller', ['$scope', '$state', '$rootScope', 'blockUI', 'messages', 'ngDialog',
        'kafouService', 'sharedDataService', 'lookupDataService', 'flightDetailsAddEditService', '$stateParams',
            function ($scope, $state, $rootScope, blockUI, messages, ngDialog, kafouService, sharedDataService, lookupDataService, flightDetailsAddEditService, $stateParams) {

                var ipmkfSearchfilterBlockUI = blockUI.instances.get('ipmkfSearchfilterBlockUI');

                $scope.model = {};
                $scope.model.selectStaffObj = {};
                $scope.GradeList = [];
                $scope.StatusList = [];
                var formatedValue = '';
                var configFltdelay = null;

                var allFlightsDetails = {};
                var ArrivalTime = null;
                var schArrivalTime = null;
                var currentDateTime = new Date();
                var elapsedTime;

                function setDate() {
                    var currentDate = new Date();
                    $scope.model.ToDateObj = angular.copy(currentDate);

                    currentDate.setMonth(currentDate.getMonth() - 1);
                    $scope.model.FromDateObj = currentDate;
                };

                var toUTCDate = function (date) {
                    var _utc = new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());
                    return _utc;
                };

                $scope.ResetKFSearch = function () {
        
                    $scope.model = {};
                    $scope.model.selectStaffObj = {};
                    setDate();
                    formatedValue = '';
                    $state.go("kfsearch", {}, { reload: true });
                };

                function FormateItems(item) {
                    formatedValue = '';
                    angular.forEach(item, function (data) {
                        formatedValue += data.Value + ",";
                    });
                    return formatedValue.substr(0, formatedValue.length - 1);
                };

                $scope.search = function () {
                    
                    $scope.model.SubmittedByCrew = $scope.model.selectStaffObj ? $scope.model.selectStaffObj.FlightCrewId : '';
                    $scope.model.FlightNumber = $scope.model.FlightNumberUI ? "QR" + $scope.model.FlightNumberUI : '';
                    $scope.model.FromDate = $scope.model.FromDateObj ? sharedDataService.getDateOnly($scope.model.FromDateObj) : '';
                    $scope.model.ToDate = $scope.model.ToDateObj ? sharedDataService.getDateOnly($scope.model.ToDateObj) : '';
                    $scope.model.SectorFrom = $scope.model.SectorFromObj ? $scope.model.SectorFromObj.Text : '';
                    $scope.model.SectorTo = $scope.model.SectorToObj ? $scope.model.SectorToObj.Text : '';

                    $scope.model.RecognitionStatusList = $scope.model.StatusObj ? FormateItems($scope.model.StatusObj) : '';

                    $scope.model.GradeList = $scope.model.GradeObj ? FormateItems($scope.model.GradeObj) : '';

                    getSearchData();
                };

                function getSearchData() {

                    ipmkfSearchfilterBlockUI.start();

                    sharedDataService.getConfigFromDBForKey('PASTHRSDPYFLIGHTSFORENTERCCR', function (data) {
                        ipmkfSearchfilterBlockUI.stop();
                        configFltdelay = data;

                    }, function (error) {
                        toastr.error(messages.HOUERROR01);
                        ipmkfSearchfilterBlockUI.stop();
                    });

                    //ipmkfSearchfilterBlockUI.start();
                    //kafouService.getkafouFlights('', function (result) {
                    //    ipmkfSearchfilterBlockUI.stop();

                    //    allFlightsDetails = result;

                    //}, function (error) {
                    //    toastr.error(messages.HOUERROR01);
                    //    ipmkfSearchfilterBlockUI.stop();
                    //});

                    //kafouService.isEnterDelayForFlight($scope.model.FlightDetsID, function (result) { 
                        
                    //    ipmFlightDelayTabBlockUI.stop();
                    //}, function (error) {
                    //    toastr.error(messages.HOUERROR01);
                    //    ipmFlightDelayTabBlockUI.stop();
                    //});




                    ipmkfSearchfilterBlockUI.start();
                    kafouService.getkfSearchResult($scope.model, function (success) {
                        $scope.grid.data = [];
                        $scope.grid.data = success.data;
                        ipmkfSearchfilterBlockUI.stop();
                    }, function (error) {
                        ipmkfSearchfilterBlockUI.stop();
                    });
                };

                $scope.GetCrewKafou = function (rowData) {

                    if (rowData.RecognitionStatus.toUpperCase() == 'DRAFT') {
                        ipmkfSearchfilterBlockUI.start();
                        kafouService.hasFlightHoursElapsed(rowData.FlightID, configFltdelay, function (result) {

                            ipmkfSearchfilterBlockUI.stop();

                            if (result.toUpperCase() == 'TRUE') {
                                $scope.dialogTitle = "Information";
                                $scope.dialogMessage = messages.KAFOUMSG03 + ' Continue ?';
                                ngDialog.open({
                                    scope: $scope,
                                    preCloseCallback: function (value) {
                                        if (value == 'Post') {
                                            $state.go('kfsearch.kafoudetails', { FlightDetsID: rowData.FlightID, kfgrp: rowData.RecognitionType, kfstatus: 'view', kfrecogid: rowData.RecognitionId });
                                        }
                                    }
                                });
                            } else {
                                $state.go('kfsearch.kafoudetails', { FlightDetsID: rowData.FlightID, kfgrp: rowData.RecognitionType, kfstatus: 'edit', kfrecogid: rowData.RecognitionId });
                            }


                        }, function (error) {
                            toastr.error(messages.HOUERROR01);
                            ipmkfSearchfilterBlockUI.stop();
                        });
                    } else {
                        $state.go('kfsearch.kafoudetails', { FlightDetsID: rowData.FlightID, kfgrp: rowData.RecognitionType, kfstatus: 'view', kfrecogid: rowData.RecognitionId });
                    }

                    


                    //if (allFlightsDetails) {

                    //    for (var i = 0, len = allFlightsDetails.length; i < len; i++) {

                    //        if (allFlightsDetails[i].FlightNumber == rowData.FlightNumber) {

                    //            var currentDateTime = toUTCDate(new Date);

                    //            if (flight.ActArrTime.ToString("dd-MMM-yyyy HH:mm") == DateTime.MinValue.ToString("dd-MMM-yyyy HH:mm"))
                    //                elapsedTime = currentDateTime - allFlightsDetails[i].ScheArrTime;
                    //            else
                    //                elapsedTime = currentDateTime - allFlightsDetails[i].ActArrTime;

                    //            break;
                    //        }
                    //    }

                        


                    //    //angular.forEach(allFlightsDetails, function (data) {
                    //    //    if (data.FlightNumber == rowData.FlightNumber) {
                                
                    //    //    }
                    //    //});


                    //    //$state.go('kfsearch.kafoudetails', { FlightDetsID: rowData.FlightID, kfgrp: false, kfstatus: '123' });
                    //} else {
                    //    //readonly go
                    //}
                };

                $scope.refreshStaff = function (data) {

                    flightDetailsAddEditService.getAutoSuggestStaffInformation({ name: "FlightCrew", CrewSearchCriteria: data }, function (success) {

                        $scope.autoList = success.data;
                    }, function (error) {

                    });
                };

                function initialize() {
                    setDate();

                    //$scope.GradeList.push('CS');
                    //$scope.GradeList.push('CSD');
                    //$scope.GradeList.push('F1');
                    //$scope.GradeList.push('F2');

                    //$scope.StatusList.push('Drafted');
                    //$scope.StatusList.push('Submitted');
                    //$scope.StatusList.push('Approved');
                    //$scope.StatusList.push('Rejected');

                    $scope.grid = {
                        gridApi: {},
                        enableFiltering: true,
                        showGridFooter: true,
                        enableRowSelection: true,
                        enableFullRowSelection: true,
                        paginationPageSizes: [5, 10, 15, 20, 25],
                        enableSelectAll: true,
                        paginationPageSize: 10,
                        enablePagination: true,
                        enablePaginationControls: true,
                        data: [],
                        subgrid: 'false',
                        //rowHeight: 30,
                        //minRowsToShow: 1,
                        columnDefs: [
                                {
                                    field: "StaffNumber", name: "Staff", enableHiding: false, width: "15%",
                                    cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.GetCrewKafou(row.entity)">{{row.entity.StaffNumber}} ({{row.entity.FlightNumber}})</a><div>'
                                },
                                { field: "StaffDetails", name: "Staff Name", enableHiding: false, width: "25%" },
                                { field: "FlightDetails", name: "Flight Details", enableHiding: false, width: "20%" },
                                {
                                    field: "RecognitionDate", name: "Date", enableHiding: false, width: "20%", sort: { direction: 'desc' },
                                    cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["RecognitionDate"] | date:"dd-MMM-yyyy (HH:mm)" }}</div>'
                                },
                                { field: "RecognitionStatus", name: "Status", enableHiding: false, width: "20%" }
                        ]
                    };

                    ipmkfSearchfilterBlockUI.start();
                    lookupDataService.getLookupList('Sector', $scope.Text, function (result) {
                        $scope.SectorAllFromList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text, Desc: obj.FilterText } });
                        $scope.SectorAllToList = angular.copy($scope.SectorAllFromList);
                        ipmkfSearchfilterBlockUI.stop();
                    });

                    ipmkfSearchfilterBlockUI.start();
                    kafouService.getkfSearchParams('', function (result) {

                        $scope.GradeList = result.RecognisedCrewGradeList;
                        $scope.StatusList = result.RecognitionStatusList;

                        ipmkfSearchfilterBlockUI.stop();
                    }, function (error) {
                        ipmkfSearchfilterBlockUI.stop();
                    });

                    $scope.search()
                }

                initialize();

            }]);