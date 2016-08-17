'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.stationInfo.controller', ['$scope', '$http',  'stationInfoService',  'analyticsService',  '$state', '$rootScope', '$sce', 'blockUI','$stateParams',
    function ($scope, $http, stationInfoService, analyticsService, $state, $rootScope, $sce, blockUI, $stateParams) {
        //Controller Scope Initialization
        var iblockUI = blockUI.instances.get('ipmBlockUIRoster');//ipmStationInfoBlockUI
        var Initialize = function () {
            //Declare all scop properties

            $scope.Yes = 1;
            $scope.No = 2;
            $scope.Loading = 0;
            $scope.isDataLoad = $scope.Loading;

            $rootScope.stationInfoViewModel = [];
            $scope.filter = {
                StationCode: ''
            }

            if ($rootScope.FilterSearchData  && $rootScope.FilterSearchData.City) {

                if ($rootScope.FilterSearchData.City && $rootScope.FilterSearchData.City != null && $rootScope.FilterSearchData.City && $rootScope.FilterSearchData.City != '') {
                    $scope.filter = {
                        StationCode: $rootScope.FilterSearchData.City
                    }
                    loadStationInfoList($scope.filter, true);
                }
            } else {

                if ($rootScope.selectedRosterItem) {
                    $scope.filter = {
                        StationCode: $rootScope.selectedRosterItem.Arrival
                    }
                    loadStationInfoList($scope.filter, $rootScope.StationInfoIsReload);
                }
            }

            $rootScope.selectedStationInfo = '';

           
        }

        function loadStationInfoList(filter, isReload) {
            iblockUI.start();
            stationInfoService.getStationInfoList(filter, function (result) {

                $rootScope.stationInfoViewModel = result.data.StationInfo;

                $scope.isDataLoad = $rootScope.stationInfoViewModel.IsDataLoaded;
                //$rootScope.stationInfoViewModel.CrewInformation = $sce.trustAsHtml($rootScope.stationInfoViewModel.CrewInformation);
                //$rootScope.stationInfoViewModel.CustomerInformation = $sce.trustAsHtml($rootScope.stationInfoViewModel.CustomerInformation);
                UIChanges();
                $rootScope.StationInfoIsReload = false;
                iblockUI.stop();
            },
            function (error) {
                iblockUI.stop();
            }, null, isReload 
           );
        }

        $scope.decodeText = function (data) {
            if (data && data.lenght>0) {
                return $sce.trustAsHtml(data);
            }
            return '';
        }
        $scope.isSelected = function (section) {

            return $rootScope.selectedStationInfo === section;
        }

        Initialize();
        function UIChanges() {
            //$('#stationInfoTab').tabCollapse();
        }
    }]);
