
angular.module('app.ipm.module').factory('documentService', ['webAPIService', function (webAPIService) {
    return {
        getDocumentList: _getDocumentList
    }
    function _getDocumentList(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/document', data, successCall, errorCall, alwaysCall);
    }

}]);

