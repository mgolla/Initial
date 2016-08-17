
angular.module('app.ipm.module').factory('visionMissionService', ['webAPIService', function (webAPIService) {
    return {
        getVisionMissionList: _getVisionMissionList
    }
    function _getVisionMissionList(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPostSt('api/visionmission', data, successCall, errorCall, alwaysCall);
    }

}]);

