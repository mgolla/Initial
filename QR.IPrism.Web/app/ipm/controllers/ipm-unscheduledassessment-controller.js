/*********************************************************************
* File Name     : ipm-unscheduledassessment-controller.js
* Description   : Controller for unscheduled Assessment  module.
* Create Date   : 14 April 2016
* Modified Date : 14 April 2016
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.unscheduledassessment.controller', ['$scope', '$rootScope', '$state', 'assessmentsearchService', 'flightDetailsAddEditService', 'searchService', 'assessmentServices', 'lookupDataService', 'messages', 'blockUI', 'appSettings', 'ngDialog', 'analyticsService', 'toastr', '$window',
        function ($scope, $rootScope, $state, assessmentsearchService, flightDetailsAddEditService, searchService, assessmentServices, lookupDataService,
            messages, blockUI, appSettings, ngDialog, analyticsService, toastr, $window) {

            var ipmUSAssessmentBlockUI = blockUI.instances.get('ipmUSAssessmentBlockUI');
            // Private variables initialization
            var initiaMultiselectValue = { 'Value': '', 'Text': '--All--' };


            // Initialize scope variables

            $scope.sectorFromList = [];
            $scope.sectorToList = [];
            $scope.data = {};
            $scope.data.StaffNumberObj = {};
            $scope.data.FlightDateObj;
            $scope.assessmentSearchModel = {
                StaffNumber: null,
                FlightNo: null,
                FlightDate: null,
                SectorFrom: null,
                SectorTo: null
            }

            function DeleteAssessment(id) {
                ipmUSAssessmentBlockUI.start();
                assessmentServices.deleteAssessment(id, function (success) {
                    ipmUSAssessmentBlockUI.stop();
                    getSavedScheduledAssmtData();
                }, function (error) {
                    ipmUSAssessmentBlockUI.stop();
                });
            }

            //Controller Scope Initialization

            function initialize() {
                $scope.autoList = [];
                $scope.getDesc = function (row) {

                    var para = {
                        AssessmentID: row.AssessmentID,
                        Grade: row.Grade,
                        CustomStatus: row.AssessmentStatus,
                        AssessmentType: row.AssessmentType

                    }

                    $state.go('asmntdtl', { myparam: { asmtdetail: para }, back: "unasmntdtl" });
                }

                $scope.deleteEVR = function (row) {
                    $scope.dialogTitle = "Confirmation";
                    $scope.dialogMessage = messages.ASMTDELETE;
                    ngDialog.open({
                        scope: $scope,
                        preCloseCallback: function (value) {
                            if (value == 'Post') {
                                DeleteAssessment(row.AssessmentID);
                            }
                        }
                    });
                };

                $scope.gridSavedUnscheduleAssmt = {
                    gridApi: {},
                    data: [],
                    enableFiltering: true,
                    showGridFooter: true,
                    paginationPageSizes: [5, 10, 15, 20, 25],
                    paginationPageSize: 10,
                    enablePagination: true,
                    enablePaginationControls: true,
                    subgrid: 'false',
                    columnDefs: [
                          {
                              field: "StaffNumber", name: "Staff No", width: "10%",
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.getDesc(row.entity)">{{row.entity.StaffNumber}}</a><div>'
                          },
                          { field: "FlightNo", name: "Flight No" },

                          {
                              field: "AssessmentDate", name: "Assessment Date", type: 'date',
                              cellTemplate: '<div class="ui-grid-cell-contents"><span>{{row.entity["DateofAssessment"] | date:"dd-MMM-yyyy" }}</span>'
                          },
                          { field: "AssessmentStatus", name: "Status" },
                          {
                              field: "remove", name: "Remove", width: "10%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents text-center"><a href="javascript:void(0);" ng-click="grid.appScope.deleteEVR(row.entity)"><i class="glyphicon glyphicon-trash redText"></i></a></div>'
                          }
                    ]
                }
                getSavedScheduledAssmtData();
                loadLookupData();
            }


            $scope.refreshStaff = function (data) {

                if (data.length > 2) {

                    assessmentsearchService.getAutoSuggestStaffByGrade({ name: "Assessee", CrewSearchCriteria: data }, function (success) {

                        $scope.autoList = success.data;
                    }, function (error) {
                    });
                }
            };


            function loadLookupData() {

                lookupDataService.getLookupList('Sector', null, function (result) {
                    $scope.sectorFromList = [];
                    $scope.sectorToList = [];
                    $scope.sectorFromList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text, Desc: obj.FilterText } });
                    $scope.sectorToList = angular.copy($scope.sectorFromList);

                    //$scope.sectorFromList.unshift(initiaMultiselectValue);
                    // $scope.assessmentSearchModel.SectorFromObj = $scope.sectorFromList[0];
                });
            }

            function getSavedScheduledAssmtData() {
                ipmUSAssessmentBlockUI.start();
                assessmentsearchService.getsavedscheduledassessment("", function (success) {
                    //$scope.grid.data = success;

                    $scope.gridSavedUnscheduleAssmt.data = success.data;
                    // $scope.assessmentSearchModel = success;




                    ipmUSAssessmentBlockUI.stop();
                }, function (error) {
                    //ipmUSAssessmentBlockUI.stop();
                });
            }

            $scope.getAutosearchStaffData = function (searchText) {
                ipmUSAssessmentBlockUI.start();
                searchService.autoSuggestStaffInfo(searchText, null, function (success) {


                    $scope.SearchedCrewData = success.data;
                    $scope.Data = $scope.SearchedCrewData;

                    ipmUSAssessmentBlockUI.stop();
                }, function (error) {
                    //ipmUSAssessmentBlockUI.stop();
                });

            }
            function checkData(model, name) {
                if ($scope.isNullOrEmpty(model, name)) {

                    toastr.warning('Please enter ' + name + ' .');
                    return false;
                }
                return true;
            }

            function checkFlightDate(model) {
                var flightDate = new Date(model);
                var flightDay = new Date(flightDate.getFullYear(), flightDate.getMonth(), flightDate.getDate());

                var today = new Date();

                if (model > new Date()) {

                    toastr.warning('The assessment can be done on or after  ' + flightDay.getDate() + '/' + (flightDay.getMonth() + 1) + '/' + flightDay.getFullYear() + ' .');
                    return false;
                }
                return true;
            }

            $scope.isNullOrEmpty = function (value) {
                return (!(value && value != null && value.toString().trim().length > 0));
            };

            function validateUnscheduledData() {

                $scope.assessmentSearchModel.SectorFrom = $scope.assessmentSearchModel.SectorFromObj ? $scope.assessmentSearchModel.SectorFromObj.Text : '';
                $scope.assessmentSearchModel.SectorTo = $scope.assessmentSearchModel.SectorToObj ? $scope.assessmentSearchModel.SectorToObj.Text : '';

                $scope.isBreak = true;
                if ($scope.isBreak) {
                    $scope.isBreak = checkData($scope.data.FlightDateObj, 'FlightDate');
                    if ($scope.isBreak) {
                        var currentDate = new Date($scope.data.FlightDateObj);
                        $scope.assessmentSearchModel.FlightDate = (currentDate.getMonth() + 1) + '/' + currentDate.getDate() + '/' + currentDate.getFullYear();
                    }
                }
                if ($scope.isBreak) {
                    $scope.isBreak = checkFlightDate($scope.assessmentSearchModel.FlightDate);
                }
                if ($scope.isBreak) {
                    $scope.isBreak = checkData($scope.data.StaffNumberObj, 'StaffNumber');
                    if ($scope.isBreak) {
                        $scope.assessmentSearchModel.StaffNumber = $scope.data.StaffNumberObj.FlightCrewId;
                        $scope.isBreak = checkData($scope.assessmentSearchModel.StaffNumber, 'StaffNumber');
                    }
                }



                if ($scope.isBreak) {
                    $scope.isBreak = checkData($scope.assessmentSearchModel.FlightNo, 'FlightNo');
                }

                if ($scope.isBreak) {
                    $scope.isBreak = checkData($scope.assessmentSearchModel.SectorFrom, 'SectorFrom');
                }
                if ($scope.isBreak) {
                    $scope.isBreak = checkData($scope.assessmentSearchModel.SectorTo, 'SectorTo');
                }
                if ($scope.isBreak) {
                    assessmentsearchService.validateUnscheduledData($scope.assessmentSearchModel, function (success) {
                        if (success.data.ResponseId != 'N') {
                            var para = {
                                Grade: $scope.data.StaffNumberObj.StaffGrade,
                                StaffNumber: $scope.assessmentSearchModel.StaffNumber,
                                AssesseeGrade: $scope.data.StaffNumberObj.StaffGrade,
                                AssesseeCrewDetID: $scope.assessmentSearchModel.StaffNumber,
                                AssessmentType: 'Unscheduled',
                                FlightNumber: 'QR' + $scope.assessmentSearchModel.FlightNo,
                                FlightDate: $scope.assessmentSearchModel.FlightDate,
                                SectorFrom: $scope.assessmentSearchModel.SectorFrom,
                                SectorTo: $scope.assessmentSearchModel.SectorTo,
                                //FlightDetID :        
                                AssessmentStatus: 'Scheduled',
                                AssessmentDate: $scope.assessmentSearchModel.FlightDate
                            }

                            $state.go('asmntdtl', { myparam: { asmtdetail: para }, back: "unasmntdtl" });

                            //var filter = {
                            //    StaffNumber: $scope.assessmentSearchModel.StaffNumber,
                            //    FlightNo: $scope.assessmentSearchModel.FlightNo,
                            //    FlightDate: $scope.assessmentSearchModel.FlightDate,
                            //    SectorFrom: $scope.assessmentSearchModel.SectorFrom,
                            //    SectorTo: $scope.assessmentSearchModel.SectorTo
                            //}

                            //$state.go('asmntdtl', { myparam: filter });
                            // getSavedScheduledAssmtData();
                        } else {
                            if (success.data.Message && (!$scope.isNullOrEmpty(success.data.Message))) {
                                toastr.warning(success.data.Message);
                            }
                        }
                        //ipmUSAssessmentBlockUI.stop();
                    }, function (error) {
                        //ipmUSAssessmentBlockUI.stop();
                    });
                }
            }

            $scope.onClickrecordAssessment = function () {

                validateUnscheduledData();
            }

            $scope.Reset = function () {

                $scope.assessmentSearchModel.StaffNumber = "";
                $scope.assessmentSearchModel.FlightNo = "";
                $scope.assessmentSearchModel.FlightDate = "";
                $scope.assessmentSearchModel.SectorFrom = "";
                $scope.assessmentSearchModel.SectorTo = "";
                getSavedScheduledAssmtData();
            };

            initialize();

        }]);