
angular.module('app.ipm.module').factory('iFEGuideService', ['webAPIService', function (webAPIService) {
    return {
        getDepartmentNewsIFEGuideList: _getDepartmentNewsIFEGuideList
    }
    function _getDepartmentNewsIFEGuideList(data, successCall, errorCall, alwaysCall, reload) {

        webAPIService.apiPostSt('api/IFEGuide', data, successCall, errorCall, alwaysCall, reload);
    }

}]);

