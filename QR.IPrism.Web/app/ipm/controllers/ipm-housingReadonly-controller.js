
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.housingReadonly.controller', ['$rootScope', '$scope', 'lookupDataService', 'housingService', '$state', 'FileUploader', '$stateParams',
        'sharedDataService', 'appSettings', 'ngDialog', 'analyticsService', 'messages', 'toastr', 'blockUI', '$window', '$filter', 'bundleMessage',
        function ($rootScope, $scope, lookupDataService, housingService, $state, FileUploader, $stateParams, sharedDataService, appSettings, ngDialog,
            analyticsService, messages, toastr, blockUI, $window, $filter, bundleMessage) {

            var ipmhousingReadonlyBlockUI = blockUI.instances.get('ipmhousingReadonlyBlockUI');

            $scope.housingdata = {};
            $scope.housingEntitlements = {};
            $scope.fileType = [];
            $scope.fileTypeObj = [];
            $scope.removedFiles = [];

            /* used for uploading document,used in srd-fileUpload.js */
            $scope.uploadType = 'HousingAmend';

            $scope.messages = messages;

            $scope.requestNumber = $stateParams.RequestNumber;

            function readonlyView() {

                housingService.getrequestdetails($stateParams.RequestNumber, function (success) {
                    ipmhousingReadonlyBlockUI.stop();

                    if (success.RequestDetails) {

                        $scope.housingdata = success.RequestDetails;
                        $scope.comments = success.Messages;
                        $scope.files = success.Files;

                        showCancelBtnLogic(success.RequestDetails);
                    } else {
                        ipmhousingReadonlyBlockUI.stop();
                        toastr.error(messages.NODATAFOUND);
                    }
                }, function (error) {
                    ipmhousingReadonlyBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });

                housingService.getExistingAccomm(null, function (success) {
                    $scope.existingAccomm = success;
                }, function (error) {
                    toastr.error(messages.HOUERROR01);
                });

                //opens the specific file based upon mime type
                $scope.openImage = function (data) {

                    sharedDataService.openFile(data, $scope.fileTypeObj);
                    return false;
                }

                $scope.cancelRequest = function () {

                    $scope.dialogTitle = "Confirmation";
                    $scope.dialogMessage = messages.HOUCONFIRMCANCEL;
                    ngDialog.open({
                        scope: $scope,
                        preCloseCallback: function (value) {
                            if (value == 'Post') {
                                cancelReq();
                                //analyticsService.trackEvent('Action', 'Add', 'View', 'Create Housing Request');
                            }
                        }
                    });
                }

                $scope.removeDocument = function (file, $index) {

                    if ($scope.removedFiles.indexOf(file.FileId) == -1) {
                        $scope.removedFiles.push(file.FileId);
                    }

                    $scope.files.splice($index, 1);
                };

                $scope.amend = function () {

                    $scope.dialogTitle = "Confirmation";
                    $scope.dialogMessage = messages.HOUCONFIRMNEW;
                    ngDialog.open({
                        scope: $scope,
                        preCloseCallback: function (value) {
                            if (value == 'Post') {
                                updateDocuments();
                                //analyticsService.trackEvent('Action', 'Add', 'View', 'Create Housing Request');
                            }
                        }
                    });
                };

                getHousingEntitlements();

                getMimeType();
            }

            function updateDocuments() {

                ipmhousingReadonlyBlockUI.start();

                if ($scope.$$childHead.uploader.queue.length == 0 && $scope.removedFiles.length == 0) {

                    $state.go('housing');

                } else {
                    deleteDocument();
                    UploadNewDocument();
                }
            };

            /*  */
            function UploadNewDocument() {
                if ($scope.$$childHead.uploader.queue.length > 0) {
                    $scope.requestId = $stateParams.RequestId;
                    $scope.$$childHead.uploader.uploadAll();
                }
            }

            function deleteDocument() {
                var uploaderLength = $scope.$$childHead.uploader.queue.length;

                if ($scope.removedFiles.length > 0) {

                    housingService.deleteDocuments($scope.removedFiles, function (success) {
                        if (uploaderLength == 0) {
                            toastr.info(messages.HOUAMENDSUCCESS);
                            ipmhousingReadonlyBlockUI.stop();
                            $state.go('housing');
                        }
                    }, function (error) {
                        // ipmhousingReadonlyBlockUI.stop();
                    });
                } else {

                }
            };

            function getMimeType() {
                // Gets allowed file type for document upload
                sharedDataService.getCommonInfo('MIMETYPE', function (result) {
                    angular.forEach(result, function (data) {
                        $scope.fileType.push(data.Text);
                        $scope.fileTypeObj.push(data);
                    });
                }, function (error) {

                });
            };

            function cancelReq() {
                ipmhousingReadonlyBlockUI.start();
                housingService.cancelrequest($scope.housingdata.RequestId, function (success) {

                    toastr.info(messages.HOUCANCELREQ);
                    ipmhousingReadonlyBlockUI.stop();
                    $state.go("housing", {}, { reload: true });
                }, function (error) {
                    ipmhousingReadonlyBlockUI.stop();
                });
            };

            function getHousingEntitlements() {
                housingService.getHousingEntitlements(messages.HOU1015, function (success) {
                    var currentDate = new Date();
                    $scope.housingEntitlements = success;
                    $scope.housingEntitlements.currentYear = currentDate.getFullYear();
                    $scope.housingEntitlements.nextYear = currentDate.getFullYear() + 1;

                }, function (error) {
                    ipmhousingReadonlyBlockUI.stop();
                });
            }

            function getCrewEntitlement() {
                ipmhousingReadonlyBlockUI.start();
                housingService.crewentitlement(null, function (success) {
                    $scope.CrewEntitlementDetails = success;
                    ipmhousingReadonlyBlockUI.stop();
                }, function (error) {
                    ipmhousingReadonlyBlockUI.stop();
                });
            };

            /* Cancel request is only available if request is in open and in-progress state
                and for specific requests */
            function showCancelBtnLogic(requestData) {

                if (requestData.RequestStatus == (messages.HOU1004 || messages.HOU1005)) {
                    $scope.openRequest = true;
                }

                //HOU1004: "Open"
                //HOU1005: "In Progress"
                //HOU1006: "Cancelled"
                //HOU1007: "Cancelled by HO"
                //HOU1008: "Cancelled by Crew"
                //HOU1009: "Approved"
                //HOU1010: "Closed"
                switch (requestData.RequestType) {

                    //HOU1013: "Change Accommodation" 
                    //HOU1016: "Moving In"             
                    case messages.HOU1013:
                    case messages.HOU1016:
                    case messages.HOU1019:
                        getCrewEntitlement();
                        housingService.getNationalityByFlat(requestData.BuildingDetails.FlatId, function (success) {
                            $scope.housingdata.Nationality = success.Nationality;
                        }, function (error) {
                            console.log(error);
                        });
                        break;

                        //HOU1016: "Moving In"
                        //HOU1018: "Stay Out Request"
                        //HOU1019: "Swap Rooms"
                        // Also add logic for showing postpone and amend

                        //int noOfDays = DateTime.Now.Subtract(requestDetails[0].RequestedDate).Days;
                        //if (noOfDays <= 3)
                        //{
                        //    ButtonPostpone.Visible = true;
                        //    ButtonAmend.Visible = true;
                        //}
                }
            }

            ipmhousingReadonlyBlockUI.start();
            readonlyView();
        }]);