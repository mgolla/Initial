
"use strict"
angular.module('app.ipm.module').directive('hotelinfo', [function () {
    return {
        restrict: "E",
        templateUrl: "/app/ipm/partials/ipmHotelInfo.html" ,
          controller: 'ipm.hotelInfo.controller'
    }
}]);


