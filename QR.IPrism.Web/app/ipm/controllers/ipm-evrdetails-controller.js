'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.evrdetails.controller', ['$scope', '$state', '$rootScope', 'lookupDataService', 'evrSearchService', 'sharedDataService', 'blockUI', '$stateParams', 'messages',
            function ($scope, $state, $rootScope, lookupDataService, evrSearchService, sharedDataService, blockUI, $stateParams, messages) {

            var evrviewData = {};
            var fileContent;
            $scope.fileType = [];
            $scope.fileTypeObj = [];
            var ipmeVRDetsReadonlyBlockUI = blockUI.instances.get('ipmeVRDetsReadonlyBlockUI');
            $scope.back = $stateParams.back;

            function bindwithUI() {
                $scope.FactsComment = evrviewData.Facts;
                $scope.ActionsComment = evrviewData.Action;
                $scope.ResultComment = evrviewData.Result;
                $scope.evrstatus = evrviewData.VrStatusName;
                $scope.evrNo = evrviewData.VrNo;
                //from here
                $scope.IsCritical = evrviewData.IsCritical;
                $scope.StaffNumber = evrviewData.StaffNumber;

                angular.forEach(evrviewData.VRAllCrewDetail, function (VRCrewDetail) {
                    if (VRCrewDetail.StaffNumber == evrviewData.StaffNumber)
                        $scope.StaffName = VRCrewDetail.StaffName;
                });

                $scope.Grade = evrviewData.Grade;
                $scope.IsCSR = evrviewData.IsCSR;
                $scope.IsOHS = evrviewData.IsOHS;
                $scope.IsPO = evrviewData.IsPO;
                $scope.ReportAboutName = evrviewData.ReportAboutName;
                $scope.CategoryName = evrviewData.CategoryName;
                $scope.SubCategoryName = evrviewData.SubCategoryName;
                $scope.IsCabInClassFC = evrviewData.IsCabInClassFC;
                $scope.IsCabInClassJC = evrviewData.IsCabInClassJC;
                $scope.IsCabInClassYC = evrviewData.IsCabInClassYC;
                $scope.deptgrid.data = evrviewData.VRDeptDetail;
                $scope.passengergrid.data = evrviewData.VRPassengerDetail;
                $scope.crewdetailsgrid.data = evrviewData.VRCrewDetail;
                $scope.VrStatusName = evrviewData.VrStatusName;
                $scope.CrewComments = evrviewData.CrewComments;
                $scope.attachmentgrid.data = evrviewData.VRDocumentDetail;
                $scope.actionResgrid.data = evrviewData.vrAtDeptList;
            };

            //$scope.BackToEVRList = function () {
            //    $state.go($stateParams.back);
            //};

            function loadData() {
                
                ipmeVRDetsReadonlyBlockUI.start();

                evrSearchService.getEVRDetails($stateParams.evrSubmtdId, function (success) {

                    if (success !== null) {
                        evrviewData = success;
                        bindwithUI();
                    } else {
                    }

                    ipmeVRDetsReadonlyBlockUI.stop();

                }, function (error) {
                    ipmeVRDetsReadonlyBlockUI.stop();
                });
            };

            function getMimeType() {
                sharedDataService.getCommonInfo('MIMETYPE', function (result) {
                    angular.forEach(result, function (data) {
                        $scope.fileType.push(data.Text);
                        $scope.fileTypeObj.push(data);
                    });
                }, function (error) {

                });
            };

            $scope.getVRFile = function (data) {

                var VRDocumentDetailModel = {};
                VRDocumentDetailModel = data;

                evrSearchService.postEVRFileDownload(VRDocumentDetailModel, function (result) {
                    if (result !== null) {

                        data.FileName = data.VrDocName;
                        data.FileContent = data.VrDocContent;
                        sharedDataService.openFile(data, $scope.fileTypeObj);

                        //var evrfiledwnld = document.createElement("a");
                        //document.body.appendChild(evrfiledwnld);
                        //evrfiledwnld.style = "display: none";
                        //var blob = new Blob([result.data], { type: "octet/stream" }),
                        //    url = window.URL.createObjectURL(blob);
                        //evrfiledwnld.href = url;
                        //evrfiledwnld.download = data.VrDocName;
                        //evrfiledwnld.click();
                        //window.URL.revokeObjectURL(url);                        
                    } else {
                        toastr.error(messages.UNABLETOOPENFILE);
                    }
                }, function (error) {
                    toastr.error(messages.UNKNOWNERROCCURED);
                });
            };

            function initialize() {
                getMimeType();

                $scope.deptgrid = {
                    gridApi: {},
                    data: [],
                    subgrid: 'false',
                    columnDefs: [
                          { field: "DeptName", name: "Department", enableHiding: false },
                          { field: "SectionName", name: "Section", width: "33.3%", enableHiding: false },
                          { field: "DeptType", name: "Type", width: "33.4%", enableHiding: false }
                    ]
                };

                $scope.passengergrid = {
                    gridApi: {},
                    data: [],
                    subgrid: 'false',
                    columnDefs: [
                          { field: "FFPNumber", displayName: "FFP Number", width: "33.3%", enableHiding: false },
                          { field: "PassengerName", name: "Passenger Name", enableHiding: false },
                          { field: "SeatNumber", name: "Seat Number", width: "30%", enableHiding: false }
                    ]
                };

                $scope.crewdetailsgrid = {
                    gridApi: {},
                    data: [],
                    subgrid: 'false',
                    columnDefs: [
                          { field: "StaffNumber", name: "Crew Staff Id", width: "30%", enableHiding: false },
                          { field: "StaffName", name: "Crew Staff Name",  enableHiding: false },
                          { field: "Grade", name: "Grade", width: "30%", enableHiding: false }
                    ]
                };

                $scope.attachmentgrid = {
                    gridApi: {},
                    data: [],
                    subgrid: 'false',
                    columnDefs: [
                          {
                              field: "VrDocName", name: "Attached Documents", width: "100%", enableHiding: false,
                              cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.getVRFile(row.entity)">{{row.entity.VrDocName}}</a><div>'
                          }
                    ]
                };

                $scope.actionResgrid = {
                    gridApi: {},
                    data: [],
                    subgrid: 'false',
                    columnDefs: [
                          { field: "VRNo", displayName: "VR No", width: "12%", enableHiding: false },
                          { field: "DeptCode", name: "Code", width: "12%", enableHiding: false },
                          { field: "DepartmentName", name: "Name", width: "20%", enableHiding: false },
                          { field: "Comment", name: "Comment", enableHiding: false },
                          { field: "StaffName", name: "StaffName", width: "20%", enableHiding: false },
                    ]
                };

                loadData();
            };

            initialize();
        }]);
