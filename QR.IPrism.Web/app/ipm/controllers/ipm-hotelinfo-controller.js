
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.hotelInfo.controller', ['$scope', '$http',  'hotelInfoService',  'analyticsService',  '$state', '$rootScope', '$sce', 'blockUI','$stateParams',
                                  function ($scope, $http, hotelInfoService, analyticsService, $state, $rootScope, $sce, blockUI, $stateParams) {
                                      //Controller Scope Initialization
                                      var iblockUI = blockUI.instances.get('ipmBlockUIRoster');//ipmHotelInfoBlockUI
                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $rootScope.hotelInfoViewModel = [];
                                          $rootScope.filter = {
                                              AirportCode: ''
                                          }
                                          $rootScope.selectedHotelInfo = '';
                                          $scope.isDataAvailable = null;
                                          
                                       
                                          if ($rootScope.FilterSearchData && $rootScope.FilterSearchData.City) {

                                              $rootScope.filter = {
                                                  AirportCode: $rootScope.FilterSearchData.City
                                              }
                                              loadHotelInfoList($rootScope.filter, true);

                                          } else {
                                              if ($rootScope.selectedRosterItem.Arrival) {
                                                  $rootScope.filter.AirportCode = $rootScope.selectedRosterItem.Arrival;
                                                  loadHotelInfoList($rootScope.filter, $rootScope.HotelIsReload);

                                              }
                                          }

                                         

                                         
                                      }

                                      function loadHotelInfoList(filter, isReload) {
                                          iblockUI.start();
                                          hotelInfoService.getHotelInfoList(filter, function (result) {
                                              $rootScope.hotelInfoViewModel = result.data.HotelInfoModel;
                                              if ($rootScope.hotelInfoViewModel && $rootScope.hotelInfoViewModel.HotelName) {
                                                  $scope.isDataAvailable = true;
                                              } else {
                                                  $scope.isDataAvailable = false;
                                              }
                                             
                                              $rootScope.hotelInfoViewModel.Total = parseFloat($rootScope.hotelInfoViewModel.MealsAllowanceQarBreakfast) + parseFloat($rootScope.hotelInfoViewModel.MealsAllowanceQarLunch) + parseFloat($rootScope.hotelInfoViewModel.MealsAllowanceQarDinner);
                                             
                                              UIChanges();
                                              $rootScope.HotelIsReload = false;
                                              iblockUI.stop();
                                          },
                                            function (error) {
                                                iblockUI.stop();
                                            }, null, isReload 
                                          );
                                      }

                                      $scope.decodeText = function (data) {
                                          return $sce.trustAsHtml(data);
                                      }
                                      $scope.isSelected = function (section) {

                                          return $rootScope.selectedHotelInfo === section;
                                      }

                                      Initialize();
                                      function UIChanges() {
                                          
                                         
                                      }
                                      
                                  }]);



