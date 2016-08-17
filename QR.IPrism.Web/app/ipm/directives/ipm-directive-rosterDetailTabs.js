"use strict"
angular.module('app.ipm.module').directive('rosterdetailtabs', [function () {
    return {
        restrict: "E",
        templateUrl: "/app/ipm/partials/ipmRosterDetailsTabs.html",
        controller: 'ipm.rosterDetailTabs.controller'
    }
}]);
