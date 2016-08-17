
angular.module('app.shared.components').controller('ipm.ivrkeycontacts.controller', ['$scope', '$http', '$location', 'sharedDataService', 'blockUI',
    function ($scope, $http, $location, sharedDataService, blockUI) {

        var ipmkeycontactsBlockUI = blockUI.instances.get('ipmkeycontactsBlockUI');

        function loadData() {

            ipmkeycontactsBlockUI.start();
            sharedDataService.getKeyContacts('', function (result) {

                $scope.keycontactsgrid.data = result;
                ipmkeycontactsBlockUI.stop();
            }, function (error) {
                ipmkeycontactsBlockUI.stop();
            });
        };

        function initialize() {

            $scope.keycontactsgrid = {
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
                        {
                            field: "TITLE", name: "Office", enableHiding: false,
                            cellTemplate: '<div class="ui-grid-cell-contents" title="{{row.entity.TITLE}}">{{row.entity.TITLE}}<div>'
                        },
                        {
                            field: "EMAIL", name: "Email", enableHiding: false,
                            cellTemplate: '<div class="ui-grid-cell-contents" title="{{row.entity.EMAIL}}">{{row.entity.EMAIL}}<div>'
                        },
                        {
                            field: "PHONE", name: "Tel", enableHiding: false, width: "15%",
                            cellTemplate: '<div class="ui-grid-cell-contents" title="{{row.entity.PHONE}}">{{row.entity.PHONE}}<div>'
                        },
                        {
                            field: "MOBILE", name: "Mobile", enableHiding: false, width: "15%",
                            cellTemplate: '<div class="ui-grid-cell-contents" title="{{row.entity.MOBILE}}">{{row.entity.MOBILE}}<div>'
                        },
                        {
                            field: "FAX", name: "Fax", enableHiding: false, width: "10%",
                            cellTemplate: '<div class="ui-grid-cell-contents" title="{{row.entity.FAX}}">{{row.entity.FAX}}<div>'
                        }
                ]
            };

            loadData();

        };

        initialize();

    }]);