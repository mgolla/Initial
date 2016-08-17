"use strict";
angular.module('app.shared.components').directive('srdImage', [function () {
    return {
        restrict: 'E',
        scope: {
            labelClass: '@', //field label class
            cls: '@', //Field class
            alt: '@',
            src: '=',
            label: '@'
        },
        templateUrl: '/app/shared/directives/Images/srd-image.html',
        link: function (scope, element, attrs, ctrls) {

            var img = element.find('img');

            img.bind('error', function () {
                img[0].src = '/Content/css/styles/images/imageNotFound.jpg';
            });
        },
        controller: ['$scope', function ($scope) {

        }]
    };
}]);