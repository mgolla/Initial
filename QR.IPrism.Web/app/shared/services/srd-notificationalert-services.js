
angular.module('app.shared.components').factory('notificationAlertService', ['webAPIService', function (webAPIService) {
    var dict = {}
    this.notifications;
    this.alertShown = false;

    return {
        getNotificationAlertSVPList: _getNotificationAlertSVPList,
        getAllNotification :_getAllNotification,
        //getNotificationAlertSVPListSt : _getNotificationAlertSVPListSt,
        getAlterNotificationHeaderList: _getAlterNotificationHeaderList,
        updateAlterNotificationHeader:_updateAlterNotificationHeader,
         updateAlterNotificationHeaderSt:_updateAlterNotificationHeaderSt
    }
    //function _getNotificationAlertSVPListSt(data, successCall, errorCall, alwaysCall) {
    //    webAPIService.apiPostSt('api/NotificationAlert/post', data, successCall, errorCall, alwaysCall);
    //}
    function _getNotificationAlertSVPList(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPost('api/NotificationAlert/post', data, successCall, errorCall, alwaysCall);
    }

    function _getAllNotification(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPost('api/allNotification', data, successCall, errorCall, alwaysCall);
    }

    function _getAlterNotificationHeaderList(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPostSt('api/AlterNotificationHeader/post', data, successCall, errorCall, alwaysCall);
    }
    function _updateAlterNotificationHeaderSt(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPostSt('api/AlterNotificationHeaderUpdate/post', data, successCall, errorCall, alwaysCall);
    }
     function _updateAlterNotificationHeader(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPost('api/AlterNotificationHeaderUpdate/post', data, successCall, errorCall, alwaysCall);
    }
   
}]);

