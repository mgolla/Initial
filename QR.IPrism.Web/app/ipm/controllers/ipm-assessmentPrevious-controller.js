/*********************************************************************
* File Name     : ipm-assessmentPrevious-controller.js
* Description   : Controller for view Previous Assessment.
* Create Date   : 29th Jan 2016
* Modified Date : 29th Jan 2016
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.assessmentPrevious.controller', ['$scope', '$rootScope', '$state', 'assessmentListService', 'assessmentServices', 'messages', 'blockUI', 'sharedDataService',
        function ($scope, $rootScope, $state, assessmentListService, assessmentServices, messages, blockUI, sharedDataService) {

            var ipmassessmentListBlockUI = blockUI.instances.get('ipmassessmentListBlockUI');;
            $scope.model = {};

            function Reset() {
                var currentDate = new Date();
                $scope.model.ToDateObj = angular.copy(currentDate);
                currentDate.setFullYear(currentDate.getFullYear() - 1);
                $scope.model.FromDateObj = currentDate;
            }

            //Controller Scope Initialization
            function initialize() {

                $scope.getDesc = function (row) {
                    if (new Date(row.CutOffDate) > new Date(row.DateofAssessment)) {
                        $state.go('prevassessment.old', { back: 'prevassessment', AssessmentID: row.AssessmentID });
                    } else {
                        $state.go('prevassessment.new', { back: 'prevassessment', id: row.AssessmentID });
                    }
                }

                $scope.Search = function () {
                    getAssessmentListData();
                };

                $scope.Reset = function () {
                    Reset();
                    getAssessmentListData();
                };

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
                              field: "FlightNo", name: "Flight #", width: "10%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="javascript:void(0);" ng-click="grid.appScope.getDesc(row.entity)">{{row.entity.FlightNo}}</a><div>'
                          },
                          {
                              field: "FormatedDate", name: "S T D", width: "20%", type: 'date', enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="javascript:void(0);" ng-click="grid.appScope.getDesc(row.entity)">{{row.entity["FormatedDate"] | date:"dd-MMM-yyyy" }}</a><div>'
                          },
                          { field: "SectorTo", name: "Sector", width: "20%", enableHiding: false },
                          {
                              field: "DateofAssessment", name: "Assessment Date", type: 'date', width: "20%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["DateofAssessment"]  | date:"dd-MMM-yyyy"}}</div>'
                          },
                          {
                              field: "CrewName", name: "Assessor", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{ row.entity.CrewName }}</div>'
                          }
                    ]
                }
                Reset();
                getAssessmentListData();
            }

            function getAssessmentListData() {
                ipmassessmentListBlockUI.start();
                $scope.model.FromDate = $scope.model.FromDateObj ? sharedDataService.getDateOnly($scope.model.FromDateObj) : '';
                $scope.model.ToDate = $scope.model.ToDateObj ? sharedDataService.getDateOnly($scope.model.ToDateObj) : '';

                assessmentListService.getPreviousAssessments($scope.model, function (success) {
                    $scope.grid.data = success.data;
                    ipmassessmentListBlockUI.stop();
                }, function (error) {
                    ipmassessmentListBlockUI.stop();
                });
            }

            initialize();

        }]);