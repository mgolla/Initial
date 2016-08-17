
"use strict"
angular.module('app.ipm.module').directive('monthlyroster', [function () {
    return {
        restrict: "E",
        templateUrl: "/app/ipm/partials/ipmRosterMonthly.html",
        controller: 'ipm.monthlyRoster.controller'
    }
}]);

