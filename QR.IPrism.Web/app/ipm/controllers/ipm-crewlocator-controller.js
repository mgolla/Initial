
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.crewLocator.controller',
										 ['$scope', '$http', 'searchService', 'analyticsService', '$state', 'blockUI', 'NgMap', 'toastr', '$rootScope', '$timeout',

  function ($scope, $http, searchService, analyticsService, $state, blockUI, NgMap, toastr, $rootScope, $timeout) {

      //Controller Scope Initialization
      var icrewLocatorblockUI = blockUI.instances.get('ipmcrewLocatorBlockUI');
      var vm = this;

      var Initialize = function () {
          //Declare all scop properties
          $scope.onFlight = 'FLIGHT';
          $scope.onStation = 'STATION';
          $scope.processStatus = '';
          $scope.mapTab = 1;
          $scope.detailTab = 2;
          $scope.mainMapObject;
          $scope.locationCrewDetailViewModel;
          vm.crewLocatorViewModel;
          NgMap.getMap().then(function (map) {
              $scope.map = map;
          });



          $scope.pageHieght = 600;
          $scope.selectedTab = 1;
          $scope.selectedCrewLocatorTab;
          $scope.selectedCrewLocator = null;

          $scope.showCustomMarker = function (evt, model, mapObject) {

              $scope.selectedCrewLocatorTab = model;
              $scope.mainMapObject = mapObject;
              mapObject.showInfoWindow(evt, 'bar')


          };



          vm.closeCustomMarker = function (evt) {
              this.style.display = 'none';
          };

          loadCrewLocatorList($scope.filter);


          $scope.filter = {
              StaffID: "",
              AirportCode: "",
              City: "",
              Country: "",
              FlightNo: "",
              Grade: "",
              Location: ""
          }

          $scope.filterData = {
              para: '',
              type: ''
          }
          initializeLocationCrewDetailGrid();
          pageHieght();
      }

      function initializeLocationCrewDetailGrid() {
          $scope.grid = {
              gridApi: {},
              enableFiltering: true,
              showGridFooter: true,
              enableColumnResizing: true,
              paginationPageSizes: [5, 10, 15, 20, 25],
              paginationPageSize: 10,
              enablePagination: true,
              enablePaginationControls: true,
              data: [],
              subgrid: 'false',
              columnDefs: [

                  { field: "StaffNo", name: "Staff No", enableHiding: false, sort: { direction: 'desc' } },
                  { field: "DutyCode", name: "Duty Code", enableHiding: false }
                  ,
                  { field: "DutyGroup", name: "Duty Group", enableHiding: false },
                  { field: "FlightNo", name: "Flight No", enableHiding: false },
                  { field: "FromSector", name: "From Sector", enableHiding: false },
                  { field: "ToSector", name: "To Sector", enableHiding: false },
                 
                    { field: "Indicator", name: "Indicator", enableHiding: false },
                     { field: "Location", name: "Location", enableHiding: false }


              ]
          };
      }
      $scope.filterDataBaseOnType = function () {
          $scope.onCrewLocatorTabChange($scope.detailTab);
          $scope.grid.data = [];



          if ($scope.filterData.type == $scope.onFlight) {
              var data = $.grep($scope.locationCrewDetailViewModel, function (v, i) {

                  return v.Indicator == $scope.onFlight;
              });

              $timeout(function () {
                  $scope.grid.data = data;
              });

          } else if ($scope.filterData.type == $scope.onStation) {
              var data = $.grep($scope.locationCrewDetailViewModel, function (v, i) {

                  return v.Indicator == $scope.onStation;
              });
              $timeout(function () {
                  $scope.grid.data = data;
              });
          } else {
              var data = $scope.locationCrewDetailViewModel;

              $timeout(function () {
                  $scope.grid.data = data;
              });
          }



      };

      function getLocationCrewDetailList(filter) {
          icrewLocatorblockUI.start();
          $scope.grid.data = [];
          searchService.getLocationCrewDetails(filter, function (result) {
              $scope.locationCrewDetailViewModel = result.data;

              $scope.onCrewLocatorTabChange($scope.detailTab);
              if ($scope.filterData.type == $scope.onFlight) {
                  var data = $.grep($scope.locationCrewDetailViewModel, function (v, i) {

                      return v.Indicator == $scope.onFlight;
                  });


                  initializeLocationCrewDetailGrid();
                 

                  $scope.grid.data = data;



              } else if ($scope.filterData.type == $scope.onStation) {
                  var data = $.grep($scope.locationCrewDetailViewModel, function (v, i) {

                      return v.Indicator == $scope.onStation;
                  });

                  initializeLocationCrewDetailGrid();

                  $scope.grid.data = data;


              } else {
                  var data = $scope.locationCrewDetailViewModel;


                  initializeLocationCrewDetailGrid();


                  $scope.grid.data = data;


              }


              icrewLocatorblockUI.stop();
          },
           function (error) {
               icrewLocatorblockUI.stop();
           }
         );
      }



      function loadCrewLocatorList(filter) {
          icrewLocatorblockUI.start();
          searchService.getCrewLocators(filter, function (result) {
              vm.crewLocatorViewModel = result.data;
              icrewLocatorblockUI.stop();
          },
          function (error) {
              icrewLocatorblockUI.stop();
          }
          );
      }



      $scope.isSelected = function (section) {

          return $scope.selectedCrewLocator === section;
      }
      $scope.onCrewLocatorTabChange = function (tab) {
          $scope.selectedTab = tab;


      };
      $scope.isCrewLocatorSelectedTab = function (tab) {
          return $scope.selectedTab === tab;

      };

      $scope.onCrewLocatorClickSearch = function (type) {
          if ($scope.selectedCrewLocatorTab) {

              $scope.filterData = {
                  para: $scope.selectedCrewLocatorTab,
                  type: type
              }
             
              if ($scope.filterData && $scope.filterData.para != null && $scope.filterData.para.LocationCode != null && $scope.filterData.para.LocationCode.toString().trim().length > 0) {


                  $scope.filter.AirportCode = $scope.filterData.para.LocationCode;
                  getLocationCrewDetailList($scope.filter);
              }
          } else {
              toastr.warning('Selection not found!');
          }
      }

      $scope.onClickRefresh = function (type) {


          $scope.filterData = {
              para: "",
              type: ""
          }
          getLocationCrewDetailList($scope.filter);


      }

      $scope.onClickRunProcess = function () {

          icrewLocatorblockUI.start();
          searchService.getCrewLocators($scope.filter, function (result) {
              $scope.processStatus = result.data;
              icrewLocatorblockUI.stop();
              toastr.warning($scope.processStatus);
          },
          function (error) {
              icrewLocatorblockUI.stop();
          }
          );

      }

      function pageHieght() {
          var getHeight = $(window).height() - 110;
          $scope.pageHieght = getHeight;

          $('.content_wrapper').css('min-height', getHeight + 'px');
      }


      Initialize();

  }]);



