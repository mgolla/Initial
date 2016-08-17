
"use strict"
angular.module('app.ipm.module').directive('notificationalert', [function () {
    return {
        restrict: "E",
        templateUrl: '/app/ipm/partials/ipmAlertTabsNotification.html',
        controller: 'ipm.notificationAlert.controller'
    }
}]);


