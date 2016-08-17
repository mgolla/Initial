'use strict'
angular.module('app.ipm.module').controller('ipm.searchSOS.controller',
                 ['$scope', '$rootScope', '$http', '$state', '$stateParams', 'lookupDataService', 'searchService', 'blockUI', '$filter', '$stickyState',
      function ($scope, $rootScope, $http, $state, $stateParams, lookupDataService, searchService, blockUI, $filter, $stickyState) {
          var isearchblockUI = blockUI.instances.get('isearchblockUI');
          var Initialize = function () {

          
              $scope.model = {};
              $scope.model.selectedFlight;
              $scope.crewSearch = {
                  FromSector: '',
                  ToSector: ''
              };
              $scope.filterFlight = {
                  FromSector: '',
                  ToSector: '',
                  FromDate: '',
                  ToDate: '',
              }
              $scope.filter = {
                  FromSector: '',
                  ToSector: '',
                  FromDate: '',
                  ToDate: '',
              }

              $scope.sosSearch = {
                  Flight: '',
                  FromSector: '',
                  ToSector: ''
              };


              $scope.searchViewModel;

            

              $scope.selectedSearchTab = 0;
              $scope.selectedSearch = '';
              $scope.CitiesAll=[];

              searchService.getAllCities($scope);
              searchService.getAllFlightsToday();
              //var para = $stateParams.filter;
              //if ($stateParams.filter && $stateParams.filter) {

              //    if ($stateParams.filter.FromSector && $stateParams.filter.FromSector != null && $stateParams.filter.FromSector.Text) {
              //        // $scope.filter.FromSector = $stateParams.filter.FromSector.Text;
              //        $scope.sosSearch.FromSector = $stateParams.filter.FromSector;
              //        if ($stateParams.filter.ToSector && $stateParams.filter.ToSector != null && $stateParams.filter.ToSector.Text) {
              //            //$scope.filter.ToSector = $stateParams.filter.ToSector.Text;
              //            $scope.sosSearch.ToSector = $stateParams.filter.ToSector;
              //        }
              //    }
              //    if ($stateParams.filter.Flight && $stateParams.filter.Flight != null && $stateParams.filter.Flight.FlightNumber) {
              //        //$scope.filter.Flight = $stateParams.filter.Flight.Text;
              //        $scope.sosSearch.Flight = $stateParams.filter.Flight;
              //        $scope.onClickSearch();
              //    }

              //}
          }

        
          $scope.onClickSearch= function () {

              $scope.sosFlter = {
                  Flight: '',
                  FromSector: '',
                  ToSector: ''
              };

              if ($scope.sosSearch && $scope.sosSearch) {

                  if ($scope.sosSearch.FromSector && $scope.sosSearch.FromSector != null && $scope.sosSearch.FromSector.Text) {
                      $scope.sosFlter.FromSector = $scope.sosSearch.FromSector.Text;
                   
                      if ($scope.sosSearch.ToSector && $scope.sosSearch.ToSector != null && $scope.sosSearch.ToSector.Text) {
                          $scope.sosFlter.ToSector = $scope.sosSearch.ToSector.Text;
                        
                      }
                  }
                  if ($scope.sosSearch.Flight && $scope.sosSearch.Flight != null && $scope.sosSearch.Flight.FlightNumber) {
                      $scope.sosFlter.Flight = $scope.sosSearch.Flight.FlightNumber.replace('QR','');
                      ;
                    
                  }
                  if ($scope.sosFlter && $scope.sosFlter.Flight && $scope.sosFlter.FromSector
                     && $scope.sosFlter.ToSector && $scope.sosFlter.Flight != '' && $scope.sosFlter.FromSector != '' && $scope.sosFlter.ToSector != '') {

                      $state.go('ipss.sos', { filter: $scope.sosFlter });

                  }

              }
          }
          $scope.onClickPOMonthly = function () {
              var filter = {
                  StaffID: '07000'
              }
              $state.go('pomonthly', { filter: filter });
          }
          Initialize();
      }]);


