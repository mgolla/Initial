/*********************************************************************
* File Name     : ipm-assessmentlist-controller.js
* Description   : Controller for AssessmentList Request module.
* Create Date   : 29th Jan 2016
* Modified Date : 29th Jan 2016
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.assessmentPreviousAll.controller', ['$scope', '$stateParams', '$state', 'assessmentsearchService', 'messages', 'blockUI',
        function ($scope, $stateParams, $state, assessmentsearchService, messages, blockUI) {

            // Private variables initialization
            var ipmassessmentListBlockUI = blockUI.instances.get('blockui');;

            $scope.header = "Previous Assessments"
            $scope.back = "poasmnt";

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
                          field: "FlightNumber", name: "Flight #", width: "10%"
                      },
                      {
                          field: "FlightDate", name: "Flight Date", width: "15%", type: 'date',
                          cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["FlightDate"] | date:"dd-MMM-yyyy" }}<div>'
                      },
                      { field: "SectorFrom", name: "Sector", width: "10%" },                     
                      { field: "AssessmentStatus", name: "AssessmentStatus"},
                      { field: "AssessmentType", name: "AssessmentType" },
                      {
                          field: "AssessmentDate", name: "Assessment Date", type: 'date',
                           cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["AssessmentDate"] | date:"dd-MMM-yyyy" }}<div>'
                      },
                      {
                          field: "AssessorStaffName", name: "AssessorStaffName", width: "25%"
                      }
                ]
            }

            ipmassessmentListBlockUI.start();
            assessmentsearchService.getAllPreviousAssessment($stateParams.id, function (success) {
                $scope.grid.data = success;
                ipmassessmentListBlockUI.stop();
            }, function (error) {
                ipmassessmentListBlockUI.stop();
            });

        }]);