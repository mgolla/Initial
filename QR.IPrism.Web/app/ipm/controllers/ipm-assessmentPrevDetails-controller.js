/*********************************************************************
* File Name     : ipm-assessmentPrevDetails-controller.js
* Description   : Controller for AssessmentList Request module.
* Create Date   : 29th Jan 2016
* Modified Date : 29th Jan 2016
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.assessmentPreviousDetails.controller', ['$scope', '$stateParams', '$state', '$sce','assessmentListService', 'assessmentServices',
        'messages', 'blockUI','toastr',
        function ($scope, $stateParams, $state,$sce, assessmentListService, assessmentServices, messages, blockUI, toastr) {

            var ipmassessmentListBlockUI = blockUI.instances.get('ipmassessmentListBlockUI');
            $scope.model = [];
            $scope.headerDetails = {};
            $scope.totalScore = 0;

            function getMyAssessmentList() {

                ipmassessmentListBlockUI.start();
                assessmentServices.getMyAssessmentList($stateParams.AssessmentID, function (result) {
                    ipmassessmentListBlockUI.stop();
                    if (result && result[0]) {
                      
                        $scope.headerDetails = result[0][0];
                        if ($scope.headerDetails.AssesseeGrade == "CS" || $scope.headerDetails.AssesseeGrade == "CSD") {
                            $scope.partialPage = '/app/ipm/partials/ipmAssessmentPrevDetailsCSCSD.html';
                        } else {
                            $scope.partialPage = '/app/ipm/partials/ipmAssessmentPrevDetailsF1F2.html';
                        }
                        $scope.model = result;

                        angular.forEach(result, function (item) {
                            if (item[0]) {
                                $scope.totalScore = $scope.totalScore + parseInt(item[0].Rating);
                            }
                        });
                    } else {
                        toastr.clear();
                        toastr.info('No assessment records found');
                    }
                }, function (error) {
                    ipmassessmentListBlockUI.stop();
                });
            }

            $scope.goBack = function () {
                $state.go($stateParams.back);
            };

            $scope.to_trusted = function (html_code) {

                if (html_code && html_code.length > 0) {
                    return $sce.trustAsHtml(html_code.replace(/\n\r?/g, '<br />'));
                }
            }

            $scope.GetTotal = function (total) {
                if (total >= 95) {
                    return 'SEE';
                } else if (total >= 80 && total < 95) {
                    return 'EE';
                } else if (total >= 60 && total < 80) {
                    return 'COM';
                } else if (total >= 40 && total < 60) {
                    return 'DEV';
                } else if (total >= 20 && total < 40) {
                    return 'SD';
                }
            };

            getMyAssessmentList();
        }]);