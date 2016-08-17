
'use strict'
angular.module('app.ipm.module').factory('crewProfileService', ['webAPIService', function (webAPIService) {
    return {
        getCrewPersonalDetails: _getCrewPersonalDetails,
       // getCrewPersonalDetailsHeader: _getCrewPersonalDetailsHeader,
        getCrewTrainingHistory: _getCrewTrainingHistory,
        getCrewQualnVisa: _getCrewQualnVisa,
        getCrewCareerPath: _getCrewCareerPath,
        getCrewIDPDets: _getCrewIDPDets,
        getCrewMyDoc: _getCrewMyDoc,
        getCrewDestVstd: _getCrewDestVstd
    }
    function _getCrewPersonalDetails(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPostSt('api/crewpersonaldetails/', data, successCall, errorCall, alwaysCall);
    }
    //function _getCrewPersonalDetailsHeader(data, successCall, errorCall, alwaysCall) {

    //    webAPIService.apiGetSt('api/crewpersonaldetailsheader/' + data, null, successCall, errorCall, alwaysCall);
    //}

    function _getCrewTrainingHistory(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGetSt('api/crewtraininghistory/' + data, null, successCall, errorCall, alwaysCall);
    }

    function _getCrewQualnVisa(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGetSt('api/crewqualvisa/' + data, null, successCall, errorCall, alwaysCall);
    }

    function _getCrewCareerPath(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGetSt('api/crewcareerpath/' + data, null, successCall, errorCall, alwaysCall);
    }

    function _getCrewIDPDets(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGetSt('api/crewidp/' + data, null, successCall, errorCall, alwaysCall);
    }

    function _getCrewMyDoc(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGetSt('api/crewmydoc/' + data, null, successCall, errorCall, alwaysCall);
    }

    function _getCrewDestVstd(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGetSt('api/crewdstvstd/' + data, null, successCall, errorCall, alwaysCall);
    }

}]);