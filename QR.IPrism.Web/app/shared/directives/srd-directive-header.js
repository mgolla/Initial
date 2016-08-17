"use strict"
angular.module('app.shared.components').directive('appheader', [function () {
    return {
        restrict: "E",
        templateUrl: "/app/shared/templates/header.html",
        controller: "srd.menu.controller"
    }
}]);

angular.module('app.shared.components').directive('setHeightMain', [function ($window) {
    return {
        link: function (scope, element, attrs) {

            var vpw = $(window).width();
            if (vpw < 769) {

                var getHeight = $(window).height() - 100;
                //$('.content_wrapper').css({ 'min-height': getHeight + 'px' });
                $('.content_wrapper').css('min-height', getHeight + 'px');
                element.css('min-height', getHeight);
            }

            else if (vpw > 769) {

                var getHeight = $(window).height() - 100;
                //$('.content_wrapper').css({ 'min-height': getHeight + 'px' });
                $('.content_wrapper').css('min-height', getHeight + 'px');
                element.css('min-height', getHeight);
            }

        }
    }

}]);

angular.module('app.shared.components').directive('ngIncludeTemplate', function () {
    return {  
        templateUrl: function(elem, attrs) { return attrs.ngIncludeTemplate; },  
        restrict: 'A',  
        scope: {  
            'ngIncludeVariables': '&'  
        },  
        link: function(scope, elem, attrs) {  
            var vars = scope.ngIncludeVariables();  
            Object.keys(vars).forEach(function(key) {  
                scope[key] = vars[key];  
            });  
        }  
    }  
})