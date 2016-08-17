
angular.module('app.ipm.module').factory('crewInfoService', ['webAPIService', function (webAPIService) {
    return {
        getCrewInfoList: _getCrewInfoList
    }
    function _getCrewInfoList(data, successCall, errorCall, alwaysCall, reload) {

        webAPIService.apiPostSt('api/crewinfo', data, successCall, errorCall, alwaysCall, reload);
    }

}]);

