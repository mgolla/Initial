"use strict";
angular.module('app.shared.components').directive('srdDropdown', [function () {
    return {
        restrict: 'E',
        scope: {
            labelClass: '@', //Label class
            fieldClass: '@', //Field class
            list: '=', //List items (ng-options)
            label: '@', //Label name
            title: '@', //Field title
            valueField: '@', //Field value
            nameField: '@', //Field value
            model: '=', //Field model name
            ngchange: '&',//On change function
            required: '@' //Field required attribute true/false
        },
        templateUrl: '/app/shared/directives/Dropdown/srd-Dropdown.html',
        link: function (scope, element, attrs) {

            if (scope.labelClass == undefined) {
                scope.labelClass = "col-md-6";
            }
            if (scope.fieldClass == undefined) {
                scope.fieldClass = "col-md-6";
            }

        },
    };
}]);