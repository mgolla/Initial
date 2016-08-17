/*********************************************************************
* File Name     : ipm-assessment-controller.js
* Description   : Controller for Assessment module
* Create Date   : 2nd March 2016
* Modified Date : 
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'

angular.module('app.ipm.module')
       .controller('ipm.assessmentsearch.controller', ['$scope', '$state', '$rootScope', 'assessmentsearchService', 'lookupDataService', 'messages', 'blockUI', 'sharedDataService',
           function ($scope, $state, $rootScope, assessmentsearchService, lookupDataService, messages, blockUI, sharedDataService) {

               var ipmUSAssessmentBlockUI = blockUI.instances.get('ipmUSAssessmentBlockUI');
               var initiaMultiselectValue = { 'Value': '', 'Text': '--All--' };

               $scope.gradeList = [];
               $scope.assmtStatusList = [];
               $scope.model = {};
               $scope.assessment = {};

               function setDate() {
                   var currentDate = new Date();
                   $scope.model.ToDate = angular.copy(currentDate);
                   currentDate.setMonth(currentDate.getMonth() - 1);
                   $scope.model.FromDate = currentDate;
               };

               function PageEvent() {

                   $scope.changeStaff = function (model) {
                       angular.forEach($scope.gradeList, function (grade) {
                           if (model == grade.Text) {
                               $scope.model.GradeObj = grade;
                           }
                       });
                   }

                   $scope.Search = function () {

                       $scope.assessment.StaffNumber = $scope.model.StaffNumber;
                       $scope.assessment.AssessmentStatus = $scope.model.AssessmentStatusObj ? $scope.model.AssessmentStatusObj.Text : '';
                       $scope.assessment.Grade = $scope.model.GradeObj ? $scope.model.GradeObj.Text : '';
                       $scope.assessment.FromDate = $scope.model.FromDate ? sharedDataService.getDateOnly($scope.model.FromDate) : '';
                       $scope.assessment.ToDate = $scope.model.ToDate ? sharedDataService.getDateOnly($scope.model.ToDate) : '';

                       getAssessmentsearchData();

                   };

                   $scope.ResetSearch = function () {

                       $scope.model.StaffNumber = "";
                       $scope.model.AssessmentStatusObj = "";
                       $scope.model.GradeObj = "";
                       $scope.grid.data = [];

                       setDate();
                   };

                   $scope.refreshStaff = function (data) {
                       if (data.length > 2) {
                           assessmentsearchService.getAutoSuggestStaffByGrade({ name: "FlightCrew", CrewSearchCriteria: data }, function (success) {

                               $scope.autoList = success.data;
                           }, function (error) {

                           });
                       }
                   };
               }

               function initialize() {

                   PageEvent();
                   setDate();
                   $scope.getDesc = function (row) {

                       if (new Date(row.CutOffDate) > new Date(row.AssessmentDate)) {

                           $state.go('srchasmnt.pasmt', { back: 'srchasmnt', AssessmentID: row.AssessmentID });

                       } else {
                           $scope.para = {
                               asmtdetail: {
                                   AssessmentID: row.AssessmentID,
                                   Grade: row.Grade,
                                   CustomStatus: row.AssessmentStatus,
                                   AssessmentType: row.AssessmentType
                               }
                           }

                           $state.go('srchasmnt.asmtro', { back: 'srchasmnt', id: row.AssessmentID });
                       }
                   }

                   $scope.grid = {
                       gridApi: {},
                       enableFiltering: true,
                       showGridFooter: true,
                       paginationPageSizes: [5, 10, 15, 20, 25],
                       paginationPageSize: 10,
                       enablePagination: true,
                       enablePaginationControls: true,
                       data: [],
                       subgrid: 'false',
                       columnDefs: [
                           {
                               field: "", name: "Staff #", width: "10%",
                               cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.getDesc(row.entity)">{{row.entity.StaffNumber}}</a><div>'
                           },
                           {
                               field: "StaffName", name: "Staff Name",
                               cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity.StaffName}}({{row.entity.Grade}})<div>'
                           },
                           {
                               field: "AssessmentDate", name: "Assessment Date", type: 'date', width: "20%",
                               cellTemplate: '<div class="ui-grid-cell-contents"><span>{{row.entity["AssessmentDate"] | date:"dd-MMM-yyyy" }}</span></div>'
                           },
                           { field: "AssessmentStatus", name: " Assessment Status", width: "20%" },
                           {
                               field: "ExpDateOfCompletion", name: "Expected Date Of Completion", type: 'date', width: "20%",
                               cellTemplate: '<div class="ui-grid-cell-contents"><span>{{row.entity["ExpDateOfCompletion"] | date:"dd-MMM-yyyy" }}</span></div>'
                           }
                       ]
                   }
                   //ipmassessmentSearchBlockUI.start();
                   loadData();
               }

               function loadData() {

                   assessmentsearchService.getGradeByLoggedPerson(null, function (result) {
                       $scope.gradeList = [];
                       var data = result.data.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });
                       $scope.gradeList = data;
                   });

                   lookupDataService.getLookupList('AssmtStatus', null, function (result) {
                       $scope.assmtStatusList = [];
                       var statuses = result.map(function (obj) {
                           return { Value: obj.Value, Text: obj.Text }
                       });

                       $scope.assmtStatusList = $.grep(statuses, function (v, i) {
                           return v.Text.toLowerCase() != 'scheduled' && v.Text.toLowerCase() != 'delayed';
                       });

                       $scope.assmtStatusList.unshift(initiaMultiselectValue);
                       $scope.model.AssessmentStatusObj = $scope.assmtStatusList[0];
                   });
               }

               function getAssessmentsearchData() {

                   ipmUSAssessmentBlockUI.start();

                   assessmentsearchService.getAssmtSearchResult($scope.assessment, function (success) {

                       if (success && success.data) {
                           $scope.grid.data = success.data;
                       }
                       ipmUSAssessmentBlockUI.stop();
                   },
                   function (error) {
                       ipmUSAssessmentBlockUI.stop();
                   }, null);

               }

               initialize();
           }
       ]);