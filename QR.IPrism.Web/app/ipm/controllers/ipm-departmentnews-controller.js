
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.departmentNews.controller', ['$scope', '$http',  'departmentNewsService','fileService',  'analyticsService',  '$state', '$rootScope', 'blockUI',
                                  function ($scope, $http,  departmentNewsService,fileService,  analyticsService,  $state,$rootScope,blockUI) {
                                    //Controller Scope Initialization
									var idepartmentNewsIFEGuideblockUI = blockUI.instances.get('ipmdepartmentNewsIFEGuideBlockUI');
                                      var Initialize = function () {
                                          //Declare all scop properties

                                      
                                         

                                          $scope.departmentNewsIFEGuideViewModel;
										   $scope.filter = {
										       NewsType: 1
                                              
                                          }
										  $scope.selectedDepartmentNewsIFEGuide=null;

                                          loadDepartmentNewsIFEGuideList($scope.filter);                                              
                                      }

									   function loadDepartmentNewsIFEGuideList(filter) {
									     idepartmentNewsIFEGuideblockUI.start();
									     departmentNewsService.getDepartmentNewsIFEGuideList(filter, function (result) {
                                              $scope.departmentNewsIFEGuideViewModel = result.data.DepartmentNewsIFEGuides;
                                              if ($scope.departmentNewsIFEGuideViewModel!=null  && $scope.departmentNewsIFEGuideViewModel.length > 0) {
                                                  $scope.selectedDepartmentNewsIFEGuide = $scope.departmentNewsIFEGuideViewModel[0];
                                              }
                                              appSettings.isNotDepNewsLoad = false;
                                             
                                          idepartmentNewsIFEGuideblockUI.stop();
                                          },
                                           function (error) {
                                               idepartmentNewsIFEGuideblockUI.stop();
                                           }, null, appSettings.isNotDepNewsLoad
                                         );
                                      }

									   $scope.isSelected = function (section) {

                                          return $scope.selectedDepartmentNewsIFEGuide === section;
									   }
									   $scope.onDepNSelectedChange = function (section) {

									    $scope.selectedDepartmentNewsIFEGuide = section;
									   }
									   $scope.onSelectedChangeDepNews = function (DocType, FileCode, fileID, DocumentId, DocumentName) {
									       var fileType = '';
									       if ($scope.selectedDepartmentNewsIFEGuide.FileType) {

									           fileType =  $scope.selectedDepartmentNewsIFEGuide.FileType;
									       }

									       var filterPara = {
									           DocType: DocType,
									           FileType: fileType,
									           FileCode: FileCode,
									           FileId: fileID,
									           DocumentId: DocumentId,
									           DocumentName: DocumentName,
									           isTab: false
									       };

									       if (DocumentName) {
									           filterPara.DocumentName = filterPara.DocumentName.split('.')[0];

									       }
									       fileService.openDocTypes(filterPara, function (result) {

									       },
                                           function (error) {
                                               console.log('error : ' + error);
                                           });
									   }

									   $scope.isNullOrEmpty = function (section) {

									       return (!section) || section === null || section.trim() === '';
									   }

                                      Initialize();                                     
                                     
                                  }]);



