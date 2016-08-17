"use strict";
angular.module('app.shared.components').directive('srdFileupload', [function ($scope) {
    return {
        restrict: 'E',
        scope: {
            uploader: '=',
            model: '='
        },
        templateUrl: '/app/shared/directives/FileUpload/srd-Fileupload.html',
        controller: ['$scope', 'FileUploader', 'messages', 'appSettings', 'toastr', '$state', '$filter', 'sharedDataService',
        function ($scope, FileUploader, messages, appSettings, toastr, $state, $filter, sharedDataService) {

            $scope.fileSize = function (item) {
                var size = parseInt(item) / 1024 / 1024;
                return parseInt(size) == 0 ? $filter('number')(size * 1024, 2) + " KB" : $filter('number')(size, 2) + " MB";
            }

            var uploader = $scope.uploader = new FileUploader({
                url: appSettings.API + 'api/fileupload',
                headers: { 'ForgeryVerificationToken': sharedDataService.getToken() }
            });

            // File must be jpeg or png
            uploader.filters.push({
                name: 'mime_type_filter',
                fn: function (item) {

                    var allowedFileType = $scope.$parent.fileType.indexOf(item.type) > -1 ? true : false;
                    if (!allowedFileType) {
                        toastr.error(messages.HOU1020);
                    }

                    if ((item.size / 1024 / 1024) > 5) {
                        allowedFileType = false;
                        toastr.error(messages.HOU1022);
                    }
                    return allowedFileType;
                }
            });

            // FILTERS
            uploader.filters.push({
                name: 'customFilter',
                fn: function (item /*{File|FileLikeObject}*/, options) {

                    var allowedFileCount = this.queue.length < 10;
                    if (!allowedFileCount) {
                        toastr.error(messages.FILECOUNTLIMIT ? messages.FILECOUNTLIMIT : "Only 10 files are allowed to upload.");
                    }
                    return allowedFileCount;
                }
            });

            uploader.onBeforeUploadItem = function (item) {
                if ($scope.$parent.uploadType == 'Housing' || $scope.$parent.uploadType == 'HousingAmend') {

                    item.formData = [{
                        'RequestId': $scope.$parent.requestId,
                        'UploadType': 'Housing'
                    }];
                } else if ($scope.$parent.uploadType == 'EVRSave') {

                    item.formData = [{
                        'RequestId': $scope.$parent.VrId,
                        'UploadType': $scope.$parent.uploadType,
                        'fileNamePrefix': $scope.$parent.VrNo
                    }];
                } else if ($scope.$parent.uploadType == 'RecAsmt') {

                    item.formData = [{
                        'RequestId': $scope.$parent.assessmentID,
                        'UploadType': $scope.$parent.uploadType,
                        'fileNamePrefix': 'Asmt'
                    }];
                } else if ($scope.$parent.uploadType == 'Kafousave') {

                    item.formData = [{
                        'RequestId': $scope.$parent.kafouID,
                        'UploadType': $scope.$parent.uploadType,
                        'fileNamePrefix': 'KafouDoc'
                    }];
                }
            };

            uploader.onErrorItem = function (item) {
                if (item && item.file && item.file.name) {
                    toastr.error(item.file.name + ' is either invalid or corrupt file.' + messages.HOU1020);
                } else {
                    toastr.error('Unable to upload file.Please try again later or upload a valid file type');
                }
            };

            uploader.onCompleteAll = function (item) {

                if ($scope.$parent.uploadType == 'Housing') {
                    toastr.info(messages.HOUCREATESUCCESS.replace('@reqno', $scope.$parent.requestNumber));
                    $state.go('housing');
                } else if ($scope.$parent.uploadType == 'HousingAmend') {
                    toastr.info(messages.HOUAMENDSUCCESS);
                    $state.go('housing');
                } else if ($scope.$parent.uploadType == 'EVRSave') {
                    $scope.$parent.onEvrDocSaveCmplte();

                } else if ($scope.$parent.uploadType == 'Kafousave') {

                }
            };
        }],
        link: function (scope, element, attrs) {

        }
    };
}]);