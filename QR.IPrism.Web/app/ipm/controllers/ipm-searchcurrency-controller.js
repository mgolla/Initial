'use strict'
angular.module('app.ipm.module').controller('ipm.searchCurrency.controller',

['$scope', '$rootScope', '$http', '$state', '$stateParams', 'lookupDataService', 'searchService', 'blockUI', '$filter', '$stickyState',
function ($scope, $rootScope, $http, $state, $stateParams, lookupDataService, searchService, blockUI, $filter, $stickyState) {
   
    var Initialize = function () {

        $scope.filter = {

            CurrencyCode: '',
            AirportCode: '',
            City: '',
            Country: ''
        };
        $scope.CitiesAll = [];
        $scope.CurrencyCodeList = [];
        $scope.CountryList = [];
        $scope.AirportCodeList = [];
       
        searchService.getAllCities($scope);
        searchService.getAllCurrencyCodes($scope);
        searchService.getAllCountry($scope);
        searchService.getAllAirportCodes($scope);

        $scope.searchViewModel;
       

    }


    $scope.onClickSearch = function () {

        $scope.filterCurrency = {

            CurrencyCode: '',
            AirportCode: '',
            City: '',
            Country: ''
        };

        if ($scope.filter.CurrencyCode) {
            $scope.filterCurrency.CurrencyCode = $scope.filter.CurrencyCode.Text;
        }
        if ($scope.filter.AirportCode) {
            $scope.filterCurrency.AirportCode = $scope.filter.AirportCode.Text;
        }
        if ($scope.filter.City) {
            $scope.filterCurrency.City = $scope.filter.City.Text;
        }
        if ($scope.filter.Country) {
            $scope.filterCurrency.Country = $scope.filter.Country.Text;
        }
       
        if ($scope.filterCurrency && ($scope.filterCurrency.CurrencyCode != '' || $scope.filterCurrency.AirportCode != '' || $scope.filterCurrency.City != '' || $scope.filterCurrency.Country != '')) {
            

            $state.go('ipcu.ipcurrencydetail', { filter: $scope.filterCurrency });
        }
    }

    Initialize();
}]);

