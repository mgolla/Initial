angular.module('app.shared.components').filter('ipmSplit', function () {
        return function (input, splitChar, splitIndex) {
            return input.split(splitChar)[splitIndex];
        }
    });