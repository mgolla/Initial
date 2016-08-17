'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.evrlisttoptengrid.controller', ['$scope', '$state', '$rootScope', 'evrSearchService', 'blockUI', 'messages', 'ngDialog',
        function ($scope, $state, $rootScope, evrSearchService, blockUI, messages, ngDialog) {

            var ipmevrTenBlockUI = blockUI.instances.get('ipmevrTenBlockUI');

            $scope.evrLastTen;
            $scope.showViewEVR = false;
            $scope.FlightDetsID;
            $scope.routeToFlightDetails = false;
            $scope.showFlightDelay = false;
            $scope.SectorFrom;
            $scope.SectorTo;
            $scope.ATD;

            function PageEvents() {

                //$scope.ShowViewButtons = function (data) {

                //    SetFlightDetailsId(data);
                //    $scope.showViewEVR = true;
                //};

                $scope.ViewEVR = function () {
                    $state.go('evrlstsState.evrviewState', { FlightDetsID: $scope.FlightDetsID, SectorFrom: $scope.SectorFrom, SectorTo: $scope.SectorTo, ATD: $scope.ATD });
                };
            };

            function SetFlightDetailsId(rowData) {
                $scope.FlightDetsID = rowData.FLIGHTDETSID;
                $scope.SectorFrom = rowData.SECTOR.split('-')[0].trim();
                $scope.SectorTo = rowData.SECTOR.split('-')[1].trim();
                $scope.ATD = rowData.ATD_UTC;
            };

            function initialize() {

                PageEvents();
                $scope.ShowViewButtons = function (row) {
                    SetFlightDetailsId(row);
                    $state.go('evrlstsState.evrviewState', { FlightDetsID: $scope.FlightDetsID, SectorFrom: $scope.SectorFrom, SectorTo: $scope.SectorTo, ATD: $scope.ATD });
                };


                $scope.evrtengrid = {
                    gridApi: {},
                    enableFiltering: true,
                    showGridFooter: true,
                    enableColumnResizing: true,
                    paginationPageSizes: [5, 10, 15, 20, 25],
                    paginationPageSize: 10,
                    enablePagination: true,
                    enablePaginationControls: true,
                    data: [],
                    subgrid: 'false',
                    columnDefs: [
                            //{
                            //    field: "FLIGHTDETSID", name: "Select", width: "5%",
                            //    cellTemplate: '<div class="ui-grid-cell-contents"><input type="radio" name="lastTenEVRs" value="row.entity.FLIGHTDETSID"><div>'

                            //},
                            {
                                field: "FLIGHTNUMBER", name: "Flight No. / Aircraft Type", enableHiding: false,
                                cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.ShowViewButtons(row.entity)">{{row.entity.FLIGHTNUMBER}}({{row.entity.AircraftType}})</a><div>'
                            },
                            { field: "SECTOR", name: "sector", enableHiding: false, width: "15%", },
                            {
                                field: "ATD_UTC", displayName: "ATD (UTC)", enableHiding: false, width: "20%", sort: { direction: 'desc' },
                                cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["ATD_UTC"] | date:"dd-MMM-yyyy (HH:mm)" }}</div>'
                            },
                            {
                                field: "ATA_UTC", displayName: "ATA (UTC)", enableHiding: false, width: "20%",
                                cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["ATA_UTC"] | date:"dd-MMM-yyyy (HH:mm)" }}</div>'
                            },
                            { field: "AircraftRegNo", name: "Aircraft Reg. No.", enableHiding: false, width: "15%" },
                            { field: "VRCount", displayName: "eVRs", enableHiding: false, width: "10%" }
                    ]
                };

                $scope.evrtengrid.multiSelect = false;
                $scope.evrtengrid.noUnselect = true;

                //$scope.evrtengrid.onRegisterApi = function (gridApi) {

                //    gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                //        $scope.ShowViewButtons(row.entity);
                //    });
                //};


                loadData();

            }

            function loadData() {

                ipmevrTenBlockUI.start();
                evrSearchService.getLastTenEVRs('', function (result) {

                    $scope.evrLastTen = result;
                    $scope.evrtengrid.data = result;
                    ipmevrTenBlockUI.stop();
                }, function (error) {
                    ipmevrTenBlockUI.stop();
                });
            };

            initialize();

        }]);