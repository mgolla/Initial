'use strict'
angular.module('app.ipm.module').controller('ipm.searchDocument.controller',
                 ['$scope', '$rootScope', '$http', '$state', 'lookupDataService', 'searchService', 'fileService', '$sce', 'messages', 'blockUI','toastr',
      function ($scope, $rootScope, $http, $state, lookupDataService, searchService, fileService, $sce, messages, blockUI, toastr) {

          var ipmSearchDoc = blockUI.instances.get('ipmSearchDoc');
          var Initialize = function () {
              $scope.selectedPage = 1;
              $scope.searchDocumentVM;
              $scope.isSearchDocumentClick = false;
              $scope.searchDocFilter = {
                  Query: '',
                  Start: '',
                  MaxSearchResult: 10,
                  IndexDirectory: '',
                  DocType: 'File',
                  FileCode: 'DOCUMENTS'

              };

              if (messages.DOCSEARCHMAXRESULT) {
                  $scope.searchDocFilter.MaxSearchResult = messages.DOCSEARCHMAXRESULT;
              }
              if (messages.DOCSEARCHINDEXDIRECTORY) {
                  $scope.searchDocFilter.IndexDirectory = messages.DOCSEARCHINDEXDIRECTORY;
              }
              if (messages.DOCSEARCHDOCTYPE) {
                  $scope.searchDocFilter.DocType = messages.DOCSEARCHDOCTYPE;
              }
              if (messages.DOCSEARCHFILECODE) {
                  $scope.searchDocFilter.FileCode = messages.DOCSEARCHFILECODE;
              }

              $scope.pageList = [];
          }

          function loadSearchList(filter) {

              ipmSearchDoc.start();
              if ($scope.searchDocFilter.Query) {
                  searchService.getDocumentSearchResult(filter, function (result) {

                      $scope.searchDocumentVM = result.data;
                      if ($scope.searchDocFilter.Start == 0) {
                          $scope.pageList = [];
                          //$scope.searchDocFilter.Start = $scope.searchDocumentVM.StartAt;
                          for (var i = 1; i <= $scope.searchDocumentVM.PageCount ; i++) {
                              $scope.pageList.push(i);
                          }
                      }
                      $scope.isSearchDocumentClick = true;
                      ipmSearchDoc.stop();
                  },
                   function (error) {
                       ipmSearchDoc.stop();
                   }
                 );
              }
          }
          $scope.decodeText = function (data) {
              return $sce.trustAsHtml(data);
          }

          $scope.onClickSearch = function () {
              var val = $scope.searchDocFilter.Query;

              if (val && val !=null && val.toString().trim().length >0) {
                  $scope.searchDocFilter.Start = 0;
                  $scope.selectedPage = 1;
                  loadSearchList($scope.searchDocFilter);

              } else {
                  toastr.warning('Please fill  in required fields');
              }

          }

          $scope.onClickPage = function (page) {
              $scope.selectedPage = page;
              $scope.searchDocFilter.Start = ((page - 1) * $scope.searchDocumentVM.MaxResults);
              loadSearchList($scope.searchDocFilter);
          }


          $scope.onClickLink = function (section) {
              var filterPara = {
                  DocType: section.DocType,
                  FileCode: section.FileCode,
                  FileId: section.Id,
                  DocumentId: section.Id,
                  DocumentName: section.Title,
                  isTab: false
              };

              fileService.openDocTypes(filterPara);
          }

          $scope.isNotSelectedPage = function (section) {

              return ($scope.selectedPage) != section;
          }



          Initialize();
      }]);


