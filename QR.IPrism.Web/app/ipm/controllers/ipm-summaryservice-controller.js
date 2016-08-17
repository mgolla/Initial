
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.summaryService.controller', ['$scope', '$http', 'summaryServiceService', 'fileService', 'analyticsService', '$state', '$rootScope', 'blockUI', '$sce', '$stateParams', 'messages',
                                  function ($scope, $http, summaryServiceService, fileService, analyticsService, $state, $rootScope, blockUI, $sce, $stateParams, messages) {
                                      //Controller Scope Initialization
                                      var iblockUI = blockUI.instances.get('ipmBlockUIRoster');//ipmSOSBlockUI
                                      function Initialize() {
                                          $scope.Yes = 1;
                                          $scope.No = 2;
                                          $scope.Loading = 0;
                                          $scope.isDataLoad = $scope.Loading;

                                          //Declare all scop properties
                                          $scope.summaryServiceViewModel;
                                         
                                          $scope.fileCodeMenuCard='MENU_CARD';

                                          if (messages.FILECODEMENUCARD && messages.FILECODEMENUCARD != null && messages.FILECODEMENUCARD != '') {
                                              $scope.fileCodeMenuCard = messages.FILECODEMENUCARD;
                                          }
                                        

                                          $scope.selectedSummaryService = '';
                                          $scope.isShowMoreInfoP = false;
                                          $scope.isShowMoreInfoE = false;
                                          $scope.isShowOtherInfoP = false;
                                          $scope.isShowOtherInfoE = false;

                                          $scope.selectedMealP ;
                                          $scope.selectedMealE ;
                                          $scope.selectedOtherInfoP ;
                                          $scope.selectedOtherInfoE;
                                          if (($scope.searchParmeter && $scope.searchParmeter != null && $scope.searchParmeter.FlightNumber.length > 0)
                                              || ($rootScope.selectedRosterItem && $rootScope.selectedRosterItem.Flight > 0)) {

                                              if ($scope.searchParmeter && $scope.searchParmeter != null && $scope.searchParmeter.FlightNumber) {

                                                  $scope.filter = {
                                                      FlightNo: $scope.searchParmeter.FlightNumber,
                                                      Sectorfrom: $scope.searchParmeter.SectorFrom,
                                                      SectorTo: $scope.searchParmeter.SectorTo

                                                  }
                                                  loadSummaryServiceList($scope.filter, true);

                                              } else {
                                                  if ($rootScope.selectedRosterItem && $rootScope.selectedRosterItem.Flight) {
                                                      $scope.filter = {
                                                          FlightNo: $rootScope.selectedRosterItem.Flight,
                                                          Sectorfrom: $rootScope.selectedRosterItem.Departure,
                                                          SectorTo: $rootScope.selectedRosterItem.Arrival

                                                      }
                                                      loadSummaryServiceList($scope.filter,$rootScope.SOSIsReload);
                                                  }

                                              }
                                          }
                                      }

									   function loadSummaryServiceList(filter,isReload) {
									       iblockUI.start();
                                          summaryServiceService.getSummaryServiceList(filter, function(result) {
                                              $scope.summaryServiceViewModel = result.data.SOSModel;

                                              $scope.isDataLoad = $scope.summaryServiceViewModel.IsDataLoaded;
                                              $rootScope.SOSIsReload = false;
                                              iblockUI.stop();
                                          },
                                            function (error) {
                                                iblockUI.stop();
                                            }, null, isReload
                                            );
                                         
                                      }

									   $scope.getPara = function (section) {

									       return section;
									   }


									   $scope.isSelected = function (section) {

									       return $scope.selectedSummaryService === section;
                                      }
									   $scope.isNotNullOrEmpty = function (section) {

									       return ((section) && section != null && section.toString().trim() !== '' && section.toString().trim().length >= 0);
									   }

									   $scope.isNotNullOrEmptyMovieSnack = function (meal) {

									       return (meal && meal.Moviesnack && meal.MoviesnackValue && $scope.isNotNullOrEmpty(meal.Moviesnack) && $scope.isNotNullOrEmpty(meal.MoviesnackValue) && meal.MoviesnackValue.toString().trim() != '0');
									   }
									   $scope.onTabChangeMoreInfoP = function (meal , isAssign) {

									       if ($scope.isShowMoreInfoP) {
									           $scope.isShowMoreInfoP = false;
									       } else {
									           $scope.isShowMoreInfoP = true;
									       }
									       if (isAssign) {
									           $scope.selectedMealP = meal;
									       }
									   }

									   $scope.isSelectedTabMoreInfoP = function () {
									       return (!$scope.isShowMoreInfoP) ;

									   };
									   $scope.onTabChangeMoreInfoE = function (meal, isAssign) {

									       if ($scope.isShowMoreInfoE) {
									           $scope.isShowMoreInfoE = false;
									       } else {
									           $scope.isShowMoreInfoE = true;
									       }
									       if (isAssign) {
									           $scope.selectedMealE = meal;
									       }
									   }
									   $scope.isSelectedTabMoreInfoE = function () {
									       return (!$scope.isShowMoreInfoE);

									   };
									   $scope.onTabChangeOtherInfoP = function () {

									       if ($scope.isShowOtherInfoP) {
									           $scope.isShowOtherInfoP = false;
									       } else {
									           $scope.isShowOtherInfoP = true;
									       }
									   }
									   $scope.isSelectedTabOtherInfoP = function () {
									       return (!$scope.isShowOtherInfoP );

									   };
									   $scope.onTabChangeOtherInfoE = function () {

									       if ($scope.isShowOtherInfoE) {
									           $scope.isShowOtherInfoE = false;
									       } else {
									           $scope.isShowOtherInfoE = true;
									       }
									   }
									   $scope.isSelectedTabOtherInfoE = function () {
									       return (!$scope.isShowOtherInfoE) ;

									   };
									   $scope.onFileSelectedChangeSOS = function (DocType, FileCode, fileID, DocumentId) {

									       var filterPara = {
									           DocType: DocType,
									           FileCode: FileCode,
									           FileId: fileID,
									           DocumentId: DocumentId,
									           isTab: true
									       };

									       fileService.openDocTypes(filterPara, function (result) {

									       },
                                           function (error) {
                                               console.log('error : ' + error);
                                           });
									   }

									  

                                      Initialize();                                     
                                     
                                  }]);


angular
    .module('app.ipm.module').filter('ifEmpty', function () {
    return function (input, defaultValue) {
        if (angular.isUndefined(input) || input === null || input === '') {
            return defaultValue;
        }

        return input;
    }
});
