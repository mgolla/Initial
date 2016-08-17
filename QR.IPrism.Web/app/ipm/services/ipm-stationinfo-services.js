
angular.module('app.ipm.module').factory('stationInfoService', ['webAPIService', function (webAPIService) {
    return {
        getStationInfoList: _getStationInfoList
    }
    function _getStationInfoList(data, successCall, errorCall, alwaysCall, reload) {

        webAPIService.apiPostSt('api/stationinfo', data, successCall, errorCall, alwaysCall, reload);
    }

}]);

