
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.documentfilemg.controller', ['$scope', '$http', 'documentService', 'fileService', 'analyticsService', '$state', '$rootScope', 'blockUI',
                                  function ($scope, $http,  documentService,fileService,  analyticsService,  $state, $rootScope, blockUI) {
                                      //Controller Scope Initialization
                                      var idocumentblockUI = blockUI.instances.get('ipmdocumentBlockUI');
                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $scope.folderDocType = "FOLDER";
                                          $scope.documentViewModel ;
                                          $scope.filter = {
                                              DocumentId: ""

                                          }
                                          $scope.selectedDocumentTab = 0;                                          
                                          
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
                                              DocumentId: DocumentId,
                                                isTab:false
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

                                      $scope.onClickBackLink = function (val) {
                                          if (val) {
                                              
                                              $state.go(val);
                                          }

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



