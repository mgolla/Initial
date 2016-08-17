'use strict'
angular.module('app.ipm.module').controller('ipm.searchHotelInfo.controller',
                 ['$scope', '$rootScope', '$http', '$state', '$stateParams', 'lookupDataService', 'searchService', 'blockUI', '$filter', '$stickyState',
      function ($scope, $rootScope, $http, $state, $stateParams, lookupDataService, searchService, blockUI, $filter, $stickyState) {
       
          var Initialize = function () {

              $scope.hotelInfoSearch = {

                  City: ''
              };

              $scope.filter = {

                  City: ''
              };

              $scope.searchViewModel;
              $scope.CitiesAll = [];
              searchService.getAllCities($scope);
              //var para = $stateParams.filter;
              //if ($stateParams.filter && $stateParams.filter.City) {

              //    if ($stateParams.filter.City && $stateParams.filter.City != null && $stateParams.filter.City.Text && $scope.hotelInfoSearch.City.Text != '') {
              //        $scope.hotelInfoSearch.City = $stateParams.filter.City;
              //        $scope.onClickSearch();
              //    }
              //}
          }


          $scope.onClickSearch = function () {

              $scope.hotelFlter = {
                  City: ''
              };

              if ($scope.hotelInfoSearch && $scope.hotelInfoSearch.City) {

                  if ($scope.hotelInfoSearch && $scope.hotelInfoSearch.City != null && $scope.hotelInfoSearch.City.Text && $scope.hotelInfoSearch.City.Text!='') {
                      $scope.hotelFlter.City = $scope.hotelInfoSearch.City.Text;
                      $state.go('ipsh.hotelinfo', { filter: $scope.hotelFlter });
                  }
              }
          }

          Initialize();
      }]);

