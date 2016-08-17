'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.evrlisttopgrid.controller', ['$scope', '$state', '$rootScope', 'evrSearchService', 'blockUI', 'messages', 'ngDialog',
        function ($scope, $state, $rootScope, evrSearchService, blockUI, messages, ngDialog) {

            var ipmevrToBlockUI = blockUI.instances.get('ipmevrToBlockUI');

            $scope.evrPending;
            $scope.showEnterEVR = false;
            $scope.showNoEVR = false;
            $scope.FlightDetsID;
            $scope.routeToFlightDetails = false;
            $scope.showFlightDelay = false;
            $scope.SectorFrom;
            $scope.SectorTo;
            $scope.ATD;

            function PageEvents() {

                $scope.ShowEVRButtons = function (data) {

                    SetFlightDetailsId(data);

                    $scope.showEnterEVR = true;

                    if (data.SUBMITTEDVRS == 0 && data.NOVRS == "N") {
                        $scope.showNoEVR = true;
                    } else {
                        $scope.showNoEVR = false;
                    }

                    if (data.DELAYTAGS) {
                        $scope.showFlightDelay = true;
                    } else {
                        $scope.showFlightDelay = false;
                    }

                    if (data.SUBMITTEDVRS == 0 && data.NOVRS != "Y" && data.DRAFTVRS == 0 && !data.ISFERRY &&
                        (!data.POSCREWCOUNT || data.POSCREWCOUNT == 0)) {
                        $scope.routeToFlightDetails = true;
                    } else {
                        $scope.routeToFlightDetails = false;
                    }

                };

                $scope.EnterFlightDelay = function () {

                    if ($scope.routeToFlightDetails) {
                        goToAddEditFlight('fd');
                    } else {
                        goToFlightDelay();
                    }
                };

                $scope.EnterEVR = function () {
                    if ($scope.routeToFlightDetails) {
                        goToAddEditFlight('evr');
                    } else {
                        goToEvrTabs();
                    }
                };

                $scope.NoEVR = function () {

                    $scope.dialogTitle = "Confirmation";
                    $scope.dialogMessage = messages.NOEVRSUBMIT;
                    ngDialog.open({
                        scope: $scope,
                        preCloseCallback: function (value) {
                            if (value == 'Post') {
                                if ($scope.routeToFlightDetails) {
                                    goToAddEditFlight('no_evr');
                                } else {
                                    NoEVRSubmit();
                                }
                                //analyticsService.trackEvent('Action', 'Add', 'View', 'Create Housing Request');
                            }
                        }
                    });


                };

            };

            function NoEVRSubmit() {

                ipmevrToBlockUI.start();
                evrSearchService.UpdateNOVR($scope.FlightDetsID, function (result) {
                    loadData();

                }, function (error) {
                    ipmevrToBlockUI.stop();
                });

            };

            function SetFlightDetailsId(rowData) {
                $scope.FlightDetsID = rowData.FLIGHTDETSID;
                $scope.SectorFrom = rowData.SECTOR.split('-')[0].trim();
                $scope.SectorTo = rowData.SECTOR.split('-')[1].trim();
                $scope.ATD = rowData.ATD_UTC;
            };

            function goToFlightDelay() {
                $state.go('evrflightDelayCause', { FlightDetsID: $scope.FlightDetsID, IsFromEvr: true });
            }

            function goToAddEditFlight(data) {
                $state.go('evrlstsState.addEditFlight', { FlightDetsID: $scope.FlightDetsID, IsFromEvr: data });
            }

            function goToEvrTabs() {
                $state.go('evrlstsState.evrtabs', { FlightDetsID: $scope.FlightDetsID });
            }

            $scope.AdjustGridHeight = function () {
                var rowHeight = 30;
                var headerHeight = 30;
                return {
                    height: ($scope.evrtopgrid.data.length * rowHeight + headerHeight) + "px"
                };
            };

            function initialize() {

                PageEvents();

                $scope.evrtopgrid = {
                    gridApi: {},
                    enableFiltering: true,
                    enableRowSelection: true,
                    enableFullRowSelection: true,
                    showGridFooter: true,
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
                                field: "FLIGHTNUMBER", name: "Flight No. / Aircraft Type", enableHiding: false,
                                cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.ShowEVRButtons(row.entity)">{{row.entity.FLIGHTNUMBER}} ({{row.entity.AIRCRAFTTYPE}})</a><div>'
                            },
                            { field: "SECTOR", name: "SECTOR", enableHiding: false, width: "13%" },
                            {
                                field: "ACTDEPTDATE", displayName: "ATD (UTC)", enableHiding: false, width: "15%", sort: { direction: 'desc' },
                                cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["ACTDEPTDATE"] | date:"dd-MMM-yyyy (HH:mm)" }}</div>'
                            },
                            { field: "DELAYTAGS", name: "Flight Delay", enableHiding: false, width: "10%" },
                            { field: "AIRCRAFTREGNO", name: "Aircraft Reg. No", enableHiding: false, width: "12%" },
                            { field: "SUBMITTEDVRSBYUSER", displayName: "eVRs", enableHiding: false, width: "8%" },
                            { field: "DRAFTVRSBYUSER", displayName: "Draft eVRs", enableHiding: false, width: "10%" },
                            { field: "NOVRS", displayName: "No eVR", enableHiding: false, width: "8%" }
                    ]
                };


                $scope.evrtopgrid.multiSelect = false;

                $scope.evrtopgrid.noUnselect = true;

                $scope.evrtopgrid.onRegisterApi = function (gridApi) {

                    gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                        $scope.ShowEVRButtons(row.entity);
                    });
                };

                loadData();

            }

            function loadData() {
               
                ipmevrToBlockUI.start();
                evrSearchService.getPendingEVRs('', function (result) {

                    $scope.evrPending = result;
                    $scope.evrtopgrid.data = result;

                    // $scope.evrtopgrid.minRowsToShow = result.length + 5;
                    // $scope.evrtopgrid.virtualizationThreshold = result.length + 5;

                    ipmevrToBlockUI.stop();
                }, function (error) {
                    ipmevrToBlockUI.stop();
                });
            };
            initialize();

        }]);