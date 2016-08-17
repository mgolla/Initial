
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.acknowledge.controller', ['$scope', 'lookupDataService', '$state', '$stateParams',
        'sharedDataService', 'appSettings', 'ngDialog', 'assessmentServices', 'analyticsService', 'messages', 'toastr', 'blockUI', '$filter', '$window',
        function ($scope, lookupDataService, $state, $stateParams, sharedDataService, appSettings, ngDialog, assessmentServices,
            analyticsService, messages, toastr, blockUI, $filter, $window) {

            var ipmAssmtAckBlockUI = blockUI.instances.get('ipmAssmtAckBlockUI');
            $scope.model = {};
            $scope.statusType = sharedDataService.getStatusType();
            $scope.submitted = false;
            $scope.model.iscommentRequired = false;

            function pageEvents() {

                $scope.goBack = function () {
                    $window.history.back();
                }

                $scope.acknowledge = function () {

                    $scope.dialogTitle = "Confirmation";
                    $scope.dialogMessage = messages.HOUCONFIRMACK;
                    ngDialog.open({
                        scope: $scope,
                        preCloseCallback: function (value) {
                            if (value == 'Post') {

                                var model = {
                                    Id: $stateParams.reqno,
                                    Status: 'D'
                                }

                                ipmAssmtAckBlockUI.start();
                                // Update notification 
                                sharedDataService.updateNotifications(model, function (result) {
                                    ipmAssmtAckBlockUI.stop();
                                    if (result) {
                                        toastr.info(messages.HOUACCEPTNOTICE);
                                        $state.go('home');
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

                $scope.behaviourAck = function (form) {

                    $scope.submitted = true;

                    if ($scope.model.IsCrewCommentsRequired != "Y" ||
                        ($scope.model.IsCrewCommentsRequired == "Y" &&
                        $scope.model.AdditionalInfo &&
                        $scope.model.AdditionalInfo.length > 0)) {

                        $scope.dialogTitle = "Confirmation";
                        $scope.dialogMessage = messages.HOUCONFIRMACK;
                        ngDialog.open({
                            scope: $scope,
                            preCloseCallback: function (value) {
                                if (value == 'Post') {

                                    var model = {
                                        Id: $stateParams.reqno,
                                        RequestGuid: $scope.model.RequestGuid,
                                        Status: $scope.model.IsCrewCommentsRequired,
                                        Message: $scope.model.AdditionalInfo
                                    }

                                    ipmAssmtAckBlockUI.start();
                                    // Update notification 
                                    sharedDataService.updateBehaviourNotifications(model, function (result) {

                                        ipmAssmtAckBlockUI.stop();
                                        if (result) {
                                            //toastr.info(messages.HOUACCEPTNOTICE);
                                            $state.go('home', { param: { IsRefreshAlerts: true } });
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
                    } else {
                        toastr.error(messages.INVALIDFIELDS);
                    }
                };
            }

            function getBehaviourNotification() {
                ipmAssmtAckBlockUI.start();
                sharedDataService.getBehaviourNotification($stateParams.reqno, function (success) {

                    $scope.model = success;
                    $scope.model.isRequired = ($scope.model.IsCrewCommentsRequired == 'Y' ? true : false);
                    console.log($scope.model.isRequired);
                    ipmAssmtAckBlockUI.stop();
                }, function (error) {
                    ipmAssmtAckBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            }

            function initialize() {

                ipmAssmtAckBlockUI.start();
                sharedDataService.getNotification($stateParams.reqno, function (success) {

                    ipmAssmtAckBlockUI.stop();

                    if (success && success.length > 0) {

                        $scope.model = success[0];
                        $scope.model.Sender = "Cabin Crew Assessments";
                        $scope.model.type = $scope.statusType[$scope.model.Status];

                        if ($stateParams.reqtype.toLowerCase() == messages.ASMTDELAYED.toLowerCase() ||
                            $stateParams.reqtype.toLowerCase() == messages.ASMTDELAYEDSAVED.toLowerCase()) {

                            $scope.model.Description = $scope.model.Description + " - Kindly acknowledge";

                            ipmAssmtAckBlockUI.start();
                            assessmentServices.getMyAssessmentList($scope.model.RequestGuid, function (result) {

                                ipmAssmtAckBlockUI.stop();
                                if (result && result[0]) {

                                    var data = result[0][0];

                                    $scope.model.Description = messages.ASMTDELAYNOTFORMAT +
                                        messages.ASMTFLIGHTDETAILS.replace('@flightno', '<b>' + data.FlightNumber + ' </b>')
                                     .replace('@sector', '<b>' + data.SectorFrom + "-" + data.SectorTo + ' </b>')
                                     .replace('@std', '<b>' + $filter('date')(new Date(data.AssessmentDate), "dd-MMM-yyyy") + ' </b>')
                                     .replace('@crew.', '<b>' + data.AssesseeStaffName + ' </b>');
                                }
                            }, function (error) {
                                ipmAssmtAckBlockUI.stop();
                            });

                        } else if ($stateParams.reqtype.toLowerCase() == "Assessment") {

                            $scope.model.Status = "Completed";
                        } else if ($stateParams.reqtype.toLowerCase() == messages.FILENOTEIDP.toLowerCase()) {

                            $scope.model.Sender = $stateParams.reqtype;
                            $scope.model.type = "Sent For Acknowledge";
                        } else if ($stateParams.reqtype.toLowerCase() == messages.FMS.toLowerCase()) {

                            $scope.model.Sender = $stateParams.reqtype;
                        } else if ($stateParams.reqtype.toLowerCase() == messages.PACKAGENOTICE.toLowerCase()) {

                            $scope.model.Sender = "Prism Admin";
                        }
                    }

                }, function (error) {
                    ipmAssmtAckBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            };

            pageEvents();
            if ($stateParams.reqtype.toLowerCase() == messages.IDPBEHAVIOUR.toLowerCase()) {
                getBehaviourNotification();
            } else {
                initialize();
            }

        }]);