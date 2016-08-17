'use strict'
angular.module('app.shared.components').factory('sharedDataService', ['webAPIService', '$location', '$rootScope', '$state', 'messages',
function (webAPIService, $location, $rootScope, $state, messages) {
    var dict = {};

    this.notificationDetails = null;

    return {
        getMenuList: _getMenuList,
        getCommonInfo: _getCommonInfo,
        getAlterNotificationHeaderList: _getAlterNotificationHeaderList,
        //getCrewPersonalDetailsHeader: _getCrewPersonalDetailsHeader,
        updateAlterNotificationHeader: _updateAlterNotificationHeader,
        openDocTypes: _openDocTypes,
        getBaseURL: _getBaseURL,
        getAllHeaderInfo: _getAllHeaderInfo,
        getStatusType: _getStatusType,
        updateNotifications: _updateNotifications,
        updateBehaviourNotifications: _updateBehaviourNotifications,
        getNotification: _getNotification,
        getBehaviourNotification: _getBehaviourNotification,
        getUserContext: _getUserContext,
        refreshContext: _refreshContext,
        impUserContext: _impUserContext,
        processToken: _processToken,
        logOut: _logOut,
        sLogOut: _sLogOut,
        redirect: _redirect,
        getToken: _getToken,
        getKeyContacts: _getKeyContacts,
        openFile: _openFile,
        getLoggedInStaffNoAndGrade: _getLoggedInStaffNoAndGrade,
        conditionalCheck: _conditionalCheck,
        getDateOnly: _getDateOnly,
        sortCrewDetails: _sortCrewDetails,
        getTableHeight: _getTableHeight,
        getConfigFromDBForKey: _getConfigFromDBForKey
    }

    function _getTableHeight(grid) {
        if (grid) {
            var rowHeight = grid.rowHeight ? grid.rowHeight : 30;
            var headerHeight = 120;//$scope.grid.headerRowHeight ? $scope.grid.headerRowHeight : 150;
            var gridFooterHeight = grid.gridFooterHeight ? grid.gridFooterHeight : 30;
            var size = grid.data.length > 0 ? (grid.data.length > 10 ? 10 : grid.data.length) : 1;

            return {
                height: (size * rowHeight + headerHeight + gridFooterHeight) + "px"
            };
        }
    }

    function _sortCrewDetails(data) {

        var FlightCrewsDetail = [];

        var CSDmembers = $.grep(data, function (v, i) {

            return v.StaffGrade == messages.CSD;
        });

        var CSmembers = $.grep(data, function (v, i) {

            return v.StaffGrade == messages.CS;
        });

        var F1members = $.grep(data, function (v, i) {

            return v.StaffGrade == messages.F1;
        });

        var F2members = $.grep(data, function (v, i) {

            return v.StaffGrade == messages.F2;
        });

        var LTmembers = $.grep(data, function (v, i) {

            return v.StaffGrade == "LT";
        });

        var POmembers = $.grep(data, function (v, i) {

            return v.StaffGrade == "PO";
        });

        return POmembers.concat(LTmembers).concat(CSDmembers).concat(CSmembers).concat(F1members).concat(F2members);
    }


    function _getDateOnly(dateVal) {
        var dt = new Date(dateVal);
        if (dt.toString() != "Invalid Date") {
            return dt.getMonth() + 1 + "/" + dt.getDate() + "/" + dt.getFullYear();
        } else {
            return null;
        }
    }

    function _conditionalCheck(val) {
        if (val.trim() == "Y") {
            return 'Yes';
        } else if (val.trim() == "N") {
            return 'No';
        } else if (val.trim() == "NA" || val.trim() == "-1") {
            return 'NA';
        }
        else {
            return val;
        }
    }

    function _getStatusType() {
        return {
            "A": "Approved",
            "Y": "Approved",
            "U": "Submitted",
            "R": "Rejected",
            "N": "Rejected",
            "I": "In Progress",
            "C": "Closed",
            "S": "Sent For Approval",
            "O": "Assigned ",
            "T": "Transferred",
            "D": "Done",
            "Completed": "Completed"
        };
    }

    function _getBehaviourNotification(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiGet('api/getBehaviourNotification/' + data, null, successCall, errorCall, alwaysCall);
    }

    function _updateBehaviourNotifications(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPost('api/postBehaviourIdp/', data, successCall, errorCall, alwaysCall);
    }

    function _updateNotifications(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPost('api/updatenotification/', data, successCall, errorCall, alwaysCall);
    }

    function _getNotification(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiGet('api/notification/' + data, null, successCall, errorCall, alwaysCall);
    }

    //Gets all Menu List
    function _getMenuList(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiGet('api/usermenu/get', null, successCall, errorCall, alwaysCall);
    }
    function _getAllHeaderInfo(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPost('api/headerInfoAll/', data, successCall, errorCall, alwaysCall);
    }

    function _getCommonInfo(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiGet('api/commoninfo/' + data, null, successCall, errorCall, alwaysCall);
    }
    function _getAlterNotificationHeaderList(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPostSt('api/AlterNotificationHeader/post', data, successCall, errorCall, alwaysCall);
    }
    //function _getCrewPersonalDetailsHeader(data, successCall, errorCall, alwaysCall) {

    //    webAPIService.apiGetSt('api/crewpersonaldetailsheader/' + data, null, successCall, errorCall, alwaysCall);
    //}
    function _updateAlterNotificationHeader(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPostSt('api/AlterNotificationHeaderUpdate/post', data, successCall, errorCall, alwaysCall);
    }
    function _getFileList(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPost('api/File/post', data, successCall, errorCall, alwaysCall);
    }
    function _getPDFFile(data, successCall, errorCall, alwaysCall) {

        webAPIService.apiPostFile('api/pdf/post', data, successCall, errorCall, alwaysCall);
    }
    function _getUserContext(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPost('api/usercontext', data, successCall, errorCall, alwaysCall);
    }
    function _getShiftDltContext(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiPost('api/shiftdltcntxt', data, successCall, errorCall, alwaysCall);
    }
    function _refreshContext(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiGet('api/refreshecontext', null, successCall, errorCall, alwaysCall);
    }
    function _impUserContext(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiGet('api/getimpusrtoken/' + data, null, successCall, errorCall, alwaysCall);
    }

    function isNotNullOrEmpty(value) {
        return (value && value != null && value.toString().trim().length > 0);
    }
    function isNullOrEmpty(value) {
        return (!(value && value != null && value.toString().trim().length > 0));
    };

    function _processToken(result) {
        angular.element(document.getElementsByName('_MN_A')[0]).val(result);
    }
    function _getToken() {
        return angular.element(document.getElementsByName('_MN_A')[0]).val();
    }
    function _getBaseURL() {
        var pathArray = $location.$$absUrl.split('/');
        var protocol = pathArray[0];
        var host = pathArray[2];
        return protocol + '//' + host;
    }

    function _logOut() {
        var pathArray = $location.$$absUrl.split('/');
        var protocol = pathArray[0];
        var host = pathArray[2];
        angular.element(document.getElementsByName('_MN_A')[0]).val('');
        window.location = protocol + '//' + host + '/Logout';
    }
    function _sLogOut() {
        var pathArray = $location.$$absUrl.split('/');
        var protocol = pathArray[0];
        var host = pathArray[2];
        angular.element(document.getElementsByName('_MN_A')[0]).val('');
        window.location = protocol + '//' + host + '/TimeOut';
    }
    function _redirect(router) {
        var pathArray = $location.$$absUrl.split('/');
        var protocol = pathArray[0];
        var host = pathArray[2];
        angular.element(document.getElementsByName('_MN_A')[0]).val('');
        window.location = protocol + '//' + host + '/' + router;
    }
    function _openDocTypes(data, successCall, errorCall, alwaysCall) {
        var videoDocType = "VIDEO";
        var folderDocType = "FOLDER";
        var fileDocType = "FILE";
        var linkType = "Link";

        var emptyType = "";
        if (isNotNullOrEmpty(data.Doc) && data.Doc == "N") {
            //var housingRequestType = "HOUSING";
            //var assessmentType = "Assessment";
            //var gap = "Guest Approval";
            //var ca = "Change Accommodation";
            //var ga = "Guest Accommodation";
            //var mo = "Moving Out of Company Accommodation";
            //var so = "Stay Out Request";
            //var sr = "Swap Rooms";
            //var mi = "Moving In";
            //var swapRecepient = "Swap Room Request Recipient Approval";

            //var bidp = "Behavior IDP";

            console.log(data.DocType);

            if (isNotNullOrEmpty(data.DocType) &&
                (data.DocType.toLowerCase() == messages.ASMTSCHEDULED.toLowerCase() ||
                data.DocType.toLowerCase() == messages.ASMTREQUESTED.toLowerCase() ||
                data.DocType.toLowerCase() == messages.ASMTDELAYED.toLowerCase() ||
                data.DocType.toLowerCase() == messages.ASMTDELAYEDSAVED.toLowerCase() ||
                data.DocType.toLowerCase() == messages.ASMTCANCELLED.toLowerCase() ||
                data.DocType.toLowerCase() == messages.FMS.toLowerCase() ||
                data.DocType.toLowerCase() == messages.FILENOTEIDP.toLowerCase() ||
                data.DocType.toLowerCase() == messages.PACKAGENOTICE.toLowerCase())) {

                $state.go('asmntack', { reqno: data.DocumentId, reqtype: data.DocType });

            } else if (data.DocType.toLowerCase() == messages.IDPBEHAVIOUR.toLowerCase()) {

                $state.go('bevidp', { reqno: data.DocumentId, reqtype: data.DocType });

            } else if (isNotNullOrEmpty(data.DocType) && data.DocType.toLowerCase() == messages.ASMT.toLowerCase()) {

                $state.go('idpack', { reqno: data.DocumentId, reqtype: data.DocType });

            } else if (isNotNullOrEmpty(data.DocType) && data.DocType.toLowerCase() == messages.HOUSING.toLowerCase()) {

                $state.go('housing.housing-readonly-ChangeAcc', { RequestNumber: data.RequestNumber, RequestId: data.RequestId });

            } else if (isNotNullOrEmpty(data.DocType) &&
                data.DocType.toLowerCase() == messages.GUESTAPPROVAL.toLowerCase() ||
                data.DocType.toLowerCase() == messages.HOU1013.toLowerCase() ||
                data.DocType.toLowerCase() == messages.HOU1015.toLowerCase() ||
                data.DocType.toLowerCase() == messages.HOU1017.toLowerCase() ||
                data.DocType.toLowerCase() == messages.HOU1018.toLowerCase() ||
                data.DocType.toLowerCase() == messages.HOU1019.toLowerCase() ||
                data.DocType.toLowerCase() == messages.HOU1016.toLowerCase() ||
                data.DocType.toLowerCase() == messages.HOUSWAPREC.toLowerCase()) {

                //$rootScope.reqno = data.DocumentId;
                //$rootScope.reqtype = data.DocType;

                $state.go('housing-ack', { reqno: data.DocumentId, reqtype: data.DocType });
            }

        } else {

            if (isNotNullOrEmpty(data.DocType) && data.DocType.toLowerCase() == videoDocType.toLowerCase()) {

                $rootScope.selectedAlertTab = 0;
                var filterPara = {
                    FileCode: data.FileCode,
                    FileId: data.FileId
                };
                if (isNotNullOrEmpty(data.DocumentName)) {
                    var baseURL = _getBaseURL() + '/video/' + data.DocumentName;
                    var filter = {
                        DocType: data.DocType,
                        FileCode: data.FileCode,
                        FileId: data.FileId,
                        VideoURL: baseURL
                        //,
                        //VideoType: VideoType
                    };

                    $state.go('videofile', { fileFilter: filter });
                } else {
                    _getFileList(filterPara, function (result) {
                        var file = result.data.Files[0];
                        if (file && file != null) {
                            var VideoFileName = file.FileName;
                            var VideoType = file.FileType;
                            var baseURL = _getBaseURL() + '/video/' + VideoFileName;
                            var filter = {
                                DocType: data.DocType,
                                FileCode: data.FileCode,
                                FileId: data.FileId,
                                VideoURL: baseURL,

                            };
                            //VideoType: VideoType
                            //if (data.isTab) {
                            $rootScope.isTab = false;
                            //    $state.go('home.videofile', { fileFilter: filter });
                            //} else {
                            //    $rootScope.isTab = true;
                            $state.go('videofile', { fileFilter: filter });
                            //}

                        }
                    },
               function (error) {
                   toastr.error(error.data);
                   //console.log('error : ' + error);
               });
                }


            }
            if (isNotNullOrEmpty(data.DocType) && data.DocType.toLowerCase() == fileDocType.toLowerCase()) {
                var filter = {
                    FileCode: data.FileCode,
                    FileId: data.FileId
                };

                _getPDFFile(filter, function (result) {
                    if (result.data && result.data.byteLength > 0) {
                        var file = new Blob([result.data], { type: 'application/pdf' });

                        var fileURL = window.URL.createObjectURL(file);
                        window.open(fileURL);
                    } else {
                        toastr.warning('File not found!');
                    }
                },
                 function (error) {
                     toastr.error(error.data);
                     //console.log('error : ' + error);
                 });
            }
            if (isNotNullOrEmpty(data.DocType) && data.DocType.toLowerCase() == folderDocType.toLowerCase()) {
                if (data.isTab) {
                    $rootScope.isTab = false;


                }

                if (data.DocumentId && data.DocumentName) {
                    appSettings.DocumentId = data.DocumentId;
                    appSettings.DocumentPath = "/" + data.DocumentName;
                    $state.go('doclibfilemg');


                }


            }
            if (isNotNullOrEmpty(data.DocType) && data.DocType.toLowerCase() == linkType.toLowerCase() && isNotNullOrEmpty(data.Link)) {

                var win = window.open(data.Link, '_blank');
                win.focus();

            }


        }
    }

    function _openFile(file, mimetype) {

        var type = file.FileName.split(".");
        angular.forEach(mimetype, function (ftype) {
            if (ftype.Value.toLocaleLowerCase() == type[type.length - 1].toLocaleLowerCase()) {

                var datauri = 'data:' + ftype.Text + ';base64,' + file.FileContent;
                var win = window.open("width=1024,height=768,resizable=yes,scrollbars=yes,toolbar=no,location=no,directories=no,status=no,menubar=no,copyhistory=no");
                win.document.location.href = datauri;
                win.document.location.download = file.FileName;
            }
        });
    }



    //function _openDocTypes(data, successCall, errorCall, alwaysCall) {
    //    var videoDocType = "VIDEO";
    //    var folderDocType = "FOLDER";
    //    var fileDocType = "FILE";
    //    var linkType = "Link";

    //    var emptyType = "";
    //    if (data.Doc && data.Doc == "N") {
    //        var housingRequestType = "HOUSING";
    //        var assessmentType = "Assessment";
    //        var gap = "Guest Approval";
    //        var ca = "Change Accommodation";
    //        var ga = "Guest Accommodation";
    //        var mo = "Moving Out of Company Accommodation";
    //        var so = "Stay Out Request";
    //        var sr = "Swap Rooms";
    //        var mi = "Moving In";
    //        var swapRecepient = "Swap Room Request Recipient Approval";

    //        console.log(data.DocType);

    //        if (data.DocType == housingRequestType) {

    //            //$rootScope.RequestNumber = data.FileCode;
    //            //$rootScope.RequestId = data.FileId;

    //            $state.go('housing-readonly-ChangeAcc', { RequestNumber: data.RequestNumber, RequestId: data.RequestId });

    //        } else if (data.DocType == gap || data.DocType == ca || data.DocType == mo || data.DocType == ga
    //            || data.DocType == so || data.DocType == sr || data.DocType == mi || data.DocType == swapRecepient) {

    //            //$rootScope.reqno = data.DocumentId;
    //            //$rootScope.reqtype = data.DocType;

    //            $state.go('housing-ack', { reqno: data.DocumentId, reqtype: data.DocType });
    //        }

    //    } else {

    //        if (data.DocType.toLowerCase() == videoDocType.toLowerCase()) {

    //            $rootScope.selectedAlertTab = 0;
    //            var filterPara = {
    //                FileCode: data.FileCode,
    //                FileId: data.FileId
    //            };
    //            if (! isNullOrEmpty(data.DocumentName)) {
    //                var baseURL = _getBaseURL() + '/video/' + data.DocumentName;
    //                var filter = {
    //                    DocType: data.DocType,
    //                    FileCode: data.FileCode,
    //                    FileId: data.FileId,
    //                    VideoURL: baseURL,
    //                    VideoType: VideoType
    //                };

    //                $state.go('videofile', { fileFilter: filterPara });
    //            } else {
    //                _getFileList(filterPara, function (result) {
    //                    var file = result.data.Files[0];
    //                    if (file && file != null) {
    //                        var VideoFileName = file.FileName;
    //                        var VideoType = file.FileType;
    //                        var baseURL = _getBaseURL() + '/video/' + VideoFileName;
    //                        var filter = {
    //                            DocType: data.DocType,
    //                            FileCode: data.FileCode,
    //                            FileId: data.FileId,
    //                            VideoURL: baseURL,
    //                            VideoType: VideoType
    //                        };

    //                        $rootScope.isTab = false;

    //                        $state.go('videofile', { fileFilter: filter });


    //                    }
    //                },
    //                function (error) {
    //                    toastr.error(error.data);

    //                });
    //            }
    //        }
    //        if (data.DocType.toLowerCase() == fileDocType.toLowerCase()) {
    //            var filter = {
    //                FileCode: data.FileCode,
    //                FileId: data.FileId
    //            };

    //            _getPDFFile(filter, function (result) {
    //                var file = new Blob([result.data], { type: 'application/pdf' });

    //                var fileURL = window.URL.createObjectURL(file);
    //                window.open(fileURL);

    //            },
    //             function (error) {


    //             });
    //        }
    //        if (data.DocType.toLowerCase() == folderDocType.toLowerCase()) {
    //            if (data.isTab) {
    //                $rootScope.isTab = false;
    //            }

    //            if (data.DocumentId && data.DocumentName) {
    //                appSettings.DocumentId = data.DocumentId;
    //                appSettings.DocumentPath = "/" + data.DocumentName;
    //                $state.go('doclibfilemg');


    //            }


    //        }


    //    }
    //}


    function fileuploader() {

        var uploader = $scope.uploader = new FileUploader({
            url: appSettings.API + 'api/fileupload'
        });

        // File must be jpeg or png
        uploader.filters.push({
            name: 'mime_type_filter',
            fn: function (item) {

                var allowedFileType = $scope.fileType.indexOf(item.type) > 0 ? true : false;
                if (!allowedFileType) {
                    toastr.error(messages.HOU1020);
                }
                return allowedFileType;
            }
        });

        // FILTERS
        uploader.filters.push({
            name: 'customFilter',
            fn: function (item /*{File|FileLikeObject}*/, options) {
                return this.queue.length < 10;
            }
        });

        uploader.onBeforeUploadItem = function (item) {
            item.formData = [{ 'RequestId': $scope.requestId }];
        };

        uploader.onCompleteAll = function () {
            toastr.info(messages.HOUCREATESUCCESS.replace('@reqno', $scope.requestNumber));
            $state.go('housing');
        };
    };

    function _getKeyContacts(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiGet('api/getkeycontacts/', data, successCall, errorCall, alwaysCall);
    };

    function _getLoggedInStaffNoAndGrade(successCall, errorCall, alwaysCall) {
        webAPIService.apiGet('api/getLoggedInStaffNoAndGrade/', successCall, errorCall, alwaysCall);
    };

    function _getConfigFromDBForKey(data, successCall, errorCall, alwaysCall) {
        webAPIService.apiGet('api/getconfigfromdb/' + data, null, successCall, errorCall, alwaysCall);
    }

}]);