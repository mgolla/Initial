
angular.module('app.ipm.module').factory('summaryServiceService', ['webAPIService', function (webAPIService) {
    return {
        getSummaryServiceList: _getSummaryServiceList
    }
    function _getSummaryServiceList(data, successCall, errorCall, alwaysCall, reload) {

        webAPIService.apiPostSt('api/SummaryOfService', data, successCall, errorCall, alwaysCall, reload);
    }

}]);

