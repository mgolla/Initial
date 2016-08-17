
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.crewTrainingHistory.controller', ['$scope', '$http', 'crewProfileService', '$state', '$rootScope', 'blockUI',
                                  function ($scope, $http, crewProfileService, $state, $rootScope, blockUI) {
                                      //Controller Scope Initialization
                                      var ipmCrewTrngHstryblockUI = blockUI.instances.get('ipmCrewTrngHstryBlockUI');

                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $scope.crewTrngHstryViewModel;

                                          $scope.grid = {
                                              gridApi: {},
                                              data: [],
                                              subgrid: 'false',
                                              columnDefs: [
                                                    { field: "Name", name: "Training Name", width: "33.3%", enableHiding: false },
                                                    {
                                                        field: "NoOfDays", name: "No. of Days", enableHiding: false,
                                                        width: "33.3%"
                                                    },
                                                    {
                                                        field: "Date", name: "Date", type: 'date', enableHiding: false,
                                                        width: "33.3%"
                                                    }
                                              ]
                                          };

                                          loadData();
                                      }

                                      function loadData() {

                                          ipmCrewTrngHstryblockUI.start();

                                          crewProfileService.getCrewTrainingHistory('', function (result) {
                                              //$scope.gridQualVisa.data = result;
                                              var trngHstryList = result.map(function (obj) {
                                                  return {
                                                      Name: obj.Name, NoOfDays: obj.NoOfDays, Date: obj.Date
                                                  }
                                              });
                                              //$scope.gridDate = qualVisaList;
                                              $scope.grid.data = trngHstryList;

                                              ipmCrewTrngHstryblockUI.stop();
                                          },
                                        function (error) {
                                            //console.log('error n getting Crew Training Details')
                                            ipmCrewTrngHstryblockUI.stop();
                                        });
                                      }

                                      Initialize();

                                  }]);