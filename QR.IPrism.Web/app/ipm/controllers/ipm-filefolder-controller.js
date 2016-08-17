
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.fileFolder.controller', ['$scope', '$http',  'documentService',  'analyticsService','fileService',  '$state', '$rootScope', 'blockUI','$stateParams',
                                  function ($scope, $http, documentService, analyticsService,fileService, $state, $rootScope, blockUI, $stateParams) {
                                      //Controller Scope Initialization
                                      var idocumentblockUI = blockUI.instances.get('ipmfileFolderBlockUI');
                                      var Initialize = function () {
                                          //Declare all scop properties
                                          var para = $stateParams.fileFilter;
                                          $scope.folderDocType = "FOLDER";
                                          $scope.documentViewModel ;
                                          $scope.filter = {
                                              DocumentId: ""

                                          }

                                          if (para && para != null) {
                                              $scope.filter.DocumentId = para.DocumentId;
                                          }
                                          $scope.selectedDocumentTab = 0;

                                          loadDocumentList($scope.filter, $scope.documentViewModel);
                                          
                                      }

                                      function loadDocumentList(filter, model) {
                                          idocumentblockUI.start();
                                          documentService.getDocumentList(filter, function (result) {
                                              $scope.documentViewModel = result.data.Documents;
                                             
                                              $scope.onDocumentTabChange(0, $scope.documentViewModel[0]);

                                              idocumentblockUI.stop();
                                          },
                                           function (error) {
                                               idocumentblockUI.stop();
                                           }
                                         );
                                      }
                                      $scope.onSelectedChange = function (DocType, FileCode, fileID, DocumentId) {

                                          var filterPara = {
                                              DocType: DocType,
                                              FileCode: FileCode,
                                              FileId: fileID,
                                              DocumentId: DocumentId
                                          };

                                          fileService.openDocTypes(filterPara, function (result) {

                                          },
                                          function (error) {
                                              console.log('error : ' + error);
                                          });
                                      }

                                      function filterDocumentList(filter, index) {
                                          idocumentblockUI.start();
                                          documentService.getDocumentList(filter, function (result) {
                                              $scope.documentViewModel[index].childDocuments = result.data.Documents;
                                              idocumentblockUI.stop();
                                          },
                                           function (error) {
                                               idocumentblockUI.stop();
                                           }
                                         );
                                      }

                                      $scope.getDocumentLinkName = function (string) {
                                          var array = string.split(' ');
                                          return array[0];
                                      }

                                      $scope.onDocumentTabChange = function (tab, selectedParent) {
                                          $scope.selectedDocumentTab = tab;
                                          $scope.filter.DocumentId = selectedParent.DocumentId;
                                          filterDocumentList($scope.filter, tab);
                                      };

                                      $scope.isDocumentSelectedTab = function (tab) {
                                          return $scope.selectedDocumentTab === tab;
                                      };

                                      $scope.isfolderDocType = function (tab) {
                                          return $scope.folderDocType === tab;
                                      };

                                      Initialize();

                                  }]);



