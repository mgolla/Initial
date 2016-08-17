
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.crewCareerPath.controller', ['$scope', '$http', 'crewProfileService', 'sharedDataService', '$state', '$rootScope', 'blockUI',
                                  function ($scope, $http, crewProfileService, sharedDataService, $state, $rootScope, blockUI) {
                                      //Controller Scope Initialization

                                      $scope.crewCareerPathViewModel = [];

                                      var ipmCrewCrerPthblockUI = blockUI.instances.get('ipmCrewCareerPathBlockUI');

                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $scope.crewCareerPathViewModel;

                                          crewProfileService.getCrewCareerPath('', function (result) {
                                              angular.forEach(result, function (data, index) {
                                                  if (index == 0) { // If first row of fromGrade is Null this means that the crew got promoted for the first time
                                                      if (data.FromGrade.toUpperCase() == "NULL") {
                                                          $scope.JoinedGrade = data.ToGrade;
                                                          $scope.CrPthJoiningDate = data.DOJ;
                                                      } else {
                                                          $scope.JoinedGrade = data.FromGrade;
                                                          $scope.CrPthJoiningDate = data.DOJ;

                                                          $scope.crewCareerPathViewModel.push(data);
                                                      }
                                                      
                                                  } else {
                                                      $scope.crewCareerPathViewModel.push(data);
                                                  }
                                              });

                                              ipmCrewCrerPthblockUI.stop();

                                            },
                                           function (error) {
                                               ipmCrewCrerPthblockUI.stop();
                                           });
                                      }

                                      Initialize();

                                  }]);