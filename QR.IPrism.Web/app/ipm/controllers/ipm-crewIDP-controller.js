
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.crewIDP.controller', ['$scope', '$http', 'crewProfileService', '$state', '$rootScope', 'blockUI',
                                  function ($scope, $http, crewProfileService, $state, $rootScope, blockUI) {
                                      //Controller Scope Initialization
                                      var iCrewIdpblockUI = blockUI.instances.get('ipmCrewIdpBlockUI');

                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $scope.crewIDPViewModel;
                                          $scope.selectedTab = 1;


                                          $scope.grid = {
                                              gridApi: {},
                                              data: [],
                                              subgrid: 'false',
                                              columnDefs: [
                                                    { field: "IDPTypeName", name: "IDP Type", width: "33.3%" }
                                              ]
                                          };

                                          loadData();
                                      }

                                      function loadData() {

                                          iCrewIdpblockUI.start();

                                          crewProfileService.getCrewIDPDets('', function (result) {
                                              var IdpList = result.map(function (obj) {
                                                  return {
                                                      IDPTypeName: obj.IDPTypeName
                                                  }
                                              });
                                              $scope.grid.data = IdpList;

                                              iCrewIdpblockUI.stop();
                                          },
                                         function (error) {
                                             //console.log('error n getting Crew Training Details')
                                             iCrewIdpblockUI.stop();
                                         });
                                      }

                                      Initialize();

                                  }]);