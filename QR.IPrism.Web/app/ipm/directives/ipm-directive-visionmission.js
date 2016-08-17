
"use strict"
angular.module('app.ipm.module').directive('visionmission', [function () {
    return {
        restrict: "E",
        templateUrl: '/app/ipm/partials/ipmNewsTabsVision.html'
                 ,
        controller: 'ipm.visionMission.controller'
    }
}]);


