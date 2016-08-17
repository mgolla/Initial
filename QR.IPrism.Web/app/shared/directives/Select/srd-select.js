'use strict'
angular.module('app.shared.components').directive('srcSelect', [function ($scope) {
    return {
        restrict: 'E',
        scope: {
            labelClass: '@', //field label class
            fieldClass: '@', //Field class
            label: '@', //input label name
            name: '@', //input name
            model: '=', // input model
            required: '=', // required
            disabled: '=?',
            readonly: '=?',
            submitted: '=?',
            select: '&?',
            list: '=',
            multiple: '@?',
        }, controller: ['$scope', function ($scope) {
         

            //if ($scope.isnumber) {
            $scope.$watch('model', function (n,o) {
                console.log(n);
            });
            // }
        }],
        link: function (scope) {
            scope.item = {};
            scope.item.value = scope.model;
        },
        templateUrl: '/app/shared/directives/Select/srd-select.html'  
    }
}]).directive('mSelectAdapt', function () {
    return {
        require: 'ngModel',
        link: function (scope, ele, attr, ctrl) {
            var list = scope.$eval(attr.mSelectAdapt);

            ctrl.$parsers.push(function (value) {
                console.log(' >>>>>>>>>>>>>> parser ', value);
                var result = value ? value.Value : null;
                console.log('result ', result);
                return result;
            });
            ctrl.$formatters.unshift(function (value) {
                console.log(' >>>>>>>>>>>>>> formatters ', value);
                var result = null;
                angular.forEach(list, function (item) {
                    if (item.Value == value) {
                        result = item;
                    }
                });
                return result;
            });
        }
    };
}).filter('propsFilter', function () {
    return function (items, props) {
        var out = [];

        if (angular.isArray(items)) {
            items.forEach(function (item) {
                var itemMatches = false;

                var keys = Object.keys(props);
                for (var i = 0; i < keys.length; i++) {
                    var prop = keys[i];
                    var text = props[prop].toLowerCase();
                    if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                        itemMatches = true;
                        break;
                    }
                }

                if (itemMatches) {
                    out.push(item);
                }
            });
        } else {
            // Let the output be the input untouched
            out = items;
        }

        return out;
    }
}).filter('unique', function () {
    return function (items, filterOn) {

        if (filterOn === false) {
            return items;
        }

        if ((filterOn || angular.isUndefined(filterOn)) && angular.isArray(items)) {
            var hashCheck = {}, newItems = [];

            var extractValueToCompare = function (item) {
                if (angular.isObject(item) && angular.isString(filterOn)) {
                    return item[filterOn];
                } else {
                    return item;
                }
            };

            angular.forEach(items, function (item) {
                var valueToCheck, isDuplicate = false;

                for (var i = 0; i < newItems.length; i++) {
                    if (angular.equals(extractValueToCompare(newItems[i]), extractValueToCompare(item))) {
                        isDuplicate = true;
                        break;
                    }
                }
                if (!isDuplicate) {
                    newItems.push(item);
                }

            });
            items = newItems;
        }

        return items;
    };
});