/*********************************************************************
* File Name     : ipm-poassessment-controller.js
* Description   : Controller for Po Assessment  module.
* Create Date   : 22nd March 2016
* Modified Date : 22nd March 2016
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.poassessment.controller', ['$scope', '$rootScope', '$state', 'assessmentsearchService', 'assessmentListService', 'lookupDataService', 'flightDetailsAddEditService', 'messages', 'blockUI', 'appSettings', 'ngDialog', 'analyticsService', 'toastr', '$window','orderByFilter',
        function ($scope, $rootScope, $state, assessmentsearchService, assessmentListService, lookupDataService, flightDetailsAddEditService,
            messages, blockUI, appSettings, ngDialog, analyticsService, toastr, $window,orderBy) {

            // Private variables initialization
            var initiaMultiselectValue = { 'Value': '', 'Text': '--All--' };
            var asmtDetailsBlockUI = blockUI.instances.get('asmtDetailsBlockUI');
            var csdGrid = blockUI.instances.get('csdGrid');
            var csGrid = blockUI.instances.get('csGrid');

            // Initialize scope variables
            $scope.assessmentViewModel = {};

            $scope.poAssessmentData = {};
            $scope.CSDListData = {};
            $scope.CSListData = {};
            $scope.model = {};
            $scope.gradeList = [];
            $scope.PendingAssessmentList = [];
            //$scope.scheduledAssmt = [];

            $scope.tabs = {
                tab: 'Assessments Scheduled (...)',
                tabcsd: 'CSD List (...)',
                tabcs: 'CS List (...)'
            }

            //Controller Scope Initialization
            function initialize() {

                $scope.goToAsmtDtl = function (row) {
                    $state.go('poasmnt.asmntdtl', { myparam: { asmtdetail: row }, back: "poasmnt" });
                };

                $scope.viewVRs = function (row) {
                    $state.go('poasmnt.prevevrs', { id: row.StaffNumber });
                };

                $scope.assmtDetails = function (row) {
                    $state.go('poasmnt.prevasmtall', { id: row.StaffNumber });
                };

                $scope.gridScheduled = {
                    gridApi: {},
                    enableFiltering: true,
                    showGridFooter: true,
                    enableColumnResizing: true,
                    paginationPageSizes: [5, 10, 15, 20, 25],
                    paginationPageSize: 10,
                    enablePagination: true,
                    enablePaginationControls: true,
                    data: [],
                    subgrid: 'false',
                    columnDefs: [
                          {
                              field: "StaffNumber", name: "Staff No", width: "8%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.goToAsmtDtl(row.entity)">{{row.entity.StaffNumber}}</a><div>'
                          },
                          { field: "CrewName", name: "Staff Name", width: "15%", enableHiding: false},
                          { field: "AssessmentStatus", name: "Assessment Status", width: "20%", enableHiding: false },
                          { field: "FlightNo", name: "Flight", width: "10%", enableHiding: false },
                          {
                              field: "", name: "Sector", width: "10%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["SectorFrom"] }}</span>-<span>{{row.entity["SectorTo"] }}</div>'
                          },
                          {
                              field: "FormatedDate", name: "Assessment Date", width: "20%", type: 'date', enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["DateofAssessment"] | date:"dd-MMM-yyyy" }}</div>'
                          },
                          {
                              field: "", name: "Cancel Schedule", width: "20%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents text-center"><a href="javascript:void(0);" ng-click="grid.appScope.cancelScheduledAsmnt(row)"><i class="glyphicon glyphicon-trash redText"></i></a></div>',
                              //cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.cancelScheduledAsmnt()">Cancel</a><div>'
                          },

                    ]
                }

                $scope.gridCsd = {
                    gridApi: {},
                    enableFiltering: true,
                    showGridFooter: true,
                    enableColumnResizing: true,
                    paginationPageSizes: [5, 10, 15, 20, 25],
                    paginationPageSize: 10,
                    enablePagination: true,
                    enablePaginationControls: true,
                    data: [],
                    subgrid: 'false',
                    columnDefs: [
                          {
                              field: "StaffNumber", name: "Staff No", width: "8%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.onClickMonthlyPOAssement(row.entity)">{{row.entity.StaffNumber}}</a><div>'
                          },
                          { field: "StaffName", name: "Staff Name" },
                          {
                              field: "LastAssessment", name: "Last Assessment", type: 'date', width: "10%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["LastAssessment"] | date:"dd-MMM-yyyy" }}</div>'
                          },
                          {
                              field: "ExpDateOfCompletion", name: "Expected Assessment Date", type: 'date', enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["ExpectedDate"] | date:"dd-MMM-yyyy" }}</div>'
                          },
                          { field: "AssessmentsScheduled", enableHiding: false, name: "Assessment Scheduled" },
                          { field: "PmName", enableHiding: false, name: "PM Name" },
                          {
                              field: "", name: "View VRs", width: "8%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.viewVRs(row.entity)">View VRS</a><div>'
                          },
                          {
                              field: "", name: "View Assessments", width: "12%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.assmtDetails(row.entity)">View Assessments</a><div>'
                          },

                    ]
                }

                $scope.gridCs = {
                    gridApi: {},
                    enableFiltering: true,
                    showGridFooter: true,
                    enableColumnResizing: true,
                    paginationPageSizes: [5, 10, 15, 20, 25],
                    paginationPageSize: 10,
                    enablePagination: true,
                    enablePaginationControls: true,
                    data: [],
                    subgrid: 'false',
                    columnDefs: [
                          {
                              field: "StaffNumber", name: "Staff No", width: "8%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.onClickMonthlyPOAssement(row.entity)">{{row.entity.StaffNumber}}</a><div>'
                          },
                          { field: "StaffName", name: "Staff Name" },
                           {
                               field: "LastAssessment", name: "Last Assessment", type: 'date', enableHiding: false,
                               cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["LastAssessment"] | date:"dd-MMM-yyyy" }}</div>'
                           },
                          {
                              field: "ExpDateOfCompletion", name: "Expected Assessment Date", type: 'date', enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["ExpectedDate"] | date:"dd-MMM-yyyy" }}</div>'
                          },
                          { field: "AssessmentsScheduled", enableHiding: false, name: "Assessment Scheduled" },
                          { field: "PmName", enableHiding: false, name: "PM Name" },
                          {
                              field: "", name: "View VRs", width: "8%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.viewVRs(row.entity)">View VRs</a><div>'
                          },
                          {
                              field: "", name: "View Assessments", width: "12%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.assmtDetails(row.entity)">View Assessments</a><div>'
                          },
                    ]
                }

                getPoAssessmentData();
                loadLookupData();
            }

            function loadLookupData() {

                lookupDataService.getLookupList('GradeCsCsd', null, function (result) {
                    $scope.gradeList = [];
                    $scope.gradeList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });
                    $scope.gradeList.unshift(initiaMultiselectValue);

                    $scope.assessmentViewModel.GradeObj = $scope.gradeList[0];
                });

                lookupDataService.getLookupList('PendingAssessment', null, function (result) {

                    $scope.PendingAssessmentList = [];
                    var PendingAssessmentList = result.map(function (obj) { return { Value: parseInt(obj.Value), Text: obj.Text } });

                    $scope.PendingAssessmentList = orderBy(PendingAssessmentList, 'Value', false);
                    $scope.assessmentViewModel.PendingAssessmentObj = $scope.PendingAssessmentList[0];
                });
            }

            function getPoAssessmentData() {

                getAssessmentList();

                $scope.assessmentViewModel.StaffNumber = $scope.assessmentViewModel.selectStaffObj ? $scope.assessmentViewModel.selectStaffObj.StaffNumber : '';
                $scope.assessmentViewModel.PendingAssessment = $scope.assessmentViewModel.PendingAssessmentObj ?
                    $scope.assessmentViewModel.PendingAssessmentObj.Value : '';
                $scope.assessmentViewModel.Grade = $scope.assessmentViewModel.GradeObj ? $scope.assessmentViewModel.GradeObj.Value : '';

                $scope.tabs.tabcsd = "CSD List (...)";
                $scope.tabs.tabcs = "CS List (...)";

                csdGrid.start();
                csGrid.start();
                assessmentsearchService.getPoAssessmentDetails($scope.assessmentViewModel, function (success) {
                    
                    csdGrid.stop();
                    csGrid.stop();
                    $scope.gridCsd.data = success.data.POAssessmentCSDList;
                    $scope.gridCs.data = success.data.POAssessmentCSList;

                    //$scope.tabs.tab = "Assessments Scheduled " + "(" + success.data.POAssessmentScheduled.length + ")";
                    $scope.tabs.tabcsd = "CSD List " + "(" + success.data.POAssessmentCSDList.length + ")";
                    $scope.tabs.tabcs = "CS List " + "(" + success.data.POAssessmentCSList.length + ")";

                }, function (error) {
                    csdGrid.stop();
                    csGrid.stop();
                });
            }

            function getAssessmentList() {

                asmtDetailsBlockUI.start();
                assessmentListService.getAssessmentListResult("", function (success) {
                    $scope.gridScheduled.data = success;
                    $scope.tabs.tab = "Assessments Scheduled " + "(" + success.length + ")";
                    $scope.myassessment = success;
                   
                    asmtDetailsBlockUI.stop();
                }, function (error) {
                    asmtDetailsBlockUI.stop();
                });
            }

            $scope.poSearchData = function () {
                getPoAssessmentData();
            }

            $scope.ResetSearch = function () {

                $scope.assessmentViewModel.selectStaffObj = "";
                $scope.assessmentViewModel.PendingAssessmentObj = "";
                $scope.assessmentViewModel.GradeObj = $scope.gradeList[0];
                getPoAssessmentData();
            };

            $scope.cancelScheduledAsmnt = function (row) {

                $scope.dialogTitle = "Confirmation";
                $scope.dialogMessage = messages.ASMNTSCHEDULECANCEL;
                ngDialog.open({
                    scope: $scope,
                    preCloseCallback: function (value) {
                        if (value == 'Post') {
                            var AssessmentID = row.entity.AssessmentID;
                            cancelScheduledReq(AssessmentID);

                        }
                    }
                });
            }

            function cancelScheduledReq(AssessmentID) {

                assessmentsearchService.cancelscheduledasmnt(AssessmentID, function (success) {

                    toastr.info(messages.ASMNTSCHEDULECANCELCONFIRM);
                    getPoAssessmentData();
                }, function (error) {

                });
            };

            $scope.onClickMonthlyPOAssement = function (row) {

                var filter = {
                    staffID: row.StaffNumber
                }

                $state.go('poasmnt.pomonthly', { staffID: row.StaffNumber });
            }

            $scope.refreshStaff = function (data) {
                if (data && data.length > 1) {
                    flightDetailsAddEditService.getAutoSuggestStaffInformation({ name: "Assessee", CrewSearchCriteria: data }, function (success) {

                        var arr = [];
                        angular.forEach(success.data, function (result) {
                            if (result.StaffGrade == "CS" || result.StaffGrade == "CSD") {
                                arr.push(result);
                            }
                        });
                        $scope.autoList = arr;
                    }, function (error) {

                    });
                }
            };

            initialize();

        }]);