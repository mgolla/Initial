"use strict";
angular.module('app.shared.components').directive('srdDateTimepicker', [function ($scope) {
    return {
        restrict: 'E',
        scope: {
            model: "=", //Model Name
            name: '@', //Field Name
            label: '@', //Label Name
            required: '=', //Field required attribute true/false
            labelClass: '@',
            fieldClass: '@',
            submitted: '=?',
            disabled: '=?'
        },
        templateUrl: '/app/shared/directives/Timepicker/srd-Timepicker.html',
        controller: ['$scope', function ($scope) {

            $scope.options = {
                icons:
                    {
                        next: 'glyphicon glyphicon-arrow-right',
                        previous: 'glyphicon glyphicon-arrow-left',
                        up: 'glyphicon glyphicon-arrow-up',
                        down: 'glyphicon glyphicon-arrow-down'
                    },
                defaultDate: $scope.model
            };
        }],
        link: function (scope, element, attrs, ctrls) {

            element.find('span.input-group-addon').bind('click', function () {
                element.find('input').focus();
            });
        }
    };
}]);