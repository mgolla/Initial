
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.crewQualnVisa.controller', ['$scope', '$http', 'crewProfileService', '$state', '$rootScope', 'blockUI',
                                  function ($scope, $http, crewProfileService, $state, $rootScope, blockUI) {
                                      //Controller Scope Initialization
                                      var iCrewQualVisablockUI = blockUI.instances.get('ipmCrewQualVisaBlockUI');

                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $scope.crewQualnVisaViewModel;
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
                                                    { field: "Qualification", name: "Qualification", width: "33.3%", enableHiding: false },
                                                    {
                                                        field: "ValidFrom", name: "From Date", enableHiding: false,
                                                        width: "33.3%"
                                                    },
                                                    {
                                                        field: "ValidTo", name: "To Date", enableHiding: false,
                                                        width: "33.4%"
                                                    }
                                              ]
                                          };

                                          loadData();
                                      }

                                      function loadData() {
                                          iCrewQualVisablockUI.start();
                                            crewProfileService.getCrewQualnVisa('', function (result) {
                                                //$scope.gridQualVisa.data = result;
                                                var qualVisaList = result.map(function (obj) {
                                                    return {
                                                        Qualification: obj.Qualification, ValidFrom: obj.ValidFrom, ValidTo: obj.ValidTo
                                                    }
                                                });
                                                $scope.grid.data = qualVisaList;
                                                iCrewQualVisablockUI.stop();
                                            },
                                            function (error) {
                                                //console.log('error n getting Crew Training Details')
                                                iCrewQualVisablockUI.stop();
                                            });
                                      }

                                      Initialize();

                                  }]);