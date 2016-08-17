'use strict'
angular.module('app.ipm.module').controller('ipm.alertTabs.controller', ['$scope','$rootScope', '$http', 'sharedDataService', 'analyticsService', '$state',
    function ($scope,$rootScope, $http, sharedDataService, analyticsService, $state) {
        var Initialize = function () {
            $scope.Alerts = 1;
            $scope.MyRequest = 2;
            $scope.SVPT = 3;           
            $rootScope.selectedAlertTab = $scope.Alerts;
        $scope.tabAlertContentUrl = "/app/ipm/partials/ipmAlertTabsNotification.html";
        $rootScope.isTab = true;     
       
       
    }

    Initialize();

    var setDefaultNewsTab = function () {
        $rootScope.selectedNewsTab = $scope.Alerts;
    }

    $scope.onAlertTabChange = function (tab) {
        $rootScope.isTab = true;
        $rootScope.selectedAlertTab = tab;
        $("#AlertTabsUI").empty();       
      
    };
    $scope.isGetSelectedTab = function (tab) {
        return $rootScope.selectedAlertTab === tab;

    };
    $rootScope.onClickVideoBackButton = function () {
        $rootScope.isTab = true;

    };
   
}]);

