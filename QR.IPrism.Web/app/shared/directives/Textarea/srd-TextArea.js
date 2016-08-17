"use strict";
angular.module('app.shared.components').directive('srdTextarea', [function ($timeout) {
    return {
        restrict: 'E',
        scope: {
            labelClass: '@', //Label class
            fieldClass: '@', //Field class
            label: '@', //Label name
            name: '@', //Field name
            rows: '@', //Textarea rows
            model: '=', //Model name
            required: '=?', //Field required attribute true/false
            valid : '=?',
            message: '@',  //Max lenght message
            maxLength: '@', //Max length
            minlength: '@', //Min length 
            form: '=', //Form name
            regex: '@',
            submitted: '=?',
            readonly : '=?',
            customtext: '@',
            ngDisabled: '@',
            onChanged : '='

        },
        templateUrl: '/app/shared/directives/Textarea/srd-TextArea.html',
        controller: ['$scope', function ($scope) {

            //$scope.$watch('model', function (newVal, oldVal) {
            //    console.log(newVal);
            //    if (newVal && ($scope.maxLength - $scope.model.length - ($scope.model.length ? $scope.model.split("\n").length - 1 : 0)) < 0) {
            //        $scope.model = oldVal;

            //        //$scope.model = newVal.substr(0, newVal.length - $scope.model.split('\n').length)
            //    } else {
            //        $scope.commentLength = $scope.maxLength - (newVal ? newVal.length : 0);
            //    }
            //});

            //$scope.$watch('model', function (newValue, oldValue) {
            //    if (newValue) {
            //        console.log(newValue.length);
            //        if (newValue.length > $scope.maxLength) {
            //            $scope.model = oldValue;
            //        } else {
            //            $scope.commentLength = $scope.maxLength - newValue.length;
            //        }                   
            //    }
            //});


            $scope.message = '';
            // Display message on TextArea Box changed event
            $scope.onChange = function ($event) {

                if ($scope.model != undefined) {
                    if ($scope.model.length != 0) {
                        $scope.message = 'left..';
                    }
                }
                else {
                    $scope.message = '';
                }
            }
        }],
        link: function (scope, element, attrs) {
            if (scope.labelClass == undefined) {
                scope.labelClass = "col-md-6";
            }
            if (scope.fieldClass == undefined) {
                scope.fieldClass = "col-md-6";
            }

            attrs.$observe('focusMe', function (value) {
                if (value === "true") {
                    //$timeout(function () {
                    //    element[0].focus();
                    //}, 5);
                }
            });

            //scope.$watch('onChanged', function (nVal) { element.val(nVal); });
            //element.bind('blur', function () {
            //    var currentValue = element.val();
            //    if (scope.onChanged !== currentValue) {
            //        scope.$apply(function () {
            //            scope.onChanged = currentValue;
            //        });
            //    }
            //});
          
        }
    };
}]);
