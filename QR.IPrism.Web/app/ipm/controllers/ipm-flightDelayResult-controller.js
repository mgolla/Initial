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
    .controller('ipm.flightdelayresult.controller', ['$scope', '$state', '$stateParams', '$rootScope', 'flightDelayService', 'lookupDataService', 'messages', 'blockUI', 'toastr','sharedDataService',
        function ($scope, $state, $stateParams, $rootScope, flightDelayService, lookupDataService, messages, blockUI, toastr, sharedDataService) {

            $scope.searchData = {};

            function getDelayResult() {
                flightDelayService.getDelaySearchResults({}, function (success) {

                    angular.forEach(success.data, function (data) {
                        if (data.FlightDelayRptId == $stateParams.FlightDelayRptId) {

                            data.depDelayMin = Math.floor((new Date(data.ActualDeptTime).getTime() - new Date(data.ScheduledDeptTime).getTime()) / 3600000);
                            data.depDelaySec = Math.floor(((new Date(data.ActualDeptTime).getTime() - new Date(data.ScheduledDeptTime).getTime()) / 60000) - (60 * data.depDelayMin));
                            data.arrDelayMin = Math.floor((new Date(data.ActualArrTime).getTime() - new Date(data.ScheduledArrTime).getTime()) / 3600000);
                            data.arrDelaySec = Math.floor(((new Date(data.ActualArrTime).getTime() - new Date(data.ScheduledArrTime).getTime()) / 60000) - (60 * data.arrDelayMin));

                            if (parseInt(data.depDelayMin) > -1) {
                                data.departureDelay = (data.depDelayMin.toString().length > 1 ? data.depDelayMin : "0" + data.depDelayMin) + ':' +
                                (data.depDelaySec.toString().length > 1 ? data.depDelaySec : "0" + data.depDelaySec)
                            } else {
                                data.departureDelay = "00:00";
                            }

                            if (parseInt(data.arrDelayMin) > -1 ) {
                                data.arrivalDelay = (data.arrDelayMin.toString().length > 1 ? data.arrDelayMin : "0" + data.arrDelayMin) + ':' +
                                 (data.arrDelaySec.toString().length > 1 ? data.arrDelaySec : "0" + data.arrDelaySec)
                            }else {
                                data.arrivalDelay =  "00:00";
                            }
                           
                            //data.depDelayMin = data.depDelayMin.length > 1 ? data.depDelayMin : "0" + data.depDelayMin;
                            //data.depDelaySec = data.depDelaySec.length > 1 ? data.depDelaySec : "0" + data.depDelaySec;
                            //data.arrDelayMin = data.arrDelayMin.length > 1 ? data.arrDelayMin : "0" + data.arrDelayMin;
                            //data.arrDelaySec = data.arrDelaySec.length > 1 ? data.arrDelaySec : "0" + data.arrDelaySec;

                            $scope.searchData = data;
                            $scope.grid.data = sharedDataService.sortCrewDetails(data.FlightCrewsDetail);

                            $scope.searchData.FlightDelay.IsGroomingCheck = sharedDataService.conditionalCheck(data.FlightDelay.IsGroomingCheck);
                            $scope.searchData.FlightDelay.IsCsdCsBriefed = sharedDataService.conditionalCheck(data.FlightDelay.IsCsdCsBriefed);

                        }
                    });
                }, function (error) {

                }, null, false);
            }

            function initialize() {

                //$scope.goBack = function () {
                //    $state.go('searchDelayFlight');
                //};

                $scope.grid = {
                    gridApi: {},
                    showGridFooter: true,
                    paginationPageSizes: [5, 10, 15, 20, 25],
                    paginationPageSize: 10,
                    enablePagination: true,
                    enablePaginationControls: true,
                    data: [],
                    subgrid: 'false',
                    columnDefs: [
                         { field: "StaffNumber", name: "Staff Number", width: "15%", enableHiding: false },
                         { field: "StaffName", name: "Staff Name", enableHiding: false },
                         { field: "StaffGrade", name: "Staff Grade", width: "15%", enableHiding: false },
                         { field: "CabinCrewPosition", name: "Crew Work Position", width: "15%", enableHiding: false },
                         { field: "AnnounceLang", name: "Announcement Language", width: "15%", enableHiding: false }
                    ]
                };

                getDelayResult();
            }

            initialize();

        }]);