
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.housingNewRequest2.controller', ['$scope', 'lookupDataService', 'housingService', '$state', 'FileUploader', '$stateParams',
        'sharedDataService', 'appSettings', 'ngDialog', 'analyticsService', 'messages', 'toastr', 'blockUI', '$window', '$filter',
        function ($scope, lookupDataService, housingService, $state, fileUploader, $stateParams, sharedDataService, appSettings, ngDialog,
            analyticsService, messages, toastr, blockUI, $window, $filter) {

            // private variable declaration
            var ipmhousingNewReqBlockUI = blockUI.instances.get('ipmhousingNewReqBlockUI');

            $scope.messages = messages;

            // scope declaration
            $scope.disableFlat = true;
            $scope.disableBedroom = true;
            $scope.disableGuestName = true;

            $scope.viewmodel = {};
            $scope.viewmodel.BuildingList = [];
            $scope.viewmodel.FlatList = [];
            $scope.viewmodel.BedroomList = [];

            $scope.model = {};
            $scope.model.StayOut = {};
            $scope.model.BuildingDetails = {};
            $scope.model.Guests = {};

            $scope.uploader;
            $scope.existingAccomm = {};
            $scope.requestDate = new Date();
            $scope.submitted = false;
            $scope.showCancel = false;
            $scope.fileType = [];
            $scope.showFriendStaffId = false;
            $scope.stayOutRequestType = [];

            $scope.disableCheckout = true;

            $scope.viewmodel.fromMindate = null;
            $scope.viewmodel.fromMaxdate = null;
            $scope.viewmodel.toMindate = null;
            $scope.viewmodel.toMaxdate = null;

            $scope.GuestList = [];
            $scope.rangeError = false;
            $scope.Friends = [];

            function compareDate(todate, fromdate) {
                if (fromdate && todate) {
                    if (fromdate < todate) {
                        $scope.rangeError = true;
                        toastr.error("From date cannot exceed to date");
                    } else {
                        $scope.rangeError = false;
                        $scope.model.Guests.NoOfDays = (new Date(fromdate) - new Date(todate)) / (1000 * 60 * 60 * 24) + 1;
                    }
                }
            }

            function pageEvents() {

                $scope.changeRequestType = function (model) {
                    // Temp for CA testing
                    //if ($scope.RequestTypeObj.Text == "Change Accommodation")
                    validatePage();
                    //else
                    //    loadPartialPage($scope.RequestTypeObj.Text);
                }

                $scope.changeBuilding = function (item) {

                    $scope.model.BedroomObj = ''
                    $scope.model.FlatObj = ''

                    if (item) {
                        $scope.disableFlat = false;
                        $scope.viewmodel.FlatList = $filter('filter')($scope.viewmodel.BuildingList, { BuildingDetailSid: item.BuildingDetailSid });

                        if ($scope.viewmodel.FlatList && $scope.viewmodel.FlatList.length == 1) {
                            $scope.model.FlatObj = $scope.viewmodel.FlatList[0];
                        }
                    } else {
                        $scope.disableFlat = true;
                        $scope.disableBedroom = true;
                    }

                    if ($scope.model.ReasonObj && $scope.model.ReasonObj.Value == "CHANGEACC_REASON2") {
                        getStaffByFlatId();
                    }
                };

                $scope.changeFlat = function (item) {

                    $scope.model.BedroomObj = '';

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

                    if ($scope.model.ReasonObj && $scope.viewmodel.model.Value == "CHANGEACC_REASON2") {
                        getStaffByFlatId();
                    }
                };

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
                    form.$setValidity('rangeValidity', !$scope.rangeError);

                    if (form.$valid) {
                        $scope.dialogTitle = "Confirmation";
                        $scope.dialogMessage = messages.HOUCONFIRMNEW;
                        ngDialog.open({
                            scope: $scope,
                            preCloseCallback: function (value) {
                                if (value == 'Post') {
                                    submitData($scope.viewmodel.RequestTypeObj.Text);
                                    //analyticsService.trackEvent('Action', 'Add', 'View', 'Create Housing Request');
                                }
                            }
                        });
                    }
                }

                $scope.clearForm = function () {

                    $scope.submitted = true;

                    $scope.model = {};
                    $scope.model.StayOut = {};
                    $scope.model.BuildingDetails = {};
                    $scope.model.Guests = {};

                    $scope.uploader.clearQueue();
                }

                $scope.changeRelation = function (model) {

                    if (model) {
                        //$scope.model.LandlineNo = item.TelephoneNo;
                        $scope.disableGuestName = false;
                    } else {
                        $scope.disableGuestName = true;
                        $scope.model.GuestNameObj = '';
                    }
                };

                $scope.changeStayoutReqDetails = function (model) {

                    if (model) {
                        switch (model.Text.toLocaleLowerCase()) {

                            case messages.HOUSWAP03.toLocaleLowerCase():

                                $scope.model.StayOut.StayOutRequestFromDate = '';
                                $scope.model.StayOut.StayOutRequestToDate = '';

                                $scope.disableSpecificDates = true;
                                $scope.disabledFromDate = false;
                                $scope.disabledToDate = true;

                                break;
                            case messages.HOUSWAP04.toLocaleLowerCase():

                                $scope.model.StayOut.StayOutRequestFromDate = '';
                                $scope.model.StayOut.StayOutRequestToDate = '';

                                $scope.disabledFromDate = true;
                                $scope.disabledToDate = true;
                                break;

                            default:
                                $scope.disableSpecificDates = false;
                                $scope.disabledFromDate = false;
                                $scope.disabledToDate = false;
                                break;
                        }
                    }
                };

                $scope.changeStayoutReason = function (model) {

                    if (model && model.Value == "STAYOUT_REASON2") {
                        $scope.attachmentNote = messages.HOUSWAP07;
                    } else {
                        $scope.attachmentNote = messages.HOUSWAP06;
                    }
                };

                $scope.changeSwapRoomRequest = function (model) {

                    $scope.model.BuildingObj = '';
                    $scope.model.BedroomList = '';
                    $scope.model.FlatObj = '';

                    $scope.viewmodel.BuildingList = angular.copy($scope.viewmodel.OccupiedBuildingList);
                    if ($scope.SwapRoomsRequest[1] == model) {

                        var building = angular.copy($scope.viewmodel.OccupiedBuildingList);
                        $scope.viewmodel.BuildingList = $filter('filter')(building, { BuildingDetailSid: $scope.existingAccomm.BuildingDetails.BuildingDetailSid });

                        $scope.viewmodel.BuildingObj = $scope.viewmodel.BuildingList[0];
                        $scope.viewmodel.FlatList = angular.copy($scope.viewmodel.BuildingList);

                    } else if ($scope.SwapRoomsRequest[0] == model) {

                        var building = angular.copy($scope.viewmodel.OccupiedBuildingList);
                        $scope.viewmodel.BuildingList = $filter('filter')(building, {
                            BuildingDetailSid: $scope.existingAccomm.BuildingDetails.BuildingDetailSid,
                            FlatId: $scope.existingAccomm.BuildingDetails.FlatId
                        });

                        $scope.model.BuildingObj = $scope.viewmodel.BuildingList[0];
                        $scope.viewmodel.BedroomList = $scope.viewmodel.FlatList = angular.copy($scope.viewmodel.BuildingList);
                        $scope.model.FlatObj = $scope.viewmodel.FlatList[0];

                        $scope.model.LandLineNo = $scope.viewmodel.BuildingList[0].TelephoneNo;
                        getNationalitiesData($scope.model.FlatObj.FlatId);
                    }

                    getStaffByFlatId();
                };

                $scope.changeBuildingSwap = function (item) {

                    $scope.model.BedroomObj = ''
                    $scope.model.FlatObj = ''

                    if (item) {
                        $scope.viewmodel.FlatList = $filter('filter')($scope.viewmodel.BuildingList, { BuildingDetailSid: item.BuildingDetailSid });

                        if ($scope.viewmodel.FlatList && $scope.viewmodel.FlatList.length == 1) {
                            $scope.model.FlatObj = $scope.viewmodel.FlatList[0];
                            getStaffByFlatId();
                        }
                    }
                };

                $scope.changeFlatSwap = function (item) {

                    $scope.model.BedroomObj = '';

                    if (item) {
                        $scope.model.LandLineNo = item.TelephoneNo;
                        getNationalitiesData(item.FlatId);

                        $scope.viewmodel.BedroomList = $filter('filter')($scope.viewmodel.BuildingList, { FlatId: item.FlatId, BuildingDetailSid: item.BuildingDetailSid });
                        if ($scope.viewmodel.BedroomList && $scope.viewmodel.BedroomList.length == 1) {
                            $scope.model.BedroomObj = $scope.viewmodel.BedroomList[0];
                        }
                    } else {
                        $scope.disableBedroom = true;
                    }

                    getStaffByFlatId();
                };
            }

            function validatePage() {

                //ipmhousingNewReqBlockUI.start();

                housingService.getHousingEntitlements($scope.viewmodel.RequestTypeObj.Text, function (success) {

                    loadPartialPage($scope.viewmodel.RequestTypeObj.Text, success);
                    //if (success && success.IsCrewEntitled == true) {
                    //    loadPartialPage($scope.RequestTypeObj.Text, success);
                    //} else {
                    //    toastr.info(success.Message);
                    //    ipmhousingNewReqBlockUI.stop();
                    //}
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            }

            function postDataChangeAcc() {

                $scope.model.BuildingDetails.BuildingDetailSid = $scope.model.BuildingObj ? $scope.model.BuildingObj.BuildingDetailSid : '';
                $scope.model.BuildingDetails.BuildingName = $scope.model.BuildingObj ? $scope.model.BuildingObj.BuildingName : '';
                $scope.model.BuildingDetails.FlatId = $scope.model.FlatObj ? $scope.model.FlatObj.FlatId : '';
                $scope.model.BuildingDetails.BedroomDetailsId = $scope.model.BedroomObj ? $scope.model.BedroomObj.BedroomDetailsId : '';

                $scope.model.RequestReason = $scope.model.ReasonObj ? $scope.model.ReasonObj.Text : '';
                $scope.model.StayOut.FriendStaffId = $scope.model.FriendStaffIdObj ? $scope.model.FriendStaffIdObj.CrewDetailsId : '';

                housingService.postmoveinrequest($scope.model, function (success) {

                    uploadAll(success.data);

                    //HOUCHANGEACC01 - need to show this info when successfully created.
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            };

            function postDataGuestAcc() {

                $scope.model.BuildingDetails.BuildingName = $scope.existingAccomm.BuildingDetails.BuildingName;
                $scope.model.BuildingDetails.FlatNumber = $scope.existingAccomm.BuildingDetails.FlatNumber;
                $scope.model.Guests.Relationship = $scope.model.RelationObj.Relationship;
                $scope.model.Guests.GuestName = $scope.model.GuestNameObj.GuestName;
                //$scope.model.RequestReason = "Guest Accommodation";

                housingService.postguestacc($scope.model, function (success) {

                    uploadAll(success.data);
                    //ipmhousingNewReqBlockUI.stop();
                    //$state.go('housing');
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            };

            function postDataMoveOutAcc() {

                $scope.model.RequestReason = $scope.viewmodel.RequestReasonObj ? $scope.viewmodel.RequestReasonObj.Text : '';

                housingService.postmovingout($scope.model, function (success) {

                    uploadAll(success.data);

                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            };

            function postStayOut() {

                $scope.model.RequestReason = $scope.model.ReasonObj ? $scope.model.ReasonObj.Text : '';
                $scope.model.StayOut.StayOutRequestTypeId = $scope.model.stayoutRequestdetObj ? $scope.model.stayoutRequestdetObj.Value : '';

                if ($scope.model.StayOutCrewRelationObj) {

                    var relativesNames = "";
                    var relativesIds = "";
                    for (var i = 0; i < $scope.model.StayOutCrewRelationObj.length; i++) {
                        relativesIds = relativesIds + $scope.model.StayOutCrewRelationObj[i].RelationType + "|";
                        relativesNames = relativesNames + $scope.model.StayOutCrewRelationObj[i].GuestName + "|";
                    }

                    $scope.model.StayOut.StayOutCrewRelationId = relativesIds.substr(0, relativesIds.length - 1);
                    $scope.model.StayOut.StayOutCrewRelationName = relativesNames.substr(0, relativesNames.length - 1);
                }

                housingService.poststayout($scope.model, function (success) {

                    uploadAll(success.data);

                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            };

            function postSwapRoom() {

                $scope.model.BuildingDetails.BuildingDetailSid = $scope.model.BuildingObj ? $scope.model.BuildingObj.BuildingDetailSid : '';
                $scope.model.BuildingDetails.BuildingName = $scope.model.BuildingObj ? $scope.model.BuildingObj.BuildingName : '';
                $scope.model.BuildingDetails.FlatId = $scope.model.FlatObj ? $scope.model.FlatObj.FlatId : '';
                $scope.model.BuildingDetails.BedroomDetailsId = $scope.model.BedroomObj ? $scope.model.BedroomObj.BedroomDetailsId : '';

                $scope.model.RequestReason = $scope.model.ReasonObj ? $scope.model.ReasonObj.Text : '';
                $scope.model.StayOut.FriendStaffId = $scope.model.FriendStaffIdObj ? $scope.model.FriendStaffIdObj.CrewDetailsId : '';
                $scope.model.StayOut.FriendStaffNo = $scope.model.FriendStaffIdObj ? $scope.model.FriendStaffIdObj.StaffNumber : '';

                $scope.model.StayOut.SwapRoomCategoryId = $scope.viewmodel.CategoryObj ? $scope.viewmodel.CategoryObj.Value : '';

                housingService.postswaproom($scope.model, function (success) {

                    uploadAll(success.data);

                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            };

            function submitData(type) {

                $scope.model.RequestId = $scope.viewmodel.RequestTypeObj ? $scope.viewmodel.RequestTypeObj.Value : '';
                $scope.model.RequestType = $scope.viewmodel.RequestTypeObj ? $scope.viewmodel.RequestTypeObj.Text : '';

                ipmhousingNewReqBlockUI.start();

                switch (type.toLocaleLowerCase()) {
                    //HOU1013: "Change Accommodation"
                    //HOU1016: "Moving In"
                    case messages.HOU1016.toLocaleLowerCase():
                    case messages.HOU1013.toLocaleLowerCase():
                        postDataChangeAcc();
                        break;

                        //HOU1015: "Guest Accommodation"
                    case messages.HOU1015.toLocaleLowerCase():
                        postDataGuestAcc();
                        break;

                        //HOU1017: "Moving Out of Company Accommodation"
                    case messages.HOU1017.toLocaleLowerCase():
                        postDataMoveOutAcc();
                        break;

                        //HOU1018: "Stay Out Request"
                    case messages.HOU1018.toLocaleLowerCase():
                        postStayOut();
                        break;

                        //HOU1019: "Swap Rooms"
                    case messages.HOU1019.toLocaleLowerCase():
                        postSwapRoom();
                        break;

                        //HOU1014: "Daily Maintenance"
                    case messages.HOU1014.toLocaleLowerCase():
                        break;
                }
            }

            function initialize() {

                var model = { LookupTypeCode: 'HousingRequestTypeByStaff' };
                ipmhousingNewReqBlockUI.start();

                lookupDataService.getLookupbyFilter(model, function (result) {
                    $scope.requestList = result.data.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });
                    $scope.viewmodel.RequestTypeObj = $scope.requestList[0];
                    // if ($scope.RequestTypeObj.Text == "Change Accommodation")
                    validatePage();
                    //  else
                    //      loadPartialPage($scope.RequestTypeObj.Text);
                });

                housingService.getExistingAccomm(null, function (success) {
                    $scope.existingAccomm = success;
                }, function (error) {
                    console.log(error);
                });

                fileuploader();
                pageEvents();
            }

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

            function getMimeType() {
                // Gets allowed file type for document upload
                sharedDataService.getCommonInfo('MIMETYPE', function (result) {
                    angular.forEach(result, function (data) {
                        $scope.fileType.push(data.Text);
                    });
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

            function getVacantBuilding() {

                ipmhousingNewReqBlockUI.start();
                housingService.getHousingVacantBuilding(null, function (success) {
                    $scope.viewmodel.BuildingList = success;
                    ipmhousingNewReqBlockUI.stop();
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            }

            function getOccupiedBuilding() {

                housingService.getOccupiedBuilding(null, function (success) {
                    $scope.viewmodel.OccupiedBuildingList = success;
                    $scope.viewmodel.BuildingList = angular.copy($scope.viewmodel.OccupiedBuildingList);
                    //ipmhousingNewReqBlockUI.stop();
                }, function (error) {
                    console.log(error);
                });
            }

            function getChangeAccReason() {
                // Gets reason list for move in type
                sharedDataService.getCommonInfo('HR_ChangeAccomReason', function (result) {
                    $scope.ReasonList = result;
                }, function (error) {

                });
            };

            function getStayOutReason() {
                // Gets reason list for move in type
                sharedDataService.getCommonInfo('HR_StayOutReason', function (result) {
                    $scope.ReasonList = result;
                    $scope.model.ReasonObj = $scope.ReasonList[0];
                }, function (error) {

                });
            };

            function getSwapRoomReason() {
                // Gets reason list for move in type
                sharedDataService.getCommonInfo('HR_ChangeAccomReason', function (result) {
                    $scope.ReasonList = result;
                }, function (error) {

                });
            };

            function getMovingOutReason() {
                // Gets reason list for move in type
                sharedDataService.getCommonInfo('HR_MovingOutReason', function (result) {
                    $scope.ReasonList = result;
                }, function (error) {

                });
            };

            function getNationalitiesData(flatId) {

                housingService.getNationalityByFlat(flatId, function (success) {
                    $scope.model.Nationality = success.Nationality;
                }, function (error) {
                    console.log(error);
                });
            };

            function getGuestDetails() {
                ipmhousingNewReqBlockUI.start();
                housingService.guestdetails(null, function (success) {
                    ipmhousingNewReqBlockUI.stop();
                    $scope.GuestList = success;
                    if (success && success.length == 1) {
                        $scope.model.RelationObj = success[0];
                        $scope.changeRelation($scope.model.RelationObj);
                    }
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            }

            function getCrewRelations() {
                ipmhousingNewReqBlockUI.start();
                housingService.getCrewRelations(null, function (success) {
                    ipmhousingNewReqBlockUI.stop();
                    $scope.GuestList = success;
                    //if (success && success.length == 1) {
                    //    $scope.viewmodel.RelationObj = success[0];
                    //    $scope.changeRelation($scope.viewmodel.RelationObj);
                    //}
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });
            }

            function uploadAll(data) {

                if (data && data.RequestNumber && data.ResponseId) {
                    if ($scope.uploader.queue.length > 0) {
                        $scope.requestId = data.ResponseId;
                        $scope.uploader.uploadAll();
                    } else {
                        ipmhousingNewReqBlockUI.stop();
                        toastr.info(messages.HOUCREATESUCCESS.replace('@reqno', data.RequestNumber));
                        $state.go('housing');
                    }
                } else {
                    ipmhousingNewReqBlockUI.stop();

                    var msg = data.Message ? messages[data.Message] : messages.HOUERROR01;
                    toastr.error(msg);
                }
            }

            function fileuploader() {

                var uploader = $scope.uploader = new fileUploader({
                    url: appSettings.API + 'api/fileupload'
                });

                // File must be jpeg or png
                uploader.filters.push({
                    name: 'mime_type_filter',
                    fn: function (item) {

                        var allowedFileType = $scope.fileType.indexOf(item.type) > 0 ? true : false;
                        if (!allowedFileType) {
                            toastr.error(messages.HOU1020);
                        }
                        return allowedFileType;
                    }
                });

                // FILTERS
                uploader.filters.push({
                    name: 'customFilter',
                    fn: function (item /*{File|FileLikeObject}*/, options) {
                        return this.queue.length < 10;
                    }
                });

                uploader.onBeforeUploadItem = function (item) {
                    item.formData = [{ 'RequestId': $scope.requestId }];
                };

                uploader.onCompleteAll = function () {
                    $state.go('housing');
                };
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

            function loadPartialPage(type, data) {

                // initially hide all validation on tab change
                $scope.submitted = false;
                $scope.housingEntitlements = data;
                var currentDate = new Date();
                getMimeType();

                // Test code - to open all request type
                //$scope.housingEntitlements.IsCrewEntitled = data.IsCrewEntitled = true;

                switch (type.toLocaleLowerCase()) {

                    //HOU1013: "Change Accommodation"    
                    case messages.HOU1013.toLocaleLowerCase():

                        if (data.IsCrewEntitled) {
                            getVacantBuilding();
                            getChangeAccReason();
                        } else {
                            toastr.info(data.Message);
                        }
                        getCrewEntitlement();
                        $state.go('housing-create.ca');
                        break;

                        //HOU1015: "Guest Accommodation"
                    case messages.HOU1015.toLocaleLowerCase():

                        if (data.IsCrewEntitled) {

                            $scope.housingEntitlements.currentYear = currentDate.getFullYear();
                            $scope.housingEntitlements.nextYear = currentDate.getFullYear() + 1;

                            $scope.$watch('model.Guests.CheckoutDate', function (nDate, oDate) {
                                if (nDate) {
                                    compareDate(new Date($scope.model.Guests.CheckinDate), new Date($scope.model.Guests.CheckoutDate));
                                }
                            });

                            $scope.$watch('model.Guests.CheckinDate', function (nDate, oDate) {

                                if (nDate) {

                                    $scope.disableCheckout = false;
                                    $scope.viewmodel.toMindate = $scope.model.Guests.CheckinDate;
                                    $scope.model.Guests.CheckoutDate = "";

                                    var daysLeft = 0;
                                    var fromDate = angular.copy($scope.model.Guests.CheckinDate);

                                    if (fromDate.getFullYear() == currentDate.getFullYear()) {
                                        daysLeft = $scope.housingEntitlements.Setup_days - $scope.housingEntitlements.Used_No_of_days - 1;

                                        //var currYearMax = new Date(fromDate.setDate(fromDate.getDate() + daysLeft));
                                        //var calendarMax = new Date(currentDate.getFullYear(), 11, 31);

                                        if (data.IsEntitledCurrentYear && data.IsEntitledNextYear) {
                                            $scope.viewmodel.toMaxdate = new Date(fromDate.setDate(fromDate.getDate() + daysLeft));
                                        } else {
                                            $scope.viewmodel.toMaxdate = new Date(currentDate.getFullYear(), 11, 31);
                                        }

                                        //if (currYearMax < calendarMax) {
                                        //    $scope.viewmodel.toMaxdate = currYearMax;
                                        //} else {
                                        //    $scope.viewmodel.toMaxdate = calendarMax;
                                        //}

                                    } else {
                                        daysLeft = $scope.housingEntitlements.Setup_days - $scope.housingEntitlements.Next_No_of_days - 1;

                                        var nextYearMax = new Date(fromDate.setDate(fromDate.getDate() + daysLeft));
                                        var calendarMax = new Date(currentDate.getFullYear() + 1, 11, 31);

                                        if (nextYearMax < calendarMax) {
                                            $scope.viewmodel.toMaxdate = nextYearMax;
                                        } else {
                                            $scope.viewmodel.toMaxdate = calendarMax;
                                        }
                                    }
                                }
                            });


                            if (data.IsEntitledCurrentYear && data.IsEntitledNextYear) {

                                $scope.viewmodel.fromMindate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 7);
                                $scope.viewmodel.fromMaxdate = new Date(currentDate.getFullYear() + 1, 11, 31);

                                $scope.viewmodel.toMindate = $scope.model.Guests.CheckinDate;
                                $scope.viewmodel.toMaxdate = new Date(currentDate.getFullYear() + 1, 11, 31);

                            } else if (data.IsEntitledCurrentYear) {

                                $scope.viewmodel.fromMindate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 7);
                                $scope.viewmodel.fromMaxdate = new Date(currentDate.getFullYear(), 11, 31);

                                $scope.viewmodel.toMindate = $scope.model.Guests.CheckinDate;
                                $scope.viewmodel.toMaxdate = new Date(currentDate.getFullYear(), 11, 31);

                            } else if (data.IsEntitledNextYear) {

                                toastr.info(messages.HOUGUESTACC04);

                                $scope.viewmodel.fromMindate = new Date(currentDate.getFullYear() + 1, 1, 1);
                                $scope.viewmodel.fromMaxdate = new Date(currentDate.getFullYear() + 1, 11, 31);

                                $scope.viewmodel.toMindate = $scope.model.Guests.CheckinDate;
                                $scope.viewmodel.toMaxdate = new Date(currentDate.getFullYear() + 1, 11, 31);
                            }

                            getGuestDetails();
                        } else {
                            toastr.info(data.Message);
                        }

                        $state.go('housing-create.ga');
                        break;

                        //HOU1016: "Moving In"
                    case messages.HOU1016.toLocaleLowerCase():

                        if (data.IsCrewEntitled) {
                            getVacantBuilding();
                            getChangeAccReason();
                        } else {
                            toastr.info(data.Message);
                        }

                        $state.go('housing-create.mi');
                        break;

                        //HOU1017: "Moving Out of Company Accommodation"
                    case messages.HOU1017.toLocaleLowerCase():

                        if (data.IsCrewEntitled) {
                            getMovingOutReason();
                        } else {
                            toastr.info(data.Message);
                        }

                        $state.go('housing-create.mo');
                        break;

                        //HOU1018: "Stay Out Request"
                    case messages.HOU1018.toLocaleLowerCase():

                        // Based on selection of request details type
                        // If Type is rostered days off for whole month , then auto populate to date to last day of month
                        $scope.$watch('model.StayOut.StayOutRequestFromDate', function (nDate, oDate) {

                            if (nDate) {

                                var reason = $scope.model.stayoutRequestdetObj ? $scope.model.stayoutRequestdetObj.Text : '';

                                if (reason && reason.toLocaleLowerCase() == messages.HOUSWAP03.toLocaleLowerCase()) {
                                    $scope.model.StayOut.StayOutRequestToDate = new Date(nDate.getFullYear(), nDate.getMonth() + 1, 0);
                                };

                                $scope.viewmodel.toMindate = nDate;
                                compareDate(new Date($scope.model.StayOut.StayOutRequestFromDate), new Date($scope.model.StayOut.StayOutRequestToDate));
                            }
                        });

                        $scope.$watch('model.StayOut.StayOutRequestToDate', function (nDate, oDate) {
                            compareDate(new Date($scope.model.StayOut.StayOutRequestFromDate), new Date($scope.model.StayOut.StayOutRequestToDate))
                        });

                        if (data.IsCrewEntitled) {
                            getStayOutRequestType();
                            getStayOutReason();
                            getCrewRelations();

                            $scope.disableSpecificDates = false;
                            $scope.viewmodel.fromMindate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 7);
                            $scope.viewmodel.toMindate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 7);
                            $scope.attachmentNote = messages.HOUSWAP06;
                        } else {
                            toastr.info(data.Message);
                        }
                        $state.go('housing-create.so');
                        break;

                        //HOU1019: "Swap Rooms"
                    case messages.HOU1019.toLocaleLowerCase():

                        if (data.IsCrewEntitled) {
                            getSwapRoomsRequestType();
                            getSwapRoomReason();
                            getOccupiedBuilding();
                        } else {
                            toastr.info(data.Message);
                        }
                        getCrewEntitlement();
                        $state.go('housing-create.sr');
                        break;

                        //HOU1014: "Daily Maintenance"
                    case messages.HOU1014.toLocaleLowerCase():
                        break;
                }
                ipmhousingNewReqBlockUI.stop();
            }

            function getStayOutRequestType() {
                // Gets reason list for move in type
                housingService.getStayOutRequestType(null, function (result) {
                    $scope.stayOutRequestType = result;
                    $scope.model.stayoutRequestdetObj = $scope.stayOutRequestType[0];
                    $scope.changeStayoutReqDetails($scope.model.stayoutRequestdetObj);
                }, function (error) {

                });
            };

            initialize();

        }]);