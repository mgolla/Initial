'use strict'
angular.module('app.ipm.module').controller('ipm.newsTabs.controller',
                 ['$scope', '$rootScope', '$http', 'sharedDataService', 'analyticsService', '$state',
      function ($scope, $rootScope, $http, sharedDataService, analyticsService, $state) {
          var Initialize = function () {
              
              $scope.OurVision = 1;
              $scope.SVPM = 2;
              $scope.Department = 3;
              $scope.Airline = 4;
              $scope.MonthlyIFE =5;
             
              $scope.selectedNewsTab = $scope.SVPM;
             // $state.go('home.svpmessage');

             // $scope.tabNewsContentUrl = "/app/ipm/partials/ipmNewsTabsSVPMessage.html";
              //setTimeout(function () {
              //    $('#SVPMTab').click();
              //}, 50);

             
          }

          Initialize();
          var setDefaultAlertTab = function () {
              $rootScope.selectedAlertTab = $scope.SVPM;
          }
          $scope.onNewsTabChange = function (tab) {
              $scope.selectedNewsTab = tab;
              //if (tab == 1) {
              //    $scope.tabNewsContentUrl = "/app/ipm/partials/ipmNewsTabsVision.html";
              //} else if (tab == 2) {
              //    $scope.tabNewsContentUrl = '/app/ipm/partials/ipmNewsTabsSVPMessage.html';
              //} else if (tab == 3) {
              //    $scope.tabNewsContentUrl = '/app/ipm/partials/ipmNewsTabsDepNews.html';
              //} else if (tab == 4) {
              //    //var script = document.createElement("script");
              //    //script.type = "text/javascript";
              //    //script.src = "/Scripts/jsapi.js";
              //    //document.head.appendChild(script);

              //    //var script1 = document.createElement("script");
              //    //script1.type = "text/javascript";
              //    //script1.src = "/app/ipm/controllers/ipm-airlinenews-controller.js";
              //    //document.head.appendChild(script1);

              //    $scope.tabNewsContentUrl = '/app/ipm/partials/ipmNewsTabsAirlineNews.html';
              //} else if (tab == 5) {
              //    $scope.tabNewsContentUrl = '/app/ipm/partials/ipmNewsTabsIFEGuide.html';
              //}  else {
              //    $scope.tabNewsContentUrl = "/app/ipm/partials/ipmNewsTabsVision.html";
              //}
              //setDefaultAlertTab();
          };
          $scope.isGetNewsSelectedTab = function (tab) {
              return $scope.selectedNewsTab === tab;

          };
          

      }]);

'use strict'
angular.module('app.ipm.module').controller('ipm.airlineNewsPage.controller', ['$scope','$timeout', '$rootScope', '$http', 'sharedDataService','departmentNewsService', 'analyticsService', '$state','blockUI','messages',
    function ($scope, $timeout, $rootScope, $http, sharedDataService,departmentNewsService, analyticsService, $state, blockUI,messages) {
        var idepartmentNewsIFEGuideblockUI = blockUI.instances.get('ipmdepartmentNewsIFEGuideBlockUI');
        var Initialize = function () {
            //Declare all scop properties
            $scope.departmentNewsIFEGuideViewModel;
            $scope.filter = {
                NewsType: 1,
                ProxyURL: messages.ProxyURL,
                RSSUrl: messages.RSSUrl,
                ProxyAccount :messages.ProxyAccount,
                ProxyPassword :messages.ProxyPassword,
                ProxyDomain :messages.ProxyDomain,
            }
            $scope.selectedDepartmentNewsIFEGuide = null;

            loadDepartmentNewsIFEGuideList($scope.filter);
        }

        function loadDepartmentNewsIFEGuideList(filter) {
            idepartmentNewsIFEGuideblockUI.start();
            departmentNewsService.getGetAirlinesNews(filter, function (result) {
                $scope.departmentNewsIFEGuideViewModel = result.data;
                if ($scope.departmentNewsIFEGuideViewModel != null && $scope.departmentNewsIFEGuideViewModel.length > 0) {
                    $scope.selectedDepartmentNewsIFEGuide = $scope.departmentNewsIFEGuideViewModel[0];
                }
                appSettings.isNotAirlineNewsLoad = false;
                idepartmentNewsIFEGuideblockUI.stop();
            },
              function (error) {
                  idepartmentNewsIFEGuideblockUI.stop();
              }, null, appSettings.isNotAirlineNewsLoad
            );
        }

        $scope.isSelected = function (section) {

            return $scope.selectedDepartmentNewsIFEGuide === section;
        }
        $scope.onDepNSelectedChange = function (section) {

            $scope.selectedDepartmentNewsIFEGuide = section;
        }
       

        $scope.isNullOrEmpty = function (section) {

            return (!section) || section === null || section.trim() === '';
        }

        var loadJS = function (url, implementationCode, location) {
            //url is URL of external file, implementationCode is the code
            //to be called from the file, location is the location to 
            //insert the <script> element

            var scriptTag = document.createElement('script');
            scriptTag.src = url;

            scriptTag.onload = implementationCode;
            scriptTag.onreadystatechange = implementationCode;

            location.appendChild(scriptTag);
        };

        $scope.loadScript = function (url, type, charset) {
            if (type === undefined) type = 'text/javascript';
            if (url) {
                var script = document.querySelector("script[src*='" + url + "']");
                if (!script) {
                    var heads = document.getElementsByTagName("head");
                    if (heads && heads.length) {
                        var head = heads[0];
                        if (head) {
                            script = document.createElement('script');
                            script.setAttribute('src', url);
                            script.setAttribute('type', type);
                            if (charset) script.setAttribute('charset', charset);
                            //head.appendChild(script);
                           document.body.appendChild(script);
                        }
                    }
                }
                return script;
            }
        };

        var yourCodeToBeCalled = function () {
            //your code goes here
        }
        var callController = function () {
            loadJS('/app/ipm/controllers/ipm-airlinenews-controller.js', yourCodeToBeCalled, document.body);
        }

        Initialize();

    }]);


angular.module('app.ipm.module').controller('ipm.airlineNewsSection.controller', ['$scope','$timeout', '$rootScope', '$http', 'sharedDataService', 'analyticsService', '$state',
    function ($scope,$timeout, $rootScope, $http, sharedDataService, analyticsService, $state) {
        var Initialize = function () {
            $scope.isLoadingAN = false;
            $scope.isLoadingSC = false;
            $timeout(function () {
                $scope.isLoadingSC = true;
                $timeout(function () {
                    $scope.isLoadingAN = true;
                }, 3000);
            }, 3000);
          
        }

        Initialize();

    }]);

angular.module('app.ipm.module').directive('script', function ($parse, $rootScope, $compile) {
    return {
        restrict: 'E',
        terminal: true,
        link: function(scope, element, attr) {
            if (attr.ngSrc) {
                var domElem = '<script src="'+attr.ngSrc+'" async defer></script>';
                $(element).append($compile(domElem)(scope));


            }
        }
    };
});