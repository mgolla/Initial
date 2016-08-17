'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.housingChangeAcc.controller', ['$rootScope', '$scope', 'lookupDataService', 'housingService', '$state', 'FileUploader', '$stateParams',
        'sharedDataService', 'appSettings', 'ngDialog', 'analyticsService', 'messages', 'toastr', 'blockUI', '$window', '$filter', 'bundleMessage',
        function ($rootScope, $scope, lookupDataService, housingService, $state, FileUploader, $stateParams, sharedDataService, appSettings, ngDialog,
            analyticsService, messages, toastr, blockUI, $window, $filter, bundleMessage) {

            // private variable declaration
            var ipmhousingNewReqBlockUI = blockUI.instances.get('ipmhousingNewReqBlockUI');

            /* scope namespace */
            $scope.model = {};
            $scope.model.StayOut = {};
            $scope.model.BuildingDetails = {};

            $scope.disableFlat = true;
            $scope.disableBedroom = true;
            $scope.disableGuestName = true;

            $scope.viewmodel = {};
            $scope.viewmodel.BuildingList = [];
            $scope.viewmodel.FlatList = [];
            $scope.viewmodel.BedroomList = [];
            $scope.model.BuildingFacilities = '';
            $scope.showFriendStaffId = false;

            //$scope.uploader;
            $scope.requestNumber;

            function pageEvents() {

                /* event for building drop-down change 
                   It checks if Flat contains single value then it auto populates Flat drop-down. */
                $scope.changeBuilding = function (item) {

                    $scope.model.BedroomObj = '';
                    $scope.model.FlatObj = '';
                    $scope.model.LandLineNo = '';
                    $scope.model.Nationality = '';
                    $scope.model.BuildingFacilities = '';

                    if (item) {
                        $scope.disableFlat = false;
                        $scope.viewmodel.FlatList = $filter('filter')($scope.viewmodel.BuildingList, { BuildingDetailSid: item.BuildingDetailSid });

                        if ($scope.viewmodel.FlatList && $scope.viewmodel.FlatList.length == 1) {
                            $scope.model.FlatObj = $scope.viewmodel.FlatList[0];
                            $scope.changeFlat($scope.model.FlatObj);
                        }
                       
                        $scope.model.BuildingFacilities = item.BuildingFacilities;

                    } else {
                        $scope.disableFlat = true;
                        $scope.disableBedroom = true;
                    }

                    if ($scope.model.ReasonObj && $scope.model.ReasonObj.Value == "CHANGEACC_REASON2") {
                        getStaffByFlatId();
                    }
                };

                /* event for flat drop-down change 
                 It checks if Bedroom contains single value then it auto populates Bedroom drop-down. */
                $scope.changeFlat = function (item) {

                    $scope.model.BedroomObj = '';
                    $scope.model.LandLineNo = '';
                    $scope.model.Nationality = '';

                    if (item) {
                        $scope.disableBedroom = false;
                        $scope.model.LandLineNo = item.TelephoneNo;
                        getNationalitiesData(item.FlatId);

                        $scope.viewmodel.BedroomList = $filter('filter')($scope.viewmodel.BuildingList, { FlatId: item.FlatId, BuildingDetailSid: item.BuildingDetailSid });
                        if ($scope.viewmodel.BedroomList && $scope.viewmodel.BedroomList.length == 1) {
                            $scope.model.BedroomObj = $scope.viewmodel.BedroomList[0];
                        }
                    } else {
                        $scope.model.LandLineNo = ''
                        $scope.disableBedroom = true;
                    }

                    if ($scope.model.ReasonObj && $scope.model.ReasonObj.Value == "CHANGEACC_REASON2") {
                        getStaffByFlatId();
                    }
                };

                /* If change reason is "Stay with friend", then show Friend drop-down */
                $scope.changeReason = function (model) {

                    if (model && model.Value == "CHANGEACC_REASON2") {
                        $scope.showFriendStaffId = true;
                        getStaffByFlatId();
                    } else {
                        $scope.showFriendStaffId = false;
                    }
                };

                $scope.submitRequest = function (form) {

                    $scope.submitted = true;

                    if (form.$valid) {
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
                }

                $scope.clearForm = function (form) {

                    $scope.submitted = false;
                    form.$setPristine();
                    form.$setUntouched();

                    $scope.model = {};
                    $scope.model.StayOut = {};
                    $scope.model.BuildingDetails = {};
                    $scope.model.Guests = {};
                    $scope.viewmodel.checked = false;

                    $scope.uploader.clearQueue();
                }
            };

            /* When Building, flat and bedroom all 3 value are selected
               and reason is "Stay with friend", then get friend id to populate in Friend dropdown */
            function getStaffByFlatId() {

                $scope.Friends = [];

                if ($scope.model.BuildingObj &&
                    $scope.model.FlatObj &&
                    $scope.model.BuildingObj.BuildingDetailSid &&
                    $scope.model.FlatObj.FlatId) {

                    var buildingmodel = {
                        BuildingDetailSid: $scope.model.BuildingObj.BuildingDetailSid,
                        FlatId: $scope.model.FlatObj.FlatId
                    }

                    ipmhousingNewReqBlockUI.start();
                    housingService.getStaffsByFlatId(buildingmodel, function (success) {
                        $scope.Friends = success.data;

                        if (success.data && success.data.length == 1) {
                            $scope.model.FriendStaffIdObj = success.data[0];
                        }
                        ipmhousingNewReqBlockUI.stop();

                    }, function (error) {
                        ipmhousingNewReqBlockUI.stop();
                        toastr.error(messages.HOUERROR01);
                    });
                }
            }

            function submitData() {

                ipmhousingNewReqBlockUI.start();

                $scope.model.RequestId = $rootScope.RequestTypeObj ? $rootScope.RequestTypeObj.Value : '';
                $scope.model.RequestType = $rootScope.RequestTypeObj ? $rootScope.RequestTypeObj.Text : '';

                $scope.model.BuildingDetails.BuildingDetailSid = $scope.model.BuildingObj ? $scope.model.BuildingObj.BuildingDetailSid : '';
                $scope.model.BuildingDetails.BuildingName = $scope.model.BuildingObj ? $scope.model.BuildingObj.BuildingName : '';
                $scope.model.BuildingDetails.FlatId = $scope.model.FlatObj ? $scope.model.FlatObj.FlatId : '';
                $scope.model.BuildingDetails.BedroomDetailsId = $scope.model.BedroomObj ? $scope.model.BedroomObj.BedroomDetailsId : '';

                $scope.model.RequestReason = $scope.model.ReasonObj ? $scope.model.ReasonObj.Text : '';
                $scope.model.StayOut.FriendStaffId = $scope.model.FriendStaffIdObj ? $scope.model.FriendStaffIdObj.CrewDetailsId : '';
                housingService.postmoveinrequest($scope.model, function (success) {

                    // $scope.uploadAll(success.data);
                    var data = success.data;
                    $scope.requestNumber = data.RequestNumber;

                    if (data && data.RequestNumber && data.ResponseId) {
                        if ($scope.uploader.queue.length > 0) {
                            $scope.requestId = data.ResponseId;
                            $scope.uploader.uploadAll();
                        } else {
                            ipmhousingNewReqBlockUI.stop();
                            toastr.info(bundleMessage.getMessages([messages.HOUCHANGEACC01, messages.HOUCREATESUCCESS.replace('@reqno', data.RequestNumber)]));
                            //toastr.info(messages.HOUCREATESUCCESS.replace('@reqno', data.RequestNumber));
                            $state.go('housing');
                        }
                    } else {
                        ipmhousingNewReqBlockUI.stop();
                        var msg = data.Message ? (messages[data.Message] ? messages[data.Message] : data.Message) : messages.HOUERROR01;
                        toastr.error(msg);
                    }
                    //HOUCHANGEACC01 - need to show this info when successfully created.
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            };

            function getVacantBuilding() {

                ipmhousingNewReqBlockUI.start();
                housingService.getHousingVacantBuilding(null, function (success) {
                    $scope.viewmodel.BuildingList = success;
                    ipmhousingNewReqBlockUI.stop();
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            }

            function getChangeAccReason() {
                // Gets reason list for move in type
                sharedDataService.getCommonInfo('HR_ChangeAccomReason', function (result) {
                    $scope.ReasonList = result;
                }, function (error) {

                });
            };

            function getCrewEntitlement() {

                housingService.crewentitlement(null, function (success) {
                    $scope.CrewEntitlementDetails = success;
                    ipmhousingNewReqBlockUI.stop();
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            }

            function getNationalitiesData(flatId) {

                housingService.getNationalityByFlat(flatId, function (success) {
                    $scope.model.Nationality = success.Nationality;
                }, function (error) {
                    console.log(error);
                });
            };
           
            function initialize() {

                housingService.getHousingEntitlements($rootScope.RequestTypeObj.Text, function (data) {

                    $scope.housingEntitlements = data;
                    if (data.IsCrewEntitled) {
                        pageEvents();
                        getVacantBuilding();
                        getChangeAccReason();
                    } else {
                        toastr.info(data.Message);
                    }
                    getCrewEntitlement();

                    ipmhousingNewReqBlockUI.stop();
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            };

            ipmhousingNewReqBlockUI.start();

            initialize();
        }]);