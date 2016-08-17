'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.evrdraft.controller', ['$scope', '$state', '$stateParams', '$rootScope', 'lookupDataService', 'evrSearchService',
        'blockUI', 'messages','ngDialog','$timeout',
        function ($scope, $state, $stateParams, $rootScope, lookupDataService, evrSearchService, blockUI, messages, ngDialog, $timeout) {

            var ipmEVRDraftBlockUI = blockUI.instances.get('ipmEVRDraftBlockUI');

            $scope.VRIdModel = {};

            function loadData(isFrom) {
                ipmEVRDraftBlockUI.start();
                evrSearchService.getevrDraftForUser($stateParams.FlightDetsID, function (result) {
                    ipmEVRDraftBlockUI.stop();
                    $scope.evrDraftGrid.data = result;

                    if (isFrom.toUpperCase() == 'FROMDELETEVR') {
                        $state.go("evrtabs.evrDraft", { isfrom: 'DELDRFT', evrdrfid: null, evrno: null });
                    } else if (isFrom.toUpperCase() == 'FROMINITIALIZE') {

                    }
                },
                function (error) {
                    ipmEVRDraftBlockUI.stop();
                });
            };

            function submitData(row) {
                evrSearchService.DeleteVR(row.VRId, function (result) {
                    loadData('fromDeleteVR');
                }, function (error) {
                });
            }

            $scope.AdjustGridHeight = function () {
                var rowHeight = 30;
                var headerHeight = 30;
                return {
                    height: ($scope.evrDraftGrid.data.length * rowHeight + headerHeight) + "px"
                };
            };

            function initialize() {

                $scope.getDesc = function (data) {
                    var flightId = $scope.evrTabModel.evrReqFeilds.flightDetsID;
                    $scope.evrTabModel.evrReqFeilds = null;
                    $timeout(function () {
                        $scope.model.active['evrForm'] = true;
                        $scope.evrTabModel.evrReqFeilds = {
                            flightDetsID: flightId,
                            evrdrfid: data.VRId,
                            evrno: data.VRNo,
                            evrInstanceId: data.VRInstaceId
                        };
                    }, 1);
                    //$state.go("evrtabs.evrForm", { evrdrfid: data.VRId, evrno: data.VRNo });
                };

                $scope.deleteEVR = function (row) {
                    $scope.dialogTitle = "Confirmation";
                    $scope.dialogMessage = messages.EVRDRAFTDELETE;
                    ngDialog.open({
                        scope: $scope,
                        preCloseCallback: function (value) {
                            if (value == 'Post') {
                                submitData(row);
                            }
                        }
                    });
                }

                $scope.evrDraftGrid = {
                    gridApi: {},
                    enableFiltering: true,
                    //enableRowSelection: true,
                    //enableFullRowSelection: true,
                    showGridFooter: true,
                    paginationPageSizes: [5, 10, 15, 20, 25],
                    //enableSelectAll: false,
                    multiSelect: false,
                    paginationPageSize: 10,
                    enablePagination: true,
                    enablePaginationControls: true,
                    data: [],
                    subgrid: 'false',
                    //rowHeight: 30,
                    columnDefs: [
                          {
                              field: "VRNo", name: "eVR ID", width: "20%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.getDesc(row.entity)">{{row.entity.VRNo}}</a><div>'
                          },
                          { field: "ReportAbtName", name: "Report About", width: "20%", enableHiding: false },
                          { field: "CategoryName", name: "Category Name", width: "20%", enableHiding: false },
                          { field: "SubCategoryName", name: "SubCategoryName", width: "20%", enableHiding: false },
                          { field: "VRMasterStatusName", name: "Status", width: "10%", enableHiding: false },
                          {
                              field: "remove", name: "Remove", width: "10%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents text-center"><a href="javascript:void(0);" ng-click="grid.appScope.deleteEVR(row.entity)"><i class="glyphicon glyphicon-trash redText"></i></a></div>',
                          }
                    ]
                };

                loadData('fromInitialize');
            };

            initialize();
        }]);