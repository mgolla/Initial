/*********************************************************************
* File Name     : ipm-housing-services.js
* Description   : Contains all the service calls for housing module.
* Create Date   : 25th Jan 2016
* Modified Date : 25th Jan 2016
* Copyright by  : Qatar Airways
*********************************************************************/

angular.module('app.ipm.module')
        .factory('housingService', ['webAPIService', function (webAPIService) {

            this.hosungSearchReload;

            return {
                getHousingSearchResult: _getHousingSearchResult,
                getHousingVacantBuilding: _getHousingVacantBuilding,
                getOccupiedBuilding: _getOccupiedBuilding,
                getNationalityByFlat: _getNationalityByFlat,
                postmoveinrequest: _postmoveinrequest,
                postmovingout: _postmovingout,
                postguestacc: _postguestacc,
                poststayout: _poststayout,
                postswaproom: _postswaproom,
                getExistingAccomm: _getExistingAccomm,
                getExistingAccommById: _getExistingAccommById,
                cancelrequest: _cancelrequest,
                crewentitlement: _crewentitlement,
                // guestentitlement : _guestentitlement,
                getrequestdetails: _getrequestdetails,
                // getLastRequest: _getlastrequest,
                // validateSwapbyFriend: _validateswapbyfriend,
                getHousingdocByRequestId: _getHousingdocByRequestId,
                getHousingEntitlements: _housingEntitlements,
                guestdetails: _guestdetails,
                getCrewRelations: _getCrewRelations,
                getcomments: _getcomments,
                getStaffsByFlatId: _getStaffsByFlatId,
                getStayOutRequestType: _getstayoutrequestype,
                getSwapRoomsRequestType: _getSwapRoomsRequestType,
                getNotifications: _getNotifications,
                entitlementForCrew: _entitlementForCrew,
                updateSwapNotification: _updateSwapNotification,
                getStaffsForSwap: _getStaffsForSwap,
                deleteDocuments: _deleteDocuments
            }

            function _getHousingSearchResult(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/housingsearch/post', data, successCall, errorCall, alwaysCall);
            }

            function _getHousingVacantBuilding(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/getvacantbuilding', data, successCall, errorCall, alwaysCall);
            }

            function _getOccupiedBuilding(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/getoccupiedbuilding', data, successCall, errorCall, alwaysCall);
            }

            function _getNationalityByFlat(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/getnationalitybyflat/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _postmoveinrequest(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/housingpost', data, successCall, errorCall, alwaysCall);
            }

            function _postguestacc(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/submitguestacc', data, successCall, errorCall, alwaysCall);
            }

            function _postmovingout(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/submitmovingout', data, successCall, errorCall, alwaysCall);
            }

            function _poststayout(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/submitstayout', data, successCall, errorCall, alwaysCall);
            }

            function _postswaproom(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/submitswaproom', data, successCall, errorCall, alwaysCall);
            }

            function _getExistingAccomm(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/getexistingaccom', data, successCall, errorCall, alwaysCall);
            }

            function _getExistingAccommById(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/existingaccbyid/' + data, successCall, errorCall, alwaysCall);
            }
            
            function _getrequestdetails(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/getrequestdetails/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _cancelrequest(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/cancelrequest/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _crewentitlement(data, successCall, errorCall, alwaysCall) {
                webAPIService.apiGet('api/crewentitlement/', null, successCall, errorCall, alwaysCall);
            }

            //function _guestentitlement(data, successCall, errorCall, alwaysCall) {
            //    webAPIService.apiGet('api/guestentitlement/' + data, null, successCall, errorCall, alwaysCall);
            //}

            //function _getlastrequest(data, successCall, errorCall, alwaysCall) {
            //    webAPIService.apiGet('api/getlastrequest/' + data, null, successCall, errorCall, alwaysCall);
            //}

            //function _validateswapbyfriend(data, successCall, errorCall, alwaysCall) {
            //    webAPIService.apiGet('api/validateswapbyfriend/', null, successCall, errorCall, alwaysCall);
            //}

            function _getHousingdocByRequestId(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/gethousingdocument/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _getcomments(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/getcomments/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _housingEntitlements(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/housingentitlements/' + data, null, successCall, errorCall, alwaysCall);
            }

            function _getStaffsByFlatId(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/staffsbyflatid', data, successCall, errorCall, alwaysCall);
            }

            function _getStaffsForSwap(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/staffsforswap', data, successCall, errorCall, alwaysCall);
            }

            function _guestdetails(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/guestdetails', null, successCall, errorCall, alwaysCall);
            }

            function _getCrewRelations(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/getcrewrelations', null, successCall, errorCall, alwaysCall);
            }

            function _getstayoutrequestype(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/getstayoutrequestype', null, successCall, errorCall, alwaysCall);
            }

            function _getSwapRoomsRequestType(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/getswaproomsrequesttype', null, successCall, errorCall, alwaysCall);
            }

            function _getNotifications(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiGet('api/housingnotification/' + data, null, successCall, errorCall, alwaysCall);
            }

          
            function _entitlementForCrew(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/entitlementforcrew/', data, successCall, errorCall, alwaysCall);
            }

            function _updateSwapNotification(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/updateswapnotification/', data, successCall, errorCall, alwaysCall);
            }

            function _deleteDocuments(data, successCall, errorCall, alwaysCall) {

                webAPIService.apiPost('api/deleteHousingDoc/', data, successCall, errorCall, alwaysCall);
            }

        }]);