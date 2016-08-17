'use strict'
angular.module('app.ipm.module').controller('ipm.searchTransport.controller', ['$scope', '$rootScope', '$http', '$state', '$stateParams',
    'lookupDataService', 'searchService', 'blockUI', '$filter', '$stickyState',
    function ($scope, $rootScope, $http, $state, $stateParams, lookupDataService, searchService, blockUI, $filter, $stickyState) {

        var ipmtransportBlockUI = blockUI.instances.get('ipmtransportBlockUI');

        function Initialize() {

            $scope.transportSearch = {};

            $scope.grid = {
                gridApi: {},
                //enableFiltering: true,
                //showGridFooter: true,
                enableColumnResizing: true,
                //paginationPageSizes: [5, 10, 15, 20, 25],
                //paginationPageSize: 10,
                //enablePagination: true,
                //enablePaginationControls: true,
                data: [],
                subgrid: 'false',
                columnDefs: [
                    { field: "FlightNumber", name: "Flight Number", enableHiding: false },
                    {
                         field: "ReportingTime", name: "ReportingTime", enableHiding: false,
                         cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["ReportingTime"] | date:"dd-MMM-yyyy HH:mm a" }}</a><div>'
                    },
                    {
                         field: "PickupTime", name: "Pickup Time", enableHiding: false,
                         cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["PickupTime"] | date:"dd-MMM-yyyy HH:mm a" }}</a><div>'
                    },
                    { field: "PickupLocation", name: "Pickup Location", enableHiding: false },
                    { field: "TripId", name: "Trip Id", enableHiding: false },
                    { field: "", name: "Trip Start Time", enableHiding: false },
                    { field: "", name: "Remarks", enableHiding: false }
                ]
            };

            setDate();
            loadCurrencyDetailList();
        }

        function setDate() {
            var currentDate = new Date();
            currentDate.setDate(currentDate.getDate() + 1);
            $scope.transportSearch.ToDate = angular.copy(currentDate);
            currentDate.setDate(currentDate.getDate() - 2);
            $scope.transportSearch.FromDate = currentDate;
        };

        //$scope.Reset = function () {
        //    setDate();
        //    loadCurrencyDetailList();
        //};


        //$scope.onClickTransportSearch = function () {

        //    loadCurrencyDetailList();
        //};

        //(function displayUTC() {
        //    var getHeight = $(window).height() - 130;
        //    //$('.content_wrapper').css({ 'min-height': getHeight + 'px' });
        //    $('.content_wrapper').css('min-height', getHeight + 'px');
        //})();


        function loadCurrencyDetailList() {
            var filter = angular.copy($scope.transportSearch);
            if ($scope.transportSearch) {
                //if ($scope.transportSearch.FromDate.toString().length > 0) {
                //    filter.FromDate = $scope.transportSearch.FromDate.toLocaleDateString();
                //}
                //if ($scope.transportSearch.ToDate.toString().length >0) {
                //    filter.ToDate = $scope.transportSearch.ToDate.toLocaleDateString();
                //}
            }

            ipmtransportBlockUI.start();
            searchService.getTransports(filter, function (result) {
                $scope.grid.data = result.data;
                ipmtransportBlockUI.stop();
            },
            function (error) {
                ipmtransportBlockUI.stop();
            });
        }

        Initialize();
    }]);

