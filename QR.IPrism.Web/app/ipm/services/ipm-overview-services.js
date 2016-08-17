angular.module('app.ipm.module').factory('overviewService', ['webAPIService', function (webAPIService) {
    return {
        getOverviewList: _getOverviewList,
        getFlightLoadList: _getFlightLoadList
    }
    function _getOverviewList(data, successCall, errorCall, alwaysCall,reload) {

        webAPIService.apiPostSt('api/overview', data, successCall, errorCall, alwaysCall, reload);
    }

    function _getFlightLoadList(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/FlightLoad', data, successCall, errorCall, alwaysCall);
    }
}]);

