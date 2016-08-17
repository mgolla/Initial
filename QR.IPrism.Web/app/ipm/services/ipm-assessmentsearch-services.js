/*********************************************************************
* File Name     : ipm-assessment-services.js
* Description   : Contains all the service calls for assessment module.
* Create Date   : 2nd March 2016
* Modified Date : 
* Copyright by  : Qatar Airways
*********************************************************************/

angular.module('app.ipm.module')
          .factory('assessmentsearchService', ['webAPIService', function (webAPIService) {

              return {
                  getAssmtSearchResult: _getAssmtSearchResult,
                  getGrade: _getGrade,
                  getAssmtStatus: _getAssmtStatus,
                  getPoAssessmentDetails: _getPoAssessmentDetails,
                  getPoSearchResult: _getPoSearchResult,
                  cancelscheduledasmnt: _cancelscheduledasmnt,
                  scheduledassessment: _scheduledassessment,
                  getPendingAssessment: _getPendingAssessment,
                  //SearchPoAssessmentDetails: _SearchPoAssessmentDetails,
                  getGradeCsdCs: _getGradeCsdCs,
                  getAsmntRoster: _getAsmntRoster,
                  //getSectorTo : _getSectorTo,
                  getsavedscheduledassessment: _getsavedscheduledassessment,
                  validateUnscheduledData: _validateUnscheduledData,
                  getCrewExpectedAsmnt: _getCrewExpectedAsmnt,
                  getAllPreviousAssessment : _getAllPreviousAssessment,
                  getGradeByLoggedPerson: _getGradeByLoggedPerson,
                  getAutoSuggestStaffByGrade: _getAutoSuggestStaffByGrade
              }

              function _getAutoSuggestStaffByGrade(data, successCall, errorCall, alwaysCall) {

                  webAPIService.apiPost('api/GetAutoSuggestStaffByGrade/', data, successCall, errorCall, alwaysCall);
              }

              function _getAssmtSearchResult(data, successCall, errorCall, alwaysCall) {
                  webAPIService.apiPost('api/getAssmtSearchresult', data, successCall, errorCall, alwaysCall);
              }
              function _getGradeByLoggedPerson(data, successCall, errorCall, alwaysCall) {
                  webAPIService.apiPost('api/getGradeByLoggedPerson', data, successCall, errorCall, alwaysCall);
              }

              function _getGrade(data,successCall, errorCall, alwaysCall) {
                  webAPIService.apiGet('api/lookup/getGrade', data, successCall, errorCall, alwaysCall);
              }

              function _getAssmtStatus(data, successCall, errorCall, alwaysCall) {
                  webAPIService.apiGet('api/lookup/getAssmtStatus', data, successCall, errorCall, alwaysCall);
              }

              function _getPendingAssessment(data, successCall, errorCall, alwaysCall) {
                  webAPIService.apiGet('api/lookup/getPendingAssessment', data, successCall, errorCall, alwaysCall);
              }

              function _getGradeCsdCs(data, successCall, errorCall, alwaysCall) {
                  webAPIService.apiGet('api/lookup/getGradeCsCsd', data, successCall, errorCall, alwaysCall);
              }

              //function _getSectorFrom(data, successCall, errorCall, alwaysCall) {
              //    webAPIService.apiGet('api/lookup/getSectorFrom', data, successCall, errorCall, alwaysCall);
              //}

              //function _getSectorTo(data, successCall, errorCall, alwaysCall) {
              //    webAPIService.apiGet('api/lookup/getSectorTo', data, successCall, errorCall, alwaysCall);
              //}

              function _getPoAssessmentDetails(data, successCall, errorCall, alwaysCall) {
                  webAPIService.apiPost('api/getPoAssessmentDetails' ,data, successCall, errorCall, alwaysCall);
              }

              //function _SearchPoAssessmentDetails(data, successCall, errorCall, alwaysCall) {
              //    webAPIService.apiPost('api/searchPoAssessmentDetails', data, successCall, errorCall, alwaysCall);
              //}

              function _getPoSearchResult(data, successCall, errorCall, alwaysCall) {
                  webAPIService.apiPost('api/getPoSearchResult', data, successCall, errorCall, alwaysCall);
              }

              function _cancelscheduledasmnt(data, successCall, errorCall, alwaysCall) {

                  webAPIService.apiGet('api/cancelScheduledasmnt/' + data, null, successCall, errorCall, alwaysCall);
              }

              //function _scheduledassessment(data, successCall, errorCall, alwaysCall) {

              //    webAPIService.apiGet('api/SchedulAssessment/' + data, null, successCall, errorCall, alwaysCall);
              //}

              function _scheduledassessment(data, successCall, errorCall, alwaysCall) {

                  webAPIService.apiPost('api/SchedulAssessment', data, successCall, errorCall, alwaysCall);
              }

              function _getsavedscheduledassessment(data, successCall, errorCall, alwaysCall) {

                  webAPIService.apiPost('api/getsavedUnscheduledAssmt', data, successCall, errorCall, alwaysCall);
              }

              function _getAllPreviousAssessment(data, successCall, errorCall, alwaysCall) {

                  webAPIService.apiGet('api/GetAllPreviousAssessment/'+ data, null, successCall, errorCall, alwaysCall);
              }

              function _validateUnscheduledData(data, successCall, errorCall, alwaysCall) {

                  webAPIService.apiPost('api/validateUnscheduledData', data, successCall, errorCall, alwaysCall);
              }

              function _getCrewExpectedAsmnt(data, successCall, errorCall, alwaysCall) {
                  webAPIService.apiPost('api/getCrewExpectedAsmnt', data, successCall, errorCall, alwaysCall);
              }

              function _getAsmntRoster(data, successCall, errorCall, alwaysCall) {
                  webAPIService.apiPost('api/AssessmentRoster/', data, successCall, errorCall, alwaysCall);
              }
          }]);