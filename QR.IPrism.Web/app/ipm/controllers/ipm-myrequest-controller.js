
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.myRequest.controller', ['$scope', '$http', 'myRequestService', 'sharedDataService', 'analyticsService', '$state', '$rootScope', 'blockUI', 'messages',
                                  function ($scope, $http, myRequestService, sharedDataService, analyticsService, $state, $rootScope, blockUI, messages) {
                                      //Controller Scope Initialization
                                      var imyRequestblockUI = blockUI.instances.get('ipmmyRequestBlockUI');
                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $scope.myRequestViewModel;
                                          $scope.filter = {
                                              StaffID: ""

                                          }
                                          $scope.selectedMyRequest = '';
                                          $scope.myRequestStatus;
                                          loadMyRequestList($scope.filter);
                                      }

                                      function loadMyRequestList(filter) {
                                          imyRequestblockUI.start();
                                          myRequestService.getMyRequestList(filter, function (result) {
                                              $scope.myRequestViewModel = result.data.MyRequests;

                                          },
                                           function (error) {
                                               imyRequestblockUI.stop();
                                           }
                                         );

                                          sharedDataService.getCommonInfo('MYREQSTS', function (result) {
                                              $scope.myRequestStatus = result;
                                              imyRequestblockUI.stop();
                                          },
                                           function (error) {
                                               imyRequestblockUI.stop();
                                           }
                                         );
                                      }
                                      $scope.onSelectedChange = function (item) {

                                          if (item.MyrequestType == "HOUSING") {

                                              switch (item.RequestType.toLocaleLowerCase()) {

                                                  //HOU1013: "Change Accommodation"  
                                                  //HOU1016: "Moving In"
                                                  case messages.HOU1016.toLocaleLowerCase():
                                                      $state.go('housing.housing-readonly-MovingIn', { RequestNumber: item.RequsetNo, RequestId: item.RequestId });
                                                      break;

                                                  case messages.HOU1013.toLocaleLowerCase():
                                                      $state.go('housing.housing-readonly-ChangeAcc', { RequestNumber: item.RequsetNo, RequestId: item.RequestId });
                                                      break;

                                                      //HOU1015: "Guest Accommodation"
                                                  case messages.HOU1015.toLocaleLowerCase():
                                                      $state.go('housing.housing-readonly-GuestAcc', { RequestNumber: item.RequsetNo, RequestId: item.RequestId });
                                                      break;

                                                      //HOU1017: "Moving Out of Company Accommodation"
                                                  case messages.HOU1017.toLocaleLowerCase():
                                                      $state.go('housing.housing-readonly-MoveOut', { RequestNumber: item.RequsetNo, RequestId: item.RequestId });
                                                      break;

                                                      //HOU1018: "Stay Out Request"
                                                  case messages.HOU1018.toLocaleLowerCase():
                                                      $state.go('housing.housing-readonly-StayOut', { RequestNumber: item.RequsetNo, RequestId: item.RequestId });
                                                      break;

                                                      //HOU1019: "Swap Rooms"
                                                  case messages.HOU1019.toLocaleLowerCase():
                                                      $state.go('housing.housing-readonly-SwapRoom', { RequestNumber: item.RequsetNo, RequestId: item.RequestId });
                                                      break;
                                              }
                                          }
                                      }


                                      $scope.getDate = function (string) {
                                          var array = string.split(' ');
                                          return array[0] + ' ' + array[1];
                                      }
                                      $scope.getMonthYear = function (string) {
                                          var array = string.split(' ');
                                          return array[1] + ' ' + array[2];
                                      }
                                      $scope.getClassByStatus = function (string) {
                                          var classCss = 'pending_req';
                                          if ($scope.myRequestStatus && $scope.myRequestStatus.length > 0) {
                                              var status = $.grep($scope.myRequestStatus, function (v, i) {

                                                  return v.Value.indexOf(string) > -1;
                                              });

                                              if (status && status.length > 0) {
                                                  classCss = status[0].Text;
                                                  if ((!classCss) || classCss.length < 1) {
                                                      classCss = 'pending_req';
                                                  }
                                              }
                                          }
                                          return classCss;
                                      }

                                      Initialize();

                                  }]);



