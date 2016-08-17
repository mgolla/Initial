
angular.module('app.ipm.module').factory('myRequestService', ['webAPIService', function (webAPIService) {
    return {
        getMyRequestList: _getMyRequestList
    }
    function _getMyRequestList(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPostSt('api/myrequest', data, successCall, errorCall, alwaysCall);
    }

}]);

