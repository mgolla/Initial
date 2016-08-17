
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.evrsubmitted.controller', ['$scope', '$http', '$state', '$rootScope', '$stateParams',
        'flightDetailsAddEditService', 'evrSearchService', 'blockUI',
        function ($scope, $http, $state, $rootScope, $stateParams,
            flightDetailsAddEditService, evrSearchService, blockUI) {

            var ipmEVRSubmittedBlockUI = blockUI.instances.get('ipmEVRSubmittedBlockUI');

            var Initialize = function () {

                

                $scope.getDesc = function (rowData) {
                    $state.go('evrlstsState.evrviewState.view', { evrSubmtdId: rowData.VRId, back: 'evrlstsState.evrviewState' });
                };

                $scope.evrDraftGrid = {
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
                              field: "VRNo", displayName: "eVR Id", width: "20%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="javascript:void(0);" ng-click="grid.appScope.getDesc(row.entity)">{{row.entity.VRNo}}</a><div>'
                          },
                          { field: "ReportAbtName", name: "Report About", width: "20%", enableHiding: false },
                          { field: "CategoryName", name: "Category Name", width: "20%", enableHiding: false },
                          { field: "SubCategoryName", name: "SubCategoryName", width: "20%", enableHiding: false },
                          { field: "VRMasterStatusName", name: "Status", enableHiding: false }
                    ]
                };

                //$scope.evrDraftGrid.onRegisterApi = function (gridApi) {

                //    gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                //        $state.go('evrlstsState.evrviewState.view', { evrSubmtdId: row.entity.VRId, back: 'evrlstsState.evrviewState' });
                //    });
                //};

                loadData();
            }

            function loadData() {

                ipmEVRSubmittedBlockUI.start();

                evrSearchService.getSubmittedEVRs($stateParams.FlightDetsID, function (result) {
                    

                    $scope.evrDraftGrid.data = result;

                    ipmEVRSubmittedBlockUI.stop();
                },
                function (error) {
                    ipmEVRSubmittedBlockUI.stop();
                });
            }

            Initialize();

        }]);