/*********************************************************************
* File Name     : ipm-housing-services.js
* Description   : Contains all the service calls for housing module.
* Create Date   : 25th Jan 2016
* Modified Date : 25th Jan 2016
* Copyright by  : Qatar Airways
*********************************************************************/

angular.module('app.ipm.module')
        .factory('flightDetailsAddEditService', ['webAPIService', function (webAPIService) {

            return {
                insertFlightDetails : _insertFlightDetails,
                getFlightDetailForPaste: _getFlightDetailForPaste,
                getAutoSuggestStaffInformation: _getAutoSuggestStaffInformation,
                getAllLookUpWithParentDetails: _getAllLookUpWithParentDetails,
                getFlightDetails: _getFlightDetails,
                getAllLookUpDetails: _getAllLookUpDetails,
                getSingleFlight: _getSingleFlight,
                isVrForFlight: _isVrForFlight,
                isDelayRportForFlight: _isDelayRportForFlight,
                deleteFlightDetails: _deleteFlightDetails,
                getCrewsForFlight: _getCrewsForFlight
            }

            function _getFlightDetails(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/GetFlightDetails', data, successCall, errorCall, alwaysCall);
            }

            function _getAllLookUpDetails(data, successCall, errorCall, alwaysCall, reload) {

                webAPIService.apiGet('api/GetAllLookUpDetails/' + data, null, successCall, errorCall, alwaysCall, reload);
            }

            function _getSingleFlight(data, successCall, errorCall, alwaysCall, reload) {

                webAPIService.apiGet('api/GetSingleFlight/' + data, null, successCall, errorCall, alwaysCall, reload);
            }

            function _getAllLookUpWithParentDetails(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/GetAllLookUpWithParentDetails/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _getFlightDetailForPaste(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/GetFlightDetailForPaste/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _getAutoSuggestStaffInformation(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/GetAutoSuggestStaffInformation/', data, successCall, errorCall, alwaysCall);
            }

            function _insertFlightDetails(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/InsertFlightDetails/', data, successCall, errorCall, alwaysCall);
            }

            function _isVrForFlight(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/IsVrForFlight/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _isDelayRportForFlight(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/IsDelayRportForFlight/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _deleteFlightDetails(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/DeleteFlightDetails/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _getCrewsForFlight(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/GetCrewsForFlight/' + data, null, successCall, errorCall, alwaysCall);
            }
            
        }]);