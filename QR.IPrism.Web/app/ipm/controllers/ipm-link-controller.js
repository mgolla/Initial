
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.link.controller', ['$scope', '$http',  'linkService',  'analyticsService',  '$state', '$rootScope','blockUI',
                                  function ($scope, $http,   linkService,  analyticsService,  $state,$rootScope,blockUI) {
                                    //Controller Scope Initialization
									var ilinkblockUI = blockUI.instances.get('ipmlinkBlockUI');
                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $scope.linkViewModel;
                                          $scope.filter = {
                                              StaffID: ""
                                              
                                          }
                                          $scope.selectedLink = '';
                                          $scope.parentLinks;
                                          loadLinkList($scope.filter);
                                          $scope.selectedAlertTab = 0;
                                      }

									   function loadLinkList(filter) {
									     ilinkblockUI.start();
                                          linkService.getLinkList(filter, function(result) {
                                              $scope.linkViewModel = result.data.Links;
                                              $scope.parentLinks = getParentLinks($scope.linkViewModel);
                                              ilinkblockUI.stop();
                                              //$('#latestNewsTab').tabCollapse();
                                          },
                                           function (error) {
                                               ilinkblockUI.stop();
                                           }
                                         );
                                      }

									   $scope.isSelected = function (section) {

									       return $scope.selectedLink === section;
                                      }
									   $scope.getChildrenLinks = function (parent) {
									       var childrenlinks = $.grep($scope.linkViewModel, function (v, i) {

									           return v.LinkGroup == parent;
									       });
									      
									       return childrenlinks;
									   }
									   function getParentLinks(items) {
									       var result = [];
									       $.each(items, function (index, item) {
									           if ($.inArray(item["LinkGroup"], result) == -1) {
									               result.push(item["LinkGroup"]);
									           }
									       });
									       return result;
									   }
									   $scope.getLinkName = function (string) {
									       var array = string.split(' ');
									       return array[0];
									   }
									   $scope.onTabChange = function (tab) {
									       $scope.selectedAlertTab = tab;

									   };
									   $scope.isGetSelectedTab = function (tab) {
									       return $scope.selectedAlertTab === tab;

									   };
                                      Initialize();                                     
                                     
                                  }]);



