'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.overviewTab.controller', ['$scope', '$http', 'overviewService', 'analyticsService', '$state', '$rootScope', 'blockUI', 'toastr',
                                  function ($scope, $http, overviewService, analyticsService, $state, $rootScope, blockUI, toastr) {
                                      //Controller Scope Initialization
                                      var iblockUI = blockUI.instances.get('ipmBlockUIRoster');//ipmOverviewBlockUI
                                    
                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $rootScope.overviewViewModel;
                                      
                                          //$rootScope.flightLoadViewModel;
                                          $rootScope.filter = {
                                              StaffNo: $rootScope.selectedRosterItem != null ? $rootScope.selectedRosterItem.StaffID : "",
                                              FlightNo: $rootScope.selectedRosterItem != null ? $rootScope.selectedRosterItem.Flight : "",
                                              Sectorfrom: $rootScope.selectedRosterItem != null ? $rootScope.selectedRosterItem.Departure : "",
                                              SectorTo: $rootScope.selectedRosterItem != null ? $rootScope.selectedRosterItem.Arrival : "",
                                              STD: $rootScope.selectedRosterItem != null ? $rootScope.selectedRosterItem.DutyDateActual.toString() : ""
                                          }

                                          $rootScope.selectedOverview = '';
                                          if ($rootScope.selectedRosterItem != null) {
                                              loadOverviewList($rootScope.filter);
                                          }
                                        
                                          //$scope.dateDiffTravelingInDays();
                                         
                                      }

                                      function loadOverviewList(filter) {


                                          //$("#myrostersection").css("-webkit-filter", "blur(10px)");
                                          iblockUI.start();
                                          overviewService.getOverviewList(filter, function (result) {
                                              //  $("#myrostersection").css("background", "url(data:image/jpg;base64," + result.data.OverviewModel.BackgroundImage + ") no-repeat center top");
                                              $rootScope.overviewViewModel = result.data.OverviewModel;
                                              //$rootScope.ETDandETE();
                                              iblockUI.stop();
                                              $rootScope.OverviewIsReload = false;
                                              //$("#myrostersection").css("background-color: transparent;", "blur(0px)");
                                              //$("#myrostersection").fadeOut("slow", function () {

                                              //    $("#myrostersection").fadeIn("slow");
                                              //});


                                              //overviewService.getFlightLoadList(filter, function (result) {
                                              //    $rootScope.flightLoadViewModel = result.data;
                                              //},
                                              // function (error) {
                                              //     toastr.error(error.data);
                                              //     //console.log('loadOverviewList :'+error.data);
                                              // });

                                          },
                                            function (error) {
                                                iblockUI.stop();
                                            }
                                            , null, $rootScope.OverviewIsReload
                                            );

                                      }

                                      $scope.dateDiffInDays = function (from, to) {


                                          if (from && to && from.length > 0 && to.length > 0) {
                                              var fromD = new Date(from);
                                              var toD = new Date(to);
                                              var min = 1000 * 60;
                                              if (fromD && toD && fromD.getDay().toString().length > 0 && toD.getDay().toString().length > 0) {
                                                  var diff = Math.floor((toD - fromD) / min);
                                                  var minutes = diff % 60;
                                                  var hours = (diff-minutes) / 60;
                                                  var final = '';
                                                  if (hours > 0) {
                                                      final = hours + 'hr ';
                                                  }
                                                  if (minutes > 0) {
                                                      final =final+ minutes + 'min';
                                                  }

                                                  return final;
                                              }
                                              else {
                                                  return '';
                                              }
                                          }
                                      }
                                      




                                      $scope.isSelected = function (section) {

                                          return $rootScope.selectedOverview === section;
                                      }

                                      Initialize();

                                  }]);
