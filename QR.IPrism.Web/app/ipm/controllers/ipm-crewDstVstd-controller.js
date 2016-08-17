
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.crewDstVstd.controller', ['$scope', '$http', 'crewProfileService', '$state', '$rootScope', 'blockUI',
                                  function ($scope, $http, crewProfileService, $state, $rootScope, blockUI) {
                                      //Controller Scope Initialization
                                      var ipmCrewDestVstdblockUI = blockUI.instances.get('ipmCrewDestVstdBlockUI');

                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $scope.selectedTab = 1;

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
                                                    { field: "City", name: "City", width: "33.3%", enableHiding: false },
                                                    {
                                                        field: "LastVisitedDate", name: "Last Travel Date", enableHiding: false,
                                                        width: "33.3%",
                                                    },
                                                    {
                                                        field: "NoOfVisits", name: "No. of Visits", enableHiding: false
                                                    }
                                              ]
                                          };

                                          loadData();
                                      }

                                      function loadData() {

                                          ipmCrewDestVstdblockUI.start();

                                          crewProfileService.getCrewDestVstd('', function (result) {
                                              var dstVstList = result.map(function (obj) {
                                                  return {
                                                      City: obj.City, LastVisitedDate: obj.LastVisitedDate, NoOfVisits: obj.NoOfVisits
                                                  }
                                              });
                                              $scope.grid.data = dstVstList;

                                              ipmCrewDestVstdblockUI.stop();
                                          },
                                         function (error) {
                                             //console.log('error n getting Crew Training Details')
                                             ipmCrewDestVstdblockUI.stop();
                                         });
                                      }


                                      Initialize();

                                  }]);