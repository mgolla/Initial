'use strict'
var ipmModule = angular.module('app.ipm.module', ['app.main.module']);

ipmModule.config(['$stateProvider', '$stickyStateProvider', '$urlRouterProvider', function ($stateProvider, $stickyStateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/home');
    $stateProvider
    .state('home', {
        url: '/home',
        sticky: true,
        params: { param: null },
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmHome.html',
                controller: 'ipm.home.controller'
            }
        }
    })
    $stateProvider.state('home.roster', {
        url: '/hro',
        sticky: true,
        views: {
            'ipmHome': {
                templateUrl: "/app/ipm/partials/ipmRoster.html",
                controller: 'ipm.roster.controller'
            }
        }
    })
    $stateProvider.state('home.alerttabs', {
        url: '/hat',
        sticky: true,
        views: {
            'ipmAlertPageUIView': {
                templateUrl: "/app/ipm/partials/ipmAlertTabs.html",
                controller: 'ipm.alertTabs.controller'

            }
        }
    })
    $stateProvider.state('home.newstabs', {
        url: '/hnt',
        sticky: true,
        views: {
            'ipmNewsPageUIView': {
                templateUrl: "/app/ipm/partials/ipmNewsTabs.html",
                controller: 'ipm.newsTabs.controller'

            }
        }
    })
    $stateProvider.state('home.documentlibrary', {
        url: '/hdl',
        sticky: true,
        views: {
            'ipmDocumentLibPageUIView': {
                templateUrl: "/app/ipm/partials/ipmDocumentLibrary.html",
                controller: 'ipm.document.controller'
            }
        }
    })
    $stateProvider.state('home.usefullink', {
        url: '/hul',
        sticky: true,
        views: {
            'ipmUsefulLinkPageUIView': {
                templateUrl: "/app/ipm/partials/ipmUsefulLink.html",
                controller: 'ipm.link.controller'
            }
        }
    })

    $stateProvider.state('home.weeklyRoaster', {
        url: '/wr',
        sticky: true,
        views: {
            "rosterView": {
                templateUrl: '/app/ipm/partials/ipmRosterWeekly.html',
                controller: 'ipm.weeklyRoster.controller'
            }
        }
    })
    $stateProvider.state('home.monthlyRoster', {
        url: '/mr',
        sticky: true,
        views: {
            "rosterView": {
                templateUrl: '/app/ipm/partials/ipmRosterMonthly.html',
                controller: 'ipm.monthlyRoster.controller'
            }
        }
    }).state('home.monthlyRoster.CodeEx', {
        url: '/mr',
        views: {
            "rosterView": {
                templateUrl: '/app/ipm/partials/ipmRosterMonthlyCodeExplanation.html',
                controller: 'ipm.codeExplanations.controller'
            }
        }
    })
         .state('home.monthlyRoster.hotelInfo', {
             url: '/mrhi',
             views: {
                 "rosterView": {
                     templateUrl: '/app/ipm/partials/ipmRosterMonthlyPrintHotelInfo.html'
                     //,
                     //controller: 'ipm.codeExplanations.controller'
                 }
             }
         })
         .state('home.monthlyRoster.FlightIndi', {
             url: '/mr',
             views: {
                 "rosterView": {
                     templateUrl: '/app/ipm/partials/ipmRosterMonthlyFlightIndicator.html',
                     controller: 'ipm.flightIndicator.controller'
                 }
             }
         })
         .state('home.monthlyRoster.UTCDiff', {
             url: '/mr',
             views: {
                 "rosterView": {
                     templateUrl: '/app/ipm/partials/ipmRosterMonthlyUTCDiff.html',
                     controller: 'ipm.UTCDiff.controller'
                 }
             }
         })
    .state('myprofile', {
        url: '/mp',
        sticky: true,
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmRequest.html',
                controller: 'ipm.request.controller'
            }
        }
    })


    $stateProvider.state('IvrMain', {
        url: '/ivr',
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmIvrKeyContacts.html',
                controller: 'ipm.ivrkeycontacts.controller'
            }
        }
    });
}]);

// Flight Delay
ipmModule.config(['$stateProvider', function ($stateProvider) {

    $stateProvider.state('addEditFlight', {
        url: '/aef',
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmFlightAddEditFilter.html',
                controller: 'ipm.flightAddEditFilter.controller'
            }
        }
    }).state('addEditFlight.details', {
        url: '/aefDetails/:FlightDetsID/:IsFromEvr/:back',
        views: {
            "addeditflightSubview": {
                templateUrl: '/app/ipm/partials/ipmFlightAddEditResult.html',
                controller: 'ipm.flightAddEditResult.controller'
            }
        }
    });

    $stateProvider.state('searchDelayFlight', {
        url: '/sdf',
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmFlightDelaySearchFilter.html',
                controller: 'ipm.flightdelayfilter.controller'
            }
        }
    }).state('searchDelayFlight.data', {
        url: '/sfdresult/:FlightDelayRptId',
        views: {
            "searchSubview": {
                templateUrl: '/app/ipm/partials/ipmFlightDelaySearchResult.html',
                controller: 'ipm.flightdelayresult.controller'
            }
        }
    });

    $stateProvider.state('enterFlightDelay', {
        url: '/efd',
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmFlightDelayEnterFilter.html',
                controller: 'ipm.flightdelayenterfilter.controller'
            }
        }
    });

    $stateProvider.state('enterFlightDelay.enterFlightDelaytab', {
        url: '/tab/:FlightDetsID',
        views: {
            "flyDlytabs": {
                templateUrl: '/app/ipm/partials/ipmFlightDelayEnterTabs.html',
                controller: 'ipm.flightdelayentertab.controller'
            }
        }
    });

    $stateProvider.state('enterFlightDelay.enterFlightDelaytab.details', {
        url: '/dt',
        views: {
            "flgDlyDetails": {
                templateUrl: '/app/ipm/partials/ipmFlightDelayEnterTabDetails.html'
                //controller: 'ipm.crewPersonalDetails.controller'
            }
        }
    });

    $stateProvider.state('enterFlightDelay.enterFlightDelaytab.cause', {
        url: '/ce',
        views: {
            "flgDlyCause": {
                templateUrl: '/app/ipm/partials/ipmFlightDelayEnterTabCause.html',
                controller: 'ipm.flightdelayentertabcause.controller'
            }
        }
    });

    $stateProvider.state('evrflightDelayCause', {
        url: '/dlycse/:FlightDetsID/:IsFromEvr',
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmFlightDelayEnterTabCause.html',
                controller: 'ipm.flightdelayentertabcause.controller'
            }
        }
    });

    //$stateProvider.state('enterFlightDelaytab', {
    //    url: '/tab/:FlightDetsID',
    //    views: {
    //        "homeview": {
    //            templateUrl: '/app/ipm/partials/ipmFlightDelayEnterTabs.html',
    //            controller: 'ipm.flightdelayentertab.controller'
    //        },
    //        "flgDlyDetails": {
    //            templateUrl: '/app/ipm/partials/ipmFlightDelayEnterTabDetails.html'
    //            //controller: 'ipm.crewPersonalDetails.controller'
    //        },
    //        "flgDlyCause": {
    //            templateUrl: '/app/ipm/partials/ipmFlightDelayEnterTabCause.html'
    //           // controller: 'ipm.flightdelayentertabcause.controller'
    //        }
    //    }
    //});

    //$stateProvider.state('enterFlightDelaytab.details', {
    //    url: '/dt',
    //    views: {
    //        "flgDlyDetails": {
    //            templateUrl: '/app/ipm/partials/ipmFlightDelayEnterTabDetails.html'
    //            //controller: 'ipm.crewPersonalDetails.controller'
    //        },
    //        "flgDlyCause": {
    //            templateUrl: '/app/ipm/partials/ipmFlightDelayEnterTabCause.html',
    //            controller: 'ipm.flightdelayentertabcause.controller'
    //        }
    //    }
    //});
}]);

//Housing Module Routing
ipmModule.config(['$stateProvider', '$stickyStateProvider', '$urlRouterProvider', function ($stateProvider, $stickyStateProvider, $urlRouterProvider) {
    $stateProvider.state('housing', {
        url: '/hs',
        sticky: true,
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmHousing.html',
                controller: 'ipm.housing.controller'
            }
        }
    }).state('housing.housingRdStayOut', {
        url: '/hsreadso/:RequestNumber/:RequestId',
        views: {
            "hosuingDetailsView": {
                templateUrl: '/app/ipm/partials/ipmHousingReadStayOut.html',
                controller: 'ipm.housingReadonly.controller'
            }
        }
    }).state('housing.housing-readonly-MovingIn', {
        url: '/hsreadca/:RequestNumber/:RequestId',
        views: {
            "hosuingDetailsView": {
                templateUrl: '/app/ipm/partials/ipmHousingReadMovingIn.html',
                controller: 'ipm.housingReadonly.controller'
            }
        }
    }).state('housing.housing-readonly-ChangeAcc', {
        url: '/hsreadmi/:RequestNumber/:RequestId',
        views: {
            "hosuingDetailsView": {
                templateUrl: '/app/ipm/partials/ipmHousingReadChangeAcc.html',
                controller: 'ipm.housingReadonly.controller'
            }
        }
    }).state('housing.housing-readonly-GuestAcc', {
        url: '/hsreadga/:RequestNumber/:RequestId',
        views: {
            "hosuingDetailsView": {
                templateUrl: '/app/ipm/partials/ipmHousingReadGuestAcc.html',
                controller: 'ipm.housingReadonly.controller'
            }
        }
    }).state('housing.housing-readonly-MoveOut', {
        url: '/hsreadmo/:RequestNumber/:RequestId',
        views: {
            "hosuingDetailsView": {
                templateUrl: '/app/ipm/partials/ipmHousingReadMoveOut.html',
                controller: 'ipm.housingReadonly.controller'
            }
        }
    }).state('housing.housing-readonly-SwapRoom', {
        url: '/hsreadsr/:RequestNumber/:RequestId',
        views: {
            "hosuingDetailsView": {
                templateUrl: '/app/ipm/partials/ipmHousingReadSwapRoom.html',
                controller: 'ipm.housingReadonly.controller'
            }
        }
    });

    $stateProvider.state('housing-create', {
        url: '/create',
        sticky: true,
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmHousingNewRequest.html',
                controller: 'ipm.housingNewRequest.controller'
            }
        }
    }).state('housing-create.ca', {
        url: '/ca',
        views: {
            "newrequestview": {
                templateUrl: '/app/ipm/partials/ipmHousingChangeAcc.html',
                controller: 'ipm.housingChangeAcc.controller'
            }
        }
    }).state('housing-create.ga', {
        url: '/ga',
        views: {
            "newrequestview": {
                templateUrl: '/app/ipm/partials/ipmHousingGuestAcc.html',
                controller: 'ipm.housingGuestAcc.controller'
            }
        }
    }).state('housing-create.mi', {
        url: '/mi',
        views: {
            "newrequestview": {
                templateUrl: '/app/ipm/partials/ipmHousingMoveIn.html',
                controller: 'ipm.housingChangeAcc.controller'
            }
        }
    }).state('housing-create.mo', {
        url: '/mo',
        views: {
            "newrequestview": {
                templateUrl: '/app/ipm/partials/ipmHousingMoveOut.html',
                controller: 'ipm.housingMoveOut.controller'
            }
        }
    }).state('housing-create.so', {
        url: '/so',
        views: {
            "newrequestview": {
                templateUrl: '/app/ipm/partials/ipmHousingStayout.html',
                controller: 'ipm.housingStayOut.controller'
            }
        }
    }).state('housing-create.sr', {
        url: '/sr',
        views: {
            "newrequestview": {
                templateUrl: '/app/ipm/partials/ipmHousingSwapRoom.html',
                controller: 'ipm.housingSwapRoom.controller'
            }
        }
    });



    //state('housing-chgacc', {
    //    url: '/chgacc',
    //    views: {
    //        "homeview": {
    //            templateUrl: '/app/ipm/partials/ipmHousingChangeAcc.html',
    //            controller: 'ipm.housingNewRequest.controller'
    //        }
    //    }
    //}).state('housing-guestacc', {
    //    url: '/gstacc',
    //    views: {
    //        "homeview": {
    //            templateUrl: '/app/ipm/partials/ipmHousingGuestAcc.html',
    //            controller: 'ipm.housingNewRequest.controller'
    //        }
    //    }
    //}).state('housing-guestaccread', {
    //    url: '/gstaccread',
    //    views: {
    //        "homeview": {
    //            templateUrl: '/app/ipm/partials/ipmHousingGuestAccRead.html',
    //            controller: 'ipm.housingNewRequest.controller'
    //        }
    //    }
    //}).state('housing-swapout', {
    //    url: '/swpout',
    //    views: {
    //        "homeview": {
    //            templateUrl: '/app/ipm/partials/ipmHousingStayout.html',
    //            controller: 'ipm.housingNewRequest.controller'
    //        }
    //    }
    //}).state('housing-moveout', {
    //    url: '/moveout',
    //    views: {
    //        "homeview": {
    //            templateUrl: '/app/ipm/partials/ipmHousingMoveOut.html',
    //            controller: 'ipm.housingNewRequest.controller'
    //        }
    //    }
    //}).state('housing-swaproom', {
    //    url: '/swprm',
    //    views: {
    //        "homeview": {
    //            templateUrl: '/app/ipm/partials/ipmHousingSwapRoom.html',
    //            controller: 'ipm.housingNewRequest.controller'
    //        }
    //    }
    //}).state('housing-My', {
    //    url: '/assesmy',
    //    views: {
    //        "homeview": {
    //            templateUrl: '/app/ipm/partials/ipmassesmentMy.html'
    //            //controller: 'ipm.housingNewRequest.controller'
    //        }
    //    }
    //});

    $stateProvider.state('housing-ack', {
        url: '/hsack/:reqtype/:reqno',
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmHousingAcknowledge.html',
                controller: 'ipm.housingAcknowledge.controller'
            }
        }
    });


}]);

ipmModule.config(['$stateProvider', '$stickyStateProvider', '$urlRouterProvider', function ($stateProvider, $stickyStateProvider, $urlRouterProvider) {

    $stateProvider
        .state('home.weeklyRoaster.rosterDetailed', {
            url: '/rd',
            sticky: true,
            views: {
                "rosterDetailedView": {
                    templateUrl: '/app/ipm/partials/ipmRosterDetailsTabs.html',
                    controller: 'ipm.rosterDetailTabs.controller'
                }
            }
        })
       .state('home.weeklyRoaster.rosterDetailed.overviewtab', {
           url: '/ov',
           sticky: true,
           views: {
               "uiViewOverview": {
                   templateUrl: '/app/ipm/partials/ipmOverviewTab.html',
                   controller: 'ipm.overviewTab.controller'
               }
           }
       })
     .state('home.weeklyRoaster.rosterDetailed.trainingtab', {
         url: '/tr',
         sticky: true,
         views: {
             "uiViewOverview": {
                 templateUrl: '/app/ipm/partials/ipmtraining.html',
                 controller: 'ipm.training.controller'
             }
         }
     })

        .state('home.weeklyRoaster.rosterDetailed.stationtab', {
            url: '/st',
            sticky: true,
            views: {
                "uiViewStationDetails": {
                    templateUrl: '/app/ipm/partials/ipmStationTab.html',
                    controller: 'ipm.stationInfo.controller'
                }
            }
        })
     .state('home.weeklyRoaster.rosterDetailed.hoteltab', {
         url: '/ht',
         sticky: true,
         views: {
             "uiViewHotelInfo": {
                 templateUrl: '/app/ipm/partials/ipmHotelTab.html',
                 controller: 'ipm.hotelInfo.controller'
             }
         }
     })
     .state('home.weeklyRoaster.rosterDetailed.crewInfo', {
         url: '/ci',
         sticky: true,
         views: {
             "uiViewCrewDetails": {
                 templateUrl: '/app/ipm/partials/ipmCrewTab.html',
                 controller: 'ipm.crewInfo.controller'
             }
         }
     })
     .state('home.weeklyRoaster.rosterDetailed.sos', {
         url: '/so',
         sticky: true,
         views: {
             "uiViewSOS": {
                 templateUrl: '/app/ipm/partials/ipmSOSTab.html',
                 controller: 'ipm.summaryService.controller'
             }
         }
     })
    .state('home.monthlyRoster.rosterDetailedTab', {
        url: '/mrd',
        sticky: true,
        views: {
            "rosterDetailedView": {
                templateUrl: '/app/ipm/partials/ipmRosterDetailsTabs.html',
                controller: 'ipm.rosterDetailTabs.controller'
            }
        }
    })
    .state('home.monthlyRoster.rosterDetailedTab.overviewtab', {
        url: '/ovt',
        sticky: true,
        views: {
            "uiViewOverview": {
                templateUrl: '/app/ipm/partials/ipmOverviewTab.html',
                controller: 'ipm.overviewTab.controller'
            }
        }
    })
        .state('home.monthlyRoster.rosterDetailedTab.stationtab', {
            url: '/stt',
            sticky: true,
            views: {
                "uiViewStationDetails": {
                    templateUrl: '/app/ipm/partials/ipmStationTab.html',
                    controller: 'ipm.stationInfo.controller'
                }
            }
        })
     .state('home.monthlyRoster.rosterDetailedTab.hoteltab', {
         url: '/htt',
         sticky: true,
         views: {
             "uiViewHotelInfo": {
                 templateUrl: '/app/ipm/partials/ipmHotelTab.html',
                 controller: 'ipm.hotelInfo.controller'
             }
         }
     })
     .state('home.monthlyRoster.rosterDetailedTab.crewInfo', {
         url: '/cit',
         sticky: true,
         views: {
             "uiViewCrewDetails": {
                 templateUrl: '/app/ipm/partials/ipmCrewTab.html',
                 controller: 'ipm.crewInfo.controller'
             }
         }
     })
     .state('home.monthlyRoster.rosterDetailedTab.sos', {
         url: '/sot',
         sticky: true,
         views: {
             "uiViewSOS": {
                 templateUrl: '/app/ipm/partials/ipmSOSTab.html',
                 controller: 'ipm.summaryService.controller'
             }
         }
     })

    ;
}]);

ipmModule.config(['$stateProvider', '$stickyStateProvider', '$urlRouterProvider', function ($stateProvider, $stickyStateProvider, $urlRouterProvider) {
    $stateProvider
      .state('notificationAll', {
          url: '/notfall',
          sticky: true,
          views: {
              "homeview": {
                  templateUrl: '/app/ipm/partials/ipmNotificationAll.html',
                  controller: 'ipm.notificationAll.controller'
              }
          }
      }).state('notificationAll.idpack', {
          url: '/idpack/:reqtype/:reqno',
          views: {
              "notificationDetails": {
                  templateUrl: '/app/ipm/partials/ipmRecordAssesmentMyReadOnly.html',
                  controller: 'ipm.assessmentReadonly.controller'
              }
          }
      }).state('notificationAll.asmntack', {
          url: '/asmntack/:reqtype/:reqno',
          views: {
              "notificationDetails": {
                  templateUrl: '/app/ipm/partials/ipmAssessmentAcknowledge.html',
                  controller: 'ipm.acknowledge.controller'
              }
          }
      }).state('notificationAll.bevidp', {
          url: '/bevidp/:reqtype/:reqno',
          views: {
              "notificationDetails": {
                  templateUrl: '/app/ipm/partials/ipmBehaviourAsmtNotf.html',
                  controller: 'ipm.acknowledge.controller'
              }
          }
      })
    .state('notificationAll.hsChangeAcc', {
        url: '/hsreadmi/:RequestNumber/:RequestId',
        views: {
            "notificationDetails": {
                templateUrl: '/app/ipm/partials/ipmHousingReadChangeAcc.html',
                controller: 'ipm.housingReadonly.controller'
            }
        }
    }).state('notificationAll.housing-ack', {
        url: '/hsack/:reqtype/:reqno',
        views: {
            "notificationDetails": {
                templateUrl: '/app/ipm/partials/ipmHousingAcknowledge.html',
                controller: 'ipm.housingAcknowledge.controller'
            }
        }
    });

    $stateProvider
       .state('home.notification', {
           url: '/anm',
           sticky: true,
           views: {
               "ipmAlertTabAlertsUIView": {
                   templateUrl: '/app/ipm/partials/ipmAlertTabsNotification.html',
                   controller: 'ipm.notificationAlert.controller'
               }
           }
       })
    .state('home.myrequest', {
        url: '/mra',
        sticky: true,
        views: {
            "ipmAlertTabMyRequestUIView": {
                templateUrl: '/app/ipm/partials/ipmAlertTabsMyRequest.html',
                controller: 'ipm.myRequest.controller'
            }
        }
    })
    .state('home.svptweet', {
        url: 'ast',
        sticky: true,
        views: {
            "ipmAlertTabSVPTUIView": {
                templateUrl: '/app/ipm/partials/ipmAlertTabsSvpTweet.html'
                //,
                //controller: 'ipm.svpMessage.controller'
            }
        }
    })
    ;
}]);

ipmModule.config(['$stateProvider', '$stickyStateProvider', '$urlRouterProvider', function ($stateProvider, $stickyStateProvider, $urlRouterProvider) {
    $stateProvider
       .state('home.depnews', {
           url: '/ndn',
           sticky: true,
           views: {
               "ipmNewsTabsDepartmentUIView": {
                   templateUrl: '/app/ipm/partials/ipmNewsTabsDepNews.html'
                   ,
                   controller: 'ipm.departmentNews.controller'
               }
           }
       })
    .state('home.ifeguide', {
        url: '/nig',
        sticky: true,
        views: {
            "ipmNewsTabsMonthlyIFEUIView": {
                templateUrl: '/app/ipm/partials/ipmNewsTabsIFEGuide.html'
                , controller: 'ipm.iFEGuide.controller'
            }
        }
    })
    .state('home.svpmessage', {
        url: '/nsm',
        sticky: true,
        views: {
            "ipmNewsTabsSVPMUIView": {
                templateUrl: '/app/ipm/partials/ipmNewsTabsSVPMessage.html'
                //,
                // controller: 'ipm.svpMessage.controller'
            }
        }
    })

    .state('home.svpvideo', {
        url: '/nsvm',
        // sticky: true,
        views: {
            "ipmNewsTabsSVPMUIView": {
                templateUrl: '/app/ipm/partials/ipmNewsTabsSVPVideo.html'
                //,
                // controller: 'ipm.svpMessage.controller'
            }
        }
    })


     .state('home.vision', {
         url: '/nvm',
         sticky: true,
         views: {
             "ipmNewsTabsUIView": {
                 templateUrl: '/app/ipm/partials/ipmNewsTabsVision.html'
                 ,
                 controller: 'ipm.visionMission.controller'
             }
         }
     })

    .state('home.airlinenews', {
        url: '/nan',
        sticky: true,
        views: {
            "ipmNewsTabsAirlineUIView": {
                //templateUrl: '/app/ipm/partials/ipmNewsTabsAirlineNews.html'
                templateUrl: '/app/ipm/partials/ipmNewsTabsAirlineNewsPage.html'
                //, controller: 'ipm.airlineNews.controller'
                , controller: 'ipm.airlineNewsPage.controller'
            }
        }
    })
     .state('home.airlinenewsPage', {
         url: '/nanp',

         views: {
             "ipmNewsTabsAirlineUIViewP": {
                 templateUrl: '/app/ipm/partials/ipmNewsTabsAirlineNewsPage.html'
                 , controller: 'ipm.airlineNewsPage.controller'
             }
         }
     })
     .state('home.airlinenewsSection', {
         url: '/nans',

         views: {
             "ipmNewsTabsAirlineUIViewP": {
                 templateUrl: '/app/ipm/partials/ipmNewsTabsAirlineNewsSection.html'
                 , controller: 'ipm.airlineNewsSection.controller'
             }
         }
     })


    ;
}]);

ipmModule.config(['$stateProvider', '$stickyStateProvider', '$urlRouterProvider', function ($stateProvider, $stickyStateProvider, $urlRouterProvider) {
    $stateProvider
       .state('file', {
           url: '/fve',
           params: { fileFilter: null },
           views: {
               "homeview": {
                   templateUrl: '/app/ipm/partials/ipmFile.html'
                   ,
                   controller: 'ipm.file.controller'
               }
           }
       })
     .state('home.videofile', {
         url: '/hmv',
         params: { fileFilter: null },
         views: {
             "ipmAlertTabsUIView": {
                 templateUrl: '/app/ipm/partials/ipmFile.html'
                 ,
                 controller: 'ipm.file.controller'
             }
         }
     })
         .state('documentViewer', {
             url: '/dvw',
             params: { fileFilter: null },
             views: {
                 "homeview": {
                     templateUrl: '/app/shared/partials/srdDocumentViewer.html'
                     ,
                     controller: 'ipm.documentviewer.controller'
                 }
             }
         })
    .state('home.filefolder', {
        url: '/hmff',
        params: { fileFilter: null },
        views: {
            "ipmAlertTabsUIView": {
                templateUrl: '/app/ipm/partials/ipmFileFolder.html'
                ,
                controller: 'ipm.fileFolder.controller'
            }
        }
    })

    ;
}]);

ipmModule.config(['$stateProvider', '$stickyStateProvider', '$urlRouterProvider', function ($stateProvider, $stickyStateProvider, $urlRouterProvider) {
    $stateProvider
      .state('doclibfilemg', {
          url: '/dlfm',

          views: {
              "homeview": {
                  templateUrl: "/app/ipm/partials/ipmDocumentLibFileMg.html",
                  controller: 'ipm.documentfilemg.controller'
              }
          }
      })
    .state('videofile', {
        url: '/vpm',
        params: { fileFilter: null },
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmVideoFile.html'
                //,
                //controller: 'ipm.file.controller'
            }
        }
    })
    ;
}]);

ipmModule.config(['$stateProvider', '$stickyStateProvider', '$urlRouterProvider', function ($stateProvider, $stickyStateProvider, $urlRouterProvider) {
    $stateProvider
      .state('rostermobile', {
          url: '/rosm',
          sticky: true,
          views: {
              "homeview": {
                  templateUrl: "/app/ipm/partials/ipmRoster.html",
                  controller: 'ipm.roster.controller'
              }
          }
      })
     .state('doclibmobile', {
         url: '/docm',
         sticky: true,
         views: {
             "homeview": {
                 templateUrl: "/app/ipm/partials/ipmDocumentLibrary.html",
                 controller: 'ipm.document.controller'
             }
         }
     })
     .state('userflinkmobile', {
         url: '/uflm',
         sticky: true,
         views: {
             "homeview": {
                 templateUrl: "/app/ipm/partials/ipmUsefulLink.html",
                 controller: 'ipm.link.controller'
             }
         }
     })
     .state('alertnmobile', {
         url: '/alnm',
         sticky: true,
         views: {
             "homeview": {
                 templateUrl: '/app/ipm/partials/ipmAlertTabsNotification.html',
                 controller: 'ipm.notificationAlert.controller'
             }
         }
     })
     .state('myreqmobile', {
         url: '/myrqm',
         sticky: true,
         views: {
             "homeview": {
                 templateUrl: '/app/ipm/partials/ipmAlertTabsMyRequest.html'
                 //,
                 //controller: 'ipm.myRequest.controller'
             }
         }
     })
     .state('svpmobile', {
         url: '/svpm',
         sticky: true,
         views: {
             "homeview": {
                 templateUrl: '/app/ipm/partials/ipmAlertTabsSvpTweet.html'
             }
         }
     })

     .state('ifegmobile', {
         url: '/ifem',
         sticky: true,
         views: {
             "homeview": {
                 templateUrl: '/app/ipm/partials/ipmNewsTabsIFEGuide.html'
                , controller: 'ipm.iFEGuide.controller'
             }
         }
     })
     .state('depnmobile', {
         url: '/depm',
         sticky: true,
         views: {
             "homeview": {
                 templateUrl: '/app/ipm/partials/ipmNewsTabsDepNews.html'
                 ,
                 controller: 'ipm.departmentNews.controller'
             }
         }
     })
     .state('svpmmobile', {
         url: '/svpmm',
         sticky: true,
         views: {
             "homeview": {
                 templateUrl: '/app/ipm/partials/ipmNewsTabsSVPMessage.html'
                 //,
                 //  controller: 'ipm.svpMessage.controller'
             }
         }
     })
     .state('visionmobile', {
         url: '/vism',
         sticky: true,
         views: {
             "homeview": {
                 templateUrl: '/app/ipm/partials/ipmNewsTabsVision.html'
                 ,
                 controller: 'ipm.visionMission.controller'
             }
         }
     })
     .state('airlinemobile', {
         url: '/alnem',
         sticky: true,
         //views: {
         //    "homeview": {
         //        templateUrl: '/app/ipm/partials/ipmNewsTabsAirlineNews.html'
         //    }
         //}
         views: {
             "homeview": {
                 //templateUrl: '/app/ipm/partials/ipmNewsTabsAirlineNews.html'
                 templateUrl: '/app/ipm/partials/ipmNewsTabsAirlineNewsPage.html'
                 //, controller: 'ipm.airlineNews.controller'
                 , controller: 'ipm.airlineNewsPage.controller'
             }
         }
     })

    ;
}]);

//Crew profile routing
ipmModule.config(['$stateProvider', '$stickyStateProvider', '$urlRouterProvider', function ($stateProvider, $stickyStateProvider, $urlRouterProvider) {
    // Crew Profile Personal Details
    $stateProvider.state('cpPersonaldetails', {
        url: '/cppd',
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmCrewProfile.html',
                controller: 'ipm-crewprofile-controller'
            }
        },
    });

    $stateProvider.state('cpPersonaldetails.details', {
        url: '/dt',
        views: {
            "ipmdetailsUIView": {
                templateUrl: '/app/ipm/partials/ipmCrewPersonalDetails.html',
                controller: 'ipm.crewPersonalDetails.controller'
            },
            "ipmcareerPathUIView": {
                templateUrl: '/app/ipm/partials/ipmCrewCareerPath.html',
                controller: 'ipm.crewCareerPath.controller'
            },
            "ipmPrfmIndicatorUIView": {
                templateUrl: '/app/ipm/partials/ipmCrewPerfrmIndicator.html',
                //controller: 'ipm.crewPersonalDetails.controller'
            },
            "ipmTrainingUIView": {
                templateUrl: '/app/ipm/partials/ipmCrewTrainingHistory.html',
                controller: 'ipm.crewTrainingHistory.controller'
            },
            "ipmTabsUIView": {
                templateUrl: '/app/ipm/partials/ipmCrewProfileTabs.html',
                controller: 'ipm-crewprofiletabs-controller'
            }
        }
    });

    $stateProvider.state('cpPersonaldetails.details.ql', {
        url: '/ql',
        sticky: true,
        views: {
            "ipmQualnVisaUIView": {
                templateUrl: '/app/ipm/partials/ipmCrewQualnVisa.html',
                controller: 'ipm.crewQualnVisa.controller'
            }
        }
    })

    $stateProvider.state('cpPersonaldetails.details.idp', {
        url: '/idp',
        sticky: true,
        views: {
            "ipmIDPUIView": {
                templateUrl: '/app/ipm/partials/ipmCrewIDP.html',
                controller: 'ipm.crewIDP.controller'
            }
        }
    })

    $stateProvider.state('cpPersonaldetails.details.mydoc', {
        url: '/mydoc',
        sticky: true,
        views: {
            "ipmMyDocUIView": {
                templateUrl: '/app/ipm/partials/ipmCrewMyDoc.html',
                controller: 'ipm.crewMyDoc.controller'
            }
        }
    })

    $stateProvider.state('cpPersonaldetails.details.dstvst', {
        url: '/dstvst',
        sticky: true,
        views: {
            "ipmDestVstdUIView": {
                templateUrl: '/app/ipm/partials/ipmCrewDestVisited.html',
                controller: 'ipm.crewDstVstd.controller'
            }
        }
    });
}]);

//eVR search routing
ipmModule.config(['$stateProvider', '$stickyStateProvider', '$urlRouterProvider', function ($stateProvider, $stickyStateProvider, $urlRouterProvider) {

    $stateProvider.state('evrSearch', {
        url: '/evrs',
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmEvrSearch.html',
                controller: 'ipm.evrsearch.controller'
            }
        }
    }).state('evrSearch.view', {
        url: '/evrsres/:evrSubmtdId/:FlightDetsID/:back',
        views: {
            "ipmevrSerachResFlightDetUIView": {
                templateUrl: '/app/ipm/partials/ipmFlightDelayEnterTabDetails.html',
                controller: 'ipm.flightDetails.controller'
            },
            "ipmevrSerachResViewUIView": {
                templateUrl: '/app/ipm/partials/ipmEvrDetails.html',
                controller: 'ipm.evrdetails.controller'
            }
        }
    });

    //.state('evrSearch.View', {
    //    url: '/evrsres/:evrSubmtdId/:FlightDetsID',
    //    views: {
    //        "evrSearchSubView": {
    //            templateUrl: '/app/ipm/partials/ipmEVRSearchRes.html',
    //            controller: 'ipm.evrsearchres.controller'
    //        }
    //    }
    //})

}]);

//Enter EVR routing
ipmModule.config(['$stateProvider', function ($stateProvider) {
    /*
    $stateProvider.state('evrtabs', {
        url: '/evrtabs/:isfrom/:FlightDetsID/:evrdrfid/:evrno',
        params: { evrLoadupdate: null },
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmEVRTabs.html',
                controller: 'ipm.evrtabs.controller'
            }
        }
    });


    $stateProvider.state('evrtabs.flghtdt', {
        url: '/flgtdets',
        views: {
            "ipmflightDetUIView": {
                templateUrl: '/app/ipm/partials/ipmFlightDelayEnterTabDetails.html',
                //controller: 'ipm.flightdelayentertab.controller'
                controller: 'ipm.flightDetails.controller'
            }
        }
    });

    $stateProvider.state('evrtabs.evrForm', {
        url: '/frm',
        views: {
            "ipmevrEnterUIView": {
                templateUrl: '/app/ipm/partials/ipmEVRMain.html',
                controller: 'ipm.evrmain.controller',
            }
        }
    });

    $stateProvider.state('evrtabs.evrDraft', {
        url: '/drft',
        views: {
            "ipmevrDraftUIView": {
                templateUrl: '/app/ipm/partials/ipmEVRDrafts.html',
                controller: 'ipm.evrdraft.controller'
            }
        }
    });*/

    //$stateProvider.state('evrtabs.evrDraft.evrForm', {
    //    url: '/evrdrfm',
    //    params: { evrdrfid: null },
    //    views: {
    //        "ipmevrDraftUIView": {
    //            templateUrl: '/app/ipm/partials/ipmEVRMain.html',
    //            controller: 'ipm.evrmain.controller',
    //        }
    //    }
    //});

    $stateProvider.state('evrlstsState', {
        url: '/evtlsts',
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmEVRListTabs.html',
                controller: 'ipm.evrlists.controller',
            }
        }
    }).state('evrlstsState.evrviewState', {
        url: '/evrview/:FlightDetsID/:SectorFrom/:SectorTo/:ATD',
        views: {
            "uiViewEvrDetails": {
                templateUrl: '/app/ipm/partials/ipmEVRViewMain.html',
                controller: 'ipm.evrviewmain.controller',
            }
        }
    }).state('evrlstsState.evrtabs', {
        url: '/evrtabs/:FlightDetsID',
        params: { evrLoadupdate: null },
        views: {
            "uiViewEvrDetails": {
                templateUrl: '/app/ipm/partials/ipmEVRTabs.html',
                controller: 'ipm.evrtabs.controller'
            }
        }
    })
    .state('evrlstsState.addEditFlight', {
        url: '/aefDetails/:FlightDetsID/:IsFromEvr',
        params: { evrLoadupdate: null },
        views: {
            "uiViewEvrDetails": {
                templateUrl: '/app/ipm/partials/ipmFlightAddEditResult.html',
                controller: 'ipm.flightAddEditResult.controller'
            }
        }
    }).state('evrlstsState.evrviewState.view', {
        url: '/view/:evrSubmtdId/:back',
        views: {
            "ipmVRDetailsUIView": {
                templateUrl: '/app/ipm/partials/ipmEvrDetails.html',
                controller: 'ipm.evrdetails.controller'
            }
        }
    });

    //.state('evrlstsState.details', {
    //    url: '/dt',
    //    views: {
    //        "ipmevrSubmittedUIView": {
    //            templateUrl: '/app/ipm/partials/ipmEVRSubmitted.html',
    //            controller: 'ipm.evrsubmitted.controller'
    //        },
    //        "ipmflightDetailsUIView": {
    //            templateUrl: '/app/ipm/partials/ipmFlightDelayEnterTabDetails.html',
    //            //controller: 'ipm.flightdelayentertab.controller'
    //            controller: 'ipm.flightDetails.controller'
    //        }
    //    }
    //})

    //.state('evrlstsState.evrviewState.details', {
    //    url: '/evrview/:FlightDetsID/:SectorFrom/:SectorTo/:ATD',
    //    views: {
    //        "uiViewEvrDetails": {
    //            templateUrl: '/app/ipm/partials/ipmEVRViewMain.html',
    //            controller: 'ipm.evrviewmain.controller',
    //        }
    //    }aefDetails
    //});




}]);

//Kafou routing
ipmModule.config(['$stateProvider', function ($stateProvider) {

    $stateProvider.state('kafoulstsState', {
        url: '/kfetr',
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmKafouEnterList.html',
                controller: 'ipm.kafouenterList.controller'
            }
        }
    });

    $stateProvider.state('kfsearch', {
        url: '/kfsh',
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmKafouSearch.html',
                controller: 'ipm.kafouSearch.controller'
            }
        }
    }).state('kfsearch.kafoudetails', {
        url: '/kfd/:FlightDetsID/:kfgrp/:kfstatus/:kfrecogid',
        views: {
            "ipmkfaddedit_frmkfSearch_subview": {
                templateUrl: '/app/ipm/partials/ipmKafouEnterList.html',
                controller: 'ipm.kafouenterList.controller'
            }
        }
    });

    $stateProvider.state('kfmy', {
        url: '/kfmy',
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmKafouMy.html',
                controller: 'ipm.kafouMy.controller'
            }
        }
    }).state('kfmy.kafoudetails', {
        url: '/kfd/:FlightDetsID/:kfgrp/:kfstatus/:kfrecogid/:isFrom',
        views: {
            "ipmkfaddedit_frmkfMy_subview": {
                templateUrl: '/app/ipm/partials/ipmKafouEnterList.html',
                controller: 'ipm.kafouenterList.controller'
            }
        }
    });

}]);

//*** Performance Routing ***//
ipmModule.config(['$stateProvider', '$stickyStateProvider', '$urlRouterProvider', function ($stateProvider, $stickyStateProvider, $urlRouterProvider) {

    $stateProvider.state('poasmnt', {
        url: '/poasm',
        params: { myparam: null },
        sticky: true,
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmPOAssessment.html',
                controller: 'ipm.poassessment.controller'
            }
        }
    }).state('poasmnt.prevevrs', {
        url: '/prevevrs/:id',
        views: {
            "poAsmtSubView": {
                templateUrl: '/app/shared/partials/ipmReadonlyGrid.html',
                controller: 'ipm.evrprevious.controller'
            }
        }
    }).state('poasmnt.prevasmtall', {
        url: '/prevasmtall/:id',
        views: {
            "poAsmtSubView": {
                templateUrl: '/app/shared/partials/ipmReadonlyGrid.html',
                controller: 'ipm.assessmentPreviousAll.controller'
            }
        }
    }).state('poasmnt.pomonthly', {
        url: '/pmr/:staffID',
        views: {
            "poAsmtSubView": {
                templateUrl: '/app/ipm/partials/ipmPoAssessmentMonthly.html',
                controller: 'ipm.poAssessmentMonthly.controller'
            }
        }
    }).state('poasmnt.asmntdtl', {
        url: '/poasdtl/:back',
        params: { myparam: null },
        views: {
            "poAsmtSubView": {
                templateUrl: '/app/ipm/partials/ipmRecordAssesmentMy.html',
                controller: 'ipm.assessment.controller'
            }
        }
    });

    $stateProvider.state('srchasmnt', {
        url: '/sral',
        sticky: true,
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmassesmentSearch.html',
                controller: 'ipm.assessmentsearch.controller'
            }
        }
    }).state('srchasmnt.pasmt', {
        url: '/pasmntdet/:back/:AssessmentID',
        views: {
            "srchasmntSubView": {
                templateUrl: '/app/ipm/partials/ipmAssessmentPrevDetails.html',
                controller: 'ipm.assessmentPreviousDetails.controller'
            }
        }
    }).state('srchasmnt.asmtro', {
        url: '/asdtlro/:back/:id',
        params: { myparam: null },
        views: {
            "srchasmntSubView": {
                templateUrl: '/app/ipm/partials/ipmRecordAssesmentMyReadOnly.html',
                controller: 'ipm.assessmentReadonly.controller'
            }
        }
    });

    $stateProvider.state('prevassessment', {
        url: '/pasmnt',
        sticky: true,
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmAssessmentPrevious.html',
                controller: 'ipm.assessmentPrevious.controller'
            }
        }
    }).state('prevassessment.old', {
        url: '/pasmntdet/:back/:AssessmentID',
        views: {
            "prevasmntSubView": {
                templateUrl: '/app/ipm/partials/ipmAssessmentPrevDetails.html',
                controller: 'ipm.assessmentPreviousDetails.controller'
            }
        }
    }).state('prevassessment.new', {
        url: '/asdtlro/:back/:id',
        params: { myparam: null },
        views: {
            "prevasmntSubView": {
                templateUrl: '/app/ipm/partials/ipmRecordAssesmentMyReadOnly.html',
                controller: 'ipm.assessmentReadonly.controller'
            }
        }
    });

    $stateProvider
        .state('myasmnt', {
            url: '/ma',
            sticky: true,
            views: {
                "homeview": {
                    templateUrl: '/app/ipm/partials/ipmAssessmentList.html',
                    controller: 'ipm.assessmentList.controller'
                }
            }
        })
        .state('myasmnttt', {
            url: '/mat',
            sticky: true,
            views: {
                "homeview": {
                    templateUrl: '/app/ipm/partials/ipmassesmentMy.html',
                    controller: 'ipm.assessmentList.controller'
                }
            }
        }).state('unasmntdtl', {
            url: '/unasdtl',
            sticky: true,
            params: { myparam: null },
            views: {
                "homeview": {
                    templateUrl: '/app/ipm/partials/ipmUnscheduledAssessment.html',
                    controller: 'ipm.unscheduledassessment.controller'
                }
            }
        })

.state('asmntdtl', {
    url: '/asdtl/:back',
    //sticky: true,
    params: { myparam: null },
    views: {
        "homeview": {
            templateUrl: '/app/ipm/partials/ipmRecordAssesmentMy.html',
            controller: 'ipm.assessment.controller'
        }
    }
}).state('asmntdtlro', {
    url: '/asdtlro/:back/:id',
    //sticky: true,
    params: { myparam: null },
    views: {
        "homeview": {
            templateUrl: '/app/ipm/partials/ipmRecordAssesmentMyReadOnly.html',
            controller: 'ipm.assessmentReadonly.controller'
        }
    }
}).state('asmntack', {
    url: '/asmntack/:reqtype/:reqno',
    views: {
        "homeview": {
            templateUrl: '/app/ipm/partials/ipmAssessmentAcknowledge.html',
            controller: 'ipm.acknowledge.controller'
        }
    }
}).state('bevidp', {
    url: '/bevidp/:reqtype/:reqno',
    views: {
        "homeview": {
            templateUrl: '/app/ipm/partials/ipmBehaviourAsmtNotf.html',
            controller: 'ipm.acknowledge.controller'
        }
    }
}).state('idpack', {
    url: '/idpack/:reqtype/:reqno',
    views: {
        "homeview": {
            templateUrl: '/app/ipm/partials/ipmRecordAssesmentMyReadOnly.html',
            controller: 'ipm.assessmentReadonly.controller'
        }
    }
}).state('crwonbrdsrchasmnt', {
    url: '/cbsral',
    sticky: true,
    views: {
        "homeview": {
            templateUrl: '/app/ipm/partials/ipmCrewOnBoardAssessmentSearch.html',
            controller: 'ipm.crewOnBoardAssessmentSearch.controller'
        }
    }
})
}]);

//*** Search ***//
ipmModule.config(['$stateProvider', '$stickyStateProvider', '$urlRouterProvider', function ($stateProvider, $stickyStateProvider, $urlRouterProvider) {
    $stateProvider
        .state('ips', {
            url: '/sea',

            views: {
                "homeview": {
                    templateUrl: '/app/ipm/partials/ipmSearch.html',
                    controller: 'ipm.search.controller'
                }
            }
        })
    .state('ipsc', {
        url: '/scr',

        params: { filter: null },
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmSearchCrew.html',
                controller: 'ipm.searchCrew.controller'
            }
        }
    })
        .state('ipsc.crewInfo', {
            url: '/sci',
            params: { filter: null },
            views: {
                "uiViewCrewSearch": {
                    templateUrl: '/app/ipm/partials/ipmCrewTab.html',
                    controller: 'ipm.crewInfo.controller'
                }
            }
        })
    .state('ipsh', {
        url: '/shi',

        params: { filter: null },
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmSearchHotelInfo.html',
                controller: 'ipm.searchHotelInfo.controller'
            }
        }
    })
    .state('ipsh.hotelinfo', {
        url: '/shihi',

        params: { filter: null },
        views: {
            "uiViewSearchHotelInfo": {
                templateUrl: '/app/ipm/partials/ipmHotelTab.html',
                controller: 'ipm.hotelInfo.controller'
            }
        }
    })
    .state('ipss', {
        url: '/sso',
        sticky: true,
        params: { filter: null },
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmSearchSOS.html',
                controller: 'ipm.searchSOS.controller'
            }
        }
    })
         .state('ipss.sos', {
             url: '/ipsso',

             params: { filter: null },
             views: {
                 "uiViewSOSSearch": {
                     templateUrl: '/app/ipm/partials/ipmSOSTab.html',
                     controller: 'ipm.summaryService.controller'
                 }
             }
         })
    .state('ipsi', {
        url: '/ssi',

        params: { filter: null },
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmSearchStationInfo.html',
                controller: 'ipm.searchStationInfo.controller'
            }
        }
    })
   .state('ipsi.stations', {
       url: '/ssist',

       params: { filter: null },
       views: {
           "uiViewSearchStations": {
               templateUrl: '/app/ipm/partials/ipmStationTab.html',
               controller: 'ipm.stationInfo.controller'
           }
       }
   })
 .state('ipwe', {
     url: '/ipwe',

     params: { filter: null },
     views: {
         "homeview": {
             templateUrl: '/app/ipm/partials/ipmSearchWeather.html',
             controller: 'ipm.searchWeather.controller'
         }
     }
 })
     .state('ipcu', {
         url: '/ipcu',

         params: { filter: null },
         views: {
             "homeview": {
                 templateUrl: '/app/ipm/partials/ipmSearchCurrency.html',
                 controller: 'ipm.searchCurrency.controller'
             }
         }
     })
        .state('crewlocator', {
            url: '/ipcl',

            params: { filter: null },
            views: {
                "homeview": {
                    templateUrl: '/app/ipm/partials/ipmCrewLocator.html',
                    controller: 'ipm.crewLocator.controller'
                }
            }
        })

        .state('crewlocatorsearch', {
            url: '/icls',

            params: { filter: null },
            views: {
                "homeview": {
                    templateUrl: '/app/ipm/partials/ipmCrewLocatorSearch.html',
                    controller: 'ipm.crewLocatorSearch.controller'
                }
            }
        })
        .state('ipcu.ipcurrencydetail', {
            url: '/iscu',

            params: { filter: null },
            views: {
                "uiViewSearchCurrency": {
                    templateUrl: '/app/ipm/partials/ipmCurrencyDetail.html',
                    controller: 'ipm.currencyDetail.controller'
                }
            }
        })
     .state('iptr', {
         url: '/iptr',

         params: { filter: null },
         views: {
             "homeview": {
                 templateUrl: '/app/ipm/partials/ipmSearchTransport.html',
                 controller: 'ipm.searchTransport.controller'
             }
         }
     })
    .state('ipsdo', {
        url: '/isdo',

        params: { filter: null },
        views: {
            "homeview": {
                templateUrl: '/app/ipm/partials/ipmSearchDocument.html',
                controller: 'ipm.searchDocument.controller'
            }
        }
    })

     //.state('iptransport', {
     //    url: '/istr',

     //    params: { filter: null },
     //    views: {
     //        "uiViewSearchTransport": {
     //            templateUrl: '/app/ipm/partials/ipmTransport.html',
     //            controller: 'ipm.Transport.controller'
     //        }
     //    }
     //})
     .state('ipweatherinfo', {
         url: '/ipwi',

         params: { filter: null },
         views: {
             "uiViewSearchWeather": {
                 templateUrl: '/app/ipm/partials/ipmWeatherInfo.html',
                 controller: 'ipm.weatherInfo.controller'
             }
         }
     })
}]);