
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.visionMission.controller', ['$scope', '$http',  'visionMissionService',  'analyticsService',  '$state', '$rootScope','blockUI','sharedDataService',
                                  function ($scope, $http, visionMissionService, analyticsService, $state, $rootScope, blockUI, sharedDataService) {
                                    //Controller Scope Initialization
									var ivisionMissionblockUI = blockUI.instances.get('ipmvisionMissionBlockUI');
                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $scope.visionMissionViewModel;
                                          $scope.visionMissionImges = [];
                                         

										   $scope.filter = {
                                              StaffID: ""
                                              
                                          }
										   $scope.selectedVisionMission;
										   $scope.selectedImage = 0;
										   $scope.selectedVisionTab = 0

										   $scope.imageCount = 3;

										  

										   $scope.changeImageFolder();
                                         // loadVisionMissionList($scope.filter);                                              
                                      }

									   function loadVisionMissionList(filter) {
									     ivisionMissionblockUI.start();
                                          visionMissionService.getVisionMissionList(filter, function(result) {
                                              $scope.visionMissionViewModel = result.data.VisionMissions;
                                              if ($scope.visionMissionViewModel.length > 0) {
                                                  $scope.selectedVisionMission = $scope.visionMissionViewModel[0];
                                              }
                                              //$.each($scope.visionMissionViewModel, function (i, val) {
                                              //    $scope.visionMissionViewModel[i].FileContent = $scope.visionMissionViewModel[i].FileContent;

                                              //});
                                              ivisionMissionblockUI.stop();
                                              $scope.changeImage();
                                          },
                                           function (error) {
                                               ivisionMissionblockUI.stop();
                                           }
                                         );
                                          
                                      }

									   
									   $scope.changeImage = function () {
									       if ($scope.visionMissionViewModel && $scope.visionMissionViewModel.length > 0) {
									           window.setInterval(function () {

									               window.setTimeout(function () {
									                   $scope.selectedImage = $scope.selectedImage + 1;
									                   if ($scope.visionMissionViewModel.length <= $scope.selectedImage) {
									                       $scope.selectedImage = 0;
									                   }
									                   $scope.selectedVisionMission = $scope.visionMissionViewModel[$scope.selectedImage]

									                  
									               }, 100);
									           }, 6 * 1000);
									       }
									     

									   };

									  
									   $scope.changeImageFolder = function () {

									      
									       if ($scope.imageCount > 0) {
									           window.setInterval(function () {

									               window.setTimeout(function () {
									                   $scope.selectedImage = $scope.selectedImage + 1;
									                   if ($scope.imageCount <= $scope.selectedImage) {
									                       $scope.selectedImage = 0;
									                   }
									                   


									               }, 100);
									           }, 6 * 1000);
									       }


									   };

									 

									   $scope.onVisionTabChange = function (tab) {
									       $scope.selectedVisionTab = tab;

									   };
									   $scope.isVisionSelectedTab = function (tab) {
									       return $scope.selectedVisionTab === tab;

									   };
									   $scope.isNullOrEmpty = function (section) {

									       return (!section) || section === null || section.trim() === '';
									   }
                                      Initialize();                                     
                                     
                                  }]);



