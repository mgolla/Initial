/*********************************************************************
* File Name     : ipm-evrsearch-controller.js
* Description   : Controller for eVR Search module.
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.evrsearch.controller', ['$scope', '$state', '$rootScope', 'lookupDataService', 'evrSearchService', 'blockUI','sharedDataService',
        function ($scope, $state, $rootScope, lookupDataService, evrSearchService, blockUI, sharedDataService) {

            // Private variables initialization

            var initialDrpValues = { 'Value': '', 'Text': '--All--' };
            var initialSectorFromDrpValues = { 'Value': '', 'Text': '--All--' };
            var initialSectorToDrpValues = { 'Value': '', 'Text': '--All--' };
            var ipmevrSearchfilterBlockUI = blockUI.instances.get('ipmevrSearchfilterBlockUI');

            // Initialize scope variables
            $scope.VRStatusList = [];
            $scope.SectorAllFromList = [];
            $scope.SectorAllToList = [];
            $scope.evrAllFilter = {};
            //$scope.evrAllSectorsFromRes = {};
            //$scope.evrAllSectorsToRes = {};

            function setDate() {
                var currentDate = new Date();
                $scope.evrAllFilter.EvrSearchToDateObj  = angular.copy(currentDate);

                currentDate.setMonth(currentDate.getMonth() - 1);
                $scope.evrAllFilter.EvrSearchFromDateObj = currentDate;
            };

            //Controller Scope Initialization
            function initialize() {
                //ipmevrsearchResBlockUI.start();

                setDate();

                $scope.getDesc = function (rowData) {
                    $state.go('evrSearch.view', { evrSubmtdId: rowData.VRId, FlightDetsID: rowData.FlightDetailId, back: 'evrSearch' });
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
                              field: "VRNo", displayName: "eVR ID",enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.getDesc(row.entity)">{{row.entity.VRNo}}</a><div>'
                          },
                          { field: "FlightNumber", name: "Flight Number", width: "15%", enableHiding: false },
                          {
                              field: "ATD_UTC", displayName: "ATD (UTC)", width: "15%", type: 'date', enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["ATD_UTC"] | date:"dd-MMM-yyyy HH:mm" }}</div>'
                          },
                          { field: "Sector", name: "Sector", width: "13%", enableHiding: false },
                          { field: "Status", name: "Status", width: "13%", enableHiding: false },
                          { field: "Department", name: "Department", width: "15%", enableHiding: false },
                          { field: "ReportAbout", name: "Report About", width: "15%", enableHiding: false }
                    ]
                };

                loadData();
            }

            function loadData() {

                lookupDataService.getLookupList('EVRAllStatus', null, function (result) {

                    $scope.VRStatusList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text }});
                    $scope.VRStatusList.unshift(initialDrpValues);

                    $scope.VRStatusList.selected = $scope.evrAllFilter.StatusObj = $scope.VRStatusList[0];
                    $scope.evrAllFilter.Status = $scope.VRStatusList[0];

                });

                lookupDataService.getLookupList('Sector', $scope.Text, function (result) {

                    $scope.SectorAllFromList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text, Desc: obj.FilterText  } });
                    //$scope.SectorAllFromList.unshift(initialSectorFromDrpValues);
                    //$scope.evrAllFilter.SectorAllFromObj = $scope.SectorAllFromList[0];

                    $scope.SectorAllToList = angular.copy($scope.SectorAllFromList);
                    // result.map(function (obj) { return { Value: obj.Value, Text: obj.Text, Desc: obj.FilterText  } });
                    //$scope.SectorAllToList.unshift(initialSectorToDrpValues);
                    //$scope.evrAllFilter.SectorAllToObj = $scope.SectorAllToList[0];

                });
            }

           

            $scope.getAllSectors = function () {

                //if ($scope.SectorAllList == null || $scope.SectorAllList == 'undefined' || $scope.SectorAllList.length == 0) {

                //}
            };

            $scope.search = function (item) {

                //$scope.evrAllFilter.RequestType = $scope.evrAllFilter.RequestTypeObj ? $scope.evrAllFilter.RequestTypeObj.Value : '';
                $scope.evrAllFilter.EvrSearchFromDate = $scope.evrAllFilter.EvrSearchFromDateObj ? sharedDataService.getDateOnly($scope.evrAllFilter.EvrSearchFromDateObj) : '';
                $scope.evrAllFilter.EvrSearchToDate = $scope.evrAllFilter.EvrSearchToDateObj ? sharedDataService.getDateOnly($scope.evrAllFilter.EvrSearchToDateObj) : '';
                $scope.evrAllFilter.SectorFrom = $scope.evrAllFilter.SectorFromObj ? $scope.evrAllFilter.SectorFromObj.Value : '';
                $scope.evrAllFilter.SectorTo = $scope.evrAllFilter.SectorToObj ? $scope.evrAllFilter.SectorToObj.Value : '';
                $scope.evrAllFilter.Status = $scope.evrAllFilter.StatusObj ? $scope.evrAllFilter.StatusObj.Value : '';
                $scope.evrAllFilter.evrId = $scope.evrAllFilter.evrIdObj ? $scope.evrAllFilter.evrIdObj : '';
                $scope.evrAllFilter.FlightNumber = $scope.evrAllFilter.FlightNumber ? $scope.evrAllFilter.FlightNumber : '';

                getSearchData();
            };

            $scope.ResetEVRSearch = function () {

                

                //$scope.VRStatusList = [];
                //$scope.SectorAllFromList = [];
                //$scope.SectorAllToList = [];
                $scope.evrAllFilter = {};

                setDate();
                $scope.VRStatusList.selected = $scope.evrAllFilter.StatusObj = $scope.VRStatusList[0];
                $scope.evrAllFilter.Status = $scope.VRStatusList[0];
            };

            function getSearchData() {

                ipmevrSearchfilterBlockUI.start();
                evrSearchService.getevrSearchResult($scope.evrAllFilter, function (success) {
                    $scope.grid.data = [];
                    $scope.grid.data = success.data.reverse();
                    ipmevrSearchfilterBlockUI.stop();
                }, function (error) {
                    ipmevrSearchfilterBlockUI.stop();
                });
            };

            initialize();
        }]);