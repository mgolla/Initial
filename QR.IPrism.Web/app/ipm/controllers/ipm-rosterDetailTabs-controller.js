'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.rosterDetailTabs.controller', ['analyticsService', '$rootScope', '$stickyState', '$state', '$scope', 'deviceDetector',
        function (analyticsService, $rootScope, $stickyState, $state, $scope,deviceDetector) {

        var Initialize = function () {
            $rootScope.selectedRDetailURL = $scope.OverviewURL;
            $rootScope.selectedRDetailTab = $scope.Overview;              
           
        }
           

        $rootScope.isSelectedRDTab = function (tab) {

            return $rootScope.selectedRDetailTab === tab;

        };

        function getDefaultBackground() {

            if ($scope.image && !deviceDetector.isMobile()) {
                $("#myrostersection").height('520px');
            } else {
                console.log('reset image');
                $("#myrostersection").removeClass('.monthlyRoster_info .myroster_header');
                $("#myrostersection").removeAttr("style");
                $("#myrostersection").addClass('myroster_defaultbackground');
            }
        }
        function getDefaultBackgroundMonthly() {
            $("#myrostersection").removeAttr("style");
            $("#myroster").addClass('monthlyRoster_info');

        }

        $rootScope.closeTabContentClick = function () {          
            $rootScope.selectedRoster = -1;
            $rootScope.clickedRosterId = -1;

            $('[id^=roster_details]').fadeOut();
            $('[id^=roster_details]').hide();
            $(".rooster-day.actvRosterEvent").removeClass("actvRosterEvent");
                        
            if ($rootScope.selectedRosterType && $rootScope.selectedRosterType == 1) {
                getDefaultBackground();
            } else {
                getDefaultBackgroundMonthly();
            }
        };

        Initialize();
 }]);