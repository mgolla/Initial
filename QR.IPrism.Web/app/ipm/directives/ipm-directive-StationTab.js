"use strict"
angular.module('app.ipm.module').directive('stationtab', [function () {
    return {
        restrict: "E",
        templateUrl: "/app/ipm/partials/ipmStationTab.html",
        controller: 'ipm.stationTab.controller'
    }
}]);