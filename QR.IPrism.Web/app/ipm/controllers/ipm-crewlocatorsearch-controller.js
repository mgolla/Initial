
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.crewLocatorSearch.controller',
										 ['$scope', '$http', 'searchService', 'analyticsService', '$state', '$rootScope', 'blockUI', '$stateParams',
                                  function ($scope, $http, searchService, analyticsService, $state, $rootScope, blockUI, $stateParams) {
                                      //Controller Scope Initialization
                                      var ilocationCrewDetailblockUI = blockUI.instances.get('ipmlocationCrewDetailBlockUI');
                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $scope.onFlight = 'ON-FLIGHT';
                                          $scope.onStation = 'ON-STATION';

                                          $scope.locationCrewDetailViewModel;
                                          $scope.locationFlightViewModel;
                                          $scope.selectedLocationCrewDetailTab = 0;
                                          $scope.selectedLocationCrewDetail = '';
                                          initializeLocationCrewDetailGrid();

                                          $scope.filter = {
                                              StaffID: "",
                                              AirportCode: "",
                                              City: "",
                                              Country: "",
                                              FlightNo: "",
                                              Grade: "",
                                              Location: ""
                                          }

                                          if ($stateParams.filter && $stateParams.filter.para != null && $stateParams.filter.para.LocationCode != null && $stateParams.filter.para.LocationCode.toString().trim().length > 0) {

                                              $scope.filter.AirportCode = $stateParams.filter.para.LocationCode;
                                              getLocationCrewDetailList($scope.filter);
                                              //loadLocationFlightList($scope.filter);
                                          }
                                      }

                                      function initializeLocationCrewDetailGrid() {
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
                                                  
                                                  { field: "StaffNo", name: "Staff No", width: "10%" },
                                                  { field: "DutyCode", name: "Duty Code", width: "10%" },
                                                  { field: "DutyGroup", name: "Duty Group", width: "10%" },
                                                  { field: "FlightNo", name: "Flight No", width: "10%" },
                                                  { field: "FromSector", name: "From Sector", width: "10%" },
                                                  { field: "ToSector", name: "To Sector", width: "10%" },
                                                  {
                                                      field: "DepartureDate", name: "Departure Date", width: "15%", type: 'date',
                                                      cellTemplate: '<span>{{row.entity["DepartureDate"] | date:"dd-MMM-yyyy hh:mm" }}</span>'
                                                  },
                                                  {
                                                      field: "ArrivalDate", name: "Arrival Date", width: "15%", type: 'date',
                                                      cellTemplate: '<span>{{row.entity["ArrivalDate"] | date:"dd-MMM-yyyy hh:mm" }}</span>'
                                                  },
                                                    { field: "Indicator", name: "Indicator", width: "10%" },
                                                     { field: "Location", name: "Location", width: "10%" }


                                              ]
                                          };
                                      }
                                      $scope.filterDataBaseOnType = function () {
                                          if ($stateParams.filter.type == $scope.onFlight) {
                                              $scope.grid.data =$.grep($scope.locationCrewDetailViewModel, function (v, i) {

                                                  return v.Indicator == $scope.onFlight;
                                              });

                                          } else if ($stateParams.filter.type == $scope.onStation) {
                                              $scope.grid.data = $.grep($scope.locationCrewDetailViewModel, function (v, i) {

                                                  return v.Indicator == $scope.onStation;
                                              });
                                          } else {
                                              $scope.grid.data = $scope.locationCrewDetailViewModel;
                                          }

                                      };

                                      function getLocationCrewDetailList(filter) {
                                          ilocationCrewDetailblockUI.start();
                                          searchService.getLocationCrewDetails(filter, function (result) {
                                              $scope.locationCrewDetailViewModel = result.data;

                                              $scope.filterDataBaseOnType();


                                              ilocationCrewDetailblockUI.stop();
                                          },
                                           function (error) {
                                               ilocationCrewDetailblockUI.stop();
                                           }
                                         );
                                      }



                                      function loadLocationFlightList(filter) {
                                          ilocationCrewDetailblockUI.start();
                                          locationFlightService.getLocationFlightList(filter, function (result) {
                                              $scope.locationFlightViewModel = result.data.LocationFlights;
                                              ilocationCrewDetailblockUI.stop();
                                          },
                                           function (error) {
                                               ilocationCrewDetailblockUI.stop();
                                           }
                                         );
                                      }


                                      function loadLocationCrewDetailList(filter) {
                                          ilocationCrewDetailblockUI.start();
                                          searchService.getLocationCrewDetails(filter, function (result) {
                                              $scope.locationCrewDetailViewModel = result.data.LocationCrewDetails;
                                              ilocationCrewDetailblockUI.stop();
                                          },
                                           function (error) {
                                               ilocationCrewDetailblockUI.stop();
                                           }
                                         );
                                      }

                                      $scope.isSelected = function (section) {

                                          return $scope.selectedLocationCrewDetail === section;
                                      }
                                      $scope.onLocationCrewDetailTabChange = function (tab) {
                                          $scope.selectedLocationCrewDetailTab = tab;

                                      };
                                      $scope.isLocationCrewDetailSelectedTab = function (tab) {
                                          return $scope.selectedLocationCrewDetailTab === tab;

                                      };
                                       (function displayUTC() {
          var getHeight = $(window).height() - 110;
          $scope.pageHieght = getHeight;
          $('#mapMain').css('min-height', getHeight + 'px');
          $('.content_wrapper').css('min-height', getHeight + 'px');
      })();
                                      Initialize();

                                  }]);



