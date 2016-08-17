'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.kafouMy.controller', ['$scope', '$state', '$rootScope', 'kafouService', 'blockUI', 'messages', 'ngDialog', '$stateParams', 'sharedDataService',
        function ($scope, $state, $rootScope, kafouService, blockUI, messages, ngDialog, $stateParams, sharedDataService) {

    var ipmKFMyBlockUI = blockUI.instances.get('ipmKFMyBlockUI');

    $scope.model = {};

    $scope.model.dynamicColumns = {
        'menus': [
                { 'field': 'FlightNo', 'name': 'Flight No' },
                { 'field': 'FlightNo2', 'name': 'Flight No2' }
        ]
    };

    $scope.test = {
        "menu": {
            "id": "file",
            "value": "File",
            "popup": {
                "menuitem": [
                  { "value": "New", "onclick": "CreateNewDoc()" },
                  { "value": "Open", "onclick": "OpenDoc()" },
                  { "value": "Close", "onclick": "CloseDoc()" }
                ]
            }
        }
    }

    function setDate() {
        var currentDate = new Date();
        $scope.model.ToDateObj = angular.copy(currentDate);

        currentDate.setMonth(currentDate.getMonth() - 1);
        $scope.model.FromDateObj = currentDate;
    };

    $scope.ResetKFMy = function () {

        $scope.model = {};
        setDate();
        $state.go("kfmy", {}, { reload: true });
    };

    $scope.AddColumn = function () {
        var mywidth2 = (100 / ($scope.kfmygrid.columnDefs.length == 0 ? 1 : $scope.kfmygrid.columnDefs.length)) + '%';
        $scope.kfmygrid.columnDefs.push({ field: 'company', enableSorting: false, width: mywidth2 });

    };

    $scope.search = function () {

        $scope.model.FromDate = $scope.model.FromDateObj ? sharedDataService.getDateOnly($scope.model.FromDateObj) : '';
        $scope.model.ToDate = $scope.model.ToDateObj ? sharedDataService.getDateOnly($scope.model.ToDateObj) : '';

        getSearchData();
    };


    function getSearchData() {

        ipmKFMyBlockUI.start();
        kafouService.getKafouMyRecog($scope.model, function (success) {

            $scope.kfmygrid.data = [];
            $scope.kfmygrid.data = success.data;

            ipmKFMyBlockUI.stop();
        }, function (error) {
            ipmKFMyBlockUI.stop();
        });
    };

    $scope.GetCrewKafou = function (rowData) {
        $state.go('kfmy.kafoudetails', { FlightDetsID: rowData.FlightID, kfgrp: false, kfstatus: 'view', kfrecogid: rowData.RecognitionId, isFrom: 'myKafou' });
    };


    function initialize() {
        setDate();

        var myWidth = '10%';

        $scope.kfmygrid = {
            gridApi: {},
            enableFiltering: true,
            enableRowSelection: true,
            enableFullRowSelection: true,
            showGridFooter: true,
            paginationPageSizes: [5, 10, 15, 20, 25],
            paginationPageSize: 10,
            enablePagination: true,
            enablePaginationControls: true,
            data: [],
            subgrid: 'false',
            columnDefs: [
                  {
                      field: "FlightNumber", name: "Flight No", width: '33.3%', enableHiding: false,
                      cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.GetCrewKafou(row.entity)">{{row.entity.FlightNumber}}</a><div>'
                  },
                  {
                      field: "RecognitionDate", name: "DATE", enableHiding: false, width: '33.3%', sort: { direction: 'desc' },
                      cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity["RecognitionDate"] | date:"dd-MMM-yyyy (HH:mm)" }}</div>'
                  },
                  { field: "FlightDetails", name: "Flight Details", width: '33.4%', enableHiding: false }
            ]
        };

        //getSearchData();
        $scope.search();
    }

    initialize();

}]);