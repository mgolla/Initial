'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.request.controller', ['$scope', '$http', '$uibModal', 'toastr', 'lookupDataService', 'requestService', 'ngDialog', 'analyticsService', 'CacheFactory', 'messages','blockUI',function ($scope, $http, $uibModal, toastr, lookupDataService, requestService, ngDialog, analyticsService, CacheFactory, messages,blockUI) {

        var ipmBlockGridLayout = blockUI.instances.get('ipmRequestEmdGridBlockUI');
        

        //Controller Scope Initialization
        var Initialize = function () {
            //Declare all scop properties
            $scope.titleList = [];
            $scope.genderList = [];
            $scope.contryList = [];
            $scope.gridDate = [];
            // initiate crew list grid
            $scope.grid = {
                gridApi: {},
                data: [],
                subgrid: 'false',
                columnDefs:
               [
                 { field: "EmpSeqId", name: "Seq No" },
                      { field: "StaffNumber", name: "Staff Number" },
                      { field: "FirstName", name: "First Name" },
                      { field: "MiddleName", name: "Middle Name" },
                      { field: "LastName", name: "Last Name" },
                      { field: "DOB", name: "Date of Birth" },
                      { field: "Country", name: "Country" },
                      { field: "Gender", name: "Gender" }
               ]
            };

            fnLoadMasterLists();
            loadCrewList();
        }
        //Initialize the controller
        Initialize();

        //This function pulls all the masters related to request screen
        function fnLoadMasterLists() {
            //Load titles
            
            lookupDataService.getLookupList('TitleCode', null, function (result) {
                $scope.titleList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });
            });

            //Load gender list
            lookupDataService.getLookupList('GenderCode', null, function (result) {
                $scope.genderList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });
            });

            //Load country list
            lookupDataService.getLookupList('CountryCode', null, function (result) {
                $scope.contryList = result.map(function (obj) { return { Value: obj.Value, Text: obj.Text } });
            });
        }
        $scope.drpdwnOnchange = function (selectedVal) {
            //alert(selectedVal);
        }

        //load crew list data
        function loadCrewList() {
            ipmBlockGridLayout.start();
            requestService.getCrewList(null,
                function (result) {

                    var crewList = result.map(function (obj) {
                        return {
                            EmpSeqId: obj.EmpSeqId, StaffNumber: obj.StaffNumber, FirstName: obj.FirstName, MiddleName: obj.MiddleName,
                            LastName: obj.LastName, DOB: obj.DOB, Country: obj.Country, Gender: obj.Gender
                        }
                    });
                    $scope.gridDate = crewList;
                    $scope.grid.data = crewList;
                    ipmBlockGridLayout.stop();
                },
                function (error) {
                    $scope.grid = null;
                    ipmBlockGridLayout.stop();
                }, null);
        }

        $scope.addNewRow = function () {
            $scope.dialogTitle = "Confirmation";
            $scope.dialogMessage = "Are you sure you want to submit?";
            ngDialog.open({
                scope: $scope,
                preCloseCallback: function (value) {
                    if (value == 'Post') {
                        postCrewDetails();
                        analyticsService.trackEvent('Action', 'Add', 'View', 'Cabin Crew View');
                    }
                }
            });
        }
        $scope.deleteRow = function () {
            toastr.success('Successfully deleted the crew details');
        }
        // Post Crew Details
        function postCrewDetails() {
            requestService.postCrewDetails(
                            $scope.model,
                            function (result) {
                                toastr.success('Successfully submitted the crew details');
                            },
                            function (result) {
                                toastr.error('Error occured while adding crew details');
                            },
                            function () {
                            });
        }
    }]);
