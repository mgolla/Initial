"use strict"
angular.module('app.ipm.module').directive('overviewtab', [function () {
    return {
        restrict: "E",
        templateUrl: "/app/ipm/partials/ipmOverviewTab.html",
        controller: 'ipm.overviewTab.controller'
    }
}]);