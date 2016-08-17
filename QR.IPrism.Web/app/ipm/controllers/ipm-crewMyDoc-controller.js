
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.crewMyDoc.controller', ['$scope', '$http', 'crewProfileService', '$state', '$rootScope', 'blockUI',
                                  function ($scope, $http, crewProfileService, $state, $rootScope, blockUI) {
                                      //Controller Scope Initialization
                                      var iCrewMyDocblockUI = blockUI.instances.get('ipmCrewMyDocBlockUI');

                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $scope.crewMyDocViewModel;
                                          $scope.selectedTab = 1;

                                          iCrewMyDocblockUI.start();

                                          crewProfileService.getCrewMyDoc('', function (result) {
                                              $scope.crewMyDocViewModel = result;
                                              iCrewMyDocblockUI.stop();
                                          },
                                         function (error) {
                                             //console.log('error n getting Crew Training Details')
                                             iCrewMyDocblockUI.stop();
                                         });
                                      }

                                      Initialize();

                                  }]);