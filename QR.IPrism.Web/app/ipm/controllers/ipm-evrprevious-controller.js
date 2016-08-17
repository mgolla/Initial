/*********************************************************************
* File Name     : ipm-evrsearch-controller.js
* Description   : Controller for eVR Search module.
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.evrprevious.controller', ['$scope', '$state', '$stateParams', 'lookupDataService', 'evrSearchService', 'blockUI',
        function ($scope, $state, $stateParams, lookupDataService, evrSearchService, blockUI) {

            var ipmevrSearchfilterBlockUI = blockUI.instances.get('blockui');

            $scope.header = "Last 10 VRs"
            $scope.back = "poasmnt";

            $scope.getDesc = function (rowData) {
                $state.go('evrSearch.view', { evrSubmtdId: rowData.VRId, FlightDetsID: rowData.FlightDetailId, back: "poasmnt" });
            };


            $scope.grid = {
                gridApi: {},
                enableFiltering: true,
                showGridFooter: true,
                paginationPageSizes: [5, 10, 15, 20, 25],
                paginationPageSize: 10,
                enablePagination: true,
                enablePaginationControls: true,
                data: [],
                subgrid: 'false',
                columnDefs: [
                      {
                          field: "VRNo", displayName: "eVR ID", width: "10%", enableHiding: false,
                          cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.getDesc(row.entity)">{{row.entity.VRNo}}</a><div>'
                      },
                      { field: "FlightNumber", name: "Flight Number", width: "15%", enableHiding: false },
                      {
                          field: "ATD_UTC", displayName: "ATD (UTC)", type: 'date', enableHiding: false,
                          width: "10%",
                          cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["ATD_UTC"] | date:"dd-MMM-yyyy" }}</div>'
                      },
                      { field: "Sector", name: "Sector", width: "15%", enableHiding: false },
                      { field: "Status", name: "Status", width: "15%", enableHiding: false },
                      { field: "Department", name: "Department", width: "20%", enableHiding: false },
                      { field: "ReportAbout", name: "Report About", enableHiding: false }
                ]
            };

            ipmevrSearchfilterBlockUI.start();
            evrSearchService.getAssemtSearchEvrs($stateParams.id, function (success) {
                $scope.grid.data = success;
                ipmevrSearchfilterBlockUI.stop();
            }, function (error) {
                ipmevrSearchfilterBlockUI.stop();
            });

        }]);