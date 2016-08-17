'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.kafouenterList.controller', ['$scope', '$state', '$rootScope', 'kafouService', 'assessmentServices', 'crewProfileService', 'flightDetailsAddEditService',
        'sharedDataService', 'blockUI', 'messages', 'ngDialog', 'toastr', '$stateParams','$filter',
        function ($scope, $state, $rootScope, kafouService, assessmentServices, crewProfileService, flightDetailsAddEditService, sharedDataService, blockUI, messages,
            ngDialog, toastr, $stateParams, $filter) {

            var ipmcCRFlightCrewsBlockUI = blockUI.instances.get('ipmcCRFlightCrewsBlockUI');

            $scope.model = {};
            $scope.model.AlreadyRecognisedCrews = [];
            var singleRecogCrew = {};
            $scope.model.FlightDetails = {};
            $scope.showEnterKfTab = false;
            $scope.qrValuetList = [];
            var formatedValue = '';

            $scope.KafouSelCrewsId = null;
            $scope.FlightSelected = null;
            $scope.FlightNumber = null;
            $scope.SelCrewStaffNum = [];
            $scope.isEnterInValid = false;

            var mSend = {};
            mSend.FlightDetails = {};
            mSend.RecognitionType = {};
            mSend.RecognitionSource = {};

            mSend.StaffDetailsList = [];
            mSend.singleStaffDet = {};
            mSend.singleStaffDet.StaffDetails = {};
            mSend.CrewRecognitionDetailsHistory = {};
            mSend.singleStaffDet.RecognitionQRValueList = [];
            mSend.singleStaffDet.singleRecgQRVal = {};
            mSend.RecognitionStatusList = {};
            mSend.RecognisedCrewDetailsModels = {};
            mSend.AttachmentList = {};

            var _statusSourceData = {};
            var _recogSourceId = null;

            $scope.fileType = [];
            $scope.fileTypeObj = [];

            $scope.upload = {};
            $scope.upload.uploader = {};
            $scope.kafouID = null;
            $scope.upload.removedFiles = [];

            $scope.uploadType = 'Kafousave';
            $scope.showGrp = true;

            $scope.isFlightSelected = false;
            $scope.status = {};
            $scope.status.wallfmeOpen = true;
            $scope.status.flightGridopen = true;
            $scope.showKfCrewTab = false;
            $scope.heading = 'Enter Kafou';
            $scope.tabs = [{ active: true }, { active: false }];
            $scope.isKFCmtnsInvalid = false;
            $scope.isKFQRValInValid = false;

            $scope.wallOfFame = [];

            function getMimeType() {
                sharedDataService.getCommonInfo('MIMETYPE', function (result) {
                    angular.forEach(result, function (data) {
                        $scope.fileType.push(data.Text);
                        $scope.fileTypeObj.push(data);
                    });
                }, function (error) {

                });
            };

            $scope.openImage = function (data) {
                sharedDataService.openFile(data, $scope.fileTypeObj);
            }

            $scope.categoryChg = function (val) {

                if (!$scope.isKFAlrdyEntred) {

                    if (val.toUpperCase() == 'INDIVIDUAL') {

                        $scope.model.crgroup = 'INDIVIDUAL';

                        $('input[type="checkbox"]').not(this).prop('checked', false);
                        $scope.SelCrewStaffNum = [];

                    } else if (val.toUpperCase() == 'GROUP') {

                        $scope.model.crgroup = 'GROUP';

                        $scope.SelCrewStaffNum = [];


                        angular.forEach($scope.kafouCrewsgrid.data, function (data) {
                            data.SelectedValue = true;
                            $scope.SelCrewStaffNum.push(data);
                        });
                    }
                }
            }

            $scope.continueEnterKafou = function () {

                if ($scope.SelCrewStaffNum.length > 0) {
                    //$scope.dialogTitle = "Confirmation";
                    //if ($scope.model.crgroup.toUpperCase() === "GROUP") {
                    //    $scope.dialogMessage = messages.KAFOUMSG06 + " for the flight " + $scope.FlightNumber;
                    //} else if ($scope.model.crgroup.toUpperCase() === "INDIVIDUAL") {
                    //    $scope.dialogMessage = messages.KAFOUMSG06 + $scope.SelCrewStaffNum[0].StaffNumber + " on the flight " + $scope.FlightNumber;
                    //}

                    //ngDialog.open({
                    //    scope: $scope,
                    //    preCloseCallback: function (value) {
                    //        if (value == 'Post') {
                    //            //$state.go("kafoulstsState.kafoudetails", { FlightDetsID: $scope.FlightSelected, crewForKF: $scope.SelCrewStaffNum, kfgrp: ($scope.model.crgroup.toUpperCase() == 'GROUP' ? true : false) });
                                
                    //        } else {
                    //        }
                    //    }
                    //});
                    $scope.showEnterKfTab = true;
                    $scope.tabs[1].active = true;
                    
                } else {
                    //$scope.isEnterInValid = true;
                    toastr.error(messages.KAFOUMSG09);
                }
            }

            function checkIfCrewAlreadyRecognzd(crewId){

                for (var i = 0; i < $scope.model.AlreadyRecognisedCrews.length; i++) {
                    
                    if ($scope.model.AlreadyRecognisedCrews[i].FlightId === $scope.FlightSelected && 
                        $scope.model.AlreadyRecognisedCrews[i].RecognizedCrewID === crewId ) {
                        return true;
                    }
                }
                return false;
            }

            function checkIfGroupAlreadyRecognzd() {

                for (var i = 0; i < $scope.model.AlreadyRecognisedCrews.length; i++) {

                    if ($scope.model.AlreadyRecognisedCrews[i].FlightId === $scope.FlightSelected &&
                        $scope.model.AlreadyRecognisedCrews[i].RecognitionType.toUpperCase() === "GROUP") {
                        return true;
                    }
                }
                return false;
            }
            
            $scope.GetFlightCrews = function (rowData) {

                $scope.SelCrewStaffNum = [];

                $scope.tabs[0].active = true;
                $scope.showEnterKfTab = false;
                $scope.model.FlightDetails = {};
                //$scope.model.AlreadyRecognisedCrews = [];
                $scope.model.crIndividualChkd = true;
                $scope.model.crGrpChkd = false;
                $scope.model.crgroup = 'Individual';
                $scope.model.AdditonalCmnts = '';
                $scope.model.KafouCmnts = '';
                $scope.model.qrValuesObj = [];
                $scope.files = [];
                //$scope.upload.uploader.queue.length = 0;
               

                $scope.model.FlightDetails.FlightNumber = rowData.FlightNumber;
                $scope.model.FlightDetails.AirCraftType = $scope.model.FlightDetails.AirCraftFamily = rowData.AirCraftType
                $scope.model.FlightDetails.SectorFrom = rowData.SectorFrom;
                $scope.model.FlightDetails.SectorTo = rowData.SectorTo;
                $scope.model.FlightDetails.ActDeptTime = rowData.ActDeptTime;
                $scope.model.FlightDetails.ActArrTime = rowData.ActArrTime;
                $scope.model.FlightDetails.AirCraftRegNo = rowData.AirCraftRegNo;
                
                $scope.isFlightSelected = true;
                $scope.status.flightGridopen = false;

                $scope.showKfCrewTab = true;
                $scope.FlightSelected = rowData.FlightDetsID;
                $scope.FlightNumber = rowData.FlightNumber;

                if (checkIfGroupAlreadyRecognzd() || 
                    $scope.model.LoggedInGrade.toUpperCase() == 'F1' || $scope.model.LoggedInGrade.toUpperCase() == 'F2') {
                        $scope.showGrp = false;
                } else {
                        $scope.showGrp = true;
                };

                getFlightCrew();

                ipmcCRFlightCrewsBlockUI.start();
                kafouService.getInitialCrewRecog($scope.FlightSelected, function (success) {

                    //$scope.model.allQRValues = success.RecognitionQRValueList;
                    $scope.qrValuetList = success.RecognitionQRValueList;

                    //$scope.CrewsForKF = $stateParams.crewForKF;

                    //angular.forEach(success.StaffDetailsList, function (data) {
                    //    angular.forEach($scope.CrewsForKF, function (mycrew) {
                    //        if (data.StaffDetails.StaffNumber == mycrew) {
                    //            //$scope.CrewStaffDetails = data;
                    //            $scope.kafouCrewsgrid.data.push({
                    //                CrewDetailsId: data.StaffDetails.CrewDetailsId,
                    //                StaffGradeId: data.StaffDetails.StaffGradeId,
                    //                StaffNumber: data.StaffDetails.StaffNumber,
                    //                StaffName: data.StaffDetails.StaffName,
                    //                StaffGrade: data.StaffDetails.StaffGrade,
                    //                WorkPosition: ''
                    //            });
                    //        }
                    //    });
                    //});
                    ipmcCRFlightCrewsBlockUI.stop();

                }, function (error) {
                    ipmcCRFlightCrewsBlockUI.stop();
                });
            };

            function getFlightCrew() {
                ipmcCRFlightCrewsBlockUI.start();
                kafouService.getCrewsForFlight($scope.FlightSelected, function (result) {
                    $scope.SelCrewStaffNum = [];
                    $scope.kafouCrewsgrid = {};
                    angular.forEach(result, function (data) {
                        data.SelectedValue = false;
                    });

                    sortCrewDetails(result);

                    ipmcCRFlightCrewsBlockUI.stop();
                }, function (error) {
                    ipmcCRFlightCrewsBlockUI.stop();
                });
            }

            function sortCrewDetails(data) {

                $scope.model.FlightCrewsDetail = [];
                $scope.CSDmembers = $.grep(data, function (v, i) {

                    return v.StaffGrade == messages.CSD;
                });
                $scope.CSmembers = $.grep(data, function (v, i) {

                    return v.StaffGrade == messages.CS;
                });
                $scope.F1members = $.grep(data, function (v, i) {

                    return v.StaffGrade == messages.F1;
                });
                $scope.F2members = $.grep(data, function (v, i) {

                    return v.StaffGrade == messages.F2;
                });

                $scope.LTmembers = $.grep(data, function (v, i) {

                    return v.StaffGrade == "LT";
                });
                $scope.POmembers = $.grep(data, function (v, i) {

                    return v.StaffGrade == "PO";
                });

                $scope.model.FlightCrewsDetail = $scope.POmembers.concat($scope.LTmembers).concat($scope.CSDmembers).concat($scope.CSmembers).concat($scope.F1members).concat($scope.F2members);

                $scope.kafouCrewsgrid.data = $scope.model.FlightCrewsDetail;

                if ($stateParams.kfrecogid) {


                    setTimeout(function () {
                        angular.forEach($scope.model.StaffDetailsList, function (data) {
                            $scope.SelCrewStaffNum.push(data.StaffDetails);
                        });

                        angular.forEach($scope.kafouCrewsgrid.data, function (data) {
                            data.SelectedValue = true;
                        });
                    }, 500);
                }
            }

            $scope.kafouCrewSelected = function (obj) {

                $scope.isEnterInValid = false;

                if ($scope.model.crgroup.toUpperCase() == 'INDIVIDUAL') {

                    if (obj.StaffNumber !== $scope.model.LoggedInNo) {

                        if (!checkIfCrewAlreadyRecognzd(obj.FlightCrewId)) {

                            angular.forEach($scope.kafouCrewsgrid.data, function (data) {
                                data.SelectedValue = false;
                            });

                            obj.SelectedValue = true;
                            $scope.SelCrewStaffNum = [];

                            if (obj.SelectedValue == true) {
                                $scope.SelCrewStaffNum.push(obj);
                            }

                        } else {

                            obj.SelectedValue = false;
                            toastr.error("Already selected crew");

                        }


                    } else {
                        obj.SelectedValue = false;
                        toastr.error(messages.KAFOUMSG01);
                    }


                } else if ($scope.model.crgroup.toUpperCase() == 'GROUP') {

                    console.log($scope.SelCrewStaffNum.length);

                    if (obj.SelectedValue == true) {
                        $scope.SelCrewStaffNum.push(obj);
                    } else {

                        $scope.SelCrewStaffNum = $.grep($scope.SelCrewStaffNum, function (value) {
                            return value.StaffNumber != obj.StaffNumber;
                        });
                    }

                    if ($scope.SelCrewStaffNum.length < 2) {
                        obj.SelectedValue = true
                        $scope.SelCrewStaffNum.push(obj);
                        toastr.error(messages.KAFOUMSG02);
                    }
                }
            }

            function initialize() {

                getMimeType();


                $scope.kafouFlightspgrid = {
                    gridApi: {},
                    enableFiltering: true,
                    //showGridFooter: true,
                    //enableRowSelection: true,
                    //enableFullRowSelection: true,
                    //paginationPageSizes: [5, 10, 15, 20, 25],
                    //enableSelectAll: true,
                    //paginationPageSize: 10,
                    //enablePagination: true,
                    //enablePaginationControls: true,
                    data: [],
                    subgrid: 'false',
                    //rowHeight: 30,
                    //minRowsToShow: 1,
                    columnDefs: [
                            {
                                field: "FlightNumber", name: "Flight No/Aircraft Type", enableHiding: false,
                                cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.GetFlightCrews(row.entity)">{{row.entity.FlightNumber}} ({{row.entity.AirCraftType}})</a><div>'
                            },
                            { field: "Sector", name: "SECTOR", enableHiding: false, width: "10%" },
                            {
                                field: "ActDeptTime", name: "ATD (UTC)", enableHiding: false, width: "15%", sort: { direction: 'desc' },
                                cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["ActDeptTime"] | date:"dd-MMM-yyyy (HH:mm)" }}</div>'
                            },
                            { field: "DelayTags", width: "15%", name: "Flight Delay", enableHiding: false, },
                            { field: "AirCraftRegNo", name: "Aircraft Reg. No.", enableHiding: false, width: "15%" },
                            { field: "Group", name: "Group", enableHiding: false, width: "8%" },
                            { field: "INDIVIDUALS", name: "Individuals", enableHiding: false, width: "10%" },
                            { field: "STATUS", name: "Status", enableHiding: false, width: "8%" }
                    ]
                };

                $scope.kafouCrewsgrid = {
                    gridApi: {},
                    enableFiltering: true,
                    showGridFooter: true,
                    paginationPageSizes: [5, 10, 15, 20, 25],
                    paginationPageSize: 10,
                    enablePagination: true,
                    enableRowSelection: true,
                    enablePaginationControls: true,
                    data: [],
                    subgrid: 'false',
                    columnDefs: [
                            //{
                            //    field: "FLIGHTDETSID", name: "Select", width: "5%",
                            //    cellTemplate: '<div class="ui-grid-cell-contents"><input type="radio" name="lastTenEVRs" value="row.entity.FLIGHTDETSID"><div>'

                            //},
                            {
                                field: "SelectCrew", name: "", enableHiding: false, width: "5%", enableFiltering: false,  enableSorting: false, enableColumnMenus: false,
                                cellTemplate: '<input type="checkbox" id={{row.entity.FlightCrewId}}  ng-model="row.entity.SelectedValue" ng-change="grid.appScope.kafouCrewSelected(row.entity)" ng-disabled="grid.appScope.isCrewReadOnly(row.entity)" value={{row.entity.FlightCrewId}} />'
                            },
                            {
                                field: "StaffNumber", name: "Staff Number", enableHiding: false, width: "25%"
                            },
                            { field: "StaffName", name: "Staff Name", enableHiding: false, width: "45%" },
                            { field: "StaffGrade", name: "Staff Grade", enableHiding: false, width: "25%" }
                    ]
                };

                $scope.kafouFlightspgrid.multiSelect = false;
                $scope.kafouCrewsgrid.multiSelect = false;

                $scope.kafouFlightspgrid.noUnselect = true;
                $scope.kafouCrewsgrid.noUnselect = true;

                loadData();

            }

            $scope.isCrewReadOnly = function (rowData) {
                return ($scope.iskfReadonly || $scope.isKFAlrdyEntred || (rowData.StaffNumber === $scope.model.LoggedInNo && $scope.model.crgroup.toUpperCase() === "INDIVIDUAL") || (checkIfCrewAlreadyRecognzd(rowData.FlightCrewId) && $scope.model.crgroup.toUpperCase() === "INDIVIDUAL"));
            };

            function loadData() {

                ipmcCRFlightCrewsBlockUI.start();
                sharedDataService.getLoggedInStaffNoAndGrade('', function (result) {
                    ipmcCRFlightCrewsBlockUI.stop();
                    $scope.model.LoggedInGrade = result.split('_$$_')[0];
                    $scope.model.LoggedInNo = result.split('_$$_')[1];;

                    if ($scope.model.LoggedInGrade.toUpperCase() == 'F1' || $scope.model.LoggedInGrade.toUpperCase() == 'F2') {
                        $scope.showGrp = false;
                    } else {
                        $scope.showGrp = true;
                    }


                }, function (error) {

                    ipmcCRFlightCrewsBlockUI.stop();
                });

                ipmcCRFlightCrewsBlockUI.start();
                kafouService.getKFWalloffame('', function (result) {
                    ipmcCRFlightCrewsBlockUI.stop();
                    $scope.wallOfFame = result;

                }, function (error) {

                    ipmcCRFlightCrewsBlockUI.stop();
                });
                

                $scope.isKFAlrdyEntred = false;
                if ($stateParams.kfrecogid) {

                    model_ReInitialize();
                    mSend_ReInitialize();

                    $scope.showKfCrewTab = true;
                    $scope.isKFAlrdyEntred = true;

                    $scope.heading = 'Kafou Details';
                    $scope.EntrdkfType = $stateParams.kfgrp.toUpperCase();

                    ipmcCRFlightCrewsBlockUI.start();
                    kafouService.getKFRecogDetails($stateParams.kfrecogid, function (result) {

                        $scope.model = result;

                        GetSingleFlight();


                        $scope.model.crgroup = $scope.model.RecognitionType.Type;
                        if ($scope.model.crgroup.toUpperCase() == "INDIVIDUAL") {
                            $scope.model.crIndividualChkd = true;
                            $scope.model.crGrpChkd = false;
                        } else {
                            $scope.model.crIndividualChkd = false;
                            $scope.model.crGrpChkd = true;
                        }

                        $scope.files = [];
                        $scope.files = result.AttachmentList;

                        $scope.model.AdditonalCmnts = result.AdditionalComments;
                        $scope.model.KafouCmnts = result.RecognitionComments;
                        $scope.qrValuetList = result.RecognitionQRValueList;

                        $scope.model.qrValuesObj = [];
                        angular.forEach($scope.qrValuetList, function (data) {
                            angular.forEach(result.StaffDetailsList[0].RecognitionQRValueList, function (crwQR) {
                                if (data.Id == crwQR.Id) {
                                    $scope.model.qrValuesObj.push(data);
                                }
                            });
                        });

                        

                        //$scope.model.allQRValues = result.RecognitionQRValueList;
                        //$scope.model.crewQRValues = result.StaffDetailsList[0].RecognitionQRValueList;

                        //$('#' + $scope.model.crewQRValues[0].Id).prop('checked', true);

                        //for (var i = 0, len = $scope.model.allQRValues.length; i < len; i++) {
                        //    for (var j = 0, lgth = $scope.model.crewQRValues.length; j < lgth; j++) {
                        //        if ($scope.model.crewQRValues[j].QRValue == $scope.model.allQRValues[i].QRValue) {
                        //            //$('#' + $scope.model.crewQRValues[j].Id).prop('checked', true);
                        //            $scope.model.allQRValues[i].Id = 'Y';
                        //        }
                        //    }
                        //}

                        $scope.model.StaffDetailsList = result.StaffDetailsList;
                        $scope.FlightSelected = result.FlightDetails.FlightDetsID;

                        if (result.RecognitionType.Type.toUpperCase() === 'INDIVIDUAL') {
                            $scope.showGrp = true;
                        };

                        //getFlightCrew();
                        $scope.SelCrewStaffNum = [];
                        $scope.kafouCrewsgrid = {};
                        $scope.model.entrdKFCrewsInfo = [];

                        angular.forEach($scope.model.StaffDetailsList, function (crwQR) {
                            $scope.model.entrdKFCrewsInfo.push(crwQR.StaffDetails);
                        });

                        sortCrewDetails($scope.model.entrdKFCrewsInfo);

                        ipmcCRFlightCrewsBlockUI.stop();
                    }, function (error) {
                        ipmcCRFlightCrewsBlockUI.stop();
                    });

                    $scope.showEnterKfTab = true;

                    if ($stateParams.kfstatus.toUpperCase() === 'EDIT') {
                        $scope.iskfReadonly = false;
                        
                    } else  {
                        $scope.iskfReadonly = true;
                    }

                   


                } else {

                    $scope.model.crIndividualChkd = true;
                    $scope.model.crGrpChkd = false;

                    $scope.model.crgroup = 'Individual';

                    ipmcCRFlightCrewsBlockUI.start();
                    kafouService.getkafouFlights('', function (result) {

                        if (result.length != 0) {
                            kafouService.getKFByFlight(FormateItems(result), function (success) {

                                var grpCount = 0;
                                var individualCount = 0;

                                for (var i = 0, len = result.length; i < len; i++) {
                                    result[i].Group = "N"
                                    result[i].INDIVIDUALS = "N"
                                    result[i].STATUS = 'Not Entered';

                                    grpCount = 0;
                                    individualCount = 0;
                                    for (var j = 0, lenth = success.length; j < lenth; j++) {
                                        if (result[i].FlightDetsID == success[j].FlightID) {
                                            if (success[j].RecognitionType.toUpperCase() == 'GROUP') {
                                                grpCount += 1;
                                                result[i].Group = "Y (" + grpCount + ")";
                                                result[i].STATUS = 'Entered';

                                            } else if (success[j].RecognitionType.toUpperCase() == 'INDIVIDUAL') {
                                                individualCount += 1;
                                                result[i].INDIVIDUALS = "Y (" + individualCount + ")";
                                                result[i].STATUS = 'Entered';

                                            }
                                        }
                                    }
                                }

                                for (var j = 0, lenth = success.length; j < lenth; j++) {
                                    singleRecogCrew = {
                                        "FlightId": success[j].FlightID,
                                        "RecognizedCrewID": success[j].RecognizedCrewID,
                                        "RecognitionType": success[j].RecognitionType
                                    };
                                    $scope.model.AlreadyRecognisedCrews.push(singleRecogCrew);
                                }

                                $scope.kafouFlightspgrid.data = result;


                                ipmcCRFlightCrewsBlockUI.stop();
                            }, function (error) {
                                ipmcCRFlightCrewsBlockUI.stop();
                            });
                        } else {
                            toastr.info("No flights found for the logged in Crew.");
                        }

                        

                       

                        ipmcCRFlightCrewsBlockUI.stop();
                    }, function (error) {
                        ipmcCRFlightCrewsBlockUI.stop();
                    });
                }

            };

            function FormateItems(item) {
                formatedValue = '';
                angular.forEach(item, function (data) {
                    formatedValue += data.FlightDetsID + ",";
                });
                return formatedValue.substr(0, formatedValue.length - 1);
            };

            function IsWorthToSave() {
                if ($scope.submitted) {


                    if ($scope.model.KafouCmnts.length < 50 && $scope.model.qrValuesObj.length == 0) {
                        $scope.isKFCmtnsInvalid = true;
                        $scope.isKFQRValInValid = true;

                        $('textarea[name="kfcmts"]').focus();
                        toastr.error(messages.KAFOUMSG08);
                        return false;
                    } else if ($scope.model.KafouCmnts.length < 50) {
                        $scope.isKFCmtnsInvalid = true;
                        $scope.isKFQRValInValid = false;
                        toastr.error(messages.KAFOUMSG10);
                        $('textarea[name="kfcmts"]').focus();
                        return false;
                    } else if ($scope.model.qrValuesObj.length == 0) {
                        $scope.isKFCmtnsInvalid = false;
                        $scope.isKFQRValInValid = true;
                        toastr.error(messages.KAFOUMSG11);
                        $('input[type=search]').focus();
                        return false;
                    }

                    return true;
                }
            };

            $scope.SaveKafou = function (form) {
                $scope.submitted = false;
                InsertUpdateKafou();
                
            };

            $scope.SubmitKafou = function (form) {
                $scope.submitted = true;
                // if (form.$valid && IsWorthToSave()) {
                if (IsWorthToSave()) {

                    $scope.dialogTitle = "Confirmation";
                    if ($scope.model.crgroup.toUpperCase() === "GROUP") {
                        $scope.dialogMessage = messages.KAFOUMSG05 + " for the flight " + $scope.model.FlightDetails.FlightNumber;
                    } else if ($scope.model.crgroup.toUpperCase() === "INDIVIDUAL") {
                        $scope.dialogMessage = messages.KAFOUMSG05 + $scope.SelCrewStaffNum[0].StaffNumber + " on the flight " + $scope.model.FlightDetails.FlightNumber;
                    }
                    ngDialog.open({
                        scope: $scope,
                        preCloseCallback: function (value) {
                            if (value == 'Post') {
                                InsertUpdateKafou();
                            }
                        }
                    });
                }
            };

            function updateDocuments() {
                if ($scope.upload.removedFiles.length > 0) {

                    assessmentServices.deleteDocuments($scope.upload.removedFiles, function (success) {

                        UploadNewDocument();
                        $scope.upload.removedFiles = [];
                    }, function (error) {

                    });
                } else {
                    UploadNewDocument();
                }
            };

            function UploadNewDocument() {
                if ($scope.upload.uploader.queue.length > 0) {
                    $scope.upload.uploader.uploadAll();
                }
            };

            $scope.removeDocument = function (file, $index) {

                if ($stateParams.kfstatus.toUpperCase() == 'EDIT') {
                    if ($scope.upload.removedFiles.indexOf(file.FileId) == -1) {
                        $scope.upload.removedFiles.push(file.FileId);
                    }
                    $scope.files.splice($index, 1);
                }
            };

            function GetSingleFlight() {

                ipmcCRFlightCrewsBlockUI.start();
                flightDetailsAddEditService.getSingleFlight($stateParams.FlightDetsID, function (success) {

                    $scope.model.FlightDetails.ActArrTime = success.ActualArrTime;
                    $scope.model.FlightDetails.ActDeptTime = success.ActualDeptTime;

                    //if (success.IsFromAIMS && success.IsManuallyEntered)
                    //    $scope.model.showManualATA = true;
                    //else
                    //    $scope.model.showManualATA = false;

                    ipmcCRFlightCrewsBlockUI.stop();

                }, function (error) {
                    ipmcCRFlightCrewsBlockUI.stop();
                });
            };

            function GetRecognitionSourceID() {
                _recogSourceId = null;
                angular.forEach(_statusSourceData.RecognitionSourceList, function (data) {
                    if (data.Text.toUpperCase() == 'IPRISM') {
                        _recogSourceId = data.Value;
                    }
                });

                return _recogSourceId;
            };

            function InsertUpdateKafou() {

                    ipmcCRFlightCrewsBlockUI.start();
                    kafouService.getkfStatus('', function (success) {

                        _statusSourceData = success;

                        kafouService.getkfRecTypeStatus('', function (success) {

                            mSend.RecognitionType.Type = $scope.model.crgroup;

                            angular.forEach(success.RecognitionTypeList, function (data) {
                                if (data.Text.toUpperCase() == mSend.RecognitionType.Type.toUpperCase()) {
                                    mSend.RecognitionType.Id = data.Value;
                                }
                            });

                            mSend.RecognitionSource.Id = GetRecognitionSourceID();

                            mSend.RaisedByStaffNo = "";

                            mSend.IsActive = "Y";
                            mSend.AdditionalComments = ($scope.model.AdditonalCmnts) ? $scope.model.AdditonalCmnts.trim() : '';
                            mSend.ApvRejComments = "";

                            mSend.RecognitionComments = ($scope.model.KafouCmnts) ? $scope.model.KafouCmnts.trim() : '';

                            mSend.FlightDetails.FlightDetsID = $scope.FlightSelected;

                            mSend.StaffDetailsList = [];
                            var tempStaffDetsList = {};

                            if ($stateParams.kfrecogid) {

                                mSend.RecognitionId = $stateParams.kfrecogid;
                                                               
                                angular.forEach($scope.model.StaffDetailsList, function (data) {

                                    //this has to be declared each and everytime orelse the loop will replace all the already pushed data to the last item match.
                                    tempStaffDetsList = {};
                                    tempStaffDetsList.StaffDetails = {};
                                    tempStaffDetsList.RecognitionQRValueList = [];

                                    //adding RecognitionDetailsId and RecogintionId
                                    tempStaffDetsList.RecognitionDetailsId = data.RecognitionDetailsId;
                                    tempStaffDetsList.RecognitionId = data.RecognitionId;
                                    tempStaffDetsList.StaffDetails = data.StaffDetails;
                                    tempStaffDetsList.RecognitionQRValueList = ($scope.model.qrValuesObj);

                                    mSend.StaffDetailsList.push(tempStaffDetsList);
                                });


                            } else {
                                angular.forEach($scope.SelCrewStaffNum, function (data) {

                                    //this has to be declared each and everytime orelse the loop will replace all the already pushed data to the last item match.
                                    tempStaffDetsList = {};
                                    tempStaffDetsList.StaffDetails = {};
                                    tempStaffDetsList.RecognitionQRValueList = [];

                                    //for the first time, we get CrewDetailsId null and FlightCrewId which is actually CrewDetailsId is available.
                                    data.CrewDetailsId = (data.CrewDetailsId) ? data.CrewDetailsId : data.FlightCrewId;

                                    tempStaffDetsList.StaffDetails = data;
                                    tempStaffDetsList.RecognitionQRValueList = ($scope.model.qrValuesObj);
                                    mSend.StaffDetailsList.push(tempStaffDetsList);

                                });
                            }

                            
                            
                            if ($scope.submitted) {
                                angular.forEach(_statusSourceData.RecognitionStatusList, function (value, key) {
                                    if (value.Text.toUpperCase() == "SUBMITTED")
                                        mSend.RecognitionStatusId = value.Value;
                                });
                            } else {
                                angular.forEach(_statusSourceData.RecognitionStatusList, function (value, key) {
                                    if (value.Text.toUpperCase() == "DRAFT")
                                        mSend.RecognitionStatusId = value.Value;
                                });
                            }
                            
                            kafouService.getkfSave(mSend, function (success) {

                                $scope.kafouID = success.data;
                                $scope.isKFAlrdyEntred = true;
                                updateDocuments();
                                mSend_ReInitialize();

                                if ($scope.submitted == true) {
                                    toastr.info(messages.KAFOUMSG12 + $scope.model.FlightDetails.FlightNumber + ' on ' + ' ' + $filter('date')(new Date(), "dd-MMM-yyyy HH:mm"));
                                } else {
                                    toastr.info(messages.KAFOUMSG04 + $scope.model.FlightDetails.FlightNumber + ' on ' + ' ' + $filter('date')(new Date(), "dd-MMM-yyyy HH:mm"));
                                }
                                
                                //if ($scope.submitted == true) 
                                {
                                    if ($stateParams.kfrecogid) {
                                        $state.go("kfsearch", {}, { reload: true });
                                    } else {
                                        $state.go("kafoulstsState", {}, { reload: true });
                                    }
                                }


                                ipmcCRFlightCrewsBlockUI.stop();
                            }, function (error) {
                                ipmcCRFlightCrewsBlockUI.stop();
                            });

                        }, function (error) {
                            ipmcCRFlightCrewsBlockUI.stop();
                        });

                        ipmcCRFlightCrewsBlockUI.stop();

                    }, function (error) {
                        ipmcCRFlightCrewsBlockUI.stop();
                    });
            };

            $scope.BackToKfSearch = function () {
                if ($stateParams.isFrom == 'myKafou') {
                    $state.go("kfmy");
                } else {
                    $state.go("kfsearch");
                }
                
            }

            function mSend_ReInitialize() {
                mSend = {};
                mSend.FlightDetails = {};
                mSend.RecognitionType = {};
                mSend.RecognitionSource = {};

                mSend.StaffDetailsList = [];
                mSend.singleStaffDet = {};
                mSend.singleStaffDet.StaffDetails = {};
                mSend.CrewRecognitionDetailsHistory = {};
                mSend.singleStaffDet.RecognitionQRValueList = [];
                mSend.singleStaffDet.singleRecgQRVal = {};
                mSend.RecognitionStatusList = {};
                mSend.RecognisedCrewDetailsModels = {};
                mSend.AttachmentList = {};
            };

            function model_ReInitialize() {
                $scope.model = {};
                $scope.qrValuetList = [];
                $scope.model.qrValuesObj = [];
            };

            initialize();

        }]);