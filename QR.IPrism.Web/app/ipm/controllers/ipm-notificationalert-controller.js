
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.notificationAlert.controller', ['$scope', '$http', 'notificationAlertService', 'fileService', 'analyticsService', '$state',
        '$rootScope', 'blockUI', 'appSettings', 'bundleMessage', 'messages', 'ngDialog',
        function ($scope, $http, notificationAlertSVPService, fileService, analyticsService, $state, $rootScope, blockUI, appSettings,
            bundleMessage, messages, ngDialog) {
            //Controller Scope Initialization
            var inotificationAlertSVPblockUI = blockUI.instances.get('ipmnotificationAlertSVPBlockUI');
            var Initialize = function () {
                $rootScope.isTab = true;
                //Declare all scop properties
                $rootScope.notificationAlertSVPViewModel;
                $rootScope.filter = {
                    StaffID: ""

                }
                $rootScope.selectedNotificationAlertSVP = '';


                loadNotificationAlertSVPList($rootScope.filter);
            }


            function loadNotificationAlertSVPList(filter) {
                inotificationAlertSVPblockUI.start();

                notificationAlertSVPService.getNotificationAlertSVPList(filter, function (result) {
                    notificationAlertSVPService.notifications = result.data.NotificationAlertSVPs;
                    //console.log(notificationAlertSVPService.notifications);

                    if (!notificationAlertSVPService.alertShown) {
                        notificationAlertSVPService.alertShown = true;
                        var lst = [];
                        angular.forEach(notificationAlertSVPService.notifications, function (v) {
                            if (v.Doc == "A") {
                                lst.push(v.DocDesc);
                            }
                        });

                        if (lst.length > 0) {

                            $scope.dialogTitle = "Alert";
                            $scope.dialogMessage = bundleMessage.getMessages(lst);
                            var dialog = ngDialog.open({
                                template: '/app/ipm/partials/ipmModalConfirmation.html',
                                scope: $scope,
                                preCloseCallback: function (value) {
                                    if (value == 'Post') {
                                        dialog.close();
                                    }
                                }
                            });
                        }
                    }

                    $rootScope.notificationAlertSVPViewModel = result.data.NotificationAlertSVPs;
                    inotificationAlertSVPblockUI.stop();
                }, function (error) {
                    inotificationAlertSVPblockUI.stop();
                });
            }

            $scope.onSelectedChange = function (DocType, FileCode, fileID, DocumentId, DocumentName, Doc, Link, alerfolder) {

                var istab = true;
                if (appSettings.isMobileHeader()) {
                    istab = false;
                }
                var filterPara = {
                    DocType: DocType,
                    FileCode: FileCode,
                    FileId: fileID,
                    DocumentId: DocumentId,
                    DocumentName: DocumentName,
                    Doc: Doc,
                    isTab: istab,
                    Link: Link
                };

                if (alerfolder && alerfolder.toString().trim().length > 0) {
                    filterPara.DocumentName = alerfolder;
                }

                fileService.openDocTypes(filterPara, function (result) {

                },
                function (error) {
                    console.log('error : ' + error);
                });
            }

            $scope.isSelected = function (section) {

                return $rootScope.selectedNotificationAlertSVP === section;
            }
            $scope.getDate = function (string) {
                var array = string.split(' ');
                return array[0] + ' ' + array[1];
            }
            $scope.getMonthYear = function (string) {
                var array = string.split(' ');
                return array[1] + ' ' + array[2];
            }
            Initialize();

        }]);



