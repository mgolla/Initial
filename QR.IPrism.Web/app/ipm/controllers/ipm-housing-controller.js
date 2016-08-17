/*********************************************************************
* File Name     : ipm-housing-controller.js
* Description   : Controller for Housing Request module.
* Create Date   : 25th Jan 2016
* Modified Date : 18th Jul 2016
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.housing.controller', ['$scope', '$state', '$rootScope', 'housingService', 'lookupDataService', 'housing', 'messages', 'blockUI', '$stickyState', 'sharedDataService',
        function ($scope, $state, $rootScope, housingService, lookupDataService, housing, messages, blockUI, $stickyState, sharedDataService) {

            /* Private variables initialization */
            var initialDrpValues = { 'Value': '', 'Text': '--All--' },
                ipmhousingBlockUI = blockUI.instances.get('ipmhousingBlockUI');

            /* Initialize scope variables */
            $scope.housingCurrentState = $state.current.name;
            $scope.requestList = [];
            $scope.statusList = [];

            /* model namespace */
            $scope.housing = {};

            /* Gets Housing request data based on search criteria */
            $scope.search = function () {

                ipmhousingBlockUI.start();

                $scope.housing.RequestType = $scope.housing.RequestTypeObj ?
                    ($scope.housing.RequestTypeObj.Text == '--All--' ? '' : $scope.housing.RequestTypeObj.Text) : '';
                $scope.housing.Status = $scope.housing.StatusObj ? ($scope.housing.StatusObj.Text == '--All--' ? '' : $scope.housing.StatusObj.Text) : '';
                $scope.housing.FromDate = $scope.FromDate ? sharedDataService.getDateOnly($scope.FromDate) : '';
                $scope.housing.ToDate = $scope.ToDate ? sharedDataService.getDateOnly($scope.ToDate) : '';

                getSearchData();
            };

            $scope.reset = function () {

                $scope.housing.RequestTypeObj = $scope.requestList[0];
                $scope.housing.StatusObj = $scope.statusList[0];
                $scope.FromDate = '';
                $scope.ToDate = '';
                $scope.grid.data = [];
                $scope.search();
            };

            $scope.createNewRequest = function () {
                $state.go('housing-create');
            }

            //Controller Scope Initialization
            function initialize() {

                $scope.getDesc = function (row) {
                    switch (row.RequestType.toLocaleLowerCase()) {

                        //HOU1013: "Change Accommodation"  
                        //HOU1016: "Moving In"
                        case messages.HOU1016.toLocaleLowerCase():
                            $state.go('housing.housing-readonly-MovingIn', { RequestNumber: row.RequestNumber, RequestId: row.RequestId });
                            break;

                        case messages.HOU1013.toLocaleLowerCase():
                            $state.go('housing.housing-readonly-ChangeAcc', { RequestNumber: row.RequestNumber, RequestId: row.RequestId });
                            break;

                            //HOU1015: "Guest Accommodation"
                        case messages.HOU1015.toLocaleLowerCase():
                            $state.go('housing.housing-readonly-GuestAcc', { RequestNumber: row.RequestNumber, RequestId: row.RequestId });
                            break;

                            //HOU1017: "Moving Out of Company Accommodation"
                        case messages.HOU1017.toLocaleLowerCase():
                            $state.go('housing.housing-readonly-MoveOut', { RequestNumber: row.RequestNumber, RequestId: row.RequestId });
                            break;

                            //HOU1018: "Stay Out Request"
                        case messages.HOU1018.toLocaleLowerCase():
                            $state.go('housing.housingRdStayOut', { RequestNumber: row.RequestNumber, RequestId: row.RequestId });
                            break;

                            //HOU1019: "Swap Rooms"
                        case messages.HOU1019.toLocaleLowerCase():
                            $state.go('housing.housing-readonly-SwapRoom', { RequestNumber: row.RequestNumber, RequestId: row.RequestId });
                            break;
                    }
                }

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
                              field: "RequestNumber", name: "Request No", width: "10%", enableHiding: false, sort: { direction: 'desc' }
                          },
                          { field: "RequestType", name: "RequestType", width: "20%", enableHiding: false },
                          {
                              field: "Description", name: "Description", enableHiding: false,
                              cellTemplate: '<div title="{{row.entity.Description}}" class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.getDesc(row.entity)">{{row.entity.Description}}</a><div>'
                          },
                          {
                              field: "RequestDate", name: "Request Date", type: 'date', enableHiding: false, width: "10%",
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["RequestDate"] | date:"dd-MMM-yyyy" }}</div>'
                          },
                          { field: "RequestStatus", name: "Status", width: "15%", enableHiding: false },
                          {
                              field: "CloseDate", name: "Pre Close Date", type: 'date', enableHiding: false, width: "15%",
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["RequestDateClose"]  | date:"dd-MMM-yyyy" }}</div>'
                          }
                    ]
                };

                ipmhousingBlockUI.start();
                loadData();
                getSearchData();

            }

            function loadData() {

                lookupDataService.getLookupList('HousingSearchRequestCode', null, function (result) {

                    $scope.requestList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });
                    $scope.requestList.unshift(initialDrpValues);

                    $scope.housing.RequestTypeObj = $scope.requestList[0];
                });

                lookupDataService.getLookupList('HousingSearchStatus', null, function (result) {

                    $scope.statusList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });
                    $scope.statusList.unshift(initialDrpValues);

                    $scope.housing.StatusObj = $scope.statusList[0];
                });

                ipmhousingBlockUI.stop();
            }

            function getSearchData() {
                //$stickyState.reset("housing");
                housingService.getHousingSearchResult($scope.housing, function (success) {
                    $scope.grid.data = success.data;
                    ipmhousingBlockUI.stop();
                }, function (error) {
                    ipmhousingBlockUI.stop();
                }, null);
            }

            initialize();
        }]);