'use strict'
angular.module('app.ipm.module').controller('ipm.searchCrew.controller',
                 ['$scope', '$rootScope', '$http', '$state','$stateParams', 'lookupDataService','searchService','blockUI','$filter','$stickyState',
      function ($scope, $rootScope, $http, $state, $stateParams, lookupDataService, searchService, blockUI, $filter, $stickyState) {
          var isearchblockUI = blockUI.instances.get('isearchblockUI');
          var Initialize = function () {

             
              $scope.model = {};
              $scope.model.selectedFlight;
              $scope.crewSearch = {
                  FromSector: '',
                  ToSector: '',
                  FromDate: '',
                  ToDate: ''
              };
              $scope.filter = {

                  FromSector: '',
                  ToSector: '',
                  FromDate: '',
                  ToDate: '',
              }


              $scope.searchViewModel;
              
            

              $scope.selectedSearchTab = 0;
              $scope.selectedSearch = '';
              $scope.CitiesAll = [];
              searchService.getAllCities($scope);
          }

          function loadSearchList(filter) {

              searchService.getSearchList(filter, function (result) {

                  $rootScope.Flights = result.data;

              },
               function (error) {

               }
             );
          }
         

          $scope.onClickSearch = function () {

             
              if ($scope.crewSearch && $scope.crewSearch) {
                  var fdate = [];
                  var tdate = [];
                  if ($scope.crewSearch.FromDate && $scope.crewSearch.FromDate != null) {
                      fdate = $scope.crewSearch.FromDate.toString().split(' ');
                  }
                  if ($scope.crewSearch.ToDate && $scope.crewSearch.ToDate != null) {
                      tdate = $scope.crewSearch.ToDate.toString().split(' ');
                  }
                  if ($scope.crewSearch.FromSector && $scope.crewSearch.FromSector != null && $scope.crewSearch.FromSector.Text) {
                      $scope.filter.FromSector = $scope.crewSearch.FromSector.Text;
                      if ($scope.crewSearch.ToSector && $scope.crewSearch.ToSector != null && $scope.crewSearch.ToSector.Text) {
                          $scope.filter.ToSector = $scope.crewSearch.ToSector.Text;
                      }
                  }


                  if (fdate && fdate.length > 0) {
                      $scope.filter.FromDate = fdate[2] + '-' + fdate[1] + '-' + fdate[3];
                  }
                  if (tdate && tdate.length > 0) {
                      $scope.filter.ToDate = tdate[2] + '-' + tdate[1] + '-' + tdate[3];
                  }

              }
              loadSearchList($scope.filter);

          }

          $scope.onClickSearchCrew = function () {

              var val = $scope.model.selectedFlight;
             
              if (val) {
                  var arr = val.SectorTo;
                  var fdate = $filter('date')(val.ScheduledDeptTime, 'dd-MMM-yyyy');
                  if (arr && fdate) {
                      var filter = {
                          FlightNo: val.FlightNumber.replace('QR',''),
                          Arrival: arr.trim(),
                          FlightDate: fdate
                      }
                     
                      $state.go('ipsc.crewInfo',{ filter: filter }, { reload: 'crewInfo' });
                      
              }}
              
          }

          Initialize();
      }]);


