'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.housingSwapRoom.controller', ['$rootScope', '$scope', 'lookupDataService', 'housingService', '$state', 'FileUploader', '$stateParams',
        'sharedDataService', 'appSettings', 'ngDialog', 'bundleMessage', 'analyticsService', 'messages', 'toastr', 'blockUI', '$window', '$filter',
        function ($rootScope, $scope, lookupDataService, housingService, $state, FileUploader, $stateParams, sharedDataService, appSettings, ngDialog, bundleMessage,
            analyticsService, messages, toastr, blockUI, $window, $filter) {

            // private variable declaration
            var ipmhousingNewReqBlockUI = blockUI.instances.get('ipmhousingNewReqBlockUI');

            $scope.model = {};
            $scope.model.StayOut = {};
            $scope.model.BuildingDetails = {};
            //$scope.model.Guests = {};
            $scope.uploader;
            $scope.requestNumber;
            $scope.model.BuildingFacilities = '';

            $scope.viewmodel = {};

            function pageEvents() {

                $scope.submitRequest = function (form) {

                    $scope.submitted = true;

                    if ($scope.submitted && !$scope.viewmodel.checked) {
                        toastr.error(messages.HOUERROR02);
                    } else if (form.$valid) {
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

                $scope.changeSwapRoomRequest = function (model) {

                    $scope.model.BuildingObj = '';
                    $scope.model.BedroomList = '';
                    $scope.model.FlatObj = '';

                    $scope.viewmodel.BuildingList = angular.copy($scope.viewmodel.OccupiedBuildingList);
                    var building = angular.copy($scope.viewmodel.OccupiedBuildingList);

                    if (model.Text == "Swap rooms within the same building") {

                        $scope.viewmodel.BuildingList = $filter('filter')(building, { BuildingDetailSid: $scope.existingAccomm.BuildingDetails.BuildingDetailSid });
                        $scope.model.BuildingObj = $scope.viewmodel.BuildingList[0];
                        $scope.viewmodel.FlatList = angular.copy($scope.viewmodel.BuildingList);

                    } else if (model.Text == "Swap rooms within the same flat") {

                        $scope.viewmodel.BuildingList = $filter('filter')(building, {
                            BuildingDetailSid: $scope.existingAccomm.BuildingDetails.BuildingDetailSid,
                            FlatId: $scope.existingAccomm.BuildingDetails.FlatId
                        });

                        if ($scope.viewmodel.BuildingList && $scope.viewmodel.BuildingList.length > 0) {

                            $scope.model.BuildingObj = $scope.viewmodel.BuildingList[0];
                            $scope.viewmodel.BedroomList = $scope.viewmodel.FlatList = angular.copy($scope.viewmodel.BuildingList);
                            $scope.model.FlatObj = $scope.viewmodel.FlatList[0];
                            $scope.changeFlatSwap($scope.model.BuildingObj);

                            $scope.model.LandLineNo = $scope.viewmodel.BuildingList[0].TelephoneNo;
                            getNationalitiesData($scope.model.FlatObj.FlatId);
                        }
                    }
                };

                $scope.changeBuildingSwap = function (item) {

                    $scope.model.BedroomObj = '';
                    $scope.model.FlatObj = '';
                    $scope.model.LandLineNo = '';
                    $scope.model.Nationality = '';
                    $scope.model.BuildingFacilities = '';

                    if (item) {
                        $scope.viewmodel.FlatList = $filter('filter')($scope.viewmodel.BuildingList, { BuildingDetailSid: item.BuildingDetailSid });

                        if ($scope.viewmodel.FlatList && $scope.viewmodel.FlatList.length == 1) {
                            $scope.model.FlatObj = $scope.viewmodel.FlatList[0];
                        }
                        $scope.model.BuildingFacilities = item.BuildingFacilities;
                    }
                };

                $scope.changeFlatSwap = function (item) {

                    $scope.model.BedroomObj = '';
                    $scope.model.LandLineNo = '';
                    $scope.model.Nationality = '';

                    if (item) {
                        $scope.model.LandLineNo = item.TelephoneNo;
                        getNationalitiesData(item.FlatId);

                        $scope.viewmodel.BedroomList = $filter('filter')($scope.viewmodel.BuildingList, { FlatId: item.FlatId, BuildingDetailSid: item.BuildingDetailSid });
                        if ($scope.viewmodel.BedroomList && $scope.viewmodel.BedroomList.length == 1) {
                            $scope.model.BedroomObj = $scope.viewmodel.BedroomList[0];
                            $scope.changeBedroomSwap(item);
                        }
                    } else {
                        $scope.disableBedroom = true;
                    }
                };

                $scope.changeBedroomSwap = function (item) {
                    getStaffForSwap(item);
                };

                $scope.clearForm = function (form) {

                    $scope.submitted = false;
                    form.$setPristine();
                    //form.$setValidity();
                    form.$setUntouched();

                    $scope.viewmodel.CategoryObj = '';
                    $scope.model = {};
                    $scope.model.StayOut = {};
                    $scope.model.BuildingDetails = {};
                    $scope.model.Guests = {};

                    $scope.uploader.clearQueue();
                }
            };

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
                $scope.model.StayOut.FriendStaffNo = $scope.model.FriendStaffIdObj ? $scope.model.FriendStaffIdObj.StaffNumber : '';
                $scope.model.StayOut.SwapRoomCategoryId = $scope.viewmodel.CategoryObj ? $scope.viewmodel.CategoryObj.Value : '';

                housingService.postswaproom($scope.model, function (success) {

                    //$scope.uploadAll(success.data);
                    var data = success.data;
                    if (data && data.RequestNumber && data.ResponseId) {

                        $scope.requestNumber = data.RequestNumber;

                        if ($scope.uploader.queue.length > 0) {
                            $scope.requestId = data.ResponseId;
                            $scope.uploader.uploadAll();
                        } else {
                            ipmhousingNewReqBlockUI.stop();
                            toastr.info(bundleMessage.getMessages([messages.HOUCHANGEACC01, messages.HOUCREATESUCCESS.replace('@reqno', data.RequestNumber)]));
                            $state.go('housing');
                        }
                    } else {
                        ipmhousingNewReqBlockUI.stop();
                        var msg = data.Message ? (messages[data.Message] ? messages[data.Message] : data.Message) : messages.HOUERROR01;
                        toastr.error(msg);
                    }

                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            };

            function getSwapRoomsRequestType() {

                ipmhousingNewReqBlockUI.start();
                housingService.getSwapRoomsRequestType(null, function (success) {
                    ipmhousingNewReqBlockUI.stop();
                    $scope.SwapRoomsRequest = success;

                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            };

            function getNationalitiesData(flatId) {

                ipmhousingNewReqBlockUI.start();
                housingService.getNationalityByFlat(flatId, function (success) {
                    $scope.model.Nationality = success.Nationality;
                    ipmhousingNewReqBlockUI.stop();
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            };

            function getStaffForSwap(buildingObj) {

                $scope.Friends = [];
                $scope.model.FriendStaffIdObj = '';

                if (buildingObj) {

                    var buildingmodel = {
                        BuildingDetailSid: buildingObj.BuildingDetailSid,
                        FlatId: buildingObj.FlatId,
                        BedroomDetailsId: buildingObj.BedroomDetailsId
                    }

                    ipmhousingNewReqBlockUI.start();
                    housingService.getStaffsForSwap(buildingmodel, function (success) {
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

            function getCrewEntitlement() {

                housingService.crewentitlement(null, function (success) {
                    $scope.CrewEntitlementDetails = success;
                    ipmhousingNewReqBlockUI.stop();
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            }

            function getOccupiedBuilding() {

                ipmhousingNewReqBlockUI.start();

                housingService.getOccupiedBuilding(null, function (success) {
                    $scope.viewmodel.OccupiedBuildingList = success;
                    $scope.viewmodel.BuildingList = angular.copy($scope.viewmodel.OccupiedBuildingList);

                    ipmhousingNewReqBlockUI.stop();
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            }

            function getSwapRoomReason() {
                // Gets reason list for move in type
                sharedDataService.getCommonInfo('HR_ChangeAccomReason', function (result) {
                    $scope.ReasonList = result;
                }, function (error) {

                });
            };
            
            function initialize() {

                housingService.getHousingEntitlements($scope.messages.HOU1019, function (data) {

                    $scope.housingEntitlements = data;

                    if (data.IsCrewEntitled) {

                        pageEvents();
                        getSwapRoomsRequestType();
                        getSwapRoomReason();
                        getOccupiedBuilding();
                    } else {
                        toastr.info(data.Message);
                    }

                    getCrewEntitlement();

                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            };

            ipmhousingNewReqBlockUI.start();
            initialize();
        }]);