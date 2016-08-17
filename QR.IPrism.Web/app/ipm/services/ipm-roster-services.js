angular.module('app.ipm.module').factory('rosterService', ['webAPIService', function (webAPIService) {
    return {
        getWeeklyRosterList: _getWeeklyRosterList,
        getBackgroundImage: _getBackgroundImage,
        getDefaultBackgroundImage:_getDefaultBackgroundImage,
        getMonthlyRosterList: _getMonthlyRosterList,
        getCodeExplanations:_getCodeExplanations,
        getUTCDiffs: _getUTCDiffs,
        getPrintHotelInfos: _getPrintHotelInfos
    }
    function _getWeeklyRosterList(data, successCall, errorCall, alwaysCall, reload) {

        webAPIService.apiPostSt('api/roster/GetWeeklyRosters', data, successCall, errorCall, alwaysCall, reload);
    }
    function _getBackgroundImage(data, successCall, errorCall, alwaysCall, reload) {

        webAPIService.apiPostSt('api/roster/GetBackgroundImage', data, successCall, errorCall, alwaysCall, true);
    }
    function _getDefaultBackgroundImage(data, successCall, errorCall, alwaysCall, reload) {

        webAPIService.apiPostSt('api/roster/GetDefaultBackgroundImage', data, successCall, errorCall, alwaysCall, true);
    }
    function _getMonthlyRosterList(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/roster/GetRosters', data, successCall, errorCall, alwaysCall);
    }

    function _getCodeExplanations(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/dutycodes/' + data, null, successCall, errorCall, alwaysCall);
    }
    function _getUTCDiffs(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/utcdiff/post', data, successCall, errorCall, alwaysCall);
    }
    function _getPrintHotelInfos(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/printhotelinfo/post', data, successCall, errorCall, alwaysCall);
    }
}]);

angular.module('app.ipm.module').filter('singleDecimal', function ($filter) {
    return function (input) {
        if (isNaN(input)) return input;
        return Math.round(input * 10) / 10;
    };
});

angular.module('app.ipm.module').filter('setDecimal', function ($filter) {
    return function (input, places) {
        if (isNaN(input)) return input;
        // If we want 1 decimal place, we want to mult/div by 10
        // If we want 2 decimal places, we want to mult/div by 100, etc
        // So use the following to create that factor
        var factor = "1" + Array(+(places > 0 && places + 1)).join("0");
        return Math.round(input * factor) / factor;
    };
});
