'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.training.controller', ['$scope', '$http', 'rosterService','searchService', 'analyticsService', 'messages', 'sharedDataService', '$state', '$rootScope', '$timeout', '$compile', 'blockUI', '$stickyState',
    function ($scope, $http, rosterService,searchService, analyticsService, messages, sharedDataService, $state, $rootScope, $timeout, $compile, blockUI, $stickyState) {
                                     
        var Initialize = function () {

            $scope.trainingCodeEx;
            $scope.transportDetails;

            $scope.filter = {

                TrainCode: '',
                TrainDate: '',
                IsTraining:true
            };

            if ($rootScope.selectedRosterItem && $rootScope.selectedRosterItem.Cc != null && $rootScope.selectedRosterItem.Cc != '') {
                $scope.filterCode = $rootScope.selectedRosterItem.Cc;

                loadLookupList($scope.filterCode);
                if ($rootScope.selectedRosterItem && $rootScope.selectedRosterItem.StandardTimeDepartureUtc != null && $rootScope.selectedRosterItem.StandardTimeDepartureUtc != '') {
                    $scope.filter.TrainCode = $rootScope.selectedRosterItem.Cc;
                    var tranDate = $rootScope.selectedRosterItem.StandardTimeDepartureUtc.toString().split(' ')
                    $scope.filter.TrainDate = tranDate[0];
                    $scope.filter.IsTraining = true;
                    getTransportsList($scope.filter);
                }
               
            }

           
           
        }

        $scope.dateDiffInDays = function (from, to) {
            if (from && to && from.length > 0 && to.length > 0) {
                var fromD = new Date(from);
                var toD = new Date(to);
                var min = 1000 * 60;
                if (fromD && toD && fromD.getDay().toString().length > 0 && toD.getDay().toString().length > 0) {
                    var diff = Math.floor((toD - fromD) / min);
                    var minutes = diff % 60;
                    var hours = (diff - minutes) / 60;
                    var final = '';
                    if (hours > 0) {
                        final = hours + 'hr ';
                    }
                    if (minutes > 0) {
                        final = final + minutes + 'min';
                    }

                    return final;
                }
                else {
                    return '';
                }
            }
        }

        function loadLookupList(filter) {

            rosterService.getCodeExplanations(filter, function (result) {
                $scope.trainingCodeEx = result;

            },
            function (error) {

            }
            );
        }

        function getTransportsList(filter) {

            searchService.getTransports(filter, function (result) {
                $scope.transportDetails = result.data;

            },
            function (error) {

            }
          );
        }

        Initialize();
    }]);
