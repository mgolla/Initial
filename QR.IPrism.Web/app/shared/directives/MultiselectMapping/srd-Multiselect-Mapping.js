"use strict";
angular.module('app.shared.components').directive('srdMultiselectMapping', [function () {
    return {
        restrict: "E",
        scope: {
            label: '@',
            masterlist: '=',
            modelname: '=',
            namefield: '='
        },
        templateUrl: '/app/shared/directives/MultiselectMapping/srd-Multiselect-Mapping.html',
        controller: ['$scope', function ($scope) {
            
            //Select/un-select all function
            $scope.checkAll = function (checkAllList) {
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
            $scope.clearFilter = function () {
                $scope.searchText = "";
            }
        }]        
    };

}]);