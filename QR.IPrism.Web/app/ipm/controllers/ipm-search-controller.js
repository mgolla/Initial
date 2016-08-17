'use strict'
angular.module('app.ipm.module').controller('ipm.search.controller',
                 ['$scope', '$rootScope', '$http', '$state', 'lookupDataService', 'searchService', '$filter', 'toastr', 'blockUI', '$timeout',
      function ($scope, $rootScope, $http, $state, lookupDataService, searchService, $filter, toastr, blockUI, $timeout) {

          var ipmSearchBlockUI = blockUI.instances.get('ipmSearchBlockUI');
          var ipmSearchBlockUICrew = blockUI.instances.get('ipmSearchBlockUICrew');
          var ipmSearchBlockUISOS = blockUI.instances.get('ipmSearchBlockUISOS');
          var ipmSearchBlockUIStation = blockUI.instances.get('ipmSearchBlockUIStation');

          function Initialize() {
              PageEvents();
              $rootScope.selectedRosterItem = null;
              $scope.model = {};
              $scope.model.selectedTab = 'CrewDetails';

              $scope.FlightsCrew = [];
              $scope.FlightsSOS = [];
              $scope.AirportCodeList = [];

              $scope.FlightListCrew = null;
              $scope.FlightListSOS = null;

              defineModels();
              loadData();
          }

          function loadData() {

              //ipmSearchBlockUI.start();
              lookupDataService.getLookupList('Sector', null, function (result) {
                  $scope.AirportCodeList = result.map(function (obj) { return { Value: obj.FilterText, Text: obj.Text, Desc: obj.FilterText } });
              });
          }

          function setDate() {
              var currentDate = new Date();
              $scope.filter.FromDate = angular.copy(currentDate);
          };

          function defineModels() {

              $scope.filter = {
                  FromSector: '',
                  ToSector: '',
                  FromDate: '',
                  CrewFlight: ''
              }

              $rootScope.FilterSearchData = {
                  FlightNo: '',
                  Arrival: '',
                  FlightDate: '',
                  City: '',
                  FromSector: '',
                  ToSector: '',
                  CurrencyCode: '',
                  AirportCode: '',
                  Country: '',
                  FromDate: '',
                  ToDate: '',
              }

              setDate();
          }
          function refreshFilters() {
              $rootScope.OverviewIsReload = true;
              $rootScope.CrewDetIsReload = true;
              $rootScope.SOSIsReload = true;
              $rootScope.HotelIsReload = true;
              $rootScope.StationInfoIsReload = true;
          }

          function PageEvents() {
              //New Change: New Method added as this is to be called on "Load Flight" button for Crew Deatils Screen
              $scope.onClickLoadFlights = function () {
                  refreshFilters();
                  if ($scope.filter.FromDate != '' && $scope.filter.ToSectorObj && $scope.filter.FromSectorObj) {

                      $scope.filter.CrewFlightObj = '';

                      var data = {
                          FromDate: $scope.filter.FromDate,
                          FromSector: $scope.filter.FromSectorObj ? $scope.filter.FromSectorObj.Text : '',
                          ToSector: $scope.filter.ToSectorObj ? $scope.filter.ToSectorObj.Text : ''
                      }

                      ipmSearchBlockUICrew.start();
                      searchService.getSearchList(data, function (result) {

                          $scope.FlightsCrew = result.data;

                          if ($scope.FlightsCrew && $scope.FlightsCrew.length < 1) {
                              toastr.info('No records found.');
                          }
                          ipmSearchBlockUICrew.stop();
                      },
                       function (error) {
                           ipmSearchBlockUICrew.stop();
                       }
                     );

                  } else {
                      toastr.warning('Please select all the filters');
                  }
              }

              //New Change: New Method added as this is to be called on "Load Flight" button for SOS Screen
              $scope.onClickLoadSOSFlights = function () {
                  refreshFilters();
                  //if ($scope.filter.ToSector != '' || $scope.filter.FromSectorObj != '') check why its || and not && according to the old logic.
                  if ($scope.filter.ToSectorObj && $scope.filter.FromSectorObj) {

                      $scope.filter.SosFlightObj = '';
                      var data = {
                          FromSector: $scope.filter.FromSectorObj ? $scope.filter.FromSectorObj.Text : '',
                          ToSector: $scope.filter.ToSectorObj ? $scope.filter.ToSectorObj.Text : ''
                      }

                      ipmSearchBlockUISOS.start();
                      searchService.getSearchList(data, function (result) {

                          $scope.FlightsSOS = result.data;
                          if ($scope.FlightsSOS && $scope.FlightsSOS.length < 1) {
                              toastr.info('No records found.');
                          }
                          ipmSearchBlockUISOS.stop();
                      },
                       function (error) {
                           ipmSearchBlockUISOS.stop();
                       }
                     );
                  } else {
                      toastr.warning('Please select all the filters');
                  }
              }

              $scope.onClickStationInoSearch = function () {
                  refreshFilters();
                  $scope.IsLoadStation = false;
                  if ($scope.filter && $scope.filter.ToSectorObj) {

                     
                      $timeout(function () {
                          $scope.IsLoadStation = true;
                          $rootScope.FilterSearchData.City = $scope.filter.ToSectorObj.Text;
                          $scope.SelectedRDTabSearch = 'StationInfoTab';
                      }, 100);

                  } else {
                      toastr.warning('Please fill  in required fields');
                  }
              }

              $scope.onClickHideMenu = function () {
                  refreshFilters();
                  if ($scope.isLoaded) {
                      $('.search-menu').slideUp("slow", function () {

                      });

                      $scope.isLoaded = false;
                  } else {
                      $('.search-menu').slideDown("slow", function () {

                      });
                      $scope.isLoaded = true;
                  }
              }

              $scope.onClickReset = function () {
                  refreshFilters();
                  defineModels();

                  $scope.FlightsCrew = [];
                  $scope.FlightListCrew = null;
                  $scope.FlightsSOS = [];
                  $scope.FlightListSOS = null;
                  $scope.IsLoadStation = false;
              }

              $scope.onChangeFiltersFlightCrew = function (model) {
                  refreshFilters();
                  $scope.FlightListCrew = null;
                  $timeout(function () {
                      $scope.FlightListCrew = model;
                  }, 100);

              }

              $scope.onChangeFiltersFlightSOS = function (model) {
                  refreshFilters();
                  $scope.FlightListSOS = null;
                  $timeout(function () {
                      $scope.FlightListSOS = model;
                  }, 100);
              }

              $scope.searchSos = function (data) {
                  var searchParmeter = { FlightNumber: data };
                  $scope.onChangeFiltersFlightSOS(searchParmeter);
              }
          }

          Initialize();
      }]);