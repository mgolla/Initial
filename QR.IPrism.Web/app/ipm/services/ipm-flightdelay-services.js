/*********************************************************************
* File Name     : ipm-housing-services.js
* Description   : Contains all the service calls for housing module.
* Create Date   : 25th Jan 2016
* Modified Date : 25th Jan 2016
* Copyright by  : Qatar Airways
*********************************************************************/

angular.module('app.ipm.module')
        .factory('flightDelayService', ['webAPIService', function (webAPIService) {

            return {
                getDelaySearchResults: _getDelaySearchResults,
                getEnterFlightDelayResults: _getEnterFlightDelayResults,
               // getsectordetails: _getsectordetails,
                getdelaylookupvalues: _getdelaylookupvalues,
                setFlightDelayDetails: _setFlightDelayDetails,
                getdelayReason: _getdelayReason,
                isEnterDelayForFlight: _isEnterDelayForFlight
            }

            function _getDelaySearchResults(data, successCall, errorCall, alwaysCall, reload) {

                webAPIService.apiPostSt('api/GetDelaySearchResults', data, successCall, errorCall, alwaysCall, reload);
            }

            function _getEnterFlightDelayResults(data, successCall, errorCall, alwaysCall, reload) {

                webAPIService.apiPostSt('api/GetEnterFlightDelayResults', data, successCall, errorCall, alwaysCall, reload);
            }

            function _setFlightDelayDetails(data, successCall, errorCall, alwaysCall, reload) {

                webAPIService.apiPost('api/SetFlightDelayDetails', data, successCall, errorCall, alwaysCall, reload);
            }

            function _isEnterDelayForFlight(data, successCall, errorCall, alwaysCall, reload) {

                webAPIService.apiGet('api/IsEnterDelayForFlight/' + data, null, successCall, errorCall, alwaysCall, reload);
            }

            //function _getsectordetails(data, successCall, errorCall, alwaysCall) {

            //    webAPIService.apiGet('api/GetSectorDetails', data, successCall, errorCall, alwaysCall);
            //}

            function _getdelaylookupvalues(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/GetDelaylookupvalues', data, successCall, errorCall, alwaysCall);
            }

            function _getdelayReason(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/GetDelayReason/' + data, null, successCall, errorCall, alwaysCall);
            }

        }]);