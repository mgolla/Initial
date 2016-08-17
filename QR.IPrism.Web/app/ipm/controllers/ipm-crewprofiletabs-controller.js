
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm-crewprofiletabs-controller', ['$scope', '$state', 'blockUI',
        function ($scope, $state, blockUI) {

            var Initialize = function () {
                $state.go("cpPersonaldetails.details.dstvst");
            }

            $scope.onCrewTabChange = function (tab) {
                if (tab == 'DST') {
                    $state.go("cpPersonaldetails.details.dstvst");
                }
                else if (tab == 'QAL') {
                    $state.go("cpPersonaldetails.details.ql");
                }
                else if (tab == 'MyDoc') {
                    $state.go("cpPersonaldetails.details.mydoc");
                }
                else if (tab == 'IDP') {
                    $state.go("cpPersonaldetails.details.idp");
                }
            };

            Initialize();
        }]);