'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.housingMoveOut.controller', ['$rootScope','$scope', 'lookupDataService', 'housingService', '$state', 'FileUploader', '$stateParams',
        'sharedDataService', 'appSettings', 'ngDialog', 'analyticsService', 'messages', 'toastr', 'blockUI', '$window', '$filter',
        function ($rootScope,$scope, lookupDataService, housingService, $state, fileUploader, $stateParams, sharedDataService, appSettings, ngDialog,
            analyticsService, messages, toastr, blockUI, $window, $filter) {

            // private variable declaration
            var ipmhousingNewReqBlockUI = blockUI.instances.get('ipmhousingNewReqBlockUI');

            $scope.model = {};
            $scope.model.StayOut = {};
            $scope.model.BuildingDetails = {};
            $scope.model.Guests = {};

            $scope.requestNumber;

            function pageEvents() {

                $scope.submitRequest = function (form) {

                    $scope.submitted = true;

                    if (form.$valid && $scope.uploader.queue.length > 0) {
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
                    } else if ($scope.uploader.queue.length < 1) {
                       // $scope.invalidAttachment = true;
                        toastr.error($scope.messages.HOUATTACHMENTMO);
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

                    $scope.uploader.clearQueue();
                }
            };
            
            function submitData() {

                ipmhousingNewReqBlockUI.start();
                $scope.model.RequestId = $rootScope.RequestTypeObj ? $rootScope.RequestTypeObj.Value : '';
                $scope.model.RequestType = $rootScope.RequestTypeObj ? $rootScope.RequestTypeObj.Text : '';
                $scope.model.RequestReason = $scope.model.ReasonObj ? $scope.model.ReasonObj.Text : '';
                $scope.model.Guests.CheckoutDate = $scope.model.FromDate ? sharedDataService.getDateOnly($scope.model.FromDate) : '';

                housingService.postmovingout($scope.model, function (success) {

                    var data = success.data;
                    if (data && data.RequestNumber && data.ResponseId) {

                        $scope.requestNumber = data.RequestNumber;
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
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                    toastr.error(messages.HOUERROR01);
                });
            };

            function getMovingOutReason() {
                // Gets reason list for move in type
                sharedDataService.getCommonInfo('HR_MovingOutReason', function (result) {
                    $scope.ReasonList = result;
                }, function (error) {

                });
            };

            function initialize() {

                var currentDate = new Date();
                $scope.mindate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 7);

                housingService.getHousingEntitlements($scope.messages.HOU1017, function (data) {

                    $scope.housingEntitlements = data;
                    if (data.IsCrewEntitled) {
                        pageEvents();
                        getMovingOutReason();
                    } else {
                        toastr.info(data.Message);
                    }

                    ipmhousingNewReqBlockUI.stop();
                }, function (error) {
                    ipmhousingNewReqBlockUI.stop();
                });

            };

            ipmhousingNewReqBlockUI.start();
            initialize();
        }]);