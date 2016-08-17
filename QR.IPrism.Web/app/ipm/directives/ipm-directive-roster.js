
"use strict"
angular.module('app.ipm.module').directive('roster', [function () {
    return {
        restrict: "E",
        templateUrl: "/app/ipm/partials/ipmRoster.html" ,
          controller: 'ipm.roster.controller'
    }
}]);

angular.module('app.ipm.module').directive('loadScript', [function () {
    return function (scope, element, attrs) {
        return angular.element('asasaaaaaaaaaaaaaaa').appendTo(element);

        ///angular.element('<script src="/Scripts/jsapi.js"></script> <script src="/app/ipm/controllers/ipm-airlinenews-controller.js"></script>').appendTo(element);
    }
}]);



angular.module('app.ipm.module').directive('setHeight', [function ($window) {
    return {
        link: function (scope, element, attrs) {
            var  maxHeight =119;
            var  vpw = $(window).width();
            if (vpw < 769) {

                $(".rosterList").removeClass("Roster_Monthly_new");
                $('.rosterList > ul > li').css('height', 'auto');
                $(".monthly_roster_new li").removeClass("month-fix");
            }

            else if (vpw > 769) {

                $(".rosterList").addClass("Roster_Monthly_new");
                $('.rosterList').css({ 'height': 'auto' });
                $(".monthly_roster_new li").addClass("month-fix");

                var boxes = $('.monthly_roster_new > ul > li');
                  maxHeight = Math.max.apply(
                       Math, boxes.map(function () {
                           return $(element).height();
                       }).get());
                boxes.height(maxHeight);
            }
            element.css('height', maxHeight);
        }
        }
    
}]);

