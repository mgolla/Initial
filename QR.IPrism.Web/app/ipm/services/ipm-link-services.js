
angular.module('app.ipm.module').factory('linkService', ['webAPIService', function (webAPIService) {
    return {
        getLinkList: _getLinkList
    }
    function _getLinkList(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPostSt('api/link', data, successCall, errorCall, alwaysCall);
    }

}]);

