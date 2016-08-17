'use strict'
angular.module('app.ipm.module').controller('ipm.searchWeather.controller',

['$scope', '$rootScope', '$http', '$state', '$stateParams', 'lookupDataService', 'searchService', 'blockUI', '$filter', '$stickyState',
function ($scope, $rootScope, $http, $state, $stateParams, lookupDataService, searchService, blockUI, $filter, $stickyState) {
   
    var Initialize = function () {

        $scope.filter = {

            FromDate: '',
            ToDate: '',
            AirportCode: '',
            City: '',
            Country: ''
        };
        $scope.CitiesAll = [];
        $scope.CountryList = [];
        $scope.AirportCodeList = [];

        searchService.getAllCities( $scope);
        searchService.getAllCountry($scope);
        searchService.getAllAirportCodes($scope);
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

