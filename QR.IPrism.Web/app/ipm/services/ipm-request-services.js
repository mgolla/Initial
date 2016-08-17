angular.module('app.ipm.module').factory('requestService', ['webAPIService', function (webAPIService, lookupDataService) {
    return {
        getCrewList: _getCrewList,
        postCrewDetails: _postCrewDetails
    }
    function _getCrewList(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiGet('api/employee', data, successCall, errorCall, alwaysCall);
    }
    function _postCrewDetails(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPost('api/employee', data, successCall, errorCall, alwaysCall);
    }
}]);