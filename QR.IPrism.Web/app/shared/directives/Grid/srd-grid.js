"use strict";
angular.module('app.shared.components').directive('srdGrid', [function ($scope) {
    return {
        restrict: 'EA',
        scope: {
            srdgridsource: '=',
            columndefs: '=',
            subGrid: '@',
            onrowexpand: '&',
            onrowselect: '&',
            gridapi: '='
        },
        templateUrl: '/app/shared/directives/Grid/srd-grid.html',

        link: function (scope, elm, attrs) {
            scope.onchange = function (rows, e) {

                scope.onrowselect(rows, e);
            },
            scope.onexpand = function (row) {

                scope.onrowexpand(row);
            }
        },
        controller: ['$scope', function ($scope) {
            $scope.srdGridOptions = {
                data: 'srdgridsource',
                enableSorting: true,
                enableFiltering: false,
                noUnselect: true,
                multiSelect: false,
                enableRowSelection: true,
                enableFullRowSelection: true,
                enableRowHeaderSelection: false,
                modifierKeysToMultiSelect: true,
                enableColumnResizing: true,
                virtualizationThreshold: 50,
                columndefs: $scope.columndefs,
                onRegisterApi: function (gridapi) {
                    $scope.gridapi = gridapi;
                    //onrow Expansion
                    if ($scope.subGrid === 'true') {
                        gridapi.expandable.on.rowExpandedStateChanged($scope, function (row) {
                            if (row.isExpanded) {
                                //TODO:check with global scope passing params
                                //call local scope change
                                $scope.onexpand({ row: row });
                            }
                        });
                    }

                    //onrow selection
                    gridapi.selection.on.rowSelectionChanged($scope, function (rows, e) {
                        if (e.target.localName == "i") {
                            gridapi.selection.clearSelectedRows();
                        }
                        else if (e.target.localName == "div") {
                            //TODO:check with global scope passing params
                            //call local scope change
                            $scope.onchange({ rows: rows, e: e })
                        }


                    });

                }
                
            };
            //if ($scope.subGrid === 'true') {
            //    $scope.srdGridOptions.expandableRowTemplate = 'app/admin/partials/subGrid.tpl.html';
            //}
        }]
    };
}]);