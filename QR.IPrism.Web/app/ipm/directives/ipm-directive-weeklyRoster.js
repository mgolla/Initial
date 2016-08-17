
"use strict"
angular.module('app.ipm.module').directive('weeklyroster', [function () {
    return {
        restrict: "E",
        templateUrl: "/app/ipm/partials/ipmRosterWeekly.html",
        controller: 'ipm.weeklyRoster.controller'
    }
}]);