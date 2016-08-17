"use strict";
angular.module('app.shared.components').directive('messageDirectives', [function () {
    return {
        restrict: 'A',
        replace: true,
        scope: { message: '=errorMessage' },
        templateUrl: '/app/shared/templates/error-messages.html'
    }
}]);