"use strict";
angular.module('app.shared.components').directive('srdInput', [function () {
    return {
        restrict: 'E',
        scope: {
            labelClass: '@', //field label class
            fieldClass: '@', //Field class
            type: '@', //input type
            label: '@', //input label name
            id: '@', //input id
            name: '@', //input name
            model: '=', // input model
            required: '=', // required
            min: '@',
            max: '@',
            regex: '@?',
            isnumber: '@?',
            maxlength: '@',
            minlength: '@',
            disabled: '=',
            readonly: '=',
            submitted: '=',
            placeholder: '@?',
            tabindex: '@?'
        },
        templateUrl: '/app/shared/directives/Input/srd-input.html',
        link: function (scope, element, attrs, ctrls) {

            //scope.form = ctrls[0];
            //scope.model = ctrls[1];
            //if (scope.labelClass == undefined) {
            //    scope.labelClass = "col-sm-6";
            //}
            //if (scope.fieldClass == undefined) {
            //    scope.fieldClass = "col-md-6";
            //}
            
            if (scope.minlength == '' || scope.minlength == undefined) {
                scope.minlength = 1;
            }

            if (attrs.isnumber) {

                element.bind('keyup', function (e) {
                    e.target.value = e.target.value.replace(/[^0-9]/g, '');
                });
            }
        },
        controller: ['$scope', function ($scope) {
            //if ($scope.isnumber) {
                //$scope.$watch('model', function () {
                //    if ($scope.model) {
                //        $scope.model = $scope.model.replace(/[^0-9]/g, '');
                //    }
                //});
           // }
        }]
    };
}]);
