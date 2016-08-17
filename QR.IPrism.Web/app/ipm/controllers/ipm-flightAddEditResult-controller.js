/*********************************************************************
* File Name     : ipm-flightDetailsAddEdit-controller.js
* Description   : Controller for flight request.
* Create Date   : 25th Jan 2016
* Modified Date : 25th Jan 2016
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.flightAddEditResult.controller', ['$scope', '$state', '$stateParams', '$rootScope', 'flightDetailsAddEditService', 'evrSearchService', 'lookupDataService', 'messages', 'blockUI', 'toastr', 'ngDialog',
        function ($scope, $state, $stateParams, $rootScope, flightDetailsAddEditService, evrSearchService, lookupDataService, messages, blockUI, toastr, ngDialog) {

            var ipmFlightBlockUI = blockUI.instances.get('ipmFlightBlockUI');

            $scope.model = {};
            $scope.copyData = {};
            $scope.submitted = false;

            $scope.lstSelect = [{ Value: 'NA', Text: 'NA' },
                { Value: 'Y', Text: 'YES' },
                { Value: 'N', Text: 'NO' }];

            $scope.AnnLang = [];
            $scope.cwpList = [];
            $scope.AircraftList = [];
            $scope.disabledcsd = true;
            $scope.disabledgrooming = true;
            //$scope.submitted = true;

            $scope.back = $stateParams.IsFromEvr == 'false' ? 'addEditFlight' : 'evrlstsState';

            function initialize() {

                $scope.IsFromEvr = $stateParams.IsFromEvr;

                SetFerryData();
                LookupValues();
                //CrewCompliment();

                $scope.csdBriefedCheck = function (model) {
                    if (model != $scope.lstSelect[2]) {
                        $scope.disabledcsd = true;
                        $scope.model.FlightDelay.CsdCsBriefedComment = '';
                    } else {
                        $scope.disabledcsd = false;
                    }
                };

                $scope.groomingCheck = function (model) {
                    if (model != $scope.lstSelect[2]) {
                        $scope.disabledgrooming = true;
                        $scope.model.FlightDelay.GroomingCheckComment = '';
                    } else {
                        $scope.disabledgrooming = false;
                    }
                };

                $scope.checked = function () {
                    SetFerryData();
                };

                $scope.submit = function (form) {

                    $scope.submitted = true;
                    if (form.$valid) {

                        if (!$scope.model.FlightCrewsDetail && $scope.model.FlightCrewsDetail.length == 0) {

                            toastr.error(messages.FLGDT03);
                        }
                        else if (!$scope.model.IsFerry && ($scope.model.FlightDelay.PassengerLoadFC == "NA" &&
                            $scope.model.FlightDelay.PassengerLoadJC == "NA" &&
                            $scope.model.FlightDelay.PassengerLoadYC == "NA")) {

                            toastr.error(messages.FLGDT04);
                        }
                            //else if (!$scope.model.IsFerry && ($scope.model.FlightDelay.InfantLoadFC == "NA" &&
                            //    $scope.model.FlightDelay.InfantLoadJC == "NA" &&
                            //    $scope.model.FlightDelay.InfantLoadYC == "NA")) {

                            //    toastr.error(messages.FLGDT05);
                            //}
                        else if (($scope.model.groomingCheckObj.Value.toUpperCase() == 'N' && $scope.model.FlightDelay.GroomingCheckComment.trim() == '') ||
                                    ($scope.model.csdBriefedObj.Value.toUpperCase() == 'N' && $scope.model.FlightDelay.CsdCsBriefedComment.trim() == '')) {

                            toastr.error("Enter the Grooming Check OR CSD/CS comments");
                        } else if (
                            (parseInt($scope.model.FlightDelay.SeatCapacityFC) <
                            parseInt($scope.model.FlightDelay.PassengerLoadFC)) ||

                            (parseInt($scope.model.FlightDelay.SeatCapacityJC) <
                            parseInt($scope.model.FlightDelay.PassengerLoadJC)) ||

                            (parseInt($scope.model.FlightDelay.SeatCapacityYC) <
                            parseInt($scope.model.FlightDelay.PassengerLoadYC))) {

                            toastr.error("Passenger Load cannot be greater that Seat Capacity!");
                        }
                        else {
                            $scope.dialogTitle = "Confirmation";
                            $scope.dialogMessage = messages.HOUCONFIRMNEW;
                            ngDialog.open({
                                scope: $scope,
                                preCloseCallback: function (value) {
                                    if (value == 'Post') {
                                        submitData();
                                        //analyticsService.trackEvent('Action', 'Add', 'View', 'Create Housing Request');
                                    }
                                }
                            });
                        }
                    } else {
                        toastr.error("All mandatory fields are to be entered!");
                    }
                };

                $scope.addCrewMember = function () {

                    if ($scope.model.selectStaffObj) {
                        var recordExist = false
                        angular.forEach($scope.model.FlightCrewsDetail, function (data) {
                            if (data.StaffNumber == $scope.model.selectStaffObj.StaffNumber) {
                                toastr.info(messages.FLGDT06);
                                recordExist = true;
                            }
                        });

                        if (!recordExist) {
                            angular.forEach($scope.cwpList, function (cwp) {
                                if (cwp.Text == "NA") {
                                    $scope.model.selectStaffObj.cwpObj = cwp;
                                }
                            });

                            angular.forEach($scope.AnnLang, function (ann) {
                                if (ann.Text == "NA") {
                                    $scope.model.selectStaffObj.alObj = ann;
                                }
                            });

                            $scope.model.FlightCrewsDetail.push($scope.model.selectStaffObj);
                            $scope.model.selectStaffObj = '';
                        }
                    } else {
                        toastr.info(messages.FLGDT07);
                    }
                };

                $scope.refreshStaff = function (data) {

                    flightDetailsAddEditService.getAutoSuggestStaffInformation({ name: "FlightCrew", CrewSearchCriteria: data }, function (success) {

                        $scope.autoList = success.data;
                    }, function (error) {

                    });
                };

                $scope.clearIfNA = function (data) {
                    
                    if (data == 'PassengerLoadFC') {
                        $scope.model.FlightDelay.PassengerLoadFC = ($scope.model.FlightDelay.PassengerLoadFC !== 'NA') ? $scope.model.FlightDelay.PassengerLoadFC : '';
                    } else if (data == 'PassengerLoadJC') {
                        $scope.model.FlightDelay.PassengerLoadJC = ($scope.model.FlightDelay.PassengerLoadJC !== 'NA') ? $scope.model.FlightDelay.PassengerLoadJC : '';
                    } else if (data == 'PassengerLoadYC') {
                        $scope.model.FlightDelay.PassengerLoadYC = ($scope.model.FlightDelay.PassengerLoadYC !== 'NA') ? $scope.model.FlightDelay.PassengerLoadYC : '';
                    } else if (data == 'InfantLoadFC') {
                        $scope.model.FlightDelay.InfantLoadFC = ($scope.model.FlightDelay.InfantLoadFC !== 'NA') ? $scope.model.FlightDelay.InfantLoadFC : '';
                    } else if (data == 'InfantLoadJC') {
                        $scope.model.FlightDelay.InfantLoadJC = ($scope.model.FlightDelay.InfantLoadJC !== 'NA') ? $scope.model.FlightDelay.InfantLoadJC : '';
                    } else if (data == 'InfantLoadYC') {
                        $scope.model.FlightDelay.InfantLoadYC = ($scope.model.FlightDelay.InfantLoadYC !== 'NA') ? $scope.model.FlightDelay.InfantLoadYC : '';
                    } else {
                        //for future
                    }
                    
                };

                $scope.deleteStaff = function (data) {
                    $scope.dialogTitle = "Confirmation";
                    $scope.dialogMessage = messages.EVRDRAFTDELETE;
                    ngDialog.open({
                        scope: $scope,
                        preCloseCallback: function (value) {
                            if (value == 'Post') {
                                deleteCrew(data);
                            }
                        }
                    });
                };

                $scope.clearForm = function (form) {
                    $scope.submitted = false;
                    form.$setPristine();
                    form.$setUntouched();

                    $scope.model = angular.copy($scope.copyData);
                    ResetInputs();
                    SetValueInLanguage();
                    SetValueInCwp();
                };

                $scope.pasteFrom = function () {

                    ipmFlightBlockUI.start();
                    flightDetailsAddEditService.getCrewsForFlight($scope.pasteFromFlightDetsID, function (success) {

                        sortCrewDetails(success.Result);
                        ipmFlightBlockUI.stop();
                    }, function (error) {
                        ipmFlightBlockUI.stop();
                    });
                };

                ResetInputs();
            };

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

                angular.forEach($scope.model.FlightCrewsDetail, function (data) {
                    angular.forEach($scope.AnnLang, function (ann) {
                        if (ann.Text == data.AnnounceLangVal) {
                            data.alObj = ann;
                        }
                    });
                });

                angular.forEach($scope.model.FlightCrewsDetail, function (data) {
                    angular.forEach($scope.cwpList, function (cwp) {
                        if ((cwp.Text == data.CabinCrewPositionVal) || (!data.CabinCrewPositionVal && cwp.Text == "NA")) {
                            data.cwpObj = cwp;
                        }
                    });
                });
            }

            function GetSingleFlight() {

                ipmFlightBlockUI.start();
                flightDetailsAddEditService.getSingleFlight($stateParams.FlightDetsID, function (success) {

                    $scope.model = success;
                    $scope.model.FlightDelay.DepartureDelayObj = parseInt($scope.model.FlightDelay.DepartureDelay) > -1 ? $scope.model.FlightDelay.DepartureDelay : "00:00";
                    $scope.model.FlightDelay.ArrivalDelayObj = parseInt($scope.model.FlightDelay.ArrivalDelay) > -1 ? $scope.model.FlightDelay.ArrivalDelay : "00:00";

                    $scope.copyData = angular.copy($scope.model);
                    sortCrewDetails(success.FlightCrewsDetail);
                    initialize();

                    ipmFlightBlockUI.stop();

                }, function (error) {
                    ipmFlightBlockUI.stop();
                });
            };

            function ResetInputs() {

                $scope.model.groomingCheckObj = $scope.lstSelect[0];
                $scope.model.csdBriefedObj = $scope.lstSelect[0];

                angular.forEach($scope.lstSelect, function (data) {

                    if (data.Value == $scope.model.FlightDelay.IsGroomingCheck.trim()) {
                        $scope.model.groomingCheckObj = data;
                        $scope.groomingCheck($scope.model.groomingCheckObj);
                    }
                    if (data.Value == $scope.model.FlightDelay.IsCsdCsBriefed.trim()) {
                        $scope.model.csdBriefedObj = data;
                        $scope.csdBriefedCheck($scope.model.csdBriefedObj);
                    }
                });
            };

            function SetValueInLanguage() {
                angular.forEach($scope.model.FlightCrewsDetail, function (data) {
                    angular.forEach($scope.AnnLang, function (ann) {
                        if (ann.Text == data.AnnounceLang) {
                            data.alObj = ann;
                        }
                    });
                });
            }

            function SetValueInCwp() {
                angular.forEach($scope.model.FlightCrewsDetail, function (data) {
                    angular.forEach($scope.cwpList, function (cwp) {
                        if (cwp.Text == data.CabinCrewPosition) {
                            data.cwpObj = cwp;
                        }
                    });
                });
            }

            function SetFerryData() {
                if ($scope.model.IsFerry) {
                    $scope.model.FlightDelay.PassengerLoadFC = "NA";
                    $scope.model.FlightDelay.PassengerLoadJC = "NA";
                    $scope.model.FlightDelay.PassengerLoadYC = "NA";

                    $scope.model.FlightDelay.InfantLoadFC = "NA";
                    $scope.model.FlightDelay.InfantLoadJC = "NA";
                    $scope.model.FlightDelay.InfantLoadYC = "NA";
                }
            }

            function deleteCrew(data) {

                var index = $scope.model.FlightCrewsDetail.indexOf(data);
                if (index > -1) {
                    $scope.model.FlightCrewsDetail.splice(index, 1);
                }
            };

            function submitData() {

                toastr.clear();

                $scope.model.FlightDelay.IsGroomingCheck = $scope.model.groomingCheckObj.Value;
                $scope.model.FlightDelay.IsCsdCsBriefed = $scope.model.csdBriefedObj.Value;

                var crew = '';
                angular.forEach($scope.model.FlightCrewsDetail, function (data) {

                    data.CabinCrewPosition = data.cwpObj.Value;
                    data.AnnounceLang = data.alObj.Value;

                    crew = crew + data.StaffNumber + ',';
                    $scope.model.CrewDetail = crew.substr(0, crew.length - 1);
                });

                ipmFlightBlockUI.start();
                flightDetailsAddEditService.insertFlightDetails($scope.model, function (success) {

                    ipmFlightBlockUI.stop();
                    if (success.data && !success.data.IsSuccess && success.data.Message) {
                        toastr.error(success.data.Message);
                    } else {
                        toastr.info(messages.FLGDT08);

                        if ($stateParams.IsFromEvr == 'fd') {
                            $state.go('evrflightDelayCause', { FlightDetsID: $stateParams.FlightDetsID, IsFromEvr: true });
                        } else if ($stateParams.IsFromEvr == 'evr') {
                            $state.go('evrlstsState.evrtabs', { FlightDetsID: $stateParams.FlightDetsID });
                        } else if ($stateParams.IsFromEvr == 'no_evr') {
                            evrSearchService.UpdateNOVR($stateParams.FlightDetsID, function (result) {
                                $state.go("evrlstsState", {}, { reload: true });
                            }, function (error) {
                                toastr.error("Unhandled Exception while performing NOVR. Try again later.");
                            });
                        }
                        else {
                            $state.go('addEditFlight');
                        }
                    }
                }, function (error) {
                    ipmFlightBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            }

            function LookupValues() {
                flightDetailsAddEditService.getAllLookUpDetails('Aircraft Type', function (success) {
                    $scope.AircraftList = success;
                }, function (error) {
                });

                flightDetailsAddEditService.getAllLookUpDetails('PA_Languages', function (success) {

                    $scope.AnnLang = success;
                    SetValueInLanguage();
                }, function (error) {

                });

                flightDetailsAddEditService.getAllLookUpWithParentDetails($scope.model.AirCraftType, function (success) {
                    $scope.cwpList = success;
                    SetValueInCwp();
                }, function (error) {

                });

                flightDetailsAddEditService.getFlightDetailForPaste($scope.model.FlightDetsID, function (success) {

                    if (success && success.length > 0) {
                        $scope.pasteFromSection = true;
                        $scope.pasteFromDetailsData = "Last Operated Flight : " + success[0].FlightNumber + " (" + success[0].SectorFrom + " - " + success[0].SectorTo + ")";
                        $scope.pasteFromFlightDetsID = success[0].FlightDetsID;
                    }
                }, function (error) {

                });
            };

            $scope.CS_CSD_Filter = function (item) {
                return item.StaffGrade.toUpperCase() === 'CS' || item.StaffGrade.toUpperCase() === 'CSD';
            };

            GetSingleFlight();

        }]);
