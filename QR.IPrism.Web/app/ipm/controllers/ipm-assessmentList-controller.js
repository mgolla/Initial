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
    .controller('ipm.assessmentList.controller', ['$scope', '$rootScope', '$state', 'assessmentListService', 'messages', 'blockUI','toastr',
        function ($scope, $rootScope, $state, assessmentListService, messages, blockUI, toastr) {

            // Private variables initialization
            var ipmassessmentListBlockUI = blockUI.instances.get('ipmassessmentListBlockUI');

            // Initialize scope variables
            $scope.myassessment = {};

            //Controller Scope Initialization
            function initialize() {

                $scope.getDesc = function (row) {

                    var dt = new Date();
                    dt.setHours(0, 0, 0, 0);
                    
                    var assDt = new Date(row.DateofAssessment.split("T")[0]);
                    assDt.setHours(0, 0, 0, 0);

                    if (assDt <= dt) {
                        $state.go('asmntdtl', { myparam: { asmtdetail: row }, back: 'myasmnt' });
                    } else {
                        var fdt = assDt.getDate() + "/" + (assDt.getUTCMonth() + 1) + "/" + assDt.getUTCFullYear();
                        toastr.clear();
                        toastr.info('Requested Assessment can be submitted on or after ' + fdt);
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
                              field: "StaffNumber", name: "Staff #", width: "10%", enableHiding: false, type: 'number',
                              cellTemplate: '<div class="ui-grid-cell-contents">' +
                                  '<a href="javascript:void(0);" ng-class="row.entity[\'AssessmentStatus\'] == \'Delayed\'||row.entity[\'AssessmentStatus\'] == \'DelayedSaved\'? \'redText\' : \'\'" ng-click="grid.appScope.getDesc(row.entity)">{{ row.entity[\'AssessmentStatus\'] == \'Delayed\'||row.entity[\'AssessmentStatus\'] == \'DelayedSaved\'? row.entity.StaffNumber + \'  !\': row.entity.StaffNumber}}</a><div>'
                          },
                          {
                              field: "CrewName", name: "Staff Name", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents" title="{{row.entity.CrewName}}" ng-class="row.entity[\'AssessmentStatus\'] == \'Delayed\' ||row.entity[\'AssessmentStatus\'] == \'DelayedSaved\'? \'redText\' : \'\'">{{row.entity["CrewName"]}}</div>'
                          },
                          {
                              field: "AssessmentStatus", name: "Assessment Status", width: "18%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents" ng-class="row.entity[\'AssessmentStatus\'] == \'Delayed\' ||row.entity[\'AssessmentStatus\'] == \'DelayedSaved\'? \'redText\' : \'\'">{{row.entity["AssessmentStatus"]}}</div>'
                          },
                          {
                              field: "FlightNo", name: "Flight #", width: "10%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents" ng-class="row.entity[\'AssessmentStatus\'] == \'Delayed\' ||row.entity[\'AssessmentStatus\'] == \'DelayedSaved\'? \'redText\' : \'\'">{{row.entity["FlightNo"]}}</div>'
                          },
                          {
                              field: "SectorFrom", name: "Sector", width: "12%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents" ng-class="row.entity[\'AssessmentStatus\'] == \'Delayed\' ||row.entity[\'AssessmentStatus\'] == \'DelayedSaved\'? \'redText\' : \'\'"><span>{{row.entity["SectorFrom"] }}</span>-<span>{{row.entity["SectorTo"] }}</span></div>'
                          },
                          {
                              field: "FormatedDate", name: "Assessment Date", type: 'date', width: "15%", sort: { direction: 'asc' }, enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents" ng-class="row.entity[\'AssessmentStatus\'] == \'Delayed\' ||row.entity[\'AssessmentStatus\'] == \'DelayedSaved\'? \'redText\' : \'\'">{{row.entity["FormatedDate"] | date:"dd-MMM-yyyy" }}</div>'
                          },
                          {
                              field: "ExpectedDate", name: "Expected Date of Completion", type: 'date', width: "20%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents" ng-class="row.entity[\'AssessmentStatus\'] == \'Delayed\' ||row.entity[\'AssessmentStatus\'] == \'DelayedSaved\' ? \'redText\' : \'\'">{{row.entity["ExpectedDate"] | date:"dd-MMM-yyyy" }}</div>'
                          }
                    ]
                }


                getAssessmentListData();
                setHeight();
            }

            $scope.isNullOrEmpty = function (value) {
                return (!(value && value != null && value.toString().trim().length > 0));
            };
            function getAssessmentListData() {
                ipmassessmentListBlockUI.start();
                assessmentListService.getAssessmentListResult("", function (success) {
                    $scope.grid.data = success;
                    $scope.myassessment = success;
                    ipmassessmentListBlockUI.stop();
                }, function (error) {
                    ipmassessmentListBlockUI.stop();
                });
            }

            function setHeight() {
                var getHeight = $(window).height() - 130;
                $('.content_wrapper').css('min-height', getHeight + 'px');
            }
            initialize();

        }]);