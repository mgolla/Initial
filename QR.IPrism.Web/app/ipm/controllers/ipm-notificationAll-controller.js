'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.notificationAll.controller', ['$scope', '$http', 'notificationAlertService', 'fileService', 'analyticsService', 'sharedDataService',
        '$state', 'blockUI','messages',
        function ($scope, $http, notificationAlertSVPService, fileService, analyticsService, sharedDataService, $state, blockUI, messages) {

            //Controller Scope Initialization
            var notfblockUI = blockUI.instances.get('notfblockUI');
            var initialDrpValues = { 'Value': '', 'Text': '--All--' };

            function setDate() {
                var currentDate = new Date();
                $scope.model.ToDate = angular.copy(currentDate);
                currentDate.setMonth(currentDate.getMonth() - 1);
                $scope.model.FromDate = currentDate;
            };

            function Initialize() {

                $scope.filter = { StaffID: "" };
                $scope.model = {};

                sharedDataService.getCommonInfo('NOTIFICATIONTYPE', function (result) {

                    $scope.requestList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });
                    $scope.requestList.unshift(initialDrpValues);

                    $scope.model.RequestTypeObj = $scope.requestList[0];
                });

                // setDate();
                loadGrid();
                loadNotificationAlertSVPList();
            }

            $scope.search = function () {
                loadNotificationAlertSVPList();
            };

            $scope.reset = function () {

                $scope.model.RequestTypeObj = $scope.requestList[0];
                $scope.model.FromDate = '';
                $scope.model.ToDate = '';

                loadNotificationAlertSVPList();
            };

            function isNotNullOrEmpty(value) {
                return (value && value != null && value.toString().trim().length > 0);
            }
            function loadGrid() {

                $scope.getDesc = function (row) {
                    var data = {
                        DocType: row.Type,
                        DocumentId: row.Id,
                        FileCode: row.FileCode,
                        FileId: row.fileID,
                        DocumentName: row.DocumentName,
                        Doc: 'N',
                        isTab: false,
                        Link: row.Link
                    };
                                       
                    if (isNotNullOrEmpty(data.DocType) &&
                    (data.DocType.toLowerCase() == messages.ASMTSCHEDULED.toLowerCase() ||
                    data.DocType.toLowerCase() == messages.ASMTREQUESTED.toLowerCase() ||
                    data.DocType.toLowerCase() == messages.ASMTDELAYED.toLowerCase() ||
                    data.DocType.toLowerCase() == messages.ASMTDELAYEDSAVED.toLowerCase() ||
                    data.DocType.toLowerCase() == messages.ASMTCANCELLED.toLowerCase() ||
                    data.DocType.toLowerCase() == messages.FMS.toLowerCase() ||
                    data.DocType.toLowerCase() == messages.FILENOTEIDP.toLowerCase() ||
                    data.DocType.toLowerCase() == messages.PACKAGENOTICE.toLowerCase())) {

                        $state.go('notificationAll.asmntack', { reqno: data.DocumentId, reqtype: data.DocType });

                    } else if (data.DocType.toLowerCase() == messages.IDPBEHAVIOUR.toLowerCase()) {

                        $state.go('notificationAll.bevidp', { reqno: data.DocumentId, reqtype: data.DocType });

                    } else if (isNotNullOrEmpty(data.DocType) && data.DocType.toLowerCase() == messages.ASMT.toLowerCase()) {

                        $state.go('notificationAll.idpack', { reqno: data.DocumentId, reqtype: data.DocType });

                    } else if (isNotNullOrEmpty(data.DocType) && data.DocType.toLowerCase() == messages.HOUSING.toLowerCase()) {

                        $state.go('notificationAll.hsChangeAcc', { RequestNumber: data.RequestNumber, RequestId: data.RequestId });

                    } else if (isNotNullOrEmpty(data.DocType) &&
                        data.DocType.toLowerCase() == messages.GUESTAPPROVAL.toLowerCase() ||
                        data.DocType.toLowerCase() == messages.HOU1013.toLowerCase() ||
                        data.DocType.toLowerCase() == messages.HOU1015.toLowerCase() ||
                        data.DocType.toLowerCase() == messages.HOU1017.toLowerCase() ||
                        data.DocType.toLowerCase() == messages.HOU1018.toLowerCase() ||
                        data.DocType.toLowerCase() == messages.HOU1019.toLowerCase() ||
                        data.DocType.toLowerCase() == messages.HOU1016.toLowerCase() ||
                        data.DocType.toLowerCase() == messages.HOUSWAPREC.toLowerCase()) {
                        
                        $state.go('notificationAll.housing-ack', { reqno: data.DocumentId, reqtype: data.DocType });
                    }
                }

                $scope.grid = {
                    gridApi: {},
                    enableFiltering: true,
                    showGridFooter: true,
                    enableColumnResizing: true,
                    paginationPageSizes: [5, 10, 15, 20, 25],
                    paginationPageSize: 10,
                    enablePagination: true,
                    enablePaginationControls: true,
                    data: [],
                    subgrid: 'false',
                    columnDefs: [
                          { field: "Id", name: "Notification Id", enableHiding: false, sort: { direction: 'desc' } },
                          {
                              field: "Description", name: "Description", enableHiding: false, width: '50%',
                              cellTemplate: '<div title="{{row.entity.Description}}" class="ui-grid-cell-contents">' +
                                  '<a href="" ng-click="grid.appScope.getDesc(row.entity)">{{row.entity.Description}}</a><div>'
                          },
                          { field: "Type", name: "Notification Type", enableHiding: false },
                          {
                              field: "Date", name: "Notification Date", type: 'date', enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["Date"]  | date:"dd-MMM-yyyy" }}</div>'
                          }
                    ]
                };
            }

            function loadNotificationAlertSVPList() {
                notfblockUI.start();

                $scope.filter.Id = "0";
                $scope.filter.Type = $scope.model.RequestTypeObj ? $scope.model.RequestTypeObj.Value : "";
                $scope.filter.Date = $scope.model.FromDate ? sharedDataService.getDateOnly($scope.model.FromDate) : '';
                $scope.filter.ActionByDate = $scope.model.ToDate ? sharedDataService.getDateOnly($scope.model.ToDate) : '';

                notificationAlertSVPService.getAllNotification($scope.filter, function (result) {
                    $scope.grid.data = result.data;
                    notfblockUI.stop();
                }, function (error) {
                    notfblockUI.stop();
                });
            }

            Initialize();

        }]);