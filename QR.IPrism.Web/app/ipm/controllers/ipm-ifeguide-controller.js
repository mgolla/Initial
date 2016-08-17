
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.iFEGuide.controller', ['$scope', '$http', 'iFEGuideService', 'fileService', 'analyticsService', '$state', '$rootScope', 'blockUI',
                                  function ($scope, $http, iFEGuideService, fileService, analyticsService, $state, $rootScope, blockUI) {
                                      //Controller Scope Initialization
                                      var idepartmentNewsIFEGuideblockUI = blockUI.instances.get('ipmIFEguideBlockUI');
                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $rootScope.departmentNewsIFEGuideViewModel = [];
                                          $rootScope.filter = {
                                              StaffID: ""

                                          }
                                          $scope.selectedDepartmentNewsIFEGuide = null;

                                          loadDepartmentNewsIFEGuideList($scope.filter);
                                      }

                                      function loadDepartmentNewsIFEGuideList(filter) {
                                          idepartmentNewsIFEGuideblockUI.start();
                                          iFEGuideService.getDepartmentNewsIFEGuideList(null, function (result) {
                                              $scope.departmentNewsIFEGuideViewModel = result.data.DepartmentNewsIFEGuides;
                                              if ($scope.departmentNewsIFEGuideViewModel != null && $scope.departmentNewsIFEGuideViewModel.length > 0) {
                                                  $scope.selectedDepartmentNewsIFEGuide = $scope.departmentNewsIFEGuideViewModel[0];
                                              }
                                              appSettings.isNotIFEGuideLoad = false;
                                              idepartmentNewsIFEGuideblockUI.stop();
                                          },
                                            function (error) {
                                                idepartmentNewsIFEGuideblockUI.stop();
                                            }, null, appSettings.isNotIFEGuideLoad
                                          );
                                      }

                                      $scope.onSelectedChange = function (DocType, FileCode, fileID, DocumentId) {

                                          var filterPara = {
                                              DocType: DocType,
                                              FileCode: FileCode,
                                              FileId: fileID,
                                              DocumentId: DocumentId,
                                              isTab: false
                                          };

                                          fileService.openDocTypes(filterPara, function (result) {

                                          },
                                          function (error) {
                                              console.log('error : ' + error);
                                          });
                                      }
                                      $scope.isSelected = function (section) {

                                          return $scope.selectedDepartmentNewsIFEGuide === section;
                                      }
                                      $scope.onDepNSelectedChange = function (section) {

                                          $scope.selectedDepartmentNewsIFEGuide = section;
                                      }
                                      $scope.isNullOrEmpty = function (section) {

                                          return (!section) || section === null || section.trim() === '';
                                      }
                                      Initialize();

                                  }]);



