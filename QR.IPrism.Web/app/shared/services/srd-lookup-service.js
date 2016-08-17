angular.module('app.shared.components').factory('lookupDataService', ['webAPIService', function (webAPIService) {
    var dict = {};
    return {
        getLookupList: _getLookupList,
        getLookupbyFilter : _getLookupbyFilter
    }
    function _getLookupList(lookupType, data, successCall, errorCall, alwaysCall) {
        var dataValue = data;        
       
        if (dataValue != null) {
                data = null;
                webAPIService.apiGet('api/lookup/filter/' + lookupType + '/' + dataValue, data, function (result) {
                dict[lookupType] = result;
                successCall(dict[lookupType]);
            }, errorCall, alwaysCall);
            }
            else {
                    if (dict[lookupType] != null) {
                        successCall(dict[lookupType]);
                    }
                    else {
                        webAPIService.apiGet('api/lookup/' + lookupType, data, function (result) {
                            dict[lookupType] = result;
                            successCall(dict[lookupType]);
                         }, errorCall, alwaysCall);
                    }
                  }
    }
        
    function _getLookupbyFilter(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPost('api/lookup/getlookupbyfilter', data, successCall, errorCall, alwaysCall);
    }
}]);