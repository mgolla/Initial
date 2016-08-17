
angular.module('app.ipm.module').factory('departmentNewsService', ['webAPIService', function (webAPIService) {
    return {
        getDepartmentNewsIFEGuideList: _getDepartmentNewsIFEGuideList ,
        getGetAirlinesNews:_getGetAirlinesNews
    }
    function _getDepartmentNewsIFEGuideList(data, successCall, errorCall, alwaysCall, reload) {

        webAPIService.apiPostSt('api/DepartmentNews', data, successCall, errorCall, alwaysCall, reload);
    }
    function _getGetAirlinesNews(data, successCall, errorCall, alwaysCall, reload) {

        webAPIService.apiPostSt('api/AirlinesNews/GetAirlinesNews', data, successCall, errorCall, alwaysCall, reload);
    }
    

}]);

