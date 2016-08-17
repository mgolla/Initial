'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.flightAddEditFilter.controller', ['$scope', '$state', '$rootScope', 'flightDetailsAddEditService', 'flightDelayService', 'lookupDataService', 'messages', 'blockUI', 'toastr', 'ngDialog','sharedDataService',
        function ($scope, $state, $rootScope, flightDetailsAddEditService, flightDelayService, lookupDataService, messages, blockUI, toastr, ngDialog, sharedDataService) {

            var ipmFlightdelayBlockUI = blockUI.instances.get('ipmFlightdelayBlockUI');
            $scope.model = {};

            function initialize() {
                $scope.getDesc = function (data) {
                   
                    $state.go('addEditFlight.details', { FlightDetsID: data.FlightDetsID, IsFromEvr: false, back: 'addEditFlight' });
                };

                $scope.search = function (form) {

                    $scope.model.SectorFrom = $scope.model.SectorFromObj ? $scope.model.SectorFromObj.Text : '';
                    $scope.model.SectorTo = $scope.model.SectorToObj ? $scope.model.SectorToObj.Text : '';
                    $scope.model.FromDate = $scope.model.FromDateObj ? sharedDataService.getDateOnly($scope.model.FromDateObj) : '';
                    $scope.model.FlightNumber = $scope.model.FlightNumberObj ? 'QR' + $scope.model.FlightNumberObj : '';

                    var isFNEntrd = !!$scope.model.FlightNumber;
                    var isFDEntrd = !!$scope.model.FromDate;
                    var isSFEntrd = !!$scope.model.SectorFrom;
                    var isSTEntrd = !!$scope.model.SectorTo;

                    if (isFDEntrd) {
                        if(isFNEntrd)
                        {
                            GetFlightDetails();
                        }
                        else {
                            if(isSFEntrd && isSTEntrd){
                                GetFlightDetails();
                            } else {
                                toastr.warning('Search for (Flight Number and ATD Date) or (ATD Date and Sector From - Sector To).');
                            }

                        }
                    } else {
                        toastr.warning('Enter Flight Number and ATD Date');
                    }

                };

                $scope.clearForm = function (form) {
                    $scope.model = {};
                    GetFlightDetails();
                };

                $scope.grid = {
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
                          {
                              field: "Flight Number", name: "FlightNumber", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.getDesc(row.entity)">{{row.entity.FlightNumber}}</a><div>'
                          },
                          {
                              field: "ScheduledDeptTime", displayName: "STD (UTC)", width: "20%", enableHiding: false,
                              sort: { direction: 'desc' },
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["ScheduledDeptTime"] | date:"dd-MMM-yyyy (HH:mm)" }}</div>'
                          },
                          {
                              field: "ActualDeptTime", displayName: "ATD (UTC)", width: "20%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["ActualDeptTime"] | date:"dd-MMM-yyyy (HH:mm)" }}</div>'
                          },
                          { field: "Sector", name: "Sector", width: "15%", enableHiding: false },
                          { field: "AirCraftRegNo", name: "Aircraft Reg. No.", width: "15%", enableHiding: false },
                          { field: "AirCraftType", name: "Aircraft Type", width: "15%", enableHiding: false }
                    ]
                };

                getSector();
                ipmFlightdelayBlockUI.stop();
            };

            function getSector() {

                lookupDataService.getLookupList('Sector', null, function (result) {
                    $scope.SectorList = result.map(function (obj) {
                        return { Value: obj.Value, Text: obj.Text, Desc: obj.FilterText }
                    });
                });
            };

            function GetFlightDetails() {

                ipmFlightdelayBlockUI.start();
                flightDetailsAddEditService.getFlightDetails($scope.model, function (success) {
                    ipmFlightdelayBlockUI.stop();
                    $scope.grid.data = success.data;
                }, function (error) {
                    ipmFlightdelayBlockUI.stop();
                });
            };

            GetFlightDetails();
            ipmFlightdelayBlockUI.start();
            initialize();
        }]);