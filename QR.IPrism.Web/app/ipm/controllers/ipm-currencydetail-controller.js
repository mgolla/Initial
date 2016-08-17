
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.currencyDetail.controller',
										 ['$scope', '$http',  'searchService',  'analyticsService',  '$state', '$rootScope','blockUI','$stateParams',
                                  function ($scope, $http, searchService, analyticsService, $state, $rootScope, blockUI, $stateParams) {
                                      //Controller Scope Initialization
                                     
                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $scope.currencyDetailViewModel;
                                          $scope.CurrencyDetail;
                                          $scope.filter = {

                                              CurrencyCode: '',
                                              AirportCode: '',
                                              City: '',
                                              Country: ''
                                          };
                                          $scope.selectedCurrencyDetailTab = 0;
                                          $scope.selectedCurrencyDetail = '';
                                        
                                          if ($rootScope.FilterSearchData && ($rootScope.FilterSearchData.CurrencyCode != '' || $rootScope.FilterSearchData.AirportCode != '' || $rootScope.FilterSearchData.City != '' || $rootScope.FilterSearchData.Country != '')) {
                                              $scope.filter = {

                                                  CurrencyCode: $rootScope.FilterSearchData.CurrencyCode,
                                                  AirportCode: $rootScope.FilterSearchData.AirportCode,
                                                  Country: $rootScope.FilterSearchData.Country,
                                                  City: $rootScope.FilterSearchData.City
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
                                               
                                                 { field: "FromCurrency", name: "From Currency", width: "15%" },
                                                  { field: "ToCurrency", name: "To Currency", width: "15%" },
                                                   { field: "ConversionRate", name: "Conversion Rate", width: "15%" },
                                                    { field: "InvConversionRate", name: "InvConversion Rate", width: "15%" },
                                                     { field: "AirportCode", name: "Airport Code", width: "15%" }
                                                    

                                               
                                          ]
                                      };
                                  }
									   function loadCurrencyDetailList(filter) {
									    
									       searchService.getCurrencyDetails(filter, function (result) {
									           //$scope.grid.data =  result.data;

									           if (result.data && result.data.length > 0) {
									               $scope.CurrencyDetail = result.data[0];
									           }									           
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



