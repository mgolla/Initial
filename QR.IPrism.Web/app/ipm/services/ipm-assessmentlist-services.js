/*********************************************************************
* File Name     : ipm-assessmentlist-services.js
* Description   : Contains all the service calls for housing module.
* Create Date   : 29th feb 2016
* Modified Date : 29th Feb 2016
* Copyright by  : Qatar Airways
*********************************************************************/

angular.module('app.ipm.module')
        .factory('assessmentListService', ['webAPIService', function (webAPIService) {

            return {
                getAssessmentListResult: _getAssessmentListResult,
                getPreviousAssessments: _getPreviousAssessments
            }

            function _getAssessmentListResult(data, successCall, errorCall, alwaysCall) {
                webAPIService.apiGet('api/assessmentlist', data, successCall, errorCall, alwaysCall);
            }

            function _getPreviousAssessments(data, successCall, errorCall, alwaysCall) {
                webAPIService.apiPost('api/getPreviousAssessments', data, successCall, errorCall, alwaysCall);
            }
        }]);