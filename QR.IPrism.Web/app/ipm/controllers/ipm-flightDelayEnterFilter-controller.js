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
    .controller('ipm.flightdelayenterfilter.controller', ['$scope', '$state', '$rootScope', 'flightDelayService', 'lookupDataService', 'messages', 'blockUI', 'toastr','sharedDataService',
        function ($scope, $state, $rootScope, flightDelayService, lookupDataService, messages, blockUI, toastr, sharedDataService) {

            var ipmFlightdelayBlockUI = blockUI.instances.get('ipmFlightdelayBlockUI');

            $scope.SectorList = [];
            $scope.model = {};
          
            function initialize() {

                $scope.getDesc = function (data) {

                    $state.go('enterFlightDelay.enterFlightDelaytab', { FlightDetsID: data.FlightDetsID });
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
                              field: "Flight Number", name: "Flight # / Aircraft Type", enableHiding: false,
                              sort: { direction: 'desc' },
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.getDesc(row.entity)">{{row.entity.FlightNumber}}</a><div>'
                          },
                          {
                              field: "ScheduledDeptTime", displayName: "STD (UTC)", width: "15%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["ActualDeptTime"] | date:"dd-MMM-yyyy HH:mm" }}</div>'
                          },
                          {
                              field: "ActualDeptTime", displayName: "ATD (UTC)", width: "15%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["ActualDeptTime"] | date:"dd-MMM-yyyy HH:mm" }}</div>'
                          },
                          { field: "Sector", name: "Sector", width: "15%", enableHiding: false },
                          { field: "AirCraftRegNo", name: "Aircraft Reg No.", width: "15%", enableHiding: false },
                          { field: "DelayReportStatus", name: "Delay Report Status", width: "18%", enableHiding: false }
                    ]
                };

                getSector();

                $scope.clearForm = function () {
                    $scope.model = {};
                };

                $scope.search = function (form) {

                    $scope.model.SectorTo = $scope.model.SectorToObj ? $scope.model.SectorToObj.Value : '';
                    $scope.model.SectorFrom = $scope.model.SectorFromObj ? $scope.model.SectorFromObj.Value : '';
                    $scope.model.FromDate = $scope.model.FromDateObj ? sharedDataService.getDateOnly($scope.model.FromDateObj) : '';

                    var isFNEntrd = !!$scope.model.FlightNumber;
                    var isFDEntrd = !!$scope.model.FromDate;
                    var isSFEntrd = !!$scope.model.SectorFrom;
                    var isSTEntrd = !!$scope.model.SectorTo;

                    if (isFDEntrd) {
                        if (isFNEntrd) {
                            GetFlightDelayResults();
                        }
                        else {
                            if (isSFEntrd && isSTEntrd) {
                                GetFlightDelayResults();
                            } else {
                                toastr.warning(messages.FLIGHTDELAY01);
                            }

                        }
                    } else {
                        toastr.warning(messages.FLIGHTDELAY02);
                    }
                };

                ipmFlightdelayBlockUI.stop();
            };

            function GetFlightDelayResults() {
                ipmFlightdelayBlockUI.start();
                flightDelayService.getEnterFlightDelayResults($scope.model, function (success) {

                    $scope.grid.data = success.data;
                    ipmFlightdelayBlockUI.stop();
                }, function (error) {
                    ipmFlightdelayBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                }, null, true);
            };

            function getSector() {
                //flightDelayService.getsectordetails(null, function (result) {

                //    $scope.SectorList = result.map(function (obj) {
                //        return { Value: obj.Value, Text: obj.Text , Desc : obj.FilterText }
                //    });
                //}, function (error) {
                //    ipmFlightdelayBlockUI.stop();
                //    toastr.error(messages.HOUERROR01);
                //});

                lookupDataService.getLookupList('Sector', null, function (result) {
                    $scope.SectorList = result.map(function (obj) {
                        return { Value: obj.Value, Text: obj.Text, Desc: obj.FilterText }
                    });
                });

            };
           
            ipmFlightdelayBlockUI.start();
            initialize();
        }]);