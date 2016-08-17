angular.module('app.ipm.module').factory('searchService', ['webAPIService', 'lookupDataService', '$rootScope', 

    function (webAPIService, lookupDataService, $rootScope) {
    return {
        getSearchList: _getSearchList,
        //getAllCities: _getAllCities,
        //getAllFlightsToday: _getAllFlightsToday,
        getDocumentSearchResult: _getDocumentSearchResult,
        getCurrencyDetails: _getCurrencyDetails,
        getTransports: _getTransports,
        getWeatherInfos: _getWeatherInfos,
        autoSuggestStaffInfo: _autoSuggestStaffInfo,
       // getAllCurrencyCodes: _getAllCurrencyCodes,
        //getAllCountry: _getAllCountry,
        //getAllAirportCodes: _getAllAirportCodes,
        getCrewLocators: _getCrewLocators,
        getLocationCrewDetails: _getLocationCrewDetails,
        geCrewLocatorProcess: _geCrewLocatorProcess
    }
    function _getSearchList(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/Search/GetFlights', data, successCall, errorCall, alwaysCall);
    }
    function _getDocumentSearchResult(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/Search/GetDocumentSearchResult', data, successCall, errorCall, alwaysCall);
    }
    function _getCurrencyDetails(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/Search/GetCurrencyDetails', data, successCall, errorCall, alwaysCall);
    }
    function _getTransports(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/Search/GetTransports', data, successCall, errorCall, alwaysCall);
    }
    function _getWeatherInfos(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/Search/GetWeatherInfos', data, successCall, errorCall, alwaysCall);
    }

    function _autoSuggestStaffInfo(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/Search/GetAutoSuggestStaffInfo', data, successCall, errorCall, alwaysCall);
    }

    function _getCrewLocators(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPostSt('api/Search/GetCrewLocators', data, successCall, errorCall, alwaysCall);
    }
    function _getLocationCrewDetails(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/Search/GetLocationCrewDetails', data, successCall, errorCall, alwaysCall);
    }
    function _getDutySummarys(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/Search/GetDutySummarys', data, successCall, errorCall, alwaysCall);
    }
    function _getLocationFlights(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/Search/GetLocationFlights', data, successCall, errorCall, alwaysCall);
    }
    //function _getsectordetails(data, successCall, errorCall, alwaysCall) {

    //    webAPIService.apiGet('api/GetSectorDetails', data, successCall, errorCall, alwaysCall);
    //}
    function _geCrewLocatorProcess(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/Search/GeCrewLocatorProcess', data, successCall, errorCall, alwaysCall);
    }
  
    //function _getAllCities(scope) {
    //    if (!(scope.CitiesAll && scope.CitiesAll.length > 0)) {

    //        lookupDataService.getLookupList('Sector', null, function (result) {
    //            scope.CitiesAll = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text, Desc: obj.FilterText } });
    //        });
    //    }
    //}
    //function _getAllCurrencyCodes(scope) {
    //    if (!(scope.CurrencyCodeList && scope.CurrencyCodeList.length > 0)) {
    //        lookupDataService.getLookupList('CurrecncyCodes', null, function (result) {

    //            scope.CurrencyCodeList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });

    //        });
    //    }
    //}
    //function _getAllCountry(scope) {
    //    if (!(scope.CountryList && scope.CountryList.length > 0)) {
    //        lookupDataService.getLookupList('CountryCode', null, function (result) {

    //            scope.CountryList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });

    //        });
    //    }
    //}
    //function _getAllAirportCodes(scope) {
    //    if (!(scope.AirportCodeList && scope.AirportCodeList.length > 0)) {
    //        //lookupDataService.getLookupList('AirportCodes', null, function (result) {

    //        //    scope.AirportCodeList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });

    //        //});

    //        lookupDataService.getLookupList('Sector', null, function (result) {
    //            scope.AirportCodeList = result.map(function (obj) { return { Value: obj.FilterText, Text: obj.Text, Desc: obj.FilterText } });
    //        });

    //    }
    //}
    //function _getAllFlightsToday(scope) {
    //    if (!($rootScope.FlightAllToday && $rootScope.FlightAllToday > 0)) {
    //        var filter = {
    //            FromSector: '',
    //            ToSector: '',
    //            FromDate: '',
    //            ToDate: '',
    //        }
    //       _getSearchList(filter, function (result) {

    //           $rootScope.FlightAllToday = result.data;
    //           scope.FlightListToday = result.data;

    //        },
    //            function (error) {

    //            }
    //          );
    //    }
    //}

}]);

