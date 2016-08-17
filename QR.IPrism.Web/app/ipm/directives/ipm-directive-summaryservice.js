
"use strict"
angular.module('app.ipm.module').directive('summaryservice', [function () {
    return {
        restrict: "E",
        templateUrl: "/app/ipm/partials/ipmSummaryService.html" ,
          controller: 'ipm.summaryService.controller'
    }
}]);


