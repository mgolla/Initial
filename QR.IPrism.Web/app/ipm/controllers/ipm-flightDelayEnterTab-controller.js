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
    .controller('ipm.flightdelayentertab.controller', ['$scope', '$stateParams', '$state', '$rootScope', 'flightDelayService', 'lookupDataService',
        'messages', 'blockUI', 'toastr', 'ngDialog','sharedDataService',
        function ($scope, $stateParams, $state, $rootScope, flightDelayService, lookupDataService, messages, blockUI, toastr, ngDialog, sharedDataService) {

            var ipmFlightDelayTabBlockUI = blockUI.instances.get('ipmFlightDelayTabBlockUI');

            $scope.model = {};
            $scope.ArrivalDelayReason = [];
            $scope.DepartureDelayReason = [];
            $scope.submitted = false;

            $scope.submittedData = [];
            $scope.showDeptDelay = false;
            $scope.showArrDelay = false;
            $scope.readonly = true;
            $scope.inputs = {};

            function getEnterFlightDelayResults() {

                // Set all input parameters
                $scope.inputs.FlightNumber = $stateParams.FlightDetsID;
                $scope.inputs.SectorFrom = $stateParams.SectorFrom;
                $scope.inputs.SectorTo = $stateParams.SectorTo;
                $scope.inputs.FromDate = $stateParams.ATD;

                flightDelayService.getEnterFlightDelayResults($scope.inputs, function (success) {

                    angular.forEach(success.data, function (data) {
                        if (data.FlightDetsID == $stateParams.FlightDetsID) {
                            $scope.model = data;
                            $scope.grid.data = sharedDataService.sortCrewDetails(data.FlightCrewsDetail);

                            SetCrewData()
                            //$state.go('enterFlightDelaytab.details');
                            
                            $scope.model.FlightDelay.IsGroomingCheck = sharedDataService.conditionalCheck(data.FlightDelay.IsGroomingCheck);
                            $scope.model.FlightDelay.IsCsdCsBriefed = sharedDataService.conditionalCheck(data.FlightDelay.IsCsdCsBriefed);
                        }
                    });
                    ipmFlightDelayTabBlockUI.stop();
                }, function (error) {
                    ipmFlightDelayTabBlockUI.stop();
                }, null, false);
            }



            function SetCrewData() {
                $scope.onFlgDlyTabChange = function (txt) {

                    switch (txt) {
                        case 'DelayCause':
                            $state.go('enterFlightDelay.enterFlightDelaytab.cause');
                            break;
                        default:
                            $state.go('enterFlightDelay.enterFlightDelaytab.details');
                            break;
                    }
                };
            }

            //ipmFlightDelayTabBlockUI.start();
            function initialize() {

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
                         { field: "StaffNumber", name: "Staff Number", width: "15%", enableHiding: false },
                         { field: "StaffName", name: "Staff Name", enableHiding: false },
                         { field: "StaffGrade", name: "Staff Grade", width: "15%", enableHiding: false },
                         { field: "CabinCrewPosition", name: "Crew Work Position", width: "15%", enableHiding: false },
                         { field: "AnnounceLang", name: "Announcement Language", width: "15%", enableHiding: false }
                    ]
                };

                getEnterFlightDelayResults();
            }

            //$state.go('enterFlightDelaytab.details');

            initialize();

        }]);