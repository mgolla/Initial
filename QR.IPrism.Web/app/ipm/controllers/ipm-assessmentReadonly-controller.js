
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.assessmentReadonly.controller', ['$scope', 'lookupDataService', '$state', '$stateParams',
        'sharedDataService', 'appSettings', 'ngDialog', '$window', '$sce', 'analyticsService', 'messages', 'toastr', 'blockUI', 'assessmentServices', '$filter',
        function ($scope, lookupDataService, $state, $stateParams, sharedDataService, appSettings, ngDialog, $window, $sce,
            analyticsService, messages, toastr, blockUI, assessmentServices, $filter) {

            var ipmAssmtAckBlockUI = blockUI.instances.get('ipmassessmentBlockUI');
            $scope.model = {};
            $scope.model.vModel = {};
            $scope.showNotification = false;
            $scope.assessmentDetail;
            $scope.statusType = sharedDataService.getStatusType();
            $scope.fileTypeObj = [];
            $scope.back = $stateParams.back;
            $scope.isMobile = appSettings.isMobile;

            function pageEvents() {

                $scope.openImage = function (data) {

                    sharedDataService.openFile(data, $scope.fileTypeObj);
                    return false;
                }

                //$scope.goBack = function () {
                //    $state.go($stateParams.back);
                //};

                $scope.to_trusted = function (html_code) {

                    if (html_code && html_code.length > 0) {
                        return $sce.trustAsHtml(html_code.replace(/\n\r?/g, '<br />'));
                    }
                }

                $scope.acknowledge = function () {

                    $scope.dialogTitle = "Confirmation";
                    $scope.dialogMessage = messages.HOUCONFIRMACK;
                    ngDialog.open({
                        scope: $scope,
                        preCloseCallback: function (value) {
                            if (value == 'Post') {

                                var model = {
                                    Id: sharedDataService.notificationDetails.Id,
                                    Status: 'D'
                                }

                                ipmAssmtAckBlockUI.start();
                                // Update notification 
                                sharedDataService.updateNotifications(model, function (result) {
                                    ipmAssmtAckBlockUI.stop();
                                    if (result) {

                                        toastr.error(messages.HOUACCEPTNOTICE);
                                        sharedDataService.notificationDetails = null;
                                        $state.go('home', { param: { IsRefreshAlerts: true } });
                                        $window.location.reload();

                                    } else {
                                        toastr.error(messages.HOUERROR01);
                                    }
                                }, function (error) {
                                    ipmAssmtAckBlockUI.stop();
                                    toastr.error(messages.HOUERROR01);
                                });
                            }
                        }
                    });
                };

                $scope.chkRecAsmt = function () {
                    $scope.rdRecAsmt = "RecAsmt";
                    if ($scope.model.vModel.Assessment.Objectives && $scope.model.vModel.Assessment.Objectives != null && $scope.model.vModel.Assessment.Objectives.length > 0) {
                        angular.forEach($scope.model.vModel.Assessment.Objectives, function (Objective, key) {

                            if (Objective.AssmIDPs.length > 0) {
                                $scope.showIDPForm = true;
                            }
                        });
                    }
                }

                $scope.chkRecNonAsmt = function () {
                    $scope.rdRecAsmt = "RecNonAsmt"
                    //LoadReasonForRecNonAsmt()
                }
            }

            function getMimeType() {
                sharedDataService.getCommonInfo('MIMETYPE', function (result) {
                    angular.forEach(result, function (data) {
                        $scope.fileTypeObj.push(data);
                    });
                }, function (error) {

                });
            };

            function LoadReasonForRecNonAsmt() {

                ipmAssmtAckBlockUI.start();
                lookupDataService.getLookupList('ReasonForRecNonAsmt', null, function (result) {

                    $scope.reasonForRecNonAsmtList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });
                    //$scope.reasonForRecNonAsmtList.unshift(initialDrpValues);

                    $scope.model.vModel.Assessment.ReasonForNonSubmission = $scope.reasonForRecNonAsmtList[0];

                    ipmAssmtAckBlockUI.stop();
                }, function (error) {
                    ipmAssmtAckBlockUI.stop();
                });
            };

            function getFlightDetailsData(result) {
                if (result) {
                    $scope.assessmentDetail = result;
                    $scope.assesseeStaffName = result.AssesseeStaffName;
                    $scope.assesseeStaffNo = result.AssesseeStaffNo;
                    $scope.assesseeGrade = result.AssesseeGrade;
                    $scope.AssessmentType = result.AssessmentType;
                    $scope.AssessmentStatus = result.AssessmentStatus;
                    $scope.flightNumber = result.FlightNumber;
                    $scope.date = result.FlightDate;
                    $scope.pax_Load_F = result.Pax_Load_F > 0 ? messages.ASMNTPAXF + result.Pax_Load_F : messages.ASMNTPAXFEMP;
                    $scope.pax_Load_J = result.Pax_Load_J > 0 ? messages.ASMNTPAXJ + result.Pax_Load_J : messages.ASMNTPAXJEMP;
                    $scope.pax_Load_Y = result.Pax_Load_Y > 0 ? messages.ASMNTPAXY + result.Pax_Load_Y : messages.ASMNTPAXYEMP
                    $scope.infants = result.Infants && result.Infants > 0 ? messages.ASMNTPAXINFANTS + result.Infants.toString() : messages.ASMNTPAXINFANTSEMP;
                    $scope.acType = result.AircraftType;
                    $scope.workPosition = result.CrewPosition === "" ? messages.ASMNTNA : result.CrewPosition;
                    $scope.appraiser = result.AssessorStaffName;
                    $scope.staffNo = result.AssessorStaffNo;
                    $scope.reportedPM = result.ManagerStaffName;
                    $scope.sector = result.SectorFrom + "-" + result.SectorTo;

                    $scope.remark = messages.ASMTREMARK.replace("@flightno", '<b>' + $scope.flightNumber + '</b>')
                        .replace("@sector", '<b>' + $scope.sector + '</b>')
                        .replace("@name", '<b>' + result.AssessorStaffName + '(' + result.AssessorStaffNo + ')' + '</b>')
                        .replace("@std", '<b>' + $filter('date')($scope.date, 'dd-MMM-yyyy') + '</b>');

                    ShowMessage($scope.AssessmentStatus);
                }
            }

            function ShowMessage(status) {

                var msg = "";
                switch (status) {

                    case "Scheduled":
                        msg = "Assessment is in the scheduled state.";
                        break;

                    case "Escalated":
                        msg = "Assessment is in the escalated state.";
                        break;

                    case "Delayed":
                        msg = "Assessment is in the delayed state.";
                        break;

                    case "Cancelled":
                        msg = "Assessment is in the cancelled state.";
                        break;
                }

                if (msg) {

                    $scope.dialogTitle = "Assessment Notifications";
                    $scope.dialogMessage = msg;
                    ngDialog.open({
                        template: '/app/ipm/partials/ipmModalConfirmation.html',
                        scope: $scope,
                        preCloseCallback: function (value) {
                            if (value == 'Post') {

                            }
                        }
                    });
                }
            }

            function getFlightDetails(id) {
                ipmAssmtAckBlockUI.start();

                assessmentServices.getAssessorAssesseeFlightDetails(id, function (result) {
                    if (result) {
                        getFlightDetailsData(result);
                    }
                    ipmAssmtAckBlockUI.stop();
                }, function (error) {
                    ipmAssmtAckBlockUI.stop();
                });
            }

            function getNotification(id) {

                sharedDataService.getNotification(id, function (success) {

                    $scope.showNotification = true;
                    $scope.model = success[0];
                    if (success) {

                        getAssessmentDetails($scope.model.RequestGuid);
                        getFlightDetails($scope.model.RequestGuid);

                        $scope.model.Sender = "Cabin Crew Assessments";
                        if ($stateParams.reqtype.toLowerCase() == messages.ASMTDELAYED.toLowerCase() || $stateParams.reqtype == messages.ASMTDELAYEDSAVED.toLowerCase()) {
                            $scope.model.Description = $scope.model.Description + " - Kindly acknowledge";
                        } else if ($stateParams.reqtype.toLowerCase() == messages.ASMT.toLowerCase()) {
                            $scope.model.Status = "Completed";
                        }
                    }
                }, function (error) {
                    toastr.error(messages.HOUERROR01);
                });

            };

            function getAssessmentDetails(id) {
                ipmAssmtAckBlockUI.start();
                assessmentServices.getAssessmentDetails(id, function (result) {

                    $scope.model.vModel = result;
                    getFlightDetailsData($scope.model.vModel.Assessment)
                    //$scope.model.vModel.Assessment.AdditionalComments = $scope.assessmentDetail ? $scope.assessmentDetail.AdditionalComments : '';

                    if ($scope.model.vModel.Assessment.Objectives && $scope.model.vModel.Assessment.Objectives != null &&
                        $scope.model.vModel.Assessment.Objectives.length > 0) {

                        angular.forEach($scope.model.vModel.Assessment.Objectives, function (Objective, key) {
                            angular.forEach(Objective.ExceedsExpectations, function (ExceedsExpectation, key) {
                                if (ExceedsExpectation.IsEEChecked === "Y") {
                                    ExceedsExpectation.IsEEChecked = true;
                                }
                            });
                        });
                    }

                    // condition for showing condition checked for readonly page
                    if (result.Assessment.IsRecorded == '1') {
                        $scope.chkRecAsmt();
                    } else if (result.Assessment.IsRecorded == '2') {
                        $scope.chkRecNonAsmt();
                    }

                    ipmAssmtAckBlockUI.stop();
                }, function (error) {
                    ipmAssmtAckBlockUI.stop();
                });
            };

            function notificationInit() {
                getNotification($stateParams.reqno);

            };

            function ReadonlyInit() {
                // getFlightDetails($stateParams.id);

                getAssessmentDetails($stateParams.id);


            };

            if (!$stateParams.id) {
                notificationInit();
            } else {
                ReadonlyInit();
            }

            pageEvents();
            getMimeType();

        }]);