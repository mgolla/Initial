
angular.module('app.ipm.module').factory('hotelInfoService', ['webAPIService', function (webAPIService) {
    return {
        getHotelInfoList: _getHotelInfoList
    }
    function _getHotelInfoList(data, successCall, errorCall, alwaysCall, reload) {

        webAPIService.apiPostSt('api/hotelinfo', data, successCall, errorCall, alwaysCall, reload);
    }

}]);

