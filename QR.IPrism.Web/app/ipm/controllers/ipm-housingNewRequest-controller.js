'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.housingNewRequest.controller', ['$rootScope', '$scope', 'lookupDataService', 'housingService', '$state','$stateParams',
        'sharedDataService', 'appSettings', 'ngDialog', 'analyticsService', 'messages', 'toastr', 'blockUI', '$window', '$filter',
        function ($rootScope,$scope, lookupDataService, housingService, $state, $stateParams, sharedDataService, appSettings, ngDialog,
            analyticsService, messages, toastr, blockUI, $window, $filter) {

            // private variable declaration
            var ipmhousingNewReqBlockUI = blockUI.instances.get('ipmhousingNewReqBlockUI');

            $scope.messages = messages;
           
            $scope.existingAccomm = {};
            $scope.requestDate = new Date();
            $scope.submitted = false;
            $scope.fileType = [];
            $scope.showFriendStaffId = false;

            /* used for uploading document,used in srd-fileUpload.js */
            $scope.uploadType = 'Housing';

            // Request type used for all housing request for saving data
            $rootScope.RequestTypeObj = '';

            function pageEvents() {

                $scope.changeRequestType = function (data) {
                   
                    $rootScope.RequestTypeObj = data;
                    loadPartialPage(data.Text);
                };
              
                getMimeType();
            }

            function initialize() {

                var model = { LookupTypeCode: 'HousingRequestTypeByStaff' };
                ipmhousingNewReqBlockUI.start();

                lookupDataService.getLookupbyFilter(model, function (result) {
                    $scope.requestList = result.data.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });
                    ipmhousingNewReqBlockUI.stop();
                });

                housingService.getExistingAccomm(null, function (success) {
                    $scope.existingAccomm = success;
                }, function (error) {
                    console.log(error);
                });
              
                pageEvents();
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

            function loadPartialPage(type, data) {

                switch (type.toLocaleLowerCase()) {

                    //HOU1013: "Change Accommodation"    
                    case messages.HOU1013.toLocaleLowerCase():

                        $state.go('housing-create.ca');
                        break;

                        //HOU1015: "Guest Accommodation"
                    case messages.HOU1015.toLocaleLowerCase():

                        $state.go('housing-create.ga');
                        break;

                        //HOU1016: "Moving In"
                    case messages.HOU1016.toLocaleLowerCase():

                        $state.go('housing-create.mi');
                        break;

                        //HOU1017: "Moving Out of Company Accommodation"
                    case messages.HOU1017.toLocaleLowerCase():

                        $state.go('housing-create.mo');
                        break;

                        //HOU1018: "Stay Out Request"
                    case messages.HOU1018.toLocaleLowerCase():

                        $state.go('housing-create.so');
                        break;

                        //HOU1019: "Swap Rooms"
                    case messages.HOU1019.toLocaleLowerCase():

                        $state.go('housing-create.sr');
                        break;

                        //HOU1014: "Daily Maintenance"
                    case messages.HOU1014.toLocaleLowerCase():
                        break;
                }
            }

            initialize();

        }]);