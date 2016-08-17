/*********************************************************************
* File Name     : ipm-assessment-controller.js
* Description   : Controller for Assessment module
* Create Date   : 3rd May 2016
* Modified Date : 
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'

angular.module('app.ipm.module')
       .controller('ipm.crewOnBoardAssessmentSearch.controller', ['$scope', '$state', 'assessmentsearchService', 'flightDetailsAddEditService',
           'lookupDataService', 'messages', 'blockUI', 'toastr','sharedDataService',
           function ($scope, $state, assessmentsearchService, flightDetailsAddEditService, lookupDataService, messages, blockUI, toastr, sharedDataService) {

               var ipmcrewOnBoardAsmntSearchBlockUI = blockUI.instances.get('ipmcrewOnBoardAsmntSearchBlockUI');
               $scope.model = {};
               $scope.assessment = {};

               $scope.Search = function () {

                   $scope.assessment.StaffNumber = $scope.model.selectStaffObj ? $scope.model.selectStaffObj.StaffNumber : "";
                   $scope.assessment.FlightNumber = $scope.model.FlightNumber;
                   $scope.assessment.SectorTo = $scope.model.SectorToObj ? $scope.model.SectorToObj.Text : "";
                   $scope.assessment.ToDate = $scope.model.ToDate ? sharedDataService.getDateOnly($scope.model.ToDate) : "";
                   if (!$scope.assessment.StaffNumber && (!$scope.assessment.FlightNumber && !$scope.model.SectorToObj && !$scope.assessment.ToDate)) {
                       toastr.info("Enter Search Criteria StaffNumber Or Flight Number, SectorTo, Date");
                   }
                   else {
                       getAssessmentsearchData();
                   }
               };

               $scope.refreshStaff = function (data) {

                   if (data && data.length > 1) {
                       flightDetailsAddEditService.getAutoSuggestStaffInformation({ name: "Assessee", CrewSearchCriteria: data }, function (success) {

                           $scope.autoList = success.data;
                       }, function (error) {

                       });
                   }
               };

               $scope.ResetSearch = function () {

                   $scope.model.selectStaffObj = "";
                   $scope.model.FlightNumber = "";
                   $scope.model.SectorToObj = "";
                   $scope.model.ToDate = "";
                   initialize();
               };

               function initialize() {

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
                             { field: "StaffName", enableHiding: false, name: "Staff Name" },
                             { field: "LastAssessmentDate", enableHiding: false, name: "Last Assessment Date", type: 'date', sort: { direction: 'asc' } },
                             { field: "ExpectedAssessmentDate", enableHiding: false, name: "Expected Assessment Date", type: 'date' }
                       ]
                   };

                   getSector();
               }

               function getAssessmentsearchData() {

                   ipmcrewOnBoardAsmntSearchBlockUI.start();
                   assessmentsearchService.getCrewExpectedAsmnt($scope.assessment, function (success) {

                       var searchresult = success.data.Result.map(function (obj) {
                           return {
                               StaffName: obj.StaffName,
                               LastAssessmentDate: obj.AssessmentDate,
                               ExpectedAssessmentDate: obj.ExpectedDate
                           }
                       });

                       console.log(searchresult);
                       $scope.grid.data = searchresult;
                       ipmcrewOnBoardAsmntSearchBlockUI.stop();


                   },
                   function (error) {
                       //$scope.grid = null;
                       ipmcrewOnBoardAsmntSearchBlockUI.stop();
                   }, null);

               }

               function getSector() {

                   lookupDataService.getLookupList('Sector', $scope.Text, function (result) {

                       $scope.SectorList = result.map(function (obj) {
                           return { Value: obj.Value, Text: obj.Text, Desc: obj.FilterText }
                       });
                   });
               }
               initialize();

           }
       ]);