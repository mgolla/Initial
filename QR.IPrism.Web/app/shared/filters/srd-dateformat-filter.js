﻿angular.module('app.shared.components').filter('ipmDate', ['$filter', function ($filter) {
    return function (input) {
        if (input == null) { return ""; }
        var _date = $filter('date')(new Date(input), 'dd-MMM-yyyy');
        return _date;
    };
}]);
