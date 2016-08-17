'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.roster.controller',
    ['$scope', '$http', 'rosterService', 'analyticsService', 'messages', 'sharedDataService', '$state', '$rootScope', '$timeout',
        '$compile', 'blockUI', '$stickyState', 'toastr', 'appSettings', 'deviceDetector','$filter',
                                  function ($scope, $http, rosterService, analyticsService, messages, sharedDataService, $state, $rootScope, $timeout, $compile, blockUI, $stickyState, toastr, appSettings, deviceDetector,$filter) {
                                      var ipmBlockUIRoster = blockUI.instances.get('ipmBlockUIRoster');
                                      //Controller Scope Initialization
                                      var Initialize = function () {
                                          //Declare all scop properties
                                          $rootScope.FilterSearchData = null;
                                          $rootScope.isRosterLoaded = false;

                                          $rootScope.YesRoster = 1;
                                          $rootScope.NoRoster = 2;
                                          var pathArray = window.location.href.split('/');
                                          var protocol = pathArray[0];
                                          var host = pathArray[2];
                                          $rootScope.baseURL = protocol + '//' + host + '/';

                                          $rootScope.WeeklyRoster = 1;
                                          $rootScope.MonthlyRoster = 2;

                                          $rootScope.tabRosterContentUrl = '/app/ipm/partials/ipmRosterWeekly.html';

                                          $rootScope.selectedRDetailURL = '';

                                          $rootScope.Overview = 1;
                                          $rootScope.CrewDet = 2;
                                          $rootScope.SOS = 3;
                                          $rootScope.Hotel = 4;
                                          $rootScope.StationInfo = 5;
                                          $rootScope.Loading = 6;
                                          $rootScope.Training = 7;

                                          $rootScope.OverviewIsReload = false;
                                          $rootScope.CrewDetIsReload = false;
                                          $rootScope.SOSIsReload = false;
                                          $rootScope.HotelIsReload = false;
                                          $rootScope.StationInfoIsReload = false;


                                          $scope.OverviewURL = '/app/ipm/partials/ipmOverviewTab.html';
                                          $scope.CrewDetURL = '/app/ipm/partials/ipmCrewTab.html';
                                          $scope.SOSURL = '/app/ipm/partials/ipmSOSTab.html';
                                          $scope.HotelURL = '/app/ipm/partials/ipmHotelTab.html';
                                          $scope.StationInfoURL = '/app/ipm/partials/ipmStationTab.html';
                                          $scope.LoadingURL = '/app/ipm/partials/ipmLoading.html';
                                          $scope.TrainingURL = '/app/ipm/partials/ipmTraining.html';


                                          $rootScope.ETDFlight = '';
                                          $rootScope.ETAFlight = '';
                                          $rootScope.calculatedBlockHours = '';


                                          $rootScope.backgroundURL = '';

                                          $rootScope.rosterViewModel;

                                          $rootScope.extraRosters = [];
                                          $rootScope.uctTime;
                                          $rootScope.selectedIndex;
                                          $rootScope.TimeFormats = [];
                                          $('#selectedTimeFormat').text('Local');
                                          $("#Radio3").prop("checked", true)
                                          $rootScope.selectedTimeFormat = 3;
                                          $rootScope.resetSelection();
                                          $rootScope.isNotSelectedRoster = true;
                                          $rootScope.selectedTab = 1;
                                          $rootScope.BlockedHours = 0;
                                          $rootScope.BlockedHoursActual = 0;
                                          $rootScope.isRosterChanged = false;
                                          $rootScope.rosterViewModel = {
                                              TimeFormat: 3
                                          }

                                          $rootScope.codes = [];
                                          $rootScope.departures = [];
                                          $rootScope.filter = {
                                              StaffID: "",
                                              StartDate: null,
                                              EndDate: null,
                                              RosterTypeID: 0,
                                              TimeFormat: 3,
                                              FlightDigits: 3,
                                              IsLanding: true,
                                              LeftValue: -10,
                                              RightValue: 10,
                                              LastSelectedDate: null
                                          }

                                          $rootScope.selectedRosterItem;
                                          $rootScope.selectedRosterItemParent;
                                          $rootScope.selectedRosterItemParentIndex;
                                          setStyleFormatting();
                                          $rootScope.isMobile = deviceDetector.isMobile();

                                          $rootScope.selectedRosterType = $rootScope.WeeklyRoster;
                                          $rootScope.filter.RosterTypeID = $rootScope.WeeklyRoster;

                                          $rootScope.UTCTime = 1;
                                          $rootScope.DohaTime = 2;
                                          $rootScope.LocalTime = 3;

                                          $rootScope.isTraining = false;
                                          if (messages.FLIGHTDIGITS && messages.FLIGHTDIGITS.toString().trim().length > 0) {
                                              $rootScope.filter.FlightDigits = messages.FLIGHTDIGITS;
                                          }

                                      }
                                      $scope.getTravelingTIme = function (Para) {
                                          var min = 0;
                                          if (Para && Para.length > 0) {
                                              var hoursMin = Para.replace('hrs', '').replace('hr', '').split(':');
                                              if (hoursMin.length > 1) {
                                                  min = parseInt(hoursMin[1]) + parseInt(hoursMin[0] * 60)
                                              } else {
                                                  min = parseInt(hoursMin[0] * 60)
                                              }
                                          }
                                          return min;
                                      }

                                      $scope.getTravelingTImeByDay = function (diff) {
                                          var final = "";
                                          if (diff > 0) {
                                              var minutes = diff % 60;
                                              var hours = (diff - minutes) / 60;
                                              var hr = hours;
                                              if (parseInt(hr) < 10) {
                                                  hr = '0' + hr;
                                              }

                                              if (hours > 1) {
                                                  final = hr.toString();
                                                  if (minutes > 0) {
                                                      var mint = minutes;
                                                      if (parseInt(mint) < 10) {
                                                          mint = '0' + mint;
                                                      }
                                                      final = final + ":" + mint;
                                                  }
                                                  final = final + " hrs ";
                                              }
                                              else {
                                                  final = hr.toString();
                                                  if (minutes > 0) {
                                                      var mint = minutes;
                                                      if (parseInt(mint) < 10) {
                                                          mint = '0' + mint;
                                                      }

                                                      final = final + ":" + mint;
                                                  }
                                                  final = final + " hr";
                                              }
                                          }

                                          return final;
                                      }

                                      $rootScope.resetSelection = function () {
                                          $rootScope.selectedRoster = -1;
                                          $rootScope.selectedRosterChild = -1;
                                          $rootScope.selectedRosterEvent = -1;
                                          $rootScope.clickedRosterId = -1;

                                          $rootScope.OverviewIsReload = true;
                                          $rootScope.CrewDetIsReload = true;
                                          $rootScope.SOSIsReload = true;
                                          $rootScope.HotelIsReload = true;
                                          $rootScope.StationInfoIsReload = true;
                                      }

                                      // reload the data from weekly and monthly controller 
                                      $rootScope.reloadData = function () {

                                          $rootScope.rosterViewModel.Rosters = [];




                                          $rootScope.isWideScreen = true;


                                          $rootScope.rosterViewModel;
                                          $rootScope.filter = {
                                              StaffID: "",
                                              StartDate: null,
                                              EndDate: null,
                                              RosterTypeID: $rootScope.selectedRosterType,
                                              TimeFormat: 3,
                                              IsLanding: true,
                                              LeftValue: -10,
                                              RightValue: 10,
                                              LastSelectedDate: null
                                          }

                                          $rootScope.filter.RosterTypeID = $rootScope.selectedRosterType,
                                          loadRosterList($rootScope.filter);

                                      }

                                      //load the data 
                                      function loadRosterList(filter) {
                                          $rootScope.isRosterLoaded = false;
                                          $rootScope.BlockedHours = 0;
                                          $rootScope.BlockedHoursActual = 0;

                                          $rootScope.stations = [];
                                          $rootScope.codes = [];
                                          $rootScope.departures = [];
                                          $rootScope.lookupViewModelCodeEx = [];
                                          $rootScope.lookupViewModelUTCDiff = [];

                                          ipmBlockUIRoster.start();
                                          if (messages.LEFTVALUE && messages.LEFTVALUE != '' && messages.LEFTVALUE != 0) {
                                              $rootScope.filter.LeftValue = parseInt(messages.LEFTVALUE);
                                              $rootScope.filter.LeftValue = -$rootScope.filter.LeftValue;
                                          }

                                          if (messages.RIGHTVALUE && messages.RIGHTVALUE != '' && messages.RIGHTVALUE != 0) {
                                              $rootScope.filter.RightValue = parseInt(messages.RIGHTVALUE);
                                          }
                                          if (filter.TimeFormat && filter.TimeFormat != null && filter.TimeFormat > 0) {
                                              $rootScope.selectedTimeFormat = filter.TimeFormat;
                                          }

                                          $rootScope.selectedRosterItem = null;
                                          $rootScope.selectedRoster = -1;
                                          $rootScope.clickedRosterId = -1;
                                          $rootScope.selectedRosterEvent = -1;

                                          // ipmBlockRoster.start();

                                          if ($rootScope.selectedRosterType == 1) {
                                              rosterService.getWeeklyRosterList(filter, function (result) {
                                                  $rootScope.rosterViewModel = result.data;
                                                  if ($rootScope.rosterViewModel.IsWorking && $rootScope.rosterViewModel.IsDataLoaded == $rootScope.YesRoster) {

                                                      $rootScope.selectedRosterType = $rootScope.WeeklyRoster;
                                                      $rootScope.filter.RosterTypeID = $rootScope.WeeklyRoster;




                                                      $rootScope.isRosterChanged = false;

                                                      if ((appSettings.isMobileHeader())) {
                                                          if ($rootScope.rosterViewModel && $rootScope.rosterViewModel.SelectedRosterTempID && $rootScope.rosterViewModel.SelectedRosterTempID > 0) {
                                                              setTimeout(function () {

                                                                  $("#roosterDay" + $rootScope.rosterViewModel.SelectedRosterTempID).attr("tabindex", -1).focus();
                                                                  $("#rostercell" + $rootScope.rosterViewModel.SelectedRosterTempID).addClass("mob-rostersel-onload");



                                                              }, 2);
                                                          }


                                                      } else {

                                                          setTimeout(function () {


                                                              var pos = $('#rostercell' + $rootScope.rosterViewModel.SelectedRosterTempID).position().left; //get left position of li
                                                              var currentscroll = $(".pg-cnt").scrollLeft(); // get current scroll position

                                                              pos = (pos + currentscroll) - (window.innerWidth * 0.42); // for center position if you want adjust then change this

                                                              $('.pg-cnt').animate({
                                                                  scrollLeft: pos
                                                              });

                                                              $("#roosterDay" + $rootScope.rosterViewModel.SelectedRosterTempID).attr("tabindex", -1).focus();
                                                              $("#roosterDay" + $rootScope.rosterViewModel.SelectedRosterTempID).click();

                                                          }, 2);

                                                      }

                                                      $rootScope.ETDandETE();
                                                      $rootScope.isRosterLoaded = true;
                                                  } else {
                                                      if ($rootScope.rosterViewModel.IsWorking == false) {
                                                          if ($rootScope.rosterViewModel.ErrorMgs && $rootScope.rosterViewModel.ErrorMgs.toString().length > 0) {
                                                              toastr.warning($rootScope.rosterViewModel.ErrorMgs);
                                                          } else {
                                                              toastr.warning("Roster interface  not working !");
                                                          }
                                                      }

                                                  }
                                                  ipmBlockUIRoster.stop();

                                              }, null, null, $rootScope.isRosterChanged);
                                          } else {
                                              rosterService.getMonthlyRosterList(filter, function (result) {
                                                  $rootScope.rosterViewModel = result.data;
                                                  var roster2 = $filter('limitTo')($rootScope.rosterViewModel.Rosters, 16, 16);

                                                  if ($rootScope.rosterViewModel.IsWorking && $rootScope.rosterViewModel.IsDataLoaded == $rootScope.YesRoster) {
                                                      $rootScope.selectedRosterType = $rootScope.MonthlyRoster;
                                                      $rootScope.filter.RosterTypeID = $rootScope.MonthlyRoster;

                                                      $rootScope.extraRosters = [];

                                                      for (var i = 0; i < (32 - $rootScope.rosterViewModel.Rosters.length) ; i++) {
                                                          $rootScope.extraRosters.push(i);
                                                      }



                                                      if ((appSettings.isMobileHeader())) {
                                                          if ($rootScope.rosterViewModel && $rootScope.rosterViewModel.SelectedRosterTempID && $rootScope.rosterViewModel.SelectedRosterTempID > 0) {
                                                              setTimeout(function () {

                                                                  $("#roosterDay" + $rootScope.rosterViewModel.SelectedRosterTempID).attr("tabindex", -1).focus();
                                                                  $("#rostercell" + $rootScope.rosterViewModel.SelectedRosterTempID).addClass("mob-rostersel-onload");
                                                              }, 2);

                                                          }


                                                      } else {

                                                          setTimeout(function () {

                                                              $("#roosterDay" + $rootScope.rosterViewModel.SelectedRosterTempID).click();

                                                          }, 2);

                                                          $rootScope.selectedRoster = $rootScope.rosterViewModel.SelectedRosterTempID;
                                                          $rootScope.selectedRosterEvent = $rootScope.rosterViewModel.SelectedRosterTempID;
                                                          $rootScope.clickedRosterId = $rootScope.rosterViewModel.SelectedRosterTempID;
                                                      }




                                                      $.each($rootScope.rosterViewModel.Rosters, function (i, val) {
                                                          var traverlTime = 0;
                                                          if (val.Departure != null && val.Departure.toString().trim().length > 0 && val.BTime != null && val.BTime.toString().trim().length > 0) {
                                                              $rootScope.BlockedHours = parseInt(val.BTime) + parseInt($rootScope.BlockedHours);
                                                              traverlTime = $scope.getTravelingTIme($rootScope.rosterViewModel.Rosters[i].FlightTravellingTime);

                                                          }
                                                          if (val.Rosters && val.Rosters != null && val.Rosters.length > 0) {
                                                              $.each(val.Rosters, function (ii, vali) {
                                                                  if (vali.Departure != null && vali.Departure.toString().trim().length > 0 && vali.BTime != null && vali.BTime.toString().trim().length > 0) {
                                                                      $rootScope.BlockedHours = parseInt(vali.BTime) + parseInt($rootScope.BlockedHours);
                                                                      traverlTime = traverlTime + $scope.getTravelingTIme($rootScope.rosterViewModel.Rosters[i].FlightTravellingTime);
                                                                  }
                                                              });
                                                          }
                                                          $rootScope.rosterViewModel.Rosters[i].FlightTravellingTime = $scope.getTravelingTImeByDay(traverlTime);

                                                      });


                                                      $.each($rootScope.rosterViewModel.Rosters, function (i, val) {
                                                          if (val.Cc != null && val.Cc.toString().trim().length > 0) {
                                                              $rootScope.codes.push(val.Cc);
                                                          }
                                                          if (val.Rosters && val.Rosters != null && val.Rosters.length > 0) {
                                                              $.each(val.Rosters, function (ii, vali) {
                                                                  if (vali.Cc != null && vali.Cc.toString().trim().length > 0) {
                                                                      $rootScope.codes.push(vali.Cc);
                                                                  }
                                                              });
                                                          }
                                                      });

                                                      $rootScope.codes = $.grep($rootScope.codes, function (el, index) {

                                                          return index === $.inArray(el, $rootScope.codes);

                                                      });



                                                      $.each($rootScope.rosterViewModel.Rosters, function (i, val) {


                                                          //if (val.DutyDateActualDate) {
                                                          //    var dateFull = val.DutyDateActualDate.split(' ');
                                                          //    var dateVal = dateFull[0].split('-');                                                           

                                                          //    $rootScope.rosterViewModel.Rosters[i].DutyDateActualDate = new Date('"' + dateVal[1] + ',' + dateVal[0] + ',' + dateVal[2]+'"' );
                                                          //} else {
                                                          //    var dateVal = val.DutyDate.split(' ');                                                         

                                                          //    $rootScope.rosterViewModel.Rosters[i].DutyDateActualDate = new Date('"' + dateVal[1] + ',' + dateVal[0] + ',' + dateVal[2] + '"');
                                                          //}

                                                          if (val.Departure != null && val.Departure.toString().trim().length > 0) {
                                                              $rootScope.departures.push('SYSDATE:' + val.Departure);
                                                              $rootScope.stations.push(val.Departure);

                                                          }
                                                          if (val.Arrival != null && val.Arrival.toString().trim().length > 0) {
                                                              $rootScope.departures.push('SYSDATE:' + val.Arrival);
                                                              $rootScope.insertHotel = true;
                                                              $rootScope.stations.push(val.Arrival);
                                                          }

                                                          if (val.Rosters && val.Rosters != null && val.Rosters.length > 0) {
                                                              $.each(val.Rosters, function (ii, vali) {
                                                                  if (vali.Departure != null && vali.Departure.toString().trim().length > 0) {
                                                                      $rootScope.departures.push('SYSDATE:' + vali.Departure);
                                                                      $rootScope.stations.push(vali.Departure);

                                                                  }
                                                                  if (vali.Arrival != null && vali.Arrival.toString().trim().length > 0) {
                                                                      $rootScope.departures.push('SYSDATE:' + vali.Arrival);
                                                                      $rootScope.stations.push(vali.Arrival);
                                                                      $rootScope.insertHotel = true;
                                                                  }
                                                              });
                                                          }
                                                      });

                                                      $rootScope.departures = $.grep($rootScope.departures, function (el, index) {
                                                          return index === $.inArray(el, $rootScope.departures);
                                                      });


                                                      if ($rootScope.BlockedHours != null && $rootScope.BlockedHours.toString().trim().length > 0) {
                                                          var btime = ($rootScope.BlockedHours / 60);
                                                          $rootScope.BlockedHoursActual = btime;
                                                          var hours = btime.toString().split('.');
                                                          var min = $rootScope.BlockedHours % 60;
                                                          var hr = hours[0];
                                                          if (parseInt(hr) < 10) {
                                                              hr = '0' + hr;
                                                          }
                                                          if (parseInt(min) > 0) {

                                                              var mint = min;
                                                              if (parseInt(mint) < 10) {
                                                                  mint = '0' + mint;
                                                              }
                                                              $rootScope.BlockedHours = hr + "hr " + mint + 'min';
                                                          } else {
                                                              $rootScope.BlockedHours = hr + "hr ";
                                                          }

                                                          if (parseInt(hr) > 0) {

                                                              if (min > 0) {
                                                                  var mint = min;
                                                                  if (parseInt(mint) < 10) {
                                                                      mint = '0' + mint;
                                                                  }
                                                                  $rootScope.BlockedHours = hr + ":" + mint + ' hrs';
                                                              } else {
                                                                  $rootScope.BlockedHours = hr + ' hrs';
                                                              }

                                                          } else {
                                                              if (min > 0) {
                                                                  var mint = min;
                                                                  if (parseInt(mint) < 10) {
                                                                      mint = '0' + mint;
                                                                  }
                                                                  $rootScope.BlockedHours = hr + ":" + mint + ' hr';
                                                              } else {
                                                                  $rootScope.BlockedHours = hr + ' hr';
                                                              }
                                                          }
                                                      }



                                                      $scope.filterCode = $rootScope.codes.join(",");
                                                      if ($rootScope.codes.length > 0) {
                                                          loadLookupList($scope.filterCode);

                                                      }



                                                      if ($rootScope.departures.length > 0) {
                                                          $scope.filterData = {
                                                              Type: $rootScope.departures.join("#")

                                                          }


                                                          loadUTCDiffList($scope.filterData);
                                                      }

                                                      $rootScope.ETDandETE();

                                                      $rootScope.isRosterLoaded = true;
                                                  } else {
                                                      if ($rootScope.rosterViewModel.IsWorking == false) {
                                                          if ($rootScope.rosterViewModel.ErrorMgs && $rootScope.rosterViewModel.ErrorMgs.toString().length > 0) {
                                                              toastr.warning($rootScope.rosterViewModel.ErrorMgs);
                                                          } else {
                                                              toastr.warning("Roster interface  not working !");
                                                          }
                                                      }

                                                  }
                                                  ipmBlockUIRoster.stop();
                                              });
                                          }



                                      }


                                      function loadUTCDiffList(filter) {

                                          rosterService.getUTCDiffs(filter, function (result) {
                                              $rootScope.lookupViewModelUTCDiff = result.data;

                                          },
                                          function (error) {

                                          }
                                          );
                                      }

                                      function loadLookupList(filter) {

                                          rosterService.getCodeExplanations(filter, function (result) {
                                              $rootScope.lookupViewModelCodeEx = result;

                                          },
                                          function (error) {

                                          }
                                          );
                                      }

                                      $rootScope.SplitDate = function (string, nb) {
                                          var array = string.split(' ');
                                          return array[nb];
                                      }

                                      //To filtering  data 
                                      function loadFilterRosterList() {

                                          $('[id^=roster_details]').hide();
                                          $rootScope.filter.StartDate = $scope.rosterViewModel.StartDate,
                                          $rootScope.filter.EndDate = $scope.rosterViewModel.EndDate,
                                          $rootScope.filter.RosterTypeID = $rootScope.selectedRosterType,
                                          //$rootScope.filter.TimeFormat = $scope.rosterViewModel.TimeFormat,
                                          $rootScope.filter.IsLanding = false;
                                          loadRosterList($rootScope.filter)

                                      }

                                      $scope.isSelected = function (section) {

                                          return $rootScope.selectedRoster === section;
                                      }
                                      $rootScope.isSelectedChild = function (section) {

                                          return $rootScope.selectedRosterChild === section;
                                      }


                                      $rootScope.isSelectedMobile = function (section) {

                                          if (window.innerWidth > 769) {
                                              return $rootScope.selectedRoster === section;
                                          } else {
                                              if ($rootScope.selectedRosterItem) {
                                                  return true;
                                              } else {
                                                  return false;
                                              }
                                          }

                                      }

                                      $rootScope.isSelectedRosterLoad = function (section) {

                                          if (window.innerWidth > 769) {
                                              return $rootScope.selectedRoster === section;
                                          } else {
                                              var item =$rootScope.selectedRosterItem;
                                              if ($rootScope.selectedRoster === section && item  && ((item.Departure && item.Departure != null && item.Departure != '' && item.Flight != '') ||
                                              (item.Cc && item.Cc != null && item.Cc.toString().trim().length > 0 && item.Address1Training != null && item.Address1Training.toString().trim().length > 0))) {

                                                  return true;
                                              } else {
                                                  return false;
                                              }
                                          }
                                         
                                      }

                                      


                                      $rootScope.isSelectedMobileRoster = function (section) {

                                          if (window.innerWidth > 769) {
                                              return false;
                                          } else {
                                              if ($rootScope.clickedRosterId >= 0) {
                                                  return (!($rootScope.clickedRosterId === section));
                                              } else {
                                                  return false;
                                              }
                                          }

                                      }

                                      $rootScope.isclickedMobileRoster = function (section) {

                                          if (window.innerWidth > 769) {
                                              return false;
                                          } else {
                                              if ($rootScope.clickedRosterId >= 0) {
                                                  return ($rootScope.clickedRosterId === section);
                                              } else {
                                                  return false;
                                              }
                                          }

                                      }

                                      $rootScope.isclickedRosterEventMobile = function (section) {

                                          if (window.innerWidth > 769) {
                                              return false;
                                          } else {
                                              if ($rootScope.selectedRosterEvent >= 0) {
                                                  return ($rootScope.selectedRosterEvent === section);
                                              } else {
                                                  return false;
                                              }
                                          }

                                      }



                                      $rootScope.isNotBackButtonShow = function () {

                                          if (window.innerWidth > 769) {
                                              return true;
                                          } else {
                                              if ($rootScope.clickedRosterId >= 0) {
                                                  return false;
                                              } else {
                                                  return true;
                                              }
                                          }
                                      }

                                      $rootScope.isRosterDetailShow = function () {
                                          if ($rootScope.clickedRosterId >= 0) {
                                              return true;
                                          } else {
                                              return false;
                                          }
                                      }

                                      $rootScope.isBackButtonShow = function () {
                                          if (window.innerWidth > 769) {
                                              return false;
                                          } else {
                                              if ($rootScope.clickedRosterId >= 0) {
                                                  return true;
                                              } else {
                                                  return false;
                                              }
                                          }
                                      }

                                      $scope.onClickBackButton = function () {
                                          $rootScope.clickedRosterId = -1;
                                          $rootScope.selectedRosterEvent = -1;
                                          $rootScope.selectedRoster = null;
                                          $('#roster_details').hide();

                                          if ($rootScope.selectedRosterType && $rootScope.selectedRosterType == 1) {
                                              getDefaultBackground();
                                          } else {
                                              getDefaultBackgroundMonthly();
                                          }
                                      }

                                      $rootScope.isSelectedRosterType = function (section) {
                                          return $rootScope.selectedRosterType === section;
                                      }

                                      $rootScope.nextButtonClickHandler = function () {
                                          $rootScope.resetSelection();
                                          $rootScope.filter.IsNext = 1;
                                          if ($rootScope.selectedTimeFormat) {
                                          $rootScope.filter.TimeFormat = $rootScope.selectedTimeFormat;
                                          }
                                          
                                          loadFilterRosterList();
                                      };

                                      $rootScope.onTabChange = function (tab) {
                                          $rootScope.selectedTab = tab;

                                      };
                                      $rootScope.isSelectedTab = function (tab) {

                                          return $rootScope.selectedTab === tab;

                                      };

                                      $rootScope.isMonthly = function () {

                                          if ($rootScope.selectedRosterType) {
                                              return $rootScope.selectedRosterType == $rootScope.MonthlyRoster;
                                          }
                                          else {
                                              return true;
                                          }

                                      };

                                      $rootScope.isSelectedRosterEvent = function (tempID) {

                                          return $rootScope.selectedRosterEvent === tempID;

                                      };



                                      $rootScope.isSelectedTimeFormat = function (tab) {
                                          var selectTimeF = 3;

                                          if ($rootScope.selectedTimeFormat && $rootScope.selectedTimeFormat != null) {
                                              selectTimeF = $rootScope.selectedTimeFormat;
                                          }
                                          else {
                                              if ($rootScope.filter.TimeFormat && $rootScope.filter.TimeFormat != null) {
                                                  selectTimeF = $rootScope.filter.TimeFormat;
                                              } else {
                                                  if ($rootScope.rosterViewModel.TimeFormat && $rootScope.rosterViewModel.TimeFormat != null) {
                                                      selectTimeF = $rootScope.rosterViewModel.TimeFormat;
                                                  }
                                              }

                                          }

                                          return selectTimeF == tab;
                                      };


                                      $rootScope.previousButtonClickHandler = function () {
                                          $rootScope.resetSelection();
                                          $rootScope.filter.IsNext = 2;
                                          if ($rootScope.selectedTimeFormat) {
                                              $rootScope.filter.TimeFormat = $rootScope.selectedTimeFormat;
                                          }
                                          loadFilterRosterList();
                                      };


                                      //Weekly and Monthly radio buttons -roster types 
                                      $scope.handleRadioClick = function (item) {
                                          $rootScope.filter.IsNext = 3;
                                          $rootScope.filter.TimeFormat = item;
                                          $rootScope.selectedTimeFormat = item;
                                          $rootScope.rosterViewModel.TimeFormat = item;
                                          if ($rootScope.selectedRosterItem != null) {
                                              $rootScope.filter.LastSelectedDate = $rootScope.selectedRosterItem.DutyDateActualDate;
                                          }

                                          $rootScope.isRosterChanged = true;

                                          if ($rootScope.UTCTime == $rootScope.selectedTimeFormat) {
                                              $('#selectedTimeFormat').text('UTC');
                                          } else if ($rootScope.DohaTime == $rootScope.selectedTimeFormat) {
                                              $('#selectedTimeFormat').text('DOHA');
                                          } else if ($rootScope.LocalTime == $rootScope.selectedTimeFormat) {
                                              $('#selectedTimeFormat').text('LOCAL');
                                          }

                                          //console.log('tt ' + $rootScope.selectedTimeFormat);
                                          //$rootScope.ETDandETE();

                                          $("#dropdownTimeFormat").removeClass("open");
                                          loadFilterRosterList();
                                      };
                                      function parseDate(v) {
                                          var s = v.split(" ");
                                          var months = {
                                              jan: 0, feb: 1, mar: 2, apr: 3, may: 4, jun: 5,
                                              jul: 6, aug: 7, sep: 8, oct: 9, nov: 10, dec: 11
                                          };
                                          var p = s[0].split('-');
                                          var h = s[1].split(':');
                                          return new Date(p[2], months[p[1].toLowerCase()], p[0], h[0], h[1]);
                                      }

                                      $scope.dateDiffTravelingInDays = function () {

                                          if ($rootScope.ETDFlight.toString().length > 0 && $rootScope.ETAFlight.toString().length) {

                                              var from = parseDate($rootScope.selectedRosterItem.StandardTimeDepartureUtc);
                                              var to = parseDate($rootScope.selectedRosterItem.StandardTimeArrivalUtc);

                                              if (from && to && from.toString().length > 0 && to.toString().length > 0) {
                                                  var fromD = new Date(from);
                                                  var toD = new Date(to);
                                                  var min = 1000 * 60;
                                                  if (fromD && toD && fromD.getDay().toString().length > 0 && toD.getDay().toString().length > 0) {
                                                      var diff = Math.floor((toD - fromD) / min);
                                                      var minutes = diff % 60;
                                                      var hours = (diff - minutes) / 60;
                                                      var final = '';
                                                      if (hours > 0) {
                                                          final = hours + 'hr ';
                                                      }
                                                      if (minutes > 0) {
                                                          final = final + minutes + 'min';
                                                      }
                                                      $rootScope.calculatedBlockHours = final;
                                                      return final;
                                                  }
                                                  else {
                                                      return '';
                                                  }
                                              }
                                          }


                                          return '';
                                      }

                                      $rootScope.ETDandETE = function () {
                                          $rootScope.ETDFlight = '';
                                          $rootScope.ETAFlight = '';
                                          if ($rootScope.selectedRosterItem) {

                                              if ($rootScope.rosterViewModel.TimeFormat == 1) {
                                                  if ($rootScope.selectedRosterItem.StandardTimeDepartureUtc && $rootScope.selectedRosterItem.StandardTimeArrivalUtc) {
                                                      $rootScope.ETDFlight = $rootScope.selectedRosterItem.StandardTimeDepartureUtc;
                                                      $rootScope.ETAFlight = $rootScope.selectedRosterItem.StandardTimeArrivalUtc;
                                                  }
                                              }
                                              else if ($rootScope.rosterViewModel.TimeFormat == 2) {
                                                  if ($rootScope.selectedRosterItem.StandardTimeDepartureDoha && $rootScope.selectedRosterItem.StandardTimeArrivalDoha) {
                                                      $rootScope.ETDFlight = $rootScope.selectedRosterItem.StandardTimeDepartureDoha;
                                                      $rootScope.ETAFlight = $rootScope.selectedRosterItem.StandardTimeArrivalDoha;
                                                  }
                                              }
                                              else if ($rootScope.rosterViewModel.TimeFormat == 3) {
                                                  if ($rootScope.selectedRosterItem.StandardTimeDepartureLocal && $rootScope.selectedRosterItem.StandardTimeArrivalLocal) {
                                                      $rootScope.ETDFlight = $rootScope.selectedRosterItem.StandardTimeDepartureLocal;
                                                      $rootScope.ETAFlight = $rootScope.selectedRosterItem.StandardTimeArrivalLocal;
                                                  }
                                              }
                                              else {
                                                  if ($rootScope.selectedRosterItem.StandardTimeDepartureLocal && $rootScope.selectedRosterItem.StandardTimeArrivalLocal) {
                                                      $rootScope.ETDFlight = $rootScope.selectedRosterItem.StandardTimeDepartureLocal;
                                                      $rootScope.ETAFlight = $rootScope.selectedRosterItem.StandardTimeArrivalLocal;
                                                  }
                                              }
                                              // $scope.dateDiffTravelingInDays();
                                          }

                                          var selectTimeF = 3;

                                          if ($rootScope.selectedTimeFormat && $rootScope.selectedTimeFormat != null) {
                                              selectTimeF = $rootScope.selectedTimeFormat;
                                          }
                                          else {
                                              if ($rootScope.filter.TimeFormat && $rootScope.filter.TimeFormat != null) {
                                                  selectTimeF = $rootScope.filter.TimeFormat;
                                              } else {
                                                  if ($rootScope.rosterViewModel.TimeFormat && $rootScope.rosterViewModel.TimeFormat != null) {
                                                      selectTimeF = $rootScope.rosterViewModel.TimeFormat;
                                                  }
                                              }

                                          }



                                      }

                                      function assignBackground(image) {
                                          // dirty fix for showing image in ipad
                                          if (deviceDetector.isMobile() && deviceDetector.device != "ipad") {
                                              $("#myrostersection").removeClass('.monthlyRoster_info .myroster_header');
                                              $(".applybg_mob").css("background", "url(" + image + ") no-repeat center top");
                                              $(".applybg_mob").css("background-size", "100%");
                                          } else {
                                              $("#myrostersection").removeClass('.monthlyRoster_info .myroster_header');
                                              $("#myrostersection").css("background", "url(" + image + ") no-repeat center top ");
                                              $("#myrostersection").css("background-size", "100%");
                                              //$('#myrostersection').fadeTo('slow', 0.3, function () {
                                              //    $(this).css("background", "url(" + image + ") no-repeat center top");
                                              //}).fadeTo('slow', 1);
                                          }

                                          var img = document.createElement('img');
                                          img.src = image;

                                          img.onload = function () {
                                              $scope.image = image;
                                          };

                                          img.onerror = function () {
                                              $scope.image = '';
                                              getDefaultBackground();
                                          };
                                      }

                                      function getDefaultBackground() {

                                          if ($scope.image && !deviceDetector.isMobile()) {
                                              $("#myrostersection").height('520px');
                                          } else {
                                              $(".applybg_mob").css("background", "url('') no-repeat center top");
                                              $("#myrostersection").removeClass('.monthlyRoster_info .myroster_header');
                                              $("#myrostersection").removeAttr("style");
                                              $("#myrostersection").addClass('myroster_defaultbackground');
                                          }

                                      }
                                      function getDefaultBackgroundMonthly() {

                                          if ($scope.image && !deviceDetector.isMobile()) {
                                              $("#myrostersection").height('520px');
                                          } else {
                                              $(".applybg_mob").css("background", "url('') no-repeat center top");
                                              $("#myrostersection").removeAttr("style");
                                              $("#myroster").addClass('monthlyRoster_info');
                                          }
                                      }
                                      function getRemoveBackgroundMonthly() {

                                          $("#myroster").removeClass('monthlyRoster_info');

                                      }

                                      $scope.changeBackground = function () {
                                          if ( messages.BackgroundImageExtention) {
                                              var path = $rootScope.baseURL + appSettings.BackGroundImages + $rootScope.selectedRosterItem.Arrival + "/";

                                              if (window.innerWidth < 769 || deviceDetector.isMobile()) {
                                                  path = path + "Mobile";
                                              } else {
                                                  path = path + "Desktop";
                                              }
                                              path = path + "/" + $rootScope.selectedRosterItem.Arrival + messages.BackgroundImageExtention;

                                              assignBackground(path);


                                          }
                                      }


                                      //Select the roster items 
                                      $scope.rosterDayClickHandler = function (item, id, tempID, isChild) {


                                          $scope.image = '';
                                          $rootScope.resetSelection();
                                          $rootScope.isNotSelectedRoster = true;
                                          $('#roster_details').hide();
                                          if (!appSettings.isMobileHeader()) {
                                              $rootScope.selectedRoster = id;
                                          }
                                          if ((item.Departure && item.Departure != null && item.Departure != '' && item.Flight != '') ||
                                              (item.Cc && item.Cc != null && item.Cc.toString().trim().length > 0 && item.Address1Training != null && item.Address1Training.toString().trim().length > 0)) {

                                              if (item.Cc && item.Cc != null && item.Cc != '' && item.Address1Training != null && item.Address1Training != '') {
                                                  $rootScope.isTraining = true;
                                              } else {
                                                  $rootScope.isTraining = false;
                                              }

                                              $rootScope.onRDTabChange($rootScope.Loading);
                                              $rootScope.selectedRoster = id;
                                              $rootScope.selectedRosterEvent = tempID;
                                              $rootScope.clickedRosterId = id;
                                              $rootScope.selectedRosterItem = item;

                                              if (!(isChild)) {
                                                  $rootScope.selectedRosterItemParent = item;
                                              }

                                              $scope.changeBackground();

                                              $rootScope.ETDandETE();

                                              setTimeout(function () {
                                                  if ($rootScope.isTraining) {
                                                      $rootScope.onRDTabChange($rootScope.Training);
                                                  } else {
                                                      $rootScope.onRDTabChange($rootScope.Overview);
                                                  }

                                              }, 2);


                                              $('#roster_details').slideDown("slow", function () {

                                              });
                                              $("#roosterDay" + tempID).attr("tabindex", -1).focus();

                                          } else {
                                              getDefaultBackground();
                                          }


                                      };


                                      $scope.rosterDayClickHandlerMonthly = function (item, id, tempID, detailID, liIndex, isChild, ChildID) {
                                          $scope.image = '';
                                          $rootScope.isNotSelectedRoster = true;
                                          $rootScope.resetSelection();

                                          getDefaultBackgroundMonthly();
                                          $('#roster_details').hide();


                                          if ((item.Departure && item.Departure != null && item.Departure != '' && item.Flight != '') ||
                                              (item.Cc && item.Cc != null && item.Cc.toString().trim().length > 0 && item.Address1Training != null && item.Address1Training.toString().trim().length > 0)) {

                                              if (item.Cc && item.Cc != null && item.Cc != '') {
                                                  $rootScope.isTraining = true;
                                              } else {
                                                  $rootScope.isTraining = false;
                                              }


                                              $rootScope.onRDTabChange($rootScope.Loading);
                                              $rootScope.selectedRoster = id;
                                              $rootScope.selectedRosterEvent = tempID;
                                              $rootScope.clickedRosterId = id;
                                              if (ChildID) {
                                                  $rootScope.selectedRosterChild = ChildID;
                                              }


                                              $rootScope.selectedRosterItem = item;
                                              if (!(isChild)) {
                                                  $rootScope.selectedRosterItemParent = item;
                                              }
                                              if (deviceDetector.isMobile()) {
                                                  $scope.changeBackground();
                                              }

                                              $rootScope.selectedTab = 1;
                                              $rootScope.selectedRosterItemParentIndex = liIndex;

                                              var index = 0;
                                              if (liIndex <= 15)
                                                  index = 1;
                                              else
                                                  index = 2;

                                              $rootScope.ETDandETE();

                                              $("#roster_details").insertAfter($("#monthlyRoster" + index));

                                              setTimeout(function () {
                                                  if ($rootScope.isTraining) {
                                                      $rootScope.onRDTabChange($rootScope.Training);
                                                  } else {
                                                      $rootScope.onRDTabChange($rootScope.Overview);
                                                  }
                                              }, 2);

                                              $('#roster_details').slideDown("slow", function () {

                                              });
                                              $('#back_calendar').show();

                                          } else {
                                              if (!appSettings.isMobileHeader() && item.Cc && item.Cc != null && item.Cc.toString().trim().length > 0) {
                                                  $rootScope.selectedRoster = id;
                                              }
                                          }

                                      };
                                      $rootScope.convertToDateTime = function (type) {
                                          if (type && type != null) {
                                              return new Date(type);
                                          }
                                      };
                                      $rootScope.onClickRosterType = function (type) {
                                          $scope.image = '';
                                          $rootScope.resetSelection();
                                          $rootScope.selectedRosterType = type;
                                          $rootScope.filter.RosterTypeID = $rootScope.selectedRosterType;
                                          if ($rootScope.selectedRosterType == $rootScope.WeeklyRoster) {
                                              getRemoveBackgroundMonthly();
                                              $rootScope.tabRosterContentUrl = '/app/ipm/partials/ipmRosterWeekly.html';
                                          } else {
                                              getDefaultBackgroundMonthly();
                                              $rootScope.tabRosterContentUrl = '/app/ipm/partials/ipmRosterMonthly.html';
                                          }


                                      };

                                      $rootScope.isWeeklyOrMonlthSelected = function (value) {
                                          return $rootScope.selectedRosterType === value;
                                      };

                                      $rootScope.isroosterDayOFF = function (item) {
                                          return ((item && item.Cc && item.Cc != null && item.Cc.toString().trim().length > 0 && (item.Address1Training == null || item.Address1Training.toString().trim().length < 1)));
                                      };



                                      $rootScope.isTrainingDay = function (item) {
                                          return ((item != null && item.Cc != null && item.Cc.toString().trim().length > 0));
                                      };


                                      function setStyleFormatting() {

                                          var vpw = $(window).width();
                                          if (vpw < 769) {
                                              $rootScope.isWideScreen = false;
                                          }
                                          else {
                                              $rootScope.isWideScreen = true;
                                          }
                                      }

                                      $rootScope.onRDTabChange = function (tab) {
                                          $rootScope.selectedRDetailTab = tab;
                                          $rootScope.selectedRDetailURL = '';
                                          if ($rootScope.selectedRDetailTab == $scope.Overview) {
                                              $rootScope.selectedRDetailURL = $scope.OverviewURL;
                                          }
                                          else if ($rootScope.selectedRDetailTab == $scope.CrewDet) {
                                              $rootScope.selectedRDetailURL = $scope.CrewDetURL;
                                          }
                                          else if ($rootScope.selectedRDetailTab == $scope.SOS) {
                                              $rootScope.selectedRDetailURL = $scope.SOSURL;
                                          }
                                          else if ($rootScope.selectedRDetailTab == $scope.Hotel) {
                                              $rootScope.selectedRDetailURL = $scope.HotelURL;
                                          }
                                          else if ($rootScope.selectedRDetailTab == $scope.StationInfo) {
                                              $rootScope.selectedRDetailURL = $scope.StationInfoURL;
                                          }
                                          else if ($rootScope.selectedRDetailTab == $scope.Training) {
                                              $rootScope.selectedRDetailURL = $scope.TrainingURL;
                                          }
                                          else if ($rootScope.selectedRDetailTab == $scope.Loading) {
                                              $rootScope.selectedRDetailURL = $scope.LoadingURL;
                                          }
                                          else {
                                              $rootScope.selectedRDetailURL = $rootScope.LoadingURL;
                                          }


                                      };


                                      (function displayUTC() {
                                          var x = new Date()
                                          var x1 = x.toUTCString();// changing the display to UTC string

                                          $rootScope.uctTime = x1;
                                          var element = document.getElementById("currentDateTime");
                                          if (element != null) {
                                              element.innerHTML = $rootScope.uctTime.replace("GMT", "");
                                          }

                                          $timeout(displayUTC, 1000);

                                      })();
                                      Initialize();
                                  }]);
