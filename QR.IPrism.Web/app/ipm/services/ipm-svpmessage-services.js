
angular.module('app.ipm.module').factory('svpMessageService', ['webAPIService', function (webAPIService) {
    return {
        getNotificationAlertSVPList: _getNotificationAlertSVPList
    }
    function _getNotificationAlertSVPList(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPostSt('api/SVPMessage', data, successCall, errorCall, alwaysCall);
    }

}]);

