"use strict";
angular.module('app.shared.components').directive('srdCheckAllcheckboxList', [function () {
    return {
        restrict: 'E',
        scope: {
            list: '=',
            label: '@',
            model: '=',
            required:'@'
            

        },
        templateUrl: '/app/shared/directives/CheckAllcheckboxList/srd-checkAllcheckboxList.html',
        controller: ['$scope', function ($scope) {

        
            $scope.model = [];

            // functionality to check All option in checklist box
            $scope.checkAll = function () {
                debugger;
                var i = 0;
                $scope.model.splice(0, $scope.model.length);
                if ($scope.checked) {
                    for(var i in $scope.list)
                    {
                        $scope.model.push($scope.list[i]);
                    }
                   
                    }

                else {
                    $scope.model.splice(0, $scope.model.length);
                }
            };
        
        }]
    };
}]);

