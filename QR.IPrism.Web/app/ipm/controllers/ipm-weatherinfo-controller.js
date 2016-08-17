
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.weatherInfo.controller',
										 ['$scope', '$http', 'searchService', 'analyticsService', '$state', '$rootScope', 'blockUI', '$stateParams',
                                  function ($scope, $http, searchService, analyticsService, $state, $rootScope, blockUI, $stateParams) {
                                      //Controller Scope Initialization

                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $scope.currencyDetailViewModel;
                                          $scope.weatherInfos;
                                          $scope.filter = {

                                              CurrencyCode: '',
                                              AirportCode: '',
                                              City: '',
                                              Country: '',
                                              FromDate: '',
                                              ToDate: ''
                                          };
                                          $scope.selectedCurrencyDetailTab = 0;
                                          $scope.selectedCurrencyDetail = '';
                                         // InitializeGrid();
                                          if ($rootScope.FilterSearchData && ($rootScope.FilterSearchData.CurrencyCode != '' || $rootScope.FilterSearchData.AirportCode != '' || $rootScope.FilterSearchData.City != '' || $rootScope.FilterSearchData.Country != '')) {
                                              $scope.filter = {

                                                  CurrencyCode: $rootScope.FilterSearchData.CurrencyCode,
                                                  AirportCode: $rootScope.FilterSearchData.AirportCode,
                                                  Country: $rootScope.FilterSearchData.Country,
                                                  City: $rootScope.FilterSearchData.City,
                                                  FromDate: $rootScope.FilterSearchData.FromDate,
                                                  ToDate: $rootScope.FilterSearchData.ToDate
                                              }
                                              loadCurrencyDetailList($scope.filter);
                                          }


                                      }
                                      function InitializeGrid() {
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

                                                     { field: "WeatherDate", name: "Weather Date", width: "15%" },
                                                      { field: "AirportCode", name: "Airport Code", width: "15%" },
                                                       { field: "City", name: "City", width: "15%" },
                                                        { field: "TempLow", name: "Temp Low", width: "15%" },
                                                         { field: "TempHigh", name: "Temp High", width: "15%" },
                                                          { field: "WeekDay", name: "WeekDay", width: "15%" }
                                                        


                                              ]
                                          };
                                      }
                                      function loadCurrencyDetailList(filter) {

                                          searchService.getWeatherInfos(filter, function (result) {
                                             // $scope.grid.data = result.data;
                                              $scope.weatherInfos = result.data;
                                          },
                                          function (error) {

                                          }
                                        );
                                      }

                                      $scope.isSelected = function (section) {

                                          return $scope.selectedCurrencyDetail === section;
                                      }
                                      $scope.onCurrencyDetailTabChange = function (tab) {
                                          $scope.selectedCurrencyDetailTab = tab;

                                      };
                                      $scope.isCurrencyDetailSelectedTab = function (tab) {
                                          return $scope.selectedCurrencyDetailTab === tab;

                                      };

                                      Initialize();

                                  }]);




