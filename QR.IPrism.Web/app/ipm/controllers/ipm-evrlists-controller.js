'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.evrlists.controller', ['$scope', '$state',
        function ($scope, $state) {


            $scope.model = {};
            $scope.selectedEvrURL = null;

            $scope.model.active = {
                evrtop: true,
                evrtopten: false
            }

            $scope.onEvrTabChange = function (tab) {

                $scope.model.active[tab] = true;

                if (tab == 'evrtop') {
                    $scope.selectedEvrURL = '/app/ipm/partials/ipmEVRListTopGrid.html';
                    //$state.go("evrlstsState.topgrid");
                }
                else if (tab == 'evrtopten') {
                    $scope.selectedEvrURL = '/app/ipm/partials/ipmEVRListTopTen.html';
                    //$state.go("evrlstsState.topten");
                }
            };

            function initialize() {
                $scope.onEvrTabChange('evrtop');
            }

            initialize();

        }]);