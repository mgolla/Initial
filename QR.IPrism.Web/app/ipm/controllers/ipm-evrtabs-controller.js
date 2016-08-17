'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.evrtabs.controller', ['$scope', '$state', '$rootScope', 'lookupDataService', 'evrSearchService', 'blockUI', '$stateParams',
        function ($scope, $state, $rootScope, lookupDataService, evrSearchService, blockUI, $stateParams) {

            $scope.evrDraftheading;
            $scope.evrupdateparams = null;
            $scope.isReadOnly = false;
            $rootScope.isFrmtabActive = false;
            $rootScope.isDrfttabActive = false;
            $rootScope.isFlgtDettabActive = false;
            $scope.url = null;
            $scope.model = {};
            $scope.evrTabModel = {};

            $scope.evrTabModel.evrReqFeilds = {
                flightDetsID: $stateParams.FlightDetsID,
                evrdrfid:null,
                evrno: null,
                evrInstanceId: null
            };

            $scope.model.active = {
                FD: true,
                evrForm: false,
                evrDraft: false
            }

            $scope.evrBackfrmTabs = function (val) {
                $state.go(val, {}, { reload: true });
            }

            $scope.onEvrTabChange = function (tab) {

                $scope.model.active[tab] = true;

                if (tab == 'FD') {
                    
                    $scope.isReadOnly = true;

                    //$rootScope.isFrmtabActive = false;
                    //$rootScope.isDrfttabActive = false;
                    //$rootScope.isFlgtDettabActive = true;
                    $scope.url = '/app/ipm/partials/ipmFlightDelayEnterTabDetails.html';
                    //$state.go("evrtabs.flghtdt");
                }
                else if (tab == 'evrForm') {
                  
                    //$rootScope.isFrmtabActive = true;
                    //$rootScope.isDrfttabActive = false;
                    //$rootScope.isFlgtDettabActive = false;
                    $scope.url = '/app/ipm/partials/ipmEVRMain.html';
                    //$state.go("evrtabs.evrForm", {
                    //    evrdrfid: $stateParams.evrdrfid,
                    //    evrno: $stateParams.evrno
                    //});
                }
                else if (tab == 'evrDraft') {
                    
                    //$rootScope.isFrmtabActive = false;
                    //$rootScope.isDrfttabActive = true;
                    //$rootScope.isFlgtDettabActive = false;
                    $scope.url = '/app/ipm/partials/ipmEVRDrafts.html';
                    //$state.go("evrtabs.evrDraft");
                }
            };

            function initialize() {
                evrSearchService.getevrDraftForUser($stateParams.FlightDetsID, function (result) {
                    $scope.evrDraftheading = "Draft eVR (" + result.length + ")";
                },
                function (error) {
                });

                $scope.model.active['evrForm']=true;

                //if ($stateParams.isfrom != null && $stateParams.isfrom.toUpperCase() == 'DELDRFT') {
                //    $scope.model.active['evrDraft'];//$scope.onEvrTabChange('evrDraft');
                //}
                //else
                //    $scope.model.active['evrForm'];
            }


            initialize();
        }]);