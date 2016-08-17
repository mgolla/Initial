"use strict";
angular.module('app.shared.components').directive('srdMultiselectCheckbox', [function () {
    return {
        restrict: 'EA',
        scope: {
            label: '@', //Label name
            masterlist: '=', //Master list
            modelname: '=', //Model name
            namefield: '='
        },
        templateUrl: '/app/shared/directives/MultiselectCheckbox/srd-Multiselect-Checkbox.html',
        controller: ['$scope', function ($scope) {

            $scope.checkAll = function (checkAllList) {

                //Select/un-select all function
                if (checkAllList) {
                    for (var i = $scope.modelname.length; i > 0 ; i--) {
                        $scope.modelname.splice(0, 1);
                    }
                    for (var i = 0; i < $scope.masterlist.length; i++) {
                        $scope.modelname.push($scope.masterlist[i]);
                    }
                }
                else {
                    for (var i = $scope.modelname.length; i > 0 ; i--) {
                        $scope.modelname.splice(0, 1);
                    }
                }
            };

            //Clear search field
            $scope.clearSearch = function () {
                $scope.searchText = "";
            }

        }]
    };

}]);