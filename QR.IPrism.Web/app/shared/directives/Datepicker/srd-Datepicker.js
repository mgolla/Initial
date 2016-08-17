"use strict";
angular.module('app.shared.components').directive('srdDatepicker', [function ($scope) {
    return {
        restrict: 'E',
        scope: {
            model: "=", //Model Name
            myid: "@",  //ID Name
            name: '@', //Field Name
            label: '@', //Label Name
            required: '=', //Field required attribute true/false
            labelClass: '@',
            fieldClass: '@',
            min: '=?',
            max: '=?',
            submitted: '=?',
            disabled: '=?',
            rangeError: '=?',
            dtdisabled: '=?',
            srdtabindex: '=?'
        },
        templateUrl: '/app/shared/directives/Datepicker/srd-Datepicker.html',
        controller: ['$scope', function ($scope) {

            $scope.format = 'dd-MMM-yyyy';
            $scope.dateOptions = {
                formatYear: 'yyyy',
                startingDay: 1
            };

            $scope.datePicker = function ($event) {
                //if ($event) {
                //    $event.preventDefault();
                //    $event.stopPropagation();
                //}
                this.datePickerIs = true;
            };

            $scope.datedisabled = function (status, date, mode) {
                if (status)
                    return (status && date.getDate() != 1);
                else
                    return false;
            };           
        }],
        link: function (scope, element, attrs, ctrls) {          
            if (!attrs.min) {
                scope.min = new Date(1990, 1, 1);
            }

            if (!attrs.max) {
                scope.max = new Date(2050, 5, 22);
            }
        }
    };
}]);