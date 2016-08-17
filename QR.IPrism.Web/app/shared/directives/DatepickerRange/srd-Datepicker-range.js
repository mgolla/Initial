"use strict";
angular.module('app.shared.components').directive('srdDatepickerRange', [function () {

    return {
        restrict: 'EA',
        templateUrl: '/app/shared/directives/DatepickerRange/srd-Datepicker-range.html',
        replace: true,
        scope: {
            dateAlignment: '@', //Date field allignment vertical/horizontal
            startDateClass: '@', //Start date width class
            endDateClass: '@', //End date width class
            datelabel: '@',  //Date label
            modelfrom: '=',  //From date model
            modelto: '=', //To date model
            name: '@', //field name
            required: '=', //Field required attribute true/false
            form: '=', //Form name
            

        },
        link: function (scope, element, attrs) {
            //var from = angular.copy(scope.modelfrom);
            //var to = angular.copy(scope.modelto);
            //scope.modefrom = new Date(from).setHours(0, 0, 0, 0);
            //scope.modelto = new Date(to).setHours(0, 0, 0, 0);

            //Check field vertical/horizontal allignment

            if (scope.dateAlignment == 'V') {
                scope.startDateClass = 'col-md-12 horizontalfromdate';
                scope.endDateClass = 'col-md-12';
            }
            else {
                scope.startDateClass = 'col-md-6';
                scope.endDateClass = 'col-md-6';
            }
        },
        controller: ['$scope', function ($scope) {
            
            //Watches the value change of the scope variable selectedEntitlement.EffStartDate
            $scope.$watch('modelfrom', function (newval, oldval) {
                //$filter('STSdate')($scope.selectedEntitlementRule.ValidFrom);

                $scope.validateDates();
            }, true);
            //Watches the value change of the scope variable selectedEntitlement.EffEndDate
            $scope.$watch('modelto', function (newval, oldval) {
                //$filter('STSdate')(this.modelto);
                $scope.validateDates();
            }, true);

            $scope.validateDates = function validateDates() {
                var From = this.name + 'From';
                var To = this.name + 'To';

                if (this.modelfrom !== null) {
                    if (this.modelfrom !== null && this.modelto !== null) {
                        if (this.form[From] != null && this.form[To] != null) {
                            var endDate = new Date(this.modelto);
                            var startDate = new Date(this.modelfrom);
                            if (endDate < startDate) {

                                this.form[From].$setValidity('valid', false);
                                this.form[To].$setValidity('valid', false);
                                $scope.endBeforeStart = false;
                            } else {
                                this.form[From].$setValidity('valid', true);
                                this.form[To].$setValidity('valid', true);
                                $scope.endBeforeStart = true;

                            }
                        }
                    }
                }
            };
        }]

    };


}]);