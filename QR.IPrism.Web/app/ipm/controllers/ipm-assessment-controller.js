'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.assessment.controller', ['$scope', '$state', '$rootScope', 'assessmentServices', 'lookupDataService', 'messages', 'blockUI', '$stateParams',
        'analyticsService', 'toastr', 'sharedDataService', '$window', '$filter', 'bundleMessage', 'ngDialog', 'FileUploader', '$interval', '$timeout', '$location', 'appSettings',
function ($scope, $state, $rootScope, assessmentServices, lookupDataService, messages, blockUI, $stateParams, analyticsService, toastr, sharedDataService,
    $window, $filter, bundleMessage, ngDialog, FileUploader, $interval, $timeout, $location, appSettings) {

    // Private variables initialization
    var ipmassessmentBlockUI = blockUI.instances.get('ipmassessmentBlockUI');
    var initialDrpValues = { 'Value': '', 'Text': '--Select--' };
    $scope.oneAtATime = false;
    $scope.saveSuccess = false;
    $scope.savedate = new Date();
    $scope.isInflightShow = true;
    $scope.status = {
        open: true,
        isFirstOpen: true,
        isFirstDisabled: false
    };
    $scope.assessee = {
        isOpen: true
    };

    $scope.backRouter = '';
    // Initialize scope variables

    $scope.isChanged = false;
    $scope.isWithoutAssessmentID = false;
    $scope.isIdpFilled = false;

    $scope.model = {};
    $scope.model.vModel = {};

    $scope.assessmentDetail = {};
    $scope.assesseeGrade;
    $scope.ratingGuidelinesRead = [];
    $scope.overallComment;
    $scope.assessmentRequestForm;
    $scope.recordAssessment = {}
    $scope.totalCount;
    $scope.ratSelect;
    $scope.model.asmntSubmitResult;
    //$scope.countEEAchived = 0;
    $scope.IDPCount = 0;
    $scope.totalWeightage = 0;
    $scope.showIDPForm;
    $scope.temp = {};
    $scope.temp.reasonForNonSubmission;
    $scope.temp.reasonForNonSubmissionOther;
    $scope.temp.rdRecAsmt;

    $scope.isLoadNonSubmissionOtherReason = false;

    $scope.COMStatus = messages.ASMNTCOM;
    $scope.reasonForRecNonAsmtList = [];
    //$scope.model.reasonForRecNonAsmt = $scope.model.reasonForRecNonAsmtListObj ? ($scope.model.reasonForRecNonAsmtListObj.Text == '--All--' ? '' : $scope.model.reasonForRecNonAsmtListObj.Text) : '';
    $scope.model.reasonForRecNonAsmt = [];
    $scope.uploadType = 'RecAsmt';

    $scope.invalidAttachment = false;
    $scope.fileType = [];
    $scope.fileTypeObj = [];

    $scope.removedFiles = [];
    $scope.isMobile = appSettings.isMobile;

    var promise, tab = null, message, selectedId = '';

    function PageEvents() {

        $scope.back = $stateParams.back;
        $scope.isNullOrEmpty = function (value) {
            return (!(value && value != null && value.toString().trim().length > 0));
        };

        //$scope.onClickBack = function () {
        //    $state.go($stateParams.back);
        //};

        $scope.start = function () {
            // stops any running interval to avoid two intervals running at the same time
            $scope.stop();

            // store the interval promise
            promise = $interval(function () {
                if ($scope.isChanged) {
                    $scope.isChanged = false;
                    $scope.saveAndAutoSave();
                }

            }, 220000);
        };

        $scope.$on('$destroy', function () {
            $scope.stop();
        });

        // stops the interval
        $scope.stop = function () {
            $scope.isChanged = false;
            $interval.cancel(promise);
        };

        $scope.onClickEdit = function () {
            $scope.isEditDisable = false;
        }

        $scope.clickOnHeader = function (para) {

            if (para.isOpen) { para.isOpen = false; } else { para.isOpen = true; }

        }

        $scope.openImage = function (data) {

            sharedDataService.openFile(data, $scope.fileTypeObj);
            //var image = new Image();
            //image.src = "data:image/jpg;base64," + data;

            //var w = $window.open("", "_blank");
            //w.document.write(image.outerHTML);
            return false;
        }

        $scope.chkRecAsmt = function () {
            $scope.status.open = false;
            $scope.isInflightShow = true;
            $scope.temp.rdRecAsmt = "RecAsmt";
            if ($scope.model.vModel.Assessment.Objectives && $scope.model.vModel.Assessment.Objectives != null && $scope.model.vModel.Assessment.Objectives.length > 0) {
                angular.forEach($scope.model.vModel.Assessment.Objectives, function (Objective, key) {

                    if (Objective.AssmIDPs.length > 0) {
                        $scope.showIDPForm = true;
                    }
                });
                autoSave();
            }

        }

        $scope.chkRecNonAsmt = function () {
            $scope.status.open = false;
            $scope.isInflightShow = false;
            $scope.temp.rdRecAsmt = "RecNonAsmt"
            LoadReasonForRecNonAsmt()
            $scope.stop();
            $scope.isDisablePage = false;
            if ($scope.model.vModel.Assessment.Objectives && $scope.model.vModel.Assessment.Objectives != null && $scope.model.vModel.Assessment.Objectives.length > 0) {
                clearForm();
            };
        }

        $scope.saveAndAutoSave = function (form) {

            $scope.formValid = true;
            $scope.model.vModel.Assessment.AutoSave = "Y";
            $scope.model.vModel.Assessment.IsRecorded = 1;

            postInsertUpdateAssessment();

        }

        $scope.toSubmit = function () {

            $scope.submitted = true;
            if ($scope.formValid) {
                $scope.dialogTitle = "Submit Confirmation";
                $scope.dialogMessage = messages.ASMTSUBMITCONFIRM;
                ngDialog.open({
                    scope: $scope,
                    preCloseCallback: function (value) {
                        if (value == 'Post') {
                            $scope.model.vModel.Assessment.AutoSave = "N";
                            postInsertUpdateAssessment();

                        }
                    }
                });
            }
            else {
                //$scope.submitDisable = true;
            }
        }

        $scope.submitRequest = function (form) {

            $scope.formValid = true;
            $scope.submitted = true;
            $scope.isBreakIDP = checkIDP();

            if ($scope.temp.rdRecAsmt == 'RecAsmt') {

                validateAndAssignData();

                if ($scope.isBreakIDP) {

                    if ($scope.isEditDisable) {
                        validateIDP();
                        $scope.toSubmit();
                    } else {
                        if ($scope.formValid) {
                            $scope.dialogTitle = "IDP";
                            $scope.dialogMessage = messages.ASMNTIDPREQ;
                            ngDialog.open({
                                scope: $scope,

                                preCloseCallback: function (value) {
                                    if (value == 'Post') {

                                        $scope.loadIDP();

                                        $scope.isEditDisable = true;

                                    } else {
                                        $scope.formValid = false;
                                        $scope.isBreak = false;
                                    }
                                }
                            });
                        }
                    }
                }
                else {
                    $scope.toSubmit();
                }
            }

            else if ($scope.temp.rdRecAsmt == 'RecNonAsmt') {

                if ($scope.isNullOrEmpty($scope.model.vModel.Assessment.ReasonForNonSubmission)) {

                    toastr.warning('Please enter reason for non submission .');
                    $scope.formValid = false;
                    $scope.isBreak = false;
                }
                else if ($scope.isLoadNonSubmissionOtherReason == true) {
                    if ($scope.isNullOrEmpty($scope.temp.reasonForNonSubmissionOther)) {

                        toastr.warning('Please enter other reason for non submission .');
                        $scope.formValid = false;
                        $scope.isBreak = false;
                    }
                } else if ($scope.isNullOrEmpty($scope.model.vModel.Assessment.AdditionalComments)) {

                    $scope.model.vModel.Assessment.InvalidAdditionalComments = true;
                    toastr.warning('Please enter overall comment .');
                    $scope.formValid = false;
                } else {
                    var val = checkMinimum50Charators($scope.model.vModel.Assessment.AdditionalComments, 'overall comment ');

                    if (!val) {
                        $scope.model.vModel.Assessment.InvalidAdditionalComments = true;
                        $scope.formValid = false;
                        toastr.warning(message);
                    } else {
                        $scope.model.vModel.Assessment.InvalidAdditionalComments = false;
                    }
                }

                $scope.toSubmit();

            }
        }

        $scope.onObjectiveTabChange = function (objective) {

            $scope.competencies = objective.Competencies;
            $scope.exceedsExpectations = objective.ExceedsExpectations;

        };

        $scope.onUserChangeData = function () {
            $scope.stop();
            $scope.start();
            $scope.isChanged = true;

        };

        $scope.onTextAreaChange = function () {

            $scope.onUserChangeData();

        };

        $scope.$watch("model.vModel.Assessment.Objectives", function (newValue, oldValue) {
            if (newValue !== oldValue && $scope.temp.rdRecAsmt == "RecAsmt") {
                $scope.onUserChangeData();
            }

            openTab(tab);
        }, true); // Object equality (not just reference).

        $scope.ratingValidate = function (objectiveP, competencyNameP) {

            $scope.onUserChangeData();
        };

        $scope.loadIDP = function () {

            angular.forEach($scope.model.vModel.Assessment.Objectives, function (objective, key) {
                $scope.IDPList = [];
                angular.forEach(objective.Competencies, function (Competency, key) {

                    if ((Competency.Rating === messages.ASMNTSD || Competency.Rating === messages.ASMNTDEV)) {

                        var COMList = $.grep(objective.AssmIDPs, function (v, i) {

                            return v.CompetencyName == Competency.CompetencyName;
                        });

                        if (COMList && COMList.length > 0) {


                            $scope.IDPList = $.grep($scope.IDPList, function (e) { return e.CompetencyName != Competency.CompetencyName; });


                            $scope.IDPList.push(COMList[0]);


                        } else {

                            $scope.IDPList = $.grep($scope.IDPList, function (e) { return e.CompetencyName != Competency.CompetencyName; });

                            $scope.AssmIDP = {
                                CompetencyName: Competency.CompetencyName,
                                ObjectiveName: objective.ObjectiveName,
                                Observation: Competency.Comments
                            };


                            $scope.IDPList.push($scope.AssmIDP);
                        }

                    }
                });
                objective.AssmIDPs = [];
                objective.AssmIDPs = $scope.IDPList;


            });

            $scope.showIDPForm = true;

        };

        $scope.chkValidate = function (exceedsExpectation) {
            if (!exceedsExpectation.IsEEChecked) {

                exceedsExpectation.Comments = '';
                //$scope.countEEAchived = $scope.countEEAchived + 1;
            }
        }

        $scope.onSelectReasonForNonSubmission = function (model) {
            $scope.isLoadNonSubmissionOtherReason = false;
            if (model) {

                $scope.model.vModel.Assessment.ReasonForNonSubmission = model.Text;
                $scope.model.vModel.Assessment.ReasonforNonsubmissionID = model.Value;
                if (model.Text == 'Other') {
                    $scope.isLoadNonSubmissionOtherReason = true;
                }

            } else {
                $scope.model.vModel.Assessment.ReasonForNonSubmission = '';
                $scope.model.vModel.Assessment.ReasonforNonsubmissionID = '';
            }
        }

        $scope.clearForm = function () {

            $scope.dialogTitle = "Confirmation";
            $scope.dialogMessage = "Are you sure want to reset the form?";
            ngDialog.open({
                scope: $scope,
                preCloseCallback: function (value) {
                    if (value == 'Post') {
                        clearForm();
                        //analyticsService.trackEvent('Action', 'Add', 'View', 'Create Housing Request');
                    }
                }
            });
        }

        $scope.removeDocument = function (file, $index) {

            if ($scope.removedFiles.indexOf(file.FileId) == -1) {
                $scope.removedFiles.push(file.FileId);
            }

            $scope.model.vModel.Assessment.Attachments.splice($index, 1);
        };
    }

    function getMimeType() {
        sharedDataService.getCommonInfo('MIMETYPE', function (result) {
            angular.forEach(result, function (data) {
                $scope.fileType.push(data.Text);
                $scope.fileTypeObj.push(data);
            });
        }, function (error) {

        });
    };

    function loadDataInitial() {
        if ($scope.assessmentID && !$scope.isNullOrEmpty($scope.assessmentID)) {
            getFlightDetails($scope);
            getAssessmentDetails($scope);
        }

    }
    //Controller Scope Initialization

    function initialize() {

        $scope.isDisablePage = false;
        $scope.isEditDisable = false;
        $scope.model.uploader = {};
        $scope.crewasmntdetails;
        getMimeType();
        PageEvents();

        if ($stateParams.myparam && $stateParams.myparam != null && $stateParams.myparam.asmtdetail != null) {

            if ($stateParams.myparam.asmtdetail.AssessmentID
            && $stateParams.myparam.asmtdetail.AssessmentID != null
            && $stateParams.myparam.asmtdetail.AssessmentID.toString().trim().length > 0) {

                $scope.crewasmntdetails = $stateParams.myparam.asmtdetail;
                $scope.assessmentID = $scope.crewasmntdetails.AssessmentID;
                $scope.designation = $scope.crewasmntdetails.Grade;
                $scope.customSatus = $scope.crewasmntdetails.CustomStatus;
                if ($scope.customSatus.toString().trim() == messages.ASMNTCOMPLETE
                     || $scope.customSatus.toString().trim().toUpperCase() == messages.ASMNTELAPSED.toUpperCase()) {

                    $scope.isDisablePage = true;
                }

                getFlightDetails($scope);

                if ($scope.customSatus === messages.ASMNTDRAFT || $scope.customSatus == messages.ASMNTDELAYSAVED || $scope.customSatus == messages.ASMNTSCHEDULEDSAVED || $scope.customSatus == messages.ASMNTCOMPLETE) {
                    getAssessmentDetails($scope);
                }
                else {
                    getAssessmentByGrade($scope);
                }
            } else {
                $scope.crewasmntdetails = $stateParams.myparam.asmtdetail;
                $scope.assessmentDetail = $scope.crewasmntdetails;
                $scope.designation = $scope.crewasmntdetails.Grade;

                if ($scope.designation && $scope.designation.toString().trim().length > 0) {
                    $scope.isWithoutAssessmentID = true;
                    getAssessmentByGrade($scope);
                }
            }

        } else {
            //$state.go($stateParams.back);
            $rootScope.goBack($stateParams.back);
        }
        $scope.oneAtATime = true;
        $scope.isChanged = false;
    }

    function autoSave() {
        $scope.start();




    }

    function chkObjectiveWetage() {
        $scope.totalWeightage = 0;
        if ($scope.model.vModel.Assessment.Objectives && $scope.model.vModel.Assessment.Objectives != null && $scope.model.vModel.Assessment.Objectives.length > 0) {
            angular.forEach($scope.model.vModel.Assessment.Objectives, function (Objective, key) {

                $scope.totalWeightage = $scope.totalWeightage + Objective.Weightage;

            });
        }
        if ($scope.totalWeightage != 100) {
            $scope.isDisablePage = true;
            $scope.submitDisable = true;
            toastr.info(messages.ASMNTOVERALLSCORE);
        } else {

            if ($scope.isWithoutAssessmentID && $scope.totalWeightage == 100) {
                $scope.saveAndAutoSave();
            }

        }


    }

    function getFlightDetails($scope) {
        ipmassessmentBlockUI.start();

        assessmentServices.getAssessorAssesseeFlightDetails($scope.assessmentID, function (result) {
            $scope.assessmentDetail = result;
            $scope.assesseeStaffName = result.AssesseeStaffName;
            $scope.assesseeStaffNo = result.AssesseeStaffNo;
            $scope.assesseeGrade = result.AssesseeGrade;
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

            ipmassessmentBlockUI.stop();
        }, function (error) {
            ipmassessmentBlockUI.stop();
        });
    }

    function getAssessmentByGrade($scope) {

        ipmassessmentBlockUI.start();
        assessmentServices.getAssessmnetByGrade($scope.designation, function (result) {
            $scope.model.vModel = result;
            $scope.isChanged = false;
            ipmassessmentBlockUI.stop();
            chkObjectiveWetage();
        }, function (error) {
            $scope.isWithoutAssessmentID = false;
            ipmassessmentBlockUI.stop();
        });
    }

    function getAssessmentDetails($scope) {

        ipmassessmentBlockUI.start();
        assessmentServices.getAssessmentDetails($scope.assessmentID, function (result) {
            $scope.model.vModel = result;
            $scope.isChanged = false;
            //$scope.model.vModel.Assessment.AdditionalComments = $scope.assessmentDetail.AdditionalComments;

            //if (!($scope.isNullOrEmpty($scope.model.vModel.Assessment.ReasonForNonSubmission))) {

            //    $scope.temp.rdRecAsmt = 'RecNonAsmt';
            //}else{
            //    $scope.temp.rdRecAsmt = 'RecAsmt';
            //}

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
            if ($scope.model.vModel.Assessment.IsRecorded && parseInt($scope.model.vModel.Assessment.IsRecorded) == 1) {
                $scope.chkRecAsmt();
            } else if ($scope.model.vModel.Assessment.IsRecorded && parseInt($scope.model.vModel.Assessment.IsRecorded) == 2) {
                $scope.chkRecNonAsmt();
            }
            var isIDP = checkIDP();
            if (isIDP) {
                var iDPCount = getIDPCalculate();
                if (iDPCount > 0) {
                    $scope.isEditDisable = true;
                }
            }


            ipmassessmentBlockUI.stop();
            chkObjectiveWetage();
        }, function (error) {
            ipmassessmentBlockUI.stop();
        });
    }

    function assignAssessment() {
        $scope.model.vModel.Assessment.AssesseeGrade = $scope.assessmentDetail.AssesseeGrade ? $scope.assessmentDetail.AssesseeGrade : "";
        if (($scope.crewasmntdetails.AssessmentID && $scope.crewasmntdetails.AssessmentID.toString().trim().length > 0)) {
            $scope.model.vModel.Assessment.AssessmentID = $scope.crewasmntdetails.AssessmentID ? $scope.crewasmntdetails.AssessmentID : "";
        }

        $scope.model.vModel.Assessment.AssessorCrewDetID = $scope.assessmentDetail.AssessorCrewDetID ? $scope.assessmentDetail.AssessorCrewDetID : "";
        $scope.model.vModel.Assessment.AssesseeCrewDetID = $scope.assessmentDetail.AssesseeCrewDetID ? $scope.assessmentDetail.AssesseeCrewDetID : "";
        $scope.model.vModel.Assessment.AssessmentType = $scope.assessmentDetail.AssessmentType ? $scope.assessmentDetail.AssessmentType : "";
        $scope.model.vModel.Assessment.AssessorGrade = $scope.assessmentDetail.AssessorGrade ? $scope.assessmentDetail.AssessorGrade : "";
        $scope.model.vModel.Assessment.FlightDetID = $scope.assessmentDetail.FlightDetID ? $scope.assessmentDetail.FlightDetID : "";
        $scope.model.vModel.Assessment.AssessmentStatus = $scope.assessmentDetail.AssessmentStatus ? $scope.assessmentDetail.AssessmentStatus : "";
        $scope.model.vModel.Assessment.AssessmentDate = $scope.assessmentDetail.AssessmentDate ? $scope.assessmentDetail.AssessmentDate : null;
        if (!($scope.model.vModel.Assessment.AssessmentDate && $scope.model.vModel.Assessment.AssessmentDate.toString().trim().length > 0)) {
            $scope.model.vModel.Assessment.AssessmentDate = $scope.crewasmntdetails.DateofAssessment;
        }

        $scope.model.vModel.Assessment.FlightNumber = $scope.assessmentDetail.FlightNumber ? $scope.assessmentDetail.FlightNumber : "";
        $scope.model.vModel.Assessment.FlightDate = $scope.assessmentDetail.FlightDate ? $scope.assessmentDetail.FlightDate : "";
        $scope.model.vModel.Assessment.SectorFrom = $scope.assessmentDetail.SectorFrom ? $scope.assessmentDetail.SectorFrom : "";
        $scope.model.vModel.Assessment.SectorTo = $scope.assessmentDetail.SectorTo ? $scope.assessmentDetail.SectorTo : "";

        $scope.model.vModel.Assessment.ReasonForNonSubmission = $scope.temp.reasonForNonSubmissionOther ? $scope.temp.reasonForNonSubmissionOther : ($scope.model.vModel.Assessment.ReasonForNonSubmission ? $scope.model.vModel.Assessment.ReasonForNonSubmission : "");
    }

    function calculateScores() {

        if ($scope.model.vModel.Assessment.Objectives &&
            $scope.model.vModel.Assessment.Objectives != null &&
            $scope.model.vModel.Assessment.Objectives.length > 0) {

            $scope.model.vModel.Assessment.TotalScore = 0;
            angular.forEach($scope.model.vModel.Assessment.Objectives, function (Objective, key) {
                //Objective.ObjScore = 0;
                //var type1ObjectiveScore = 0;//COM,DEV, SD
                //var type2ObjectiveScore = 0;//SEE, EE
                var type1CompetencyScore = 0;//COM,DEV, SD
                var type2CompetencyScore = 0;//SEE, EE



                var type1List = $.grep(Objective.Competencies, function (v, i) {

                    return (v.Rating == messages.ASMNTCOM || v.Rating == messages.ASMNTDEV || v.Rating == messages.ASMNTSD);
                });
                var type1Count = type1List.length;

                var type1CompetencyMark = ((Objective.Weightage * 80) / 100) / type1Count;

                //var type2Lsit = $.grep(Objective.ExceedsExpectations, function (v, i) {

                //    return v.Rating == messages.ASMNTCOM;
                //});
                var type2Count = Objective.ExceedsExpectations.length;
                var type2CompetencyMark = ((Objective.Weightage * 20) / 100) / type2Count;


                angular.forEach(Objective.Competencies, function (Competency, key) {
                    if (Competency.Rating == messages.ASMNTCOM) {
                        //Objective.ObjScore = Objective.ObjScore + Competency.CompetencyScore;
                        type1CompetencyScore = type1CompetencyScore + type1CompetencyMark;
                    }


                });
                angular.forEach(Objective.ExceedsExpectations, function (ExceedsExpectation, key) {

                    if (ExceedsExpectation.IsEEChecked === true) {
                        type2CompetencyScore = type2CompetencyScore + type2CompetencyMark;
                    }

                });
                $scope.model.vModel.Assessment.TotalScore = $scope.model.vModel.Assessment.TotalScore + type1CompetencyScore + type2CompetencyScore;


                // Overall Objective rating 
                var allCompetenciesCount = Objective.Competencies.length;

                var comList = $.grep(Objective.Competencies, function (v, i) {

                    return (v.Rating == messages.ASMNTCOM);
                });
                var comListCount = comList.length;
                var devList = $.grep(Objective.Competencies, function (v, i) {

                    return (v.Rating == messages.ASMNTDEV);
                });
                var devListCount = devList.length;
                var sdList = $.grep(Objective.Competencies, function (v, i) {

                    return (v.Rating == messages.ASMNTSD);
                });
                var sdListCount = sdList.length;


                var eeList = $.grep(Objective.ExceedsExpectations, function (v, i) {

                    return (v.IsEEChecked == true);
                });
                var eeListCount = eeList.length;

                var objectiveRating = '';
                if (allCompetenciesCount == comListCount) {

                    if (eeListCount == 0) {
                        objectiveRating = messages.ASMNTCOM;
                    } else if (eeListCount < 3) {
                        objectiveRating = messages.ASMNTEE;
                    } else if (eeListCount > 2) {
                        objectiveRating = messages.ASMNTSEE;
                    }


                } else {
                    if (sdListCount >= 1 || devListCount > 2) {
                        objectiveRating = messages.ASMNTSD;
                    } else if (devListCount < 3) {
                        objectiveRating = messages.ASMNTDEV;
                    }

                }

                Objective.Rating = objectiveRating;

            });
        }

    }

    function postInsertUpdateAssessment() {

        ipmassessmentBlockUI.start();
        $scope.onUserChangeData();
        calculateScores();
        assignAssessment();


        assessmentServices.postInsertUpdateAssessment($scope.model.vModel.Assessment, function (result) {
            $scope.model.asmntSubmitResult = result.data;
            if ($scope.model.asmntSubmitResult && $scope.model.asmntSubmitResult.AssessmentID && $scope.model.asmntSubmitResult.AssessmentID.toString().trim().length > 0) {
                $scope.model.vModel.Assessment.AssessmentID = $scope.model.asmntSubmitResult.AssessmentID;
                $scope.assessmentID = $scope.model.asmntSubmitResult.AssessmentID;
                updateDocuments();
                var date = $filter('date')(new Date(), "dd-MMM-yyyy HH:mm");

                $scope.isChanged = false;
                //if ($scope.model.uploader.queue.length > 0) {
                //    $scope.model.uploader.uploadAll();
                //}

                if ($scope.isWithoutAssessmentID) {
                    loadDataInitial();
                    $scope.isWithoutAssessmentID = false;
                } else {
                    $scope.saveSuccess = true;

                    if ($scope.model.vModel.Assessment.AutoSave == "Y") {
                        $scope.savedate = messages.ASMNTSAVEDSUCCESSFULLY + ' ' + date;
                        toastr.info(messages.ASMNTSAVEDSUCCESSFULLY + ' ' + date);
                    } else {
                        toastr.info(messages.ASMNTSUBMITSUCCESSFULLY + ' ' + $scope.assesseeStaffNo);
                    }
                }

                if ($scope.model.asmntSubmitResult.AssessmentStatus
                    && ($scope.model.asmntSubmitResult.AssessmentStatus.toUpperCase() == messages.ASMNTCOMPLETE.toUpperCase()
                    || $scope.model.asmntSubmitResult.AssessmentStatus.toUpperCase() == messages.ASMNTELAPSED.toUpperCase())) {
                    $scope.isDisablePage = true;
                    $scope.submitDisable = true;
                    //$scope.onClickBack();

                    $rootScope.goBack($stateParams.back);
                }

            } else {
                $scope.isDisablePage = true;
                $scope.submitDisable = true;
                toastr.error("Saved Failed !")
            }

            ipmassessmentBlockUI.stop();
        }, function (error) {
            $scope.isWithoutAssessmentID = false;
            ipmassessmentBlockUI.stop();
        });
    }

    function checkMinimum50Charators(model, name) {
        if ((!$scope.isNullOrEmpty(model)) && model.toString().length < 49) {

            message = name + ' : ' + messages.ASMNTMIN50CHAR;
            $scope.formValid = false;
            $scope.isBreak = false;
            return false;
        } else {
            return true;
        }

    }

    function checkIDP() {
        $scope.isBreakIDP = false;
        angular.forEach($scope.model.vModel.Assessment.Objectives, function (Objective, key) {
            if (!($scope.isBreakIDP)) {
                angular.forEach(Objective.Competencies, function (Competency, key) {
                    if (!($scope.isBreakIDP)) {



                        var COMList = $.grep(Objective.Competencies, function (v, i) {

                            return v.Rating == messages.ASMNTSD || Competency.Rating === messages.ASMNTDEV;
                        });

                        var count = COMList.length;
                        if (count > 0) {
                            $scope.isBreakIDP = true;
                            return $scope.isBreakIDP;
                        }

                    }

                })
            }
        });

        return $scope.isBreakIDP;
    }

    function getIDPCalculate() {
        $scope.countIDP = 0;
        angular.forEach($scope.model.vModel.Assessment.Objectives, function (Objective, key) {
            $scope.countIDP = $scope.countIDP + Objective.AssmIDPs.length;
        });

        return $scope.countIDP;
    }

    function validateAndAssignData() {

        tab = null;
        message = '';

        if ($scope.isNullOrEmpty($scope.model.vModel.Assessment.AdditionalComments)) {

            $scope.model.vModel.Assessment.InvalidAdditionalComments = true;
            message = 'Please enter overall comment .';
            $scope.formValid = false;
        } else {
            var val = checkMinimum50Charators($scope.model.vModel.Assessment.AdditionalComments, 'overall comment ');

            if (!val) {
                $scope.model.vModel.Assessment.InvalidAdditionalComments = true;
                $scope.formValid = false;
                selectedId = "overallcomment";
            } else {
                $scope.model.vModel.Assessment.InvalidAdditionalComments = false;
            }
        }

        if ($scope.model.vModel.Assessment.Objectives &&
             $scope.model.vModel.Assessment.Objectives != null &&
             $scope.model.vModel.Assessment.Objectives.length > 0) {

            angular.forEach($scope.model.vModel.Assessment.Objectives, function (obj, index) {

                var objkey = $scope.model.vModel.Assessment.Objectives.length - index - 1;
                var Objective = $scope.model.vModel.Assessment.Objectives[objkey];

                // Reset valid condition of comment of objective
                Objective.invalidComment = false;

                // Check validation for competencies
                angular.forEach(Objective.Competencies, function (Competency, key) {

                    // Reset valid condition of comment and rating of each competency 
                    Competency.invalidObs = false;
                    Competency.invalidRating = false;

                    if ($scope.isNullOrEmpty(Competency.Rating)) {
                        Competency.invalidRating = true;
                        message = messages.ASMNTSELRATING + ' :' + Objective.ObjectiveName + ' -' + Competency.CompetencyName;
                        $scope.formValid = false;
                        selectedId = 'sel-' + Competency.CompetencyId;
                        tab = objkey;
                    } else if ((Competency.Rating === messages.ASMNTSD || Competency.Rating === messages.ASMNTDEV) && $scope.isNullOrEmpty(Competency.Comments)) {

                        Competency.invalidObs = true;
                        message = messages.ASMNTOBSREQ + ' :' + Objective.ObjectiveName + ' -' + Competency.CompetencyName;
                        $scope.formValid = false;
                        selectedId = Competency.CompetencyId;
                        tab = objkey;

                    } else {

                        var val = checkMinimum50Charators(Competency.Comments, Objective.ObjectiveName + ' - ' + Competency.CompetencyName);

                        if (!val) {
                            Competency.invalidObs = true;
                            selectedId = Competency.CompetencyId;
                            tab = objkey;
                        } else {
                            Competency.invalidObs = false;
                        }
                    }
                });

                // minimum 3 comments is required for each objectives
                var COMCommentList = $.grep(Objective.Competencies, function (v, i) {
                    return (v.Comments != null && v.Comments.toString().trim().length > 0);
                });

                if (COMCommentList.length < 3) {

                    //Competency.invalidObs = true;
                    message = messages.ASMNTTHREEOBSVAL + ' :' + Objective.ObjectiveName;
                    $scope.formValid = false;
                    selectedId = "profileTabsContents";
                    tab = objkey;
                }

                // check validation for Exceed expectations
                angular.forEach(Objective.ExceedsExpectations, function (ExceedsExpectation, key) {

                    ExceedsExpectation.invalidObs = false;

                    if ($scope.isNullOrEmpty(ExceedsExpectation.Comments) && ExceedsExpectation.IsEEChecked === true) {

                        ExceedsExpectation.invalidObs = true;
                        message = messages.ASMNTEECOMMREQ + ' :' + Objective.ObjectiveName;
                        $scope.formValid = false;
                        selectedId = ExceedsExpectation.CompetencyId;
                        tab = objkey;
                    } else {
                        if ((!$scope.isNullOrEmpty(ExceedsExpectation.Comments)) && ExceedsExpectation.IsEEChecked === true) {
                            var val = checkMinimum50Charators(ExceedsExpectation.Comments, Objective.ObjectiveName + ' - ' + ExceedsExpectation.CompetencyName);
                            //$scope.formValid = val;
                            //$scope.isBreak = val;

                            if (!val) {
                                ExceedsExpectation.invalidObs = true;
                                selectedId = ExceedsExpectation.CompetencyId;
                                tab = objkey;
                            } else {
                                ExceedsExpectation.invalidObs = false;
                            }
                        }
                    }
                });

                if ($scope.isNullOrEmpty(Objective.Comments)) {

                    Objective.invalidComment = true;
                    message = messages.ASMNTOBJCOMMREQ + ' :' + Objective.ObjectiveName;
                    $scope.formValid = false;
                    selectedId = "comment-" + Objective.AssessmentObjectiveID;
                    tab = objkey;
                } else {
                    var val = checkMinimum50Charators(Objective.Comments, Objective.ObjectiveName);

                    if (!val) {
                        Objective.invalidComment = true;
                        selectedId = "comment-" + Objective.AssessmentObjectiveID;
                        tab = objkey;
                    }
                }
            });
        }

        if (message) {
            toastr.warning(message);
        }
        openTab(tab, selectedId);
    }

    function openTab(i, selectedId) {

        if (i != null) {
            $timeout(function () {
                $($('#profileTabsContents .recordAssesTabset ul li.selectTab')[i]).find('span').click();
                tab = null;

                $timeout(function () {
                    if (selectedId) {
                        if (selectedId == "profileTabsContents") {
                            $location.hash(selectedId);
                        } else {
                            $('#profileTabsContents .recordAssesTabset .tab-content').find('#' + selectedId + ' .form-control')[0].focus();
                        }
                    }
                }, 100);
            }, 500);
        }
    }

    function validateIDP() {

        message = "";
        selectedId = "";

        if ($scope.model.vModel.Assessment.Objectives &&
            $scope.model.vModel.Assessment.Objectives != null &&
            $scope.model.vModel.Assessment.Objectives.length > 0) {

            angular.forEach($scope.model.vModel.Assessment.Objectives, function (Objective, key) {
                angular.forEach(Objective.Competencies, function (Competency, key) {
                    angular.forEach(Objective.AssmIDPs, function (AssmIDP, key) {

                        if ($scope.isNullOrEmpty(AssmIDP.Observation)) {

                            message = messages.ASMNTIDPOBSERVATION + '- IDP:' + AssmIDP.CompetencyName;
                            $scope.formValid = false;
                            selectedId = "idp-" + AssmIDP.CompetencyId;
                            AssmIDP.invalidObs = true;
                        } else {
                            var val = checkMinimum50Charators(AssmIDP.Observation, '- IDP Observation :' + AssmIDP.CompetencyName);
                            //$scope.formValid = val;
                            if (!val) {
                                AssmIDP.invalidObs = true;
                                selectedId = "idp-" + AssmIDP.CompetencyId;
                            } else {
                                AssmIDP.invalidObs = false;
                            }
                        }
                        if ($scope.isNullOrEmpty(AssmIDP.ExpectedResult)) {

                            message = messages.ASMNTIDPEXPRESREQ + '- EE:' + AssmIDP.CompetencyName;
                            $scope.formValid = false;
                            AssmIDP.invalidERObs = true;
                            selectedId = "expidp-" + AssmIDP.CompetencyId;
                        } else {
                            var val = checkMinimum50Charators(AssmIDP.ExpectedResult, ' IDP Expected Result:' + AssmIDP.CompetencyName);
                            //$scope.formValid = val;
                            //$scope.isBreak = val;
                            if (!val) {
                                AssmIDP.invalidERObs = true;
                                selectedId = "expidp-" + AssmIDP.CompetencyId;
                            } else {
                                AssmIDP.invalidERObs = false;
                            }
                        }
                    });

                });
            });
        }

        if (message) {
            toastr.warning(message)
        }
    }

    function clearForm() {

        $scope.submitted = false;
        $scope.model.vModel.Assessment.InvalidAdditionalComments = false;

        angular.forEach($scope.model.vModel.Assessment.Objectives, function (Objective, key) {

            angular.forEach(Objective.Competencies, function (Competency, key) {
                Competency.Rating = "";
                Competency.Comments = "";

                Competency.invalidObs = false;
                Competency.invalidRating = false;

            });

            angular.forEach(Objective.ExceedsExpectations, function (ExceedsExpectation, key) {

                ExceedsExpectation.Comments = ""
                ExceedsExpectation.IsEEChecked = false;
                ExceedsExpectation.invalidObs = false;
            });
            Objective.AssmIDPs = [];
            Objective.Comments = ""
            Objective.invalidComment = false;

        });
        $scope.model.vModel.Assessment.AdditionalComments = "";
        $scope.showIDPForm = false;
        $scope.model.uploader.clearQueue();
    }

    function LoadReasonForRecNonAsmt() {
        ipmassessmentBlockUI.start();
        lookupDataService.getLookupList('ReasonForRecNonAsmt', null, function (result) {

            $scope.reasonForRecNonAsmtList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });

            var item = {
                Value: 'Other', Text: 'Other'
            }
            $scope.reasonForRecNonAsmtList.push(item);
            //$scope.reasonForRecNonAsmtList.unshift(initialDrpValues);

            //$scope.model.vModel.Assessment.ReasonForNonSubmission = $scope.reasonForRecNonAsmtList[0];

            ipmassessmentBlockUI.stop();
        }, function (error) {
            ipmassessmentBlockUI.stop();
        });

    };

    function updateDocuments() {

        if ($scope.removedFiles.length > 0) {

            assessmentServices.deleteDocuments($scope.removedFiles, function (success) {

                UploadNewDocument();
                $scope.removedFiles = [];
            }, function (error) {

            });
        } else {
            UploadNewDocument();
        }
    };

    function UploadNewDocument() {
        if ($scope.model.uploader.queue.length > 0) {
            $scope.model.uploader.uploadAll();
        }
    }

    initialize();

}]);
