
'use strict'
angular.module('app.ipm.module').factory('kafouService', ['webAPIService', function (webAPIService) {

    return {
        getkafouFlights: _getkafouFlights,
        deleteDocuments: _deleteDocuments,
        getKafouMyRecog: _getKafouMyRecog,
        getCrewsForFlight: _getCrewsForFlight,
        getkfSearchResult: _getkfSearchResult,
        getkfSearchParams: _getkfSearchParams,
        getInitialCrewRecog: _getInitialCrewRecog,
        getkfSave: _getkfSave,
        getkfStatus: _getkfStatus,
        getkfRecTypeStatus: _getkfRecTypeStatus,
        hasFlightHoursElapsed: _hasFlightHoursElapsed,
        getKFRecogDetails: _getKFRecogDetails,
        getKFWalloffame: _getKFWalloffame,
        getKFByFlight: _getKFByFlight
    }

    function _getkafouFlights(data, successCall, errorCall, alwaysCall) {

        //webAPIService.apiGet('api/kafouflights/' + data, null, successCall, errorCall, alwaysCall);
        webAPIService.apiGet('api/kafouflights/', null, successCall, errorCall, alwaysCall);
    }

    function _deleteDocuments(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPost('api/kafouDeleteComnDoc/', data, successCall, errorCall, alwaysCall);
    }

    function _getKafouMyRecog(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPost('api/kafoumyrecog/', data, successCall, errorCall, alwaysCall);
    }

    function _getCrewsForFlight(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/GetKFCrewsForFlight/' + data, null, successCall, errorCall, alwaysCall);
    }

    function _getkfSearchResult(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/kfsearch/', data, successCall, errorCall, alwaysCall);
    }

    function _getkfSearchParams(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/getKFSearchParams/', data, successCall, errorCall, alwaysCall);
    }

    function _getInitialCrewRecog(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiGet('api/getIntialKFAddEdit/' + data, null, successCall, errorCall, alwaysCall);
    }

    function _getkfSave(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/kafousave/', data, successCall, errorCall, alwaysCall);
    }

    function _getkfStatus(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/kafoustatus/', data, successCall, errorCall, alwaysCall);
    }

    function _getkfRecTypeStatus(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/kafouRcgTypeStatus/', data, successCall, errorCall, alwaysCall);
    }

    function _hasFlightHoursElapsed(flightId, flightHrs, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/kafouflighthourselapsed/' + flightId + '/' + flightHrs, null, successCall, errorCall, alwaysCall);
    }

    function _getKFRecogDetails(recogId, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/getKFRecogDetails/' + recogId, null, successCall, errorCall, alwaysCall);
    }

    function _getKFWalloffame(recogId, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/getkfwalloffame/', null, successCall, errorCall, alwaysCall);
    }

    function _getKFByFlight(flightIdlst, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/kfbyflightIdList/' + flightIdlst, null, successCall, errorCall, alwaysCall);
    }

    //function _getkfFlightDetails(data, successCall, errorCall, alwaysCall) {

    //    webAPIService.apiGet('api/kafouflightdetails/', data, successCall, errorCall, alwaysCall);
    //}
    
}]);