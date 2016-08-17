angular.module('app.ipm.module')
    .filter('ipmDateFormat', ['$filter', function ($filter) {
    return function (input) {
        if (input == null) { return ""; }
        var _date = $filter('date')(new Date(input), 'dd-MMM-yyyy');
        return _date;
    };
    }]);

angular.module('app.ipm.module')
    .filter('ipmDateTimeFormat', ['$filter', function ($filter) {
        return function (input) {
            if (input == null) { return ""; }
            var _date = $filter('date')(new Date(input), 'dd-MMM-yyyy HH:mm:ss');
            return _date;
        };
    }]);