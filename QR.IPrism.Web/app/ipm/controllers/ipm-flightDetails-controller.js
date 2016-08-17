
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.flightDetails.controller', ['$scope', '$http', '$state', '$rootScope', '$stateParams','sharedDataService',
        'flightDetailsAddEditService', 'blockUI',
        function ($scope, $http, $state, $rootScope, $stateParams,sharedDataService,
            flightDetailsAddEditService, blockUI) {

            $scope.model = {};
            $scope.heading = '';
            var ipmEVRSingleFlightDetBlockUI = blockUI.instances.get('ipmEVRSingleFlightDetBlockUI');

            function loadData() {

                ipmEVRSingleFlightDetBlockUI.start();

                flightDetailsAddEditService.getSingleFlight($stateParams.FlightDetsID, function (success) {

                    $scope.model = success;
                    
                    $scope.model.FlightDelay.IsGroomingCheck = sharedDataService.conditionalCheck(success.FlightDelay.IsGroomingCheck);
                    $scope.model.FlightDelay.IsCsdCsBriefed = sharedDataService.conditionalCheck(success.FlightDelay.IsCsdCsBriefed);


                    $scope.grid.data =  sharedDataService.sortCrewDetails( success.FlightCrewsDetail);
                    ipmEVRSingleFlightDetBlockUI.stop();

                }, function (error) {
                    ipmEVRSingleFlightDetBlockUI.stop();
                });
            }

            var Initialize = function () {

                //if ($state.current.name === 'evrviewState.details') {
                if ($state.current.name === 'evrlstsState.evrviewState' || 
                    $state.current.name === 'evrlstsState.evrtabs' || 
                    $state.current.name === 'evrSearch.view') {
                    $scope.heading = 'Flight Details';
                    $scope.isReadOnly = true;
                } else {
                    $scope.heading = 'Search Flight Details';
                }

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

                loadData();
            }

            Initialize();

        }]);