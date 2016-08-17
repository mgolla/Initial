
'use strict'
angular.module('app.ipm.module').factory('evrSearchService', ['webAPIService', function (webAPIService) {

    this.evrDrafts = "";

    return {
        getevrSearchResult: _getevrSearchResult,
        getevrFlightDets: _getevrFlightDets,
        postEVRSave: _postEVRSave,
        getevrDraftForUser: _getevrDraftForUser,
        getLastTenEVRs: _getLastTenEVRs,
        getPendingEVRs: _getPendingEVRs,
        UpdateNOVR: _UpdateNOVR,
        getSubmittedEVRs: _getSubmittedEVRs,
        getEVRDetails: _getEVRDetails,
        getEVRDraftUpdate: _getEVRDraftUpdate,
        DeleteVR: _DeleteVR,
        postEVRFileDownload: _postEVRFileDownload,
        getAssemtSearchEvrs: _getAssemtSearchEvrs,
        deleteDocuments: _deleteDocuments
        //getevrOwnerDets: _getevrOwnerDets
    }

    function _getAssemtSearchEvrs(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/evrforpoassmt/' + data, null, successCall, errorCall, alwaysCall);
    }

    function _getevrSearchResult(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/evrsearch/post', data, successCall, errorCall, alwaysCall);
    }

    function _getevrFlightDets(data, successCall, errorCall, alwaysCall) {

        //This service SP is underconstruction.
        //webAPIService.apiGetSt('api/evrcrewdets/'+ data, successCall, errorCall, alwaysCall);
    }

    //function _getevrOwnerDets(data, successCall, errorCall, alwaysCall) {

    //    webAPIService.apiPost('api/evrownerdts/post', data, successCall, errorCall, alwaysCall);
    //}

    function _postEVRSave(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/evrsavepost', data, successCall, errorCall, alwaysCall);
    }

    function _getevrDraftForUser(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/evrdraft/' + data, null, successCall, errorCall, alwaysCall);
    }

    function _getLastTenEVRs(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/evrlastten/' + data, null, successCall, errorCall, alwaysCall);
    }

    function _getPendingEVRs(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/evrpending/', null, successCall, errorCall, alwaysCall);
    }

    function _UpdateNOVR(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGetSt('api/updatenovr/' + data, null, successCall, errorCall, alwaysCall);
        //webAPIService.apiPost('api/updatenovr', data, successCall, errorCall, alwaysCall);
    }

    function _DeleteVR(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGetSt('api/evrdelete/' + data, null, successCall, errorCall, alwaysCall);
        //webAPIService.apiPost('api/updatenovr', data, successCall, errorCall, alwaysCall);
    }

    function _getSubmittedEVRs(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiGet('api/evrsubmitted/' + data, null, successCall, errorCall, alwaysCall);
    }

    function _getEVRDetails(data, successCall, errorCall, alwaysCall) {

        //webAPIService.apiPost('api/evrdetails/' + data, null, successCall, errorCall, alwaysCall);
        webAPIService.apiGet('api/evrdetails/' + data, null, successCall, errorCall, alwaysCall);
    }
    
    function _getEVRDraftUpdate (data, successCall, errorCall, alwaysCall) {

        //webAPIService.apiPost('api/evrdrftupdate/', data,  successCall, errorCall, alwaysCall);
        webAPIService.apiGet('api/evrdrftupdate/' + data, null, successCall, errorCall, alwaysCall);
    }

    function _postEVRFileDownload(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPostFile('api/evrdownload/', data, successCall, errorCall, alwaysCall);
    }

    function _deleteDocuments(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPost('api/evrDeleteComnDoc/', data, successCall, errorCall, alwaysCall);
    }

}]);