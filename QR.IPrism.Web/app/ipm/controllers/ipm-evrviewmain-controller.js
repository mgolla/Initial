
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.evrviewmain.controller', ['$scope', '$http', '$state', '$rootScope',
        'flightDetailsAddEditService', 'blockUI',

        function ($scope, $http, $state, $rootScope,
            flightDetailsAddEditService, blockUI) {

            var Initialize = function () {
                //loadData();
            }
            $scope.onEvrMainTabChange = function (tab) {

                $scope.model.active[tab] = true;

                if (tab == 'evrFltDetails') {
                    $scope.selectedEvrURL = '/app/ipm/partials/ipmFlightDelayEnterTabDetails.html';
                    //$state.go("evrlstsState.topgrid");
                }
                else if (tab == 'evrVgReport') {
                    $scope.selectedEvrURL = '/app/ipm/partials/ipmEVRSubmitted.html';
                    //$state.go("evrlstsState.topten");
                }
            };
            function loadData() {
                $state.go('evrviewState.details');
            }

            //$scope.BackToEVRList = function () {
            //    $state.go('evrlstsState');
            //};

            Initialize();

        }]);