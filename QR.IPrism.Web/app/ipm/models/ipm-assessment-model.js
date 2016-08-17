/*********************************************************************
* File Name     : ipm-assessment-model.js
* Description   : Contains all the model required for assessment module.
* Create Date   : 2nd March 2016
* Modified Date : 
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'
angular.module('app.ipm.module')
       .factory('assessment', [function () {
           var self = this;

           function AssessmentSearch(assmtSearchData) {

               self.StaffNumber = assmtSearchData.StaffNumber ? assmtSearchData.StaffNumber : "";
               self.StaffName = assmtSearchData.StaffName ? assmtSearchData.StaffName : "";
               self.AssessmentDate = assmtSearchData.AssessmentDate ? assmtSearchData.AssessmentDate : "";
               self.AssessmentStatus = assmtSearchData.AssessmentStatus ? assmtSearchData.AssessmentStatus : "";
               self.ExpDateCompletion = assmtSearchData.ExpDateCompletion ? assmtSearchData.ExpDateCompletion : "";
              
           }

           return AssessmentSearch;
       }]);