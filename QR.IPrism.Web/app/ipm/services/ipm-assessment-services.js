angular.module('app.ipm.module')
        .factory('assessmentServices', ['webAPIService', function (webAPIService) {

            return {
                getAssessmentDetails: _getAssessmentDetails,
                getAssessmnetByGrade: _getAssessmnetByGrade,
                getAssessorAssesseeFlightDetails: _getAssessorAssesseeFlightDetails,
                getRatingGuidelines: _getRatingGuidelines,
                //getAssessmentObjectivePercentages: _getAssessmentObjectivePercentages,
                postInsertUpdateAssessment: _postInsertUpdateAssessment,
                getMyAssessmentList: _getMyAssessmentList,
                deleteDocuments: _deleteDocuments,
                deleteAssessment: _deleteAssessment
            }

            function _deleteDocuments(data, successCall, errorCall, alwaysCall) {
                webAPIService.apiPost('api/deleteComnDoc/', data, successCall, errorCall, alwaysCall);
            }

            function _getAssessmentDetails(data, successCall, errorCall, alwaysCall) {
                webAPIService.apiGet('api/getassessmentdetails/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _getAssessmnetByGrade(data, successCall, errorCall, alwaysCall) {
                webAPIService.apiGet('api/getassessmnetbygrade/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _getAssessorAssesseeFlightDetails(data, successCall, errorCall, alwaysCall) {
                webAPIService.apiGet('api/getassessorassesseeflightdetails/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _getRatingGuidelines(data, successCall, errorCall, alwaysCall) {
                webAPIService.apiGet('api/getratingguidelines', data, successCall, errorCall, alwaysCall);
            }

            function _deleteAssessment(data, successCall, errorCall, alwaysCall) {
                webAPIService.apiGet('api/DeleteAssessment/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _postInsertUpdateAssessment(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/insertupdateassessment', data, successCall, errorCall, alwaysCall);
            }

            //function _getAssessmentObjectivePercentages(data, successCall, errorCall, alwaysCall) {
            //    webAPIService.apiGet('api/getassessmentobjectivepercentages', data, successCall, errorCall, alwaysCall);
            //}

            function _getMyAssessmentList(data, successCall, errorCall, alwaysCall) {
                webAPIService.apiGet('api/getViewAssmtAsync/' + data, null, successCall, errorCall, alwaysCall);
            }

        }]);