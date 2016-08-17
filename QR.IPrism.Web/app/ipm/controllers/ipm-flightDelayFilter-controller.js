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
    .controller('ipm.flightdelayfilter.controller', ['$scope', '$state', '$rootScope', 'flightDelayService', 'lookupDataService', 'messages',
        'blockUI', 'toastr','sharedDataService',
        function ($scope, $state, $rootScope, flightDelayService, lookupDataService, messages, blockUI, toastr, sharedDataService) {

            var ipmFlightdelayBlockUI = blockUI.instances.get('ipmFlightdelayBlockUI');

            $scope.DelayTypeList = [];
            $scope.DelayTypeReason = [];
            $scope.SectorList = [];

            $scope.model = {};

            function setDate() {
                var currentDate = new Date();
                $scope.model.ToDateObj = angular.copy(currentDate);

                currentDate.setMonth(currentDate.getMonth() - 1);
                $scope.model.FromDateObj = currentDate;
            };

            function initialize() {

                setDate();

                $scope.getDesc = function (data) {

                    $state.go('searchDelayFlight.data', { FlightDelayRptId: data.FlightDelayRptId });
                };

                $scope.search = function (form) {

                    $scope.model.SectorTo = $scope.model.SectorToObj ? $scope.model.SectorToObj.Value : '';
                    $scope.model.SectorFrom = $scope.model.SectorFromObj ? $scope.model.SectorFromObj.Value : '';

                    //$scope.model.DelayReason = $scope.model.DelayReasonObj ? $scope.model.DelayReasonObj.Value : '';
                    $scope.model.DelayReason = '';
                    if ($scope.model.DelayReasonObj)
                    {

                        angular.forEach($scope.model.DelayReasonObj, function (value, key) {
                            if (($scope.model.DelayReason == '' || 
                                 $scope.model.DelayReason == undefined)) {
                                $scope.model.DelayReason = value.Value + ",";
                            } else {
                                $scope.model.DelayReason += value.Value + ",";
                            }
                        });

                        $scope.model.DelayReason = $scope.model.DelayReason.substr(0, $scope.model.DelayReason.length - 1);
                    } else {
                        $scope.model.DelayReason = '';
                    }

                    $scope.model.DelayType = ($scope.model.DelayTypeObj && $scope.model.DelayTypeObj.Text.toUpperCase() !== '-- ALL --') ? $scope.model.DelayTypeObj.Value : '';
                    $scope.model.FromDate = $scope.model.FromDateObj ? sharedDataService.getDateOnly($scope.model.FromDateObj) : '';
                    $scope.model.ToDate = $scope.model.ToDateObj ? sharedDataService.getDateOnly($scope.model.ToDateObj) : '';

                    ipmFlightdelayBlockUI.start();

                    flightDelayService.getDelaySearchResults($scope.model, function (success) {

                        ipmFlightdelayBlockUI.stop();
                        $scope.grid.data = success.data;

                    }, function (error) {
                        ipmFlightdelayBlockUI.stop();
                        toastr.error(messages.HOUERROR01);
                    }, null, true);
                };

                $scope.clearForm = function (form) {

                    $scope.model = {};
                    setDate();
                    $scope.DelayTypeList.selected = $scope.model.DelayTypeObj = $scope.DelayTypeList[0];

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
                              field: "Flight Number", name: "FlightNumber", width: "13%", enableHiding: false, sort: { direction: 'desc' },
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.getDesc(row.entity)">{{row.entity.FlightNumber}}</a><div>'
                          },
                          {
                              field: "ActualDeptTime", displayName: "ATD (UTC)", width: "15%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["ActualDeptTime"] | date:"dd-MMM-yyyy HH:mm" }}</div>'
                          },
                          { field: "Sector", name: "Sector", width: "10%", enableHiding: false },
                          {
                              field: "CreatedDate", name: "Received Date", type: 'date', width: "13%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["CreatedDate"] | date:"dd-MMM-yyyy HH:mm" }}</div>'
                          },
                          { field: "FlightDelay.FlightDelayCatName", name: "Delay Reasons", enableHiding: false, width: "12%" },
                          { field: "FlightDelay.DelayType", name: "Delay Type", enableHiding: false, width: "10%" },
                          { field: "FlightDelay.DelayComment", name: "Comments", enableHiding: false },
                    ]
                };

                lookupDataService.getLookupList('Sector', null, function (result) {
                    $scope.SectorList = result.map(function (obj) {
                        return { Value: obj.Value, Text: obj.Text, Desc: obj.FilterText }
                    });
                });

                //flightDelayService.getsectordetails(null, function (result) {

                //    $scope.SectorList = result.map(function (obj) {
                //        return { Value: obj.Value, Text: obj.Text, Desc: obj.FilterText }
                //    });

                //}, function (error) {
                //    ipmFlightdelayBlockUI.stop();
                //    toastr.error(messages.HOUERROR01);
                //});

                flightDelayService.getdelaylookupvalues(null, function (result) {

                    var delayType = [];
                    angular.forEach(result, function (obj) {

                        if (obj.DelayId) {
                            delayType.push({ Value: obj.DelayId, Text: obj.DelayType });
                        }
                    });

                    $scope.DelayTypeList = delayType;

                    $scope.DelayTypeList.selected = $scope.model.DelayTypeObj = $scope.DelayTypeList[0];

                    
                    var delayReason = [];
                    angular.forEach(result, function (obj) {

                        if (obj.FlightDelayCatId) {
                            delayReason.push({ Value: obj.FlightDelayCatId, Text: obj.FlightDelayCatName });
                        }
                    });

                    $scope.DelayTypeReason = delayReason;

                }, function (error) {
                    ipmFlightdelayBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });

                ipmFlightdelayBlockUI.stop();
            };

            ipmFlightdelayBlockUI.start();
            initialize();
        }]);