
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.document.controller', ['$scope', '$http', 'documentService', 'fileService', 'analyticsService', '$state', '$rootScope', 'blockUI',
                                  function ($scope, $http, documentService, fileService, analyticsService, $state, $rootScope, blockUI) {
                                      //Controller Scope Initialization
                                      var idocumentblockUI = blockUI.instances.get('ipmdocumentBlockUI');
                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $scope.folderDocType = "FOLDER";
                                          $scope.documentViewModel;
                                          $scope.filter = {
                                              DocumentId: "",
                                              DocumentPath: null

                                          }
                                          $scope.selectedDocumentTab = -1;

                                          loadDocumentList($scope.filter, $scope.documentViewModel);

                                      }

                                      function loadDocumentList(filter, model) {
                                          idocumentblockUI.start();
                                          documentService.getDocumentList(filter, function (result) {
                                              $scope.documentViewModel = result.data.Documents;


                                              idocumentblockUI.stop();
                                          },
                                           function (error) {
                                               idocumentblockUI.stop();
                                           }
                                         );
                                      }

                                      $scope.onSelectedChange = function (DocType, FileCode, fileID, DocumentId, selectedItem) {

                                          if ($scope.folderDocType.toLowerCase() != DocType.toLowerCase()) {
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
                                          } else {
                                              appSettings.SecondDocumentId = selectedItem.DocumentId;
                                              appSettings.DocumentPath = "/" + selectedItem.DocumentName;
                                              appSettings.DocumentId = DocumentId;
                                              $state.go('doclibfilemg');
                                          }
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
                                          $scope.filter.DocumentId = selectedParent.DocumentId;
                                          appSettings.DocumentId = selectedParent.DocumentId;
                                          appSettings.DocumentPath = "/" + selectedParent.DocumentName;
                                          $state.go('doclibfilemg');
                                      };

                                      $scope.loadNextLevel = function (tab, selectedParent) {
                                          $scope.selectedDocumentTab = tab;
                                          $scope.filter.DocumentId = selectedParent.DocumentId;
                                          $scope.filter.DocumentPath = "/Documents/" + selectedParent.DocumentName;
                                          //appSettings.DocumentId = selectedParent.DocumentId;
                                          //appSettings.DocumentPath = "/" + selectedParent.DocumentName;
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



