
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.evrmain.controller', ['$scope', '$state', '$rootScope', 'lookupDataService', 'flightDetailsAddEditService', '$stateParams', '$filter',
        'messages', 'toastr', 'FileUploader', 'sharedDataService', 'evrSearchService', 'ngDialog', 'blockUI', '$interval', '$window', 'housingService',
        function ($scope, $state, $rootScope, lookupDataService, flightDetailsAddEditService, $stateParams, $filter,
            messages, toastr, FileUploader, sharedDataService, evrSearchService, ngDialog, blockUI, $interval, $window, housingService) {

            var ipmEVRTablsBlockUI = blockUI.instances.get('ipmEVRMainTablsBlockUI');

            //var initialDrpValues = { 'Value': '', 'Text': '--Select Report About--' };
            //var initialCategoryValues = { 'Value': '', 'Text': '--Select Category--' };
            //var initialSubCategoryValues = { 'Value': '', 'Text': '--Select Sub Category--' };
            $scope.evrupdateparams = {};
            $scope.VRAllDetails = {};
            $scope.vrInsertUpdate = {};

            $scope.ReportAboutList = [];
            $scope.CategoryList = [];
            $scope.SubCategoryList = [];

            $scope.inputsDept = [];
            $scope.invalidAttachment = false;
            $scope.fileType = [];
            $scope.fileTypeObj = [];
            var allPassengerData = [];

            $scope.model = {};
            $scope.model.othDeptList = [];
            var passengerData = {};
            var count = 0;
            var psngrDupCount = 0;

            $scope.Owner;
            $scope.NonOwner;

            $scope.ccSelectedList = [];

            $scope.ccFCVal = false;

            $scope.ccBCVal = false;
            $scope.ccECVal = false;
            $scope.ccAllVal = false;
            $scope.ccNAVal = false;

            //$scope.CSR = false;
            //$scope.OHS = false;
            //$scope.PMrPO = false;
            //$scope.CRITICAL = false;
            //$scope.Restricted = false;

            $scope.IsInfoVr = "N";
            $scope.IsNotIfNotReq = "N";

            $scope.IsNew;

            $scope.vrInsertUpdate.ccSelectedObj = [];
            $scope.vrInsertUpdate.othDeptObj = [];
            $scope.crewDetails = {};
            $scope.uploadType = 'EVRSave';
            $scope.model.evrUploader = {};

            $scope.VrId = '';
            $scope.VrNo = '';
            $scope.VrInstanceId = '';
            $scope.InsertType = '';
            //var drftcabinArr = [];

            var index;
            //var multiIndex = [];
            var drftVRCrewId = [];
            $scope.saveSuccess = false;
            $scope.savedate = new Date();
            $scope.flightDetails;
            $scope.draftDetails = {};
            $scope.submitted = false;

            $scope.removedFiles = [];

            $scope.disblFirstClass = true;
            $scope.disblBusinessClass = true;
            $scope.disblEconomyClass = true;
            $scope.disblAllClass = false;

            $scope.shwFirstClass = false;
            $scope.shwBusinessClass = false;
            $scope.shwEconomyClass = false;


            $scope.draftDetails.IsFirstClass = 'N';
            $scope.draftDetails.IsBusinessClass = 'N';
            $scope.draftDetails.IsEconomyClass = 'N';
            $scope.draftDetails.IsccNA = 'N';
            $scope.draftDetails.IsccAll = 'N';



            function loadCabinClass() {

                if ($scope.flightDetails.FlightDelay.SeatCapacityFC > 0) {
                    //$scope.ccSelectedList.push('First Class');
                    $scope.disblFirstClass = false;
                    $scope.shwFirstClass = true;
                }

                if ($scope.flightDetails.FlightDelay.SeatCapacityJC > 0) {
                    //$scope.ccSelectedList.push('Business Class');
                    $scope.disblBusinessClass = false;
                    $scope.shwBusinessClass = true;
                }

                if ($scope.flightDetails.FlightDelay.SeatCapacityYC > 0) {
                    //$scope.ccSelectedList.push('Economy Class');
                    $scope.disblEconomyClass = false;
                    $scope.shwEconomyClass = true;
                }

                prePopulateCabinClass();
            };

            $scope.isccFirstChk = function () {
                if ($scope.draftDetails.IsFirstClass == 'N') {
                    $('#chkAllId').prop('checked', false);
                    $scope.draftDetails.IsccAll = 'N';
                };
            };

            $scope.isccBusinessChk = function () {
                if ($scope.draftDetails.IsBusinessClass == 'N') {
                    $('#chkAllId').prop('checked', false);
                    $scope.draftDetails.IsccAll = 'N';
                };
            };

            $scope.isccEconomyChk = function () {
                if ($scope.draftDetails.IsEconomyClass == 'N') {
                    $('#chkAllId').prop('checked', false);
                    $scope.draftDetails.IsccAll = 'N';
                };
            };

            $scope.isccNAChk = function () {

                if ($scope.draftDetails.IsccNA == 'Y') {
                    //UI
                    $('#chkFirstId').prop('checked', false);
                    $('#chkBusinessId').prop('checked', false);
                    $('#chkEcomyId').prop('checked', false);
                    $('#chkNAId').prop('checked', true);
                    $('#chkAllId').prop('checked', false);
                    //diable check boxes
                    $scope.disblFirstClass = true;
                    $scope.disblBusinessClass = true;
                    $scope.disblEconomyClass = true;
                    $scope.disblAllClass = true;
                    //VALUES
                    $scope.draftDetails.IsFirstClass = 'N';
                    $scope.draftDetails.IsBusinessClass = 'N';
                    $scope.draftDetails.IsEconomyClass = 'N';
                    $scope.draftDetails.IsccNA = 'Y';
                    $scope.draftDetails.IsccAll = 'N';


                } else if ($scope.draftDetails.IsccNA == 'N') {

                    $('#chkFirstId').prop('checked', false);
                    $('#chkBusinessId').prop('checked', false);
                    $('#chkEcomyId').prop('checked', false);
                    $('#chkNAId').prop('checked', false);
                    $('#chkAllId').prop('checked', false);
                    //diable check boxes
                    $scope.disblFirstClass = false;
                    $scope.disblBusinessClass = false;
                    $scope.disblEconomyClass = false;
                    $scope.disblAllClass = false;

                    $scope.draftDetails.IsFirstClass = 'N';
                    $scope.draftDetails.IsBusinessClass = 'N';
                    $scope.draftDetails.IsEconomyClass = 'N';
                    $scope.draftDetails.IsccNA = 'N';
                    $scope.draftDetails.IsccAll = 'N';
                }

            };

            $scope.isccAllChk = function () {

                if ($scope.draftDetails.IsccAll == 'Y') {
                    $('#chkFirstId').prop('checked', true);
                    $('#chkBusinessId').prop('checked', true);
                    $('#chkEcomyId').prop('checked', true);
                    $('#chkNAId').prop('checked', false);
                    $('#chkAllId').prop('checked', true);

                    $scope.draftDetails.IsFirstClass = $scope.shwFirstClass ? 'Y' : 'N';
                    $scope.draftDetails.IsBusinessClass = $scope.shwBusinessClass ? 'Y' : 'N';
                    $scope.draftDetails.IsEconomyClass = $scope.shwEconomyClass ? 'Y' : 'N';
                    $scope.draftDetails.IsccNA = 'N';
                    $scope.draftDetails.IsccAll = 'Y';

                } else if ($scope.draftDetails.IsccAll == 'N') {
                    $('#chkFirstId').prop('checked', false);
                    $('#chkBusinessId').prop('checked', false);
                    $('#chkEcomyId').prop('checked', false);
                    $('#chkNAId').prop('checked', false);
                    $('#chkAllId').prop('checked', false);

                    $scope.draftDetails.IsFirstClass = 'N';
                    $scope.draftDetails.IsBusinessClass = 'N';
                    $scope.draftDetails.IsEconomyClass = 'N';
                    $scope.draftDetails.IsccNA = 'N';
                    $scope.draftDetails.IsccAll = 'N';
                }

            };

            function prePopulateCabinClass() {

                //if ($scope.flightDetails && $scope.draftDetails) {

                //    if ($scope.draftDetails.IsCabInClassFC == 'Y') {
                //        $scope.vrInsertUpdate.ccSelectedObj.push($scope.ccSelectedList[$scope.ccSelectedList.indexOf('First Class')]);
                //    }

                //    if ($scope.draftDetails.IsCabInClassJC == 'Y') {
                //        $scope.vrInsertUpdate.ccSelectedObj.push($scope.ccSelectedList[$scope.ccSelectedList.indexOf('Business Class')]);
                //    }

                //    if ($scope.draftDetails.IsCabInClassYC == 'Y') {
                //        $scope.vrInsertUpdate.ccSelectedObj.push($scope.ccSelectedList[$scope.ccSelectedList.indexOf('Economy Class')]);
                //    }
                //}

                if ($scope.flightDetails && $scope.draftDetails) {

                    if ($scope.draftDetails.IsCabInClassFC == 'Y') {
                        $scope.draftDetails.IsFirstClass = 'Y';
                    } else if ($scope.draftDetails.IsCabInClassFC == 'N' || $scope.draftDetails.IsCabInClassFC == '') {
                        $scope.draftDetails.IsFirstClass = 'N';
                    }

                    if ($scope.draftDetails.IsCabInClassJC == 'Y') {
                        $scope.draftDetails.IsBusinessClass = 'Y';
                    } else if ($scope.draftDetails.IsCabInClassJC == 'N' || $scope.draftDetails.IsCabInClassJC == '') {
                        $scope.draftDetails.IsBusinessClass = 'N';
                    }

                    if ($scope.draftDetails.IsCabInClassYC == 'Y') {
                        $scope.draftDetails.IsEconomyClass = 'Y';
                    } else if ($scope.draftDetails.IsCabInClassYC == 'N' || $scope.draftDetails.IsCabInClassYC == '') {
                        $scope.draftDetails.IsEconomyClass = 'N';
                    }

                    if ($scope.draftDetails.IsCabInClassNA == 'Y') {
                        $scope.draftDetails.IsccNA = 'Y';
                    } else if ($scope.draftDetails.IsCabInClassNA == 'N' || $scope.draftDetails.IsCabInClassNA == '') {
                        $scope.draftDetails.IsccNA = 'N';
                    }

                    //make IsccAll always no in this method a its not coming from DB.
                    $scope.draftDetails.IsccAll = 'N';
                }
            };

            function loadData() {

                lookupDataService.getLookupList('ReportAbout', null, function (result) {

                    $scope.ReportAboutList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text, Type: obj.Type } });
                    // $scope.ReportAboutList.unshift(initialDrpValues);

                    //$scope.evrAllFilter.Status = $scope.ReportAboutList[0];

                    $scope.grpBydeptNames = function (item) {
                        return item.Type;
                    };
                });

                //if ($stateParams.evrdrfid === null)
                {
                    flightDetailsAddEditService.getSingleFlight($scope.evrTabModel.evrReqFeilds.flightDetsID, function (success) {
                        $scope.flightDetails = success;

                        $scope.grid.data = success.FlightCrewsDetail;

                        angular.forEach($scope.grid.data, function (data) {
                            data.isCrewChkd = false;
                        });

                        loadCabinClass();
                        ipmEVRTablsBlockUI.stop();

                    }, function (error) {
                        ipmEVRTablsBlockUI.stop();
                    });
                }
            };

            function selectEvents() {

                $scope.getCategory = function (model) {

                    if (!model) {
                        //clear all other data's
                        $scope.vrInsertUpdate.categoryObj = '';
                        $scope.vrInsertUpdate.subCategryObj = '';
                    }
                    else {

                        $scope.vrInsertUpdate.categoryObj = '';
                        $scope.vrInsertUpdate.subCategryObj = '';

                        $scope.inputsDept[0] = model.Value;

                        var LookupSearchModel = { LookupTypeCode: 'CategoryForReportAbout', SearchText: model.Value, StaffNo: '' };

                        lookupDataService.getLookupbyFilter(LookupSearchModel, function (result) {

                            $scope.CategoryList = result.data.map(function (obj) { return { Value: obj.Value, Text: obj.Text, Type: obj.Type } });

                            angular.forEach($scope.CategoryList, function (data) {
                                if (data.Value == model.CategoryId) {
                                    $scope.vrInsertUpdate.categoryObj = data;
                                }
                            });

                            $scope.grpByCategory = function (item) {
                                return item.Type;
                            };
                        });
                    }
                };

                $scope.getSubCategory = function (model) {

                    if (!model) {
                        //clear all other data's
                        $scope.vrInsertUpdate.subCategryObj = '';
                    }
                    else {

                        $scope.vrInsertUpdate.subCategryObj = '';
                        $scope.inputsDept[1] = model.Value;

                        var LookupSearchModel = { LookupTypeCode: 'SubCategoryForReportAbout', SearchText: model.Value, StaffNo: '' };

                        lookupDataService.getLookupbyFilter(LookupSearchModel, function (result) {

                            $scope.SubCategoryList = result.data.map(function (obj) { return { Value: obj.Value, Text: obj.Text, Type: obj.Type } });
                            angular.forEach($scope.SubCategoryList, function (data) {
                                if (data.Value == model.SubCategoryId) {
                                    $scope.vrInsertUpdate.subCategryObj = data;
                                }
                            });

                            $scope.grpBysubCategry = function (item) {
                                return item.Type;
                            };
                        });
                    }
                };

                $scope.getDeptNOwnerDets = function (model) {

                    if (!model) {
                        //clear all other data's
                    }
                    else {

                        $scope.inputsDept[2] = model.Value;

                        var LookupSearchModel = { LookupTypeCode: 'DeptForReportAbout', ArrSearchText: $scope.inputsDept, StaffNo: '' };

                        lookupDataService.getLookupbyFilter(LookupSearchModel, function (result) {

                            $scope.model.othDeptList = result.data.map(function (obj) { return { Value: obj.Value, Text: obj.Text, Type: obj.Type } });

                            if ($scope.evrTabModel.evrReqFeilds.evrdrfid) {

                                angular.forEach($scope.model.othDeptList, function (data) {
                                    angular.forEach(model.allDepts, function (drftdept) {
                                        if (data.Value == drftdept.DeptId) {
                                            $scope.vrInsertUpdate.othDeptObj.push(data);
                                        }
                                    });
                                });
                            }

                        });

                        var LookupSearchModel = { LookupTypeCode: 'eVROwnerNonOwner', ArrSearchText: $scope.inputsDept, StaffNo: '' };

                        lookupDataService.getLookupbyFilter(LookupSearchModel, function (result) {
                            $scope.Owner = result.data[0].Value;
                            $scope.NonOwner = result.data[0].Text;
                        });
                    }
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

            function pageEvents() {

                //if ($scope.VrId == '') {
                //    $scope.IsNew = "Y";
                //}
                //else {
                //    $scope.IsNew = "N";
                //}

                $scope.vrInsertUpdate.NODId = "";

                $scope.model.FFNo = '';
                $scope.model.SeatNo = '';
                $scope.model.PsngrName = '';

                $scope.Owner = '';
                $scope.NonOwner = '';

                $scope.VrCrewsId = null;

                getMimeType();
            };

            function loadGrid() {
                $scope.grid = {
                    gridApi: {},
                    enableFiltering: true,
                    enableRowSelection: true,
                    enableFullRowSelection: true,
                    showGridFooter: true,
                    paginationPageSizes: [5, 10, 15, 20, 25],
                    paginationPageSize: 10,
                    enablePagination: true,
                    enablePaginationControls: true,
                    data: [],
                    subgrid: 'false',
                    columnDefs: [
                          { field: "StaffNumber", name: "Staff Number", width: "25%", enableHiding: false },
                          { field: "StaffName", name: "Staff Name", width: "45%", enableHiding: false },
                          { field: "StaffGrade", name: "Staff Grade", width: "20%", sort: { direction: 'asc' }, enableHiding: false },
                          {
                              field: "isCrewChkd", name: "Incident With", width: "10%", enableHiding: false,
                              cellTemplate: '<input type="checkbox" ng-model="row.entity.isCrewChkd" id={{row.entity.FlightCrewId}} value={{row.entity.FlightCrewId}} ng-change="grid.appScope.fnEvrChkClick(row.entity)"/>'
                          }
                    ]
                };

                $scope.passengergrid = {
                    gridApi: {},
                    data: [],
                    subgrid: 'false',
                    columnDefs: [
                          { field: "FFNo", name: "FFP Number", width: "25%", enableHiding: false },
                          { field: "SeatNo", name: "Seat Number", width: "20%", enableHiding: false },
                          { field: "PsngrName", name: "Passenger Name", width: "45%", enableHiding: false },
                          {
                              field: "remove", name: "Remove", width: "10%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents text-center"><a href="javascript:void(0);" ng-click="grid.appScope.deleteRow(row)"><i class="glyphicon glyphicon-trash redText"></i></a></div>',
                          }
                    ]
                };
            };

            function CheckDuplicatePassenger(passengerVals) {

                psngrDupCount = 0;

                angular.forEach(allPassengerData, function (value, key) {
                    if ((value.FFNo.trim() == passengerVals.FFNo.trim() &&
                        value.PsngrName.trim() == passengerVals.PsngrName.trim()) ||
                        value.SeatNo.trim() == passengerVals.SeatNo.trim()) {

                        psngrDupCount = psngrDupCount + 1;
                    }
                });

                return psngrDupCount;
            };

            function submitData() {
                $scope.SetInsertStatus();
                $scope.vrInsertUpdate.Status = "Submit";

                if (IsWorthToSave()) {
                    $scope.LoadVRAfterSave();
                } else {
                    toastr.error(messages.ALLMANDATORYENTER);
                    return false;
                }

            }

            function RouteAfterSubmit() {
                toastr.info("eVR# " + $scope.VrNo + " Submitted successfully");

                //After submit as the document upload will be happening so making 
                //VrId null will not be a right thing as its used.
                //$scope.VrId = null;
                //$scope.VrNo = null;
                //$scope.VrInstanceId = null;
                $scope.evrTabModel.evrReqFeilds.evrdrfid = null;

                $state.go("evrlstsState", {}, { reload: true });
            }

            function postEVRSave() {
                ipmEVRTablsBlockUI.start();

                evrSearchService.postEVRSave($scope.VRAllDetails, function (success) {
                    ipmEVRTablsBlockUI.stop();
                    if (success.data !== null) {
                        if (success.data.VrNo !== 0) {
                            if ($scope.InsertType.toUpperCase() == "INSERT") {
                                $scope.VrId = success.data.VrId;
                                $scope.VrNo = success.data.VrNo;
                                $scope.VrInstanceId = success.data.VrInstanceId;

                                updateDocuments();

                                if ($scope.vrInsertUpdate.Status.toUpperCase() == "DRAFT") {
                                    $state.go("evrlstsState", {}, { reload: true });
                                } else if ($scope.vrInsertUpdate.Status.toUpperCase() == "SUBMIT") {
                                    RouteAfterSubmit();
                                }
                            } else if ($scope.InsertType.toUpperCase() == "UPDATE") {

                                updateDocuments();

                                if ($scope.vrInsertUpdate.Status.toUpperCase() == "DRAFT") {
                                    //for future ref
                                    $state.go("evrlstsState", {}, { reload: true });
                                } else if ($scope.vrInsertUpdate.Status.toUpperCase() == "SUBMIT") {
                                    RouteAfterSubmit();
                                }
                            }

                            if ($scope.vrInsertUpdate.Status.toUpperCase() != "SUBMIT") {
                                $scope.saveSuccess = true;
                                $scope.savedate = new Date();
                                toastr.info(messages.ASMNTSAVEDSUCCESSFULLY + ' ' + $filter('date')(new Date(), "dd-MMM-yyyy HH:mm"))
                            }

                        } else {
                            ipmEVRTablsBlockUI.stop();
                            toastr.error(messages.VRNOTSUBMITTED);
                        }
                    } else {
                        ipmEVRTablsBlockUI.stop();
                        toastr.error(messages.DATANOTSUBMITTED);
                    }
                }, function (error) {
                    ipmEVRTablsBlockUI.stop();

                });
            }

            function ButtonEvents() {

                $scope.AddPassenger = function () {

                    if (!($scope.model.SeatNo.trim() === 'undefined' || $scope.model.SeatNo.trim() === '') &&
                        !($scope.model.PsngrName.trim() === 'undefined' || $scope.model.PsngrName.trim() === '')) {

                        if (CheckDuplicatePassenger($scope.model) == 0) {

                            count = parseInt(allPassengerData.length) + 1;

                            passengerData = { FFNo: $scope.model.FFNo.trim(), SeatNo: $scope.model.SeatNo.trim(), PsngrName: $scope.model.PsngrName.trim() };
                            allPassengerData[count] = passengerData


                            $scope.passengergrid.data = allPassengerData;

                            $scope.model.FFNo = '';
                            $scope.model.SeatNo = '';
                            $scope.model.PsngrName = '';
                            passengerData = '';
                        } else {
                            toastr.error(messages.DUPLICATERECORD);
                        }
                    }
                    else {
                        toastr.error(messages.EVRPASSENGERSEATEMPTY);
                    }
                };

                $scope.isSelectedChk = function (chkReprtAs) {

                    //if (chkReprtAs == 'CRITICAL') {

                    $scope.dialogTitle = "Critical Confirmation";
                    $scope.dialogMessage = messages.CRITICALEVRSUBMITTED;
                    if ($scope.draftDetails.IsCritical == 'Y') {
                        ngDialog.open({
                            scope: $scope,
                            preCloseCallback: function (value) {
                                if (value == 'Post') {
                                    $scope.draftDetails.IsCritical = 'Y';
                                    //$("#criticalId").prop("checked", true);
                                } else {
                                    $scope.draftDetails.IsCritical = 'N';
                                    // $("#criticalId").prop("checked", false);
                                }
                            }
                        });
                    } else {
                        $scope.draftDetails.IsCritical = 'N';
                        //$("#criticalId").prop("checked", false);
                    }
                    // }
                }

                $scope.LoadVRAfterSave = function () {

                    $scope.vrInsertUpdate.FlightDetId = $scope.evrTabModel.evrReqFeilds.flightDetsID;
                    $scope.vrInsertUpdate.ReportAbtId = $scope.vrInsertUpdate.RprtAbtObj.Value;
                    $scope.vrInsertUpdate.CategoryId = $scope.vrInsertUpdate.categoryObj.Value;
                    $scope.vrInsertUpdate.SubCategoryId = $scope.vrInsertUpdate.subCategryObj.Value;
                    $scope.vrInsertUpdate.IsCritical = $scope.draftDetails.IsCritical ? $scope.draftDetails.IsCritical : "N";
                    $scope.vrInsertUpdate.IsVrRestricted = $scope.draftDetails.IsVrRestricted ? $scope.draftDetails.IsVrRestricted : "N";
                    $scope.vrInsertUpdate.IsInfoVr = "N";
                    $scope.vrInsertUpdate.IsNotIfNotReq = "N";
                    $scope.vrInsertUpdate.IsCSR = $scope.draftDetails.IsCSR ? $scope.draftDetails.IsCSR : "N";
                    $scope.vrInsertUpdate.IsOHS = $scope.draftDetails.IsOHS ? $scope.draftDetails.IsOHS : "N";
                    $scope.vrInsertUpdate.IsPO = $scope.draftDetails.IsPO ? $scope.draftDetails.IsPO : "N";

                    //$scope.vrInsertUpdate.IsCabinFirstClass = "N";
                    //$scope.vrInsertUpdate.IsCabinBusinessClass = "N";
                    //$scope.vrInsertUpdate.IsCabinEconomyClass = "N";

                    //if (!($scope.vrInsertUpdate.ccSelectedObj.length == 0)) {
                    //    $scope.vrInsertUpdate.IsCabinClassNA = "N";

                    //    angular.forEach($scope.vrInsertUpdate.ccSelectedObj, function (value, key) {
                    //        if (value == "First Class") {
                    //            $scope.vrInsertUpdate.IsCabinFirstClass = "Y";
                    //        } else if (value == "Business Class") {
                    //            $scope.vrInsertUpdate.IsCabinBusinessClass = "Y";
                    //        } else if (value == "Economy Class") {
                    //            $scope.vrInsertUpdate.IsCabinEconomyClass = "Y";
                    //        }
                    //    });
                    //} else {
                    //    $scope.vrInsertUpdate.IsCabinClassNA = "Y";
                    //}


                    $scope.vrInsertUpdate.IsCabinFirstClass = ($scope.draftDetails.IsFirstClass) ? $scope.draftDetails.IsFirstClass : 'N';
                    $scope.vrInsertUpdate.IsCabinBusinessClass = ($scope.draftDetails.IsBusinessClass) ? $scope.draftDetails.IsBusinessClass : 'N';
                    $scope.vrInsertUpdate.IsCabinEconomyClass = ($scope.draftDetails.IsEconomyClass) ? $scope.draftDetails.IsEconomyClass : 'N';
                    $scope.vrInsertUpdate.IsCabinClassNA = ($scope.draftDetails.IsccNA) ? $scope.draftDetails.IsccNA : 'N';


                    $scope.vrInsertUpdate.IsRuleSetChanged = "N";
                    //$scope.vrInsertUpdate.IsRuleSetChanged = "Y";
                    $scope.vrInsertUpdate.NODId = '';

                    if (!($scope.vrInsertUpdate.othDeptObj.length == 0)) {

                        $scope.vrInsertUpdate.IsRuleSetChanged = "Y";

                        angular.forEach($scope.vrInsertUpdate.othDeptObj, function (value, key) {
                            if (($scope.vrInsertUpdate.NODId == '')) {
                                $scope.vrInsertUpdate.NODId = value.Value + "|";
                            } else {
                                $scope.vrInsertUpdate.NODId += value.Value + "|";
                            }
                        });

                        $scope.vrInsertUpdate.NODId = $scope.vrInsertUpdate.NODId.substr(0, $scope.vrInsertUpdate.NODId.length - 1);

                        //if ($scope.vrInsertUpdate.NODId !== null &&
                        //    $scope.vrInsertUpdate.NODId.split("|").length > 0) {
                        //    $scope.vrInsertUpdate.NODId = $scope.vrInsertUpdate.NODId.substring(1, $scope.vrInsertUpdate.NODId.length - 1);
                        //}
                    }

                    $scope.vrInsertUpdate.vrFactComment = $scope.vrInsertUpdate.FactsComment;
                    $scope.vrInsertUpdate.vrActionComment = $scope.vrInsertUpdate.ActionsComment;
                    $scope.vrInsertUpdate.vrResultComment = $scope.vrInsertUpdate.ResultComment;
                    $scope.vrInsertUpdate.IsAdmin = "N";
                    $scope.vrInsertUpdate.IsNew = ($scope.IsNew) ? $scope.IsNew : 'Y';

                    $scope.vrInsertUpdate.InsertType = $scope.InsertType;
                    $scope.vrInsertUpdate.VrId = $scope.VrId;
                    $scope.vrInsertUpdate.VrNo = $scope.VrNo;
                    //$scope.vrInsertUpdate.VrInstanceId = $scope.VrInstanceId; 
                    $scope.vrInsertUpdate.VrInstanceId = ($scope.evrTabModel.evrReqFeilds.evrInstanceId) ? $scope.evrTabModel.evrReqFeilds.evrInstanceId : '';

                    ////NOTE : Not required for Crew
                    //$scope.vrInsertUpdate.StaffNumber = "-1";

                    //if ($scope.VrCrewsId != null && $scope.VrCrewsId != '')
                    //    $scope.vrInsertUpdate.VrCrewsId = $scope.VrCrewsId.substring(1, $scope.VrCrewsId.length - 1);
                    if ($scope.VrCrewsId) {
                        $scope.vrInsertUpdate.VrCrewsId = $scope.VrCrewsId.substr(0, $scope.VrCrewsId.length - 1);
                    }

                    //$scope.VRAllDetails.VRPassengers = $scope.passengergrid.data; 
                    $scope.VRAllDetails.VRPassengers = allPassengerData;
                    $scope.VRAllDetails.vrInsertUpdate = $scope.vrInsertUpdate;

                    postEVRSave();

                };

                $scope.SetInsertStatus = function () {
                    if ($scope.VrId !== '') {
                        $scope.InsertType = "Update";
                    }
                    else {
                        $scope.InsertType = "Insert";
                    }
                };

                $scope.SaveVR = function () {

                    $scope.SetInsertStatus();

                    $scope.vrInsertUpdate.Status = "Draft";

                    if (IsWorthToSave()) {
                        $scope.LoadVRAfterSave();
                    } else {
                        toastr.error(messages.ALLMANDATORYENTER);
                    }
                };

                $scope.SubmitVR = function (form) {

                    $scope.submitted = true;
                    if ($scope.draftDetails.IsFirstClass == 'N' &&
                               $scope.draftDetails.IsBusinessClass == 'N' &&
                               $scope.draftDetails.IsEconomyClass == 'N' &&
                               $scope.draftDetails.IsccNA == 'N' &&
                               $scope.draftDetails.IsccAll == 'N') {

                        toastr.error(messages.ALLMANDATORYENTER)
                        return;
                    }

                    if (form.$valid) {

                        $scope.dialogTitle = "Confirmation";
                        $scope.dialogMessage = messages.HOUCONFIRMNEW;
                        $scope.dialogMessage = messages.HOUCONFIRMNEW;
                        //if ($scope.vrInsertUpdate.ccSelectedObj.length == 0) {
                        //    $scope.dialogMessage = '';
                        //    $scope.dialogMessage = "Cabin Class is selected as 'Not Applicable'." + messages.HOUCONFIRMNEW;
                        //} else {
                        //    $scope.dialogMessage = messages.HOUCONFIRMNEW;
                        //}
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
                }

                $scope.deleteRow = function (row) {
                    index = $scope.passengergrid.data.indexOf(row.entity);
                    $scope.passengergrid.data.splice(index, 1);
                };

                $scope.fnEvrChkClick = function (obj) {
                    //if ($("#" + obj.row.entity.FlightCrewId).is(":checked"))
                    //    $scope.VrCrewsId = $scope.VrCrewsId == null ? obj.row.entity.FlightCrewId + "|" : $scope.VrCrewsId + obj.row.entity.FlightCrewId + "|";
                    //else
                    //    $scope.VrCrewsId = $scope.VrCrewsId.replace(obj.row.entity.FlightCrewId + "|", "");

                    
                    if (obj.isCrewChkd)
                        $scope.VrCrewsId = $scope.VrCrewsId == null ? obj.FlightCrewId + "|" : $scope.VrCrewsId + obj.FlightCrewId + "|";
                    else
                        $scope.VrCrewsId = $scope.VrCrewsId.replace(obj.FlightCrewId + "|", "");
                }

                $scope.CheckIncidentCrews = function () {
                    if ($scope.VrCrewsId) {

                        angular.forEach($scope.VrCrewsId.split("|"), function (CrewDetailsId) {
                            angular.forEach($scope.grid.data, function (data) {
                                if (data.FlightCrewId == CrewDetailsId)
                                    data.isCrewChkd = true;
                            });
                            //$("#" + CrewDetailsId).prop("checked", true);
                        });
                    }
                };
            }

            function IsWorthToSave() {

                $scope.vrInsertUpdate.ReportAboutVal = $scope.vrInsertUpdate.RprtAbtObj ? $scope.vrInsertUpdate.RprtAbtObj.Text : '';
                $scope.vrInsertUpdate.CategoryVal = $scope.vrInsertUpdate.categoryObj ? $scope.vrInsertUpdate.categoryObj.Text : '';
                $scope.vrInsertUpdate.SubCategoryVal = $scope.vrInsertUpdate.subCategryObj ? $scope.vrInsertUpdate.subCategryObj.Text : '';

                $scope.vrInsertUpdate.vrFactComment = $scope.vrInsertUpdate.FactsComment ? $scope.vrInsertUpdate.FactsComment : '';
                $scope.vrInsertUpdate.vrActionComment = $scope.vrInsertUpdate.ActionsComment ? $scope.vrInsertUpdate.ActionsComment : '';
                $scope.vrInsertUpdate.vrResultComment = $scope.vrInsertUpdate.ResultComment ? $scope.vrInsertUpdate.ResultComment : '';

                //$scope.vrInsertUpdate.ccSelected = $scope.vrInsertUpdate.ccSelectedObj ? $scope.vrInsertUpdate.ccSelectedObj : '';

                if (!($scope.vrInsertUpdate.ReportAboutVal == "" || $scope.vrInsertUpdate.CategoryVal == "" || $scope.vrInsertUpdate.SubCategoryVal == "")) {
                    return true;
                } else {
                    return false;
                }
            };

            function autoSave() {
                var promise = $interval(function () {
                    $scope.SaveVR();
                }, 120000);


                $scope.$on('$destroy', function () {
                    $scope.stop();
                });

                // stops the interval
                $scope.stop = function () {
                    $interval.cancel(promise);
                };
            }

            //Controller Scope Initialization
            function initialize() {
                pageEvents();
                selectEvents();
                ButtonEvents();
                loadGrid();
                loadData();
                if ($state.current.name == 'evrlstsState.evrtabs') {
                    //autoSave();
                }

                if ($scope.evrTabModel.evrReqFeilds.evrdrfid) {
                    evrSearchService.getEVRDraftUpdate($scope.evrTabModel.evrReqFeilds.evrdrfid, function (result) {

                        $scope.draftDetails = result;

                        $scope.VrId = $scope.evrTabModel.evrReqFeilds.evrdrfid;
                        $scope.VrNo = $scope.evrTabModel.evrReqFeilds.evrno;

                        $scope.vrInsertUpdate.FactsComment = result.Facts;
                        $scope.vrInsertUpdate.ActionsComment = result.Action;
                        $scope.vrInsertUpdate.ResultComment = result.Result;

                        angular.forEach($scope.ReportAboutList, function (data) {
                            if (data.Value == result.ReportAboutID) {
                                $scope.vrInsertUpdate.RprtAbtObj = data;
                            }
                        });

                        // Load all categories
                        var reportAbt = { Value: result.ReportAboutID, CategoryId: result.CategoryId };
                        $scope.getCategory(reportAbt);

                        var categoryVal = { Value: result.CategoryId, SubCategoryId: result.SubCategoryId };
                        $scope.getSubCategory(categoryVal);

                        $scope.inputsDept[0] = result.ReportAboutID;
                        $scope.inputsDept[1] = result.CategoryId;

                        var deptInputs = { Value: result.SubCategoryId, allDepts: result.VRDeptEnterVR };

                        $scope.getDeptNOwnerDets(deptInputs);
                        prePopulateCabinClass();

                        $scope.passengergrid.data = result.VRPassengerEnterVR;

                        $scope.VrCrewsId = '';
                        angular.forEach(result.VRCrewEnterVR, function (crewDet) {
                            $scope.VrCrewsId += crewDet.CrewDetailsId + "|";
                        });

                        allPassengerData = result.VRPassengerEnterVR;
                        $scope.IsNew = result.IsNew;//"N";
                        //$rootScope.isFrmtabActive = true;


                        $scope.files = result.VRDocEnterVR;

                        ipmEVRTablsBlockUI.stop();
                    });
                }
            }

            $scope.openImage = function (data) {
                data.FileName = data.DocumentName;
                data.FileContent = data.DocumentContent;
                sharedDataService.openFile(data, $scope.fileTypeObj);
            }


            function updateDocuments() {
                if ($scope.removedFiles.length > 0) {

                    evrSearchService.deleteDocuments($scope.removedFiles, function (success) {

                        UploadNewDocument();
                        $scope.removedFiles = [];
                    }, function (error) {

                    });
                } else {
                    UploadNewDocument();
                }
            };

            function UploadNewDocument() {
                if ($scope.model.evrUploader.queue.length > 0) {
                    $scope.model.evrUploader.uploadAll();
                }


            }

            $scope.onEvrDocSaveCmplte = function () {

                $scope.files = [];
                $scope.model.evrUploader.queue.length = 0;

                ipmEVRTablsBlockUI.start();
                evrSearchService.getEVRDraftUpdate($scope.VrId, function (result) {
                    $scope.files = result.VRDocEnterVR;
                    ipmEVRTablsBlockUI.stop();
                }, function (error) {
                    ipmEVRTablsBlockUI.stop();

                });
            };

            $scope.removeDocument = function (file, $index) {

                if ($scope.removedFiles.indexOf(file.DocumentId) == -1) {
                    $scope.removedFiles.push(file.DocumentId);
                }

                $scope.files.splice($index, 1);
            };

            ipmEVRTablsBlockUI.start();

            initialize();

        }]);