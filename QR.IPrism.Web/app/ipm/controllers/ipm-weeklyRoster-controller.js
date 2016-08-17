'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.weeklyRoster.controller', ['$scope', '$http', 'rosterService', 'analyticsService', '$state', '$rootScope',
                                  function ($scope, $http, rosterService, analyticsService, $state, $rootScope) {
                                      var rosterType = 1;//weekly
                                      var Initialize = function () {
                                          $rootScope.isRosterChanged = true;
                                          //$rootScope.handleRosterTypeRadioClick(rosterType);
                                          $rootScope.rosterDetailsUrl = '/app/ipm/partials/ipmRosterDetailsTabs.html';
                                          if ($rootScope.isNotSelectedRoster) {

                                              $rootScope.reloadData();

                                          } else {
                                              $rootScope.isNotSelectedRoster = true;
                                          }
                                      }

                                      $scope.onClickLeft = function () {
                                          if ((!(appSettings.isMobileHeader()))) {
                                              var widthOneItem = parseInt($(".roster_flexible_cell").first().css("width")) + parseInt($(".roster_flexible_cell").first().css("margin-left"))
                                              parseInt($(".roster_flexible_cell").first().css("margin-right")) + parseInt($(".roster_flexible_cell").first().css("padding-left")) +
                                              parseInt($(".roster_flexible_cell").first().css("padding-right"));

                                              $(".pg-cnt").animate({ scrollLeft: "-=" + widthOneItem });
                                              var scroll = $(".pg-cnt").scrollLeft();
                                             
                                              var eTop = $(".actvDate ").offset();
                                              if (eTop) {
                                               
                                                  var right = (parseInt(eTop.left) + parseInt($(".actvDate ").width()));
                                               
                                                  if (eTop.left < 0) {
                                                      $rootScope.resetSelection();
                                                      $('#roster_details').hide();
                                                  }

                                                  if (scroll < right) {
                                                      $rootScope.resetSelection();
                                                      $('#roster_details').hide();
                                                  }
                                              }
                                          }
                                      }

                                      $scope.onClickRight = function () {
                                          if ((!(appSettings.isMobileHeader()))) {
                                              var widthOneItem = parseInt($(".roster_flexible_cell").first().css("width")) + parseInt($(".roster_flexible_cell").first().css("margin-left"))
                                              parseInt($(".roster_flexible_cell").first().css("margin-right")) + parseInt($(".roster_flexible_cell").first().css("padding-left")) +
                                              parseInt($(".roster_flexible_cell").first().css("padding-right"));

                                              $(".pg-cnt").animate({ scrollLeft: "+=" + widthOneItem });
                                              //console.log($(".pg-cnt").scrollLeft());
                                              var eTop = $(".actvDate ").offset();
                                              if (eTop) {
                                              
                                                  if (eTop.left < 0) {
                                                    
                                                      $rootScope.resetSelection();
                                                      $('#roster_details').hide();
                                                  }
                                              }

                                          }
                                      }
                                     
                                      
                                      Initialize();

                                  }]);
