'use strict'
angular.module('app.ipm.module').controller('ipm.searchStationInfo.controller',

['$scope', '$rootScope', '$http', '$state', '$stateParams', 'lookupDataService', 'searchService', 'blockUI', '$filter', '$stickyState',
function ($scope, $rootScope, $http, $state, $stateParams, lookupDataService, searchService, blockUI, $filter, $stickyState) {
   
    var Initialize = function () {

        $scope.stationInfoSearch = {

            City: ''
        };

       
        $scope.searchViewModel;
        $scope.CitiesAll = [];
        searchService.getAllCities($scope);
        //var para = $stateParams.filter;
        //if ($stateParams.filter && $stateParams.filter.City) {

        //    if ($stateParams.filter.City && $stateParams.filter.City != null && $stateParams.filter.City.Text && $stateParams.filter.City.Text != '') {
        //        $scope.stationInfoSearch.City = $stateParams.filter.City;
        //        $scope.onClickSearch();
        //    }
        //}
    }


    $scope.onClickSearch = function () {

        $scope.stationInfoFlter = {
            City: ''
        };

        if ($scope.stationInfoSearch && $scope.stationInfoSearch.City) {

            if ($scope.stationInfoSearch && $scope.stationInfoSearch.City != null && $scope.stationInfoSearch.City.Text && $scope.stationInfoSearch.City.Text!='') {
                $scope.stationInfoFlter.City = $scope.stationInfoSearch.City.Text;
                $state.go('ipsi.stations', { filter: $scope.stationInfoFlter });
            }
        }
    }

    Initialize();
}]);

