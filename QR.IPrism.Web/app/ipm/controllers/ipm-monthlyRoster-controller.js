'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.monthlyRoster.controller', ['$scope', '$http', 'rosterService',  'analyticsService',  '$state', '$rootScope','$filter',
                                  function ($scope, $http, rosterService, analyticsService, $state, $rootScope, $filter) {
                                      var rosterType = 2;//monthly
                                      var Initialize = function () {
                                          $rootScope.isRosterChanged = true;
                                          //$rootScope.handleRosterTypeRadioClick(rosterType);
                                       
                                          
                                          $rootScope.insertHotel = false;
                                          $scope.isOpenPrint = false;

                                          $rootScope.reloadData();
                                      }

                                      $scope.printToCart = function (IsInsertHotel) {
                                         
                                          $scope.isOpenPrint = true;
                                          var hotelInfo = '';
                                          var name = '';
                                          var staffID = '';
                                          var grade = '';
                                          var timeFormat = 'Local ';
                                          if ($rootScope.selectedTimeFormat == 1) {
                                              timeFormat = 'UTC';
                                          } else if ($rootScope.selectedTimeFormat == 1) {
                                              timeFormat = 'Doha';
                                          }
                                          else {
                                              timeFormat = 'Local';
                                          }

                                          var timeFormat = 'All times in ' + timeFormat + ', From ' + $filter('date')($rootScope.rosterViewModel.StartDate, 'dd-MMM-yyyy') + ' To ' + $filter('date')($rootScope.rosterViewModel.EndDate, 'dd-MMM-yyyy') + '.';
                                          $('#AllTimesUTCTextLbl').text(timeFormat);

                                          $('#CodeExTd').html($('#CodeExSection').html());
                                          $('#FlightInTd').html($('#FlightInSection').html());
                                          $('#UTCDiffTd').html($('#UTCDiffSection').html());
                                          $('#BlockHrsTd').html($('#BlockHrsSection').html());
                                       
                                          // var monthlyRoster = document.getElementById('roasterContent').outerHTML;
                                          var monthlyRoster = $('#pnlPrintDetailedRoster').html();
                                         

                                          var popupWinindow = window.open('', '_blank');
                                          //var head = $("head").html();
                                          //head = head.replace('rosterTable.css', 'printRosterTable.css');
                                          //head = head.replace('rel ="stylesheet"', 'rel="stylesheet" media="screen, print"');
                                         
                                          var head = '<head><style type="text/css">@charset "UTF-8";[ng\:cloak],[ng-cloak],[data-ng-cloak],[x-ng-cloak],.ng-cloak,.x-ng-cloak,.ng-hide:not(.ng-hide-animate){display:none !important;}ng\:form{display:block;}.ng-animate-shim{visibility:hidden;}.ng-anchor{position:absolute;}</style>'
                                       + '<meta http-equiv="X-UA-Compatible" content="IE=edge"> '
                                       + '<meta charset="utf-8">  '
                                       + '<meta name="viewport" content="initial-scale=1, maximum-scale=1">  '
                                       + '<title>iPrism</title> '
                                       + '<link href="/Content/ui-grid.css" rel="stylesheet" type="text/css"> '
                                       + '<link href="/Content/bootstrap.min.css" rel="stylesheet"> '
                                       + '<link href="/Content/bootstrap-theme.css" rel="stylesheet">  <link href="/Content/angular-block-ui.css" rel="stylesheet">'
                                       + '<link href="/Content/css/styles/printRosterTable.css" rel="stylesheet">'
                                       + '<link href="/Content/css/styles/dashboard.css" rel="stylesheet"> '
                                       + '<link href="/Content/css/styles/profileTabs.css" rel="stylesheet">'
                                       + '<script src="/Scripts/modernizr-2.8.3.js"></script>'
                                     
                                         + ' </head>';
                                          
                                                                                   
                                          var script = '<script src="../../../Scripts/jquery-2.1.4.min.js"></script>';
                                          var pageHeader = '';
                                          popupWinindow.document.open();
                                          popupWinindow.document.write('<html>' + head  + '<body onload="window.print()">' + pageHeader + '<br>' + monthlyRoster
                                               +  script + '</html>');
                                          popupWinindow.document.close();


//
                                       
                                       //   var win = window.open('', '_blank');
                                       //   win.document.write('<style>@page{size:landscape;}</style>'
                                       //         + '<link href="/Content/ui-grid.css" rel="stylesheet" type="text/css"> '
                                       //+ '<link href="/Content/bootstrap.min.css" rel="stylesheet"> '
                                       //+ '<link href="/Content/bootstrap-theme.css" rel="stylesheet">  <link href="/Content/angular-block-ui.css" rel="stylesheet">'
                                       ////+ '<link href="/Content/css/styles/printRosterTable.css" rel="stylesheet">'
                                       //+ '<link href="/Content/css/styles/dashboard.css" rel="stylesheet"> '
                                     
                                       //+ '<script src="/Scripts/modernizr-2.8.3.js"></script>'+'<html><head><title></title>');
                                       //   win.document.write('</head><body >aaaaaaaaaaaaaaaaa');
                                       //   win.document.write(monthlyRoster);
                                       //   win.document.write('</body></html>');
                                       //  // win.print();
                                       //  // win.close();

                                         
                                          //var win = window.open('', '_blank');
                                          //win.document.write('<style>@page{size:landscape;}</style><html><head><title></title>');
                                          //win.document.write('</head><body onload="window.print()">');
                                          //win.document.write(monthlyRoster);
                                          //win.document.write('</body></html>');
                                        
                                         // win.close();
                                       //   return true;
                                      }

                                      
                                     

                                      $scope.onClickHotelInfo = function () {
                                          if ($rootScope.insertHotel) {
                                              $rootScope.insertHotel = false;
                                          } else {
                                              $rootScope.insertHotel = true;
                                          }
                                      }
                                      $scope.formatDate = function (date) {
                                          var dateOut = new Date(date);
                                          return dateOut;
                                      };
                                      Initialize();
                                      
                                  }]);


'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.codeExplanations.controller',
		['$scope', '$http', 'rosterService', 'analyticsService', '$state', '$rootScope', 'blockUI', 
    
function ($scope, $http, rosterService, analyticsService, $state, $rootScope, blockUI) {
    //Controller Scope Initialization
    var ilookupblockUI = blockUI.instances.get('ipmlookupBlockUIcodeEx');
    var Initialize = function () {
        //Declare all scop properties
        $scope.selectedLookupTab = 0;
        $scope.selectedLookup = '';

       
    }

    

    $scope.isSelected = function (section) {

        return $scope.selectedLookup === section;
    }
    $scope.onLookupTabChange = function (tab) {
        $scope.selectedLookupTab = tab;

    };
    $scope.isLookupSelectedTab = function (tab) {
        return $scope.selectedLookupTab === tab;

    };

    Initialize();

}]);

'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.flightIndicator.controller',
		['$scope', '$http', 'sharedDataService', 'analyticsService', '$state', '$rootScope', 'blockUI',
function ($scope, $http, sharedDataService, analyticsService, $state, $rootScope, blockUI) {
    //Controller Scope Initialization
    var ilookupblockUI = blockUI.instances.get('ipmlookupBlockUIflightIndi');
    var Initialize = function () {
        //Declare all scop properties
        $scope.lookupViewModelflightIndi=[];
        $scope.filter = {
            filters: ""

        }
        $scope.selectedLookupTab = 0;
        $scope.selectedLookup = '';

        loadLookupList($scope.filter);
    }

    function loadLookupList(filter) {
        ilookupblockUI.start();
       
        sharedDataService.getCommonInfo('FLIGHTINDICATOR', function (result) {
            //$scope.lookupViewModelflightIndi = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });
            $scope.lookupViewModelflightIndi = result;
            ilookupblockUI.stop();
        },
        function (error) {
            ilookupblockUI.stop();
        }
        );
    }

    $scope.isSelected = function (section) {

        return $scope.selectedLookup === section;
    }
    $scope.onLookupTabChange = function (tab) {
        $scope.selectedLookupTab = tab;

    };
    $scope.isLookupSelectedTab = function (tab) {
        return $scope.selectedLookupTab === tab;

    };

    Initialize();

}]);

'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.UTCDiff.controller',
		['$scope', '$http', 'rosterService', 'analyticsService', '$state', '$rootScope', 'blockUI',
function ($scope, $http, rosterService, analyticsService, $state, $rootScope, blockUI) {
    //Controller Scope Initialization
    var ilookupblockUI = blockUI.instances.get('ipmlookupBlockUIUTCDiff');
    var Initialize = function () {
        //Declare all scop properties
        
        $scope.selectedLookupTab = 0;
        $scope.selectedLookup = '';

       
    }

   

    $scope.isSelected = function (section) {

        return $scope.selectedLookup === section;
    }
    $scope.onLookupTabChange = function (tab) {
        $scope.selectedLookupTab = tab;

    };
    $scope.isLookupSelectedTab = function (tab) {
        return $scope.selectedLookupTab === tab;

    };

    Initialize();

}]);

'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.printhotelinfo.controller',
		['$scope', '$http', 'rosterService', 'analyticsService', '$state', '$rootScope', 'blockUI',
function ($scope, $http, rosterService, analyticsService, $state, $rootScope, blockUI) {
    //Controller Scope Initialization
   
    var Initialize = function () {
        //Declare all scop properties
        $scope.stationsAll = $.grep($rootScope.stations, function (el, index) {
            return index === $.inArray(el, $rootScope.stations);
        });

        var filter = {
            AirportCode: $scope.stationsAll.join(",")
        }
        $scope.hotelInfoViewModel;
        $scope.selectedLookupTab = 0;
        $scope.selectedLookup = '';
             

        loadprintHotelList(filter);
    }

    function loadprintHotelList(filter) {

        rosterService.getPrintHotelInfos(filter, function (result) {
            $rootScope.hotelInfoViewModel = result.data;

        },
        function (error) {

        }
        );
    }

    $scope.isSelected = function (section) {

        return $scope.selectedLookup === section;
    }
    $scope.onLookupTabChange = function (tab) {
        $scope.selectedLookupTab = tab;

    };
    $scope.isLookupSelectedTab = function (tab) {
        return $scope.selectedLookupTab === tab;

    };

    Initialize();

}]);

