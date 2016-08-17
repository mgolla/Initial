
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.crewInfo.controller', ['$scope', '$http', 'messages', 'crewInfoService', 'analyticsService', '$state', '$rootScope', 'blockUI', '$stateParams', '$filter',
                                  function ($scope, $http, messages, crewInfoService, analyticsService, $state, $rootScope, blockUI, $stateParams, $filter) {
                                      //Controller Scope Initialization
                                      var iCrewDtlsblockUI = blockUI.instances.get('ipmBlockUIRoster');//ipmCrewDetailsBlockUI

                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $rootScope.crewInfoViewModel = [];
                                          $rootScope.filter = {};
                                          var pathArray = window.location.href.split('/');
                                          var protocol = pathArray[0];
                                          var host = pathArray[2];
                                          var baseURL = protocol + '//' + host + '/';

                                          $rootScope.filter.CrewImagePath = baseURL + appSettings.CrewPhotos;
                                          $rootScope.filter.CrewImageType = messages.CREWIMAGETYPE;
                                          $scope.Yes = 1;
                                          $scope.No = 2;
                                          $scope.Loading = 0;

                                          $scope.isDataLoad = $scope.Loading;



                                          if (($scope.searchParmeter && $scope.searchParmeter != null && $scope.searchParmeter.FlightNumber.length > 0)
                                              || ($rootScope.selectedRosterItem != null
                                              && $rootScope.selectedRosterItem.Flight.length > 0)) {

                                              if ($scope.searchParmeter && $scope.searchParmeter != null && $scope.searchParmeter.FlightNumber) {

                                                  var fdate = $filter('date')($scope.searchParmeter.ScheduledDeptTime, 'dd-MMM-yyyy');
                                                  $rootScope.filter = {

                                                      FlightNo: $scope.searchParmeter.FlightNumber,
                                                      Arrival: $scope.searchParmeter.SectorTo,
                                                      FlightDate: fdate,
                                                      CrewImagePath: baseURL + appSettings.CrewPhotos,
                                                      CrewImageType: messages.CREWIMAGETYPE

                                                  }
                                              }
                                              else if (($rootScope.selectedRosterItem != null
                                              && $rootScope.selectedRosterItem.Flight.length > 0)) {
                                                  $rootScope.filter = {

                                                      FlightNo: $rootScope.selectedRosterItem.Flight,
                                                      Arrival: $rootScope.selectedRosterItem.Arrival,
                                                      FlightDate: $rootScope.selectedRosterItem.DutyDateActual.toString(),
                                                      CrewImagePath: baseURL + appSettings.CrewPhotos,
                                                      CrewImageType: messages.CREWIMAGETYPE

                                                  }
                                              }

                                              $scope.CPmembers = [];
                                              $scope.CSDmembers = [];
                                              $scope.F1members = [];
                                              $scope.F2members = [];
                                              $scope.CSmembers = [];

                                              $rootScope.selectedCrewInfo = '';

                                              loadCrewInfoList($rootScope.filter);
                                          }
                                      }

                                      function loadCrewInfoList(filter) {
                                          iCrewDtlsblockUI.start();
                                          crewInfoService.getCrewInfoList(filter, function (result) {

                                              $rootScope.crewInfoViewModel = result.data;
                                              $scope.isDataLoad = $rootScope.crewInfoViewModel.IsDataLoaded;
                                              
                                              if (!result.data || result.data.length == 0) {
                                                  $scope.errorMsg = "No Data Available";
                                              }
                                              $rootScope.CrewDetIsReload = false;
                                              iCrewDtlsblockUI.stop();
                                          },
                                           function (error) {
                                               iCrewDtlsblockUI.stop();
                                               $scope.errorMsg = "Rosters Not Available";
                                           }, null, $rootScope.CrewDetIsReload
                                         );
                                      }
                                      $scope.getPara = function (section) {

                                          return section;
                                      }


                                      $scope.isSelected = function (section) {

                                          return $rootScope.selectedCrewInfo === section;
                                      }

                                      $scope.isNotNullCrew = function (section) {

                                          return (section && section != null && section.trim() != '');
                                      }

                                      Initialize();

                                  }]);



