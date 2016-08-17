
angular.module('app.ipm.module').factory('fileService', ['webAPIService', '$window', '$http', '$sce', '$location', '$state',
    '$rootScope', 'toastr', 'messages', 'sharedDataService',
    //'$cordovaInAppBrowser',
    function (webAPIService, $window, $http, $sce, $location, $state, $rootScope, toastr, messages, sharedDataService
        //, $cordovaInAppBrowser

        ) {
        return {
            getFileList: _getFileList,
            downloadFile: _downloadFile,
            getBaseURL: _getBaseURL,
            getPDFFile: _getPDFFile,
            openDocTypes: _openDocTypes
        }
        function _getFileList(data, successCall, errorCall, alwaysCall) {

            webAPIService.apiPost('api/File/post', data, successCall, errorCall, alwaysCall);
        }
        function _getPDFFile(data, successCall, errorCall, alwaysCall) {

            webAPIService.apiPostFile('api/pdf/post', data, successCall, errorCall, alwaysCall);
        }

        function _downloadFile(data, successCall, errorCall, alwaysCall) {



            _getPDFFile(data, function (result) {


                if (result.statusText != 'No Content') {
                    var file = new Blob([result.data], { type: 'application/pdf' });
                    var fileURL = window.URL.createObjectURL(file);
                    return fileURL;
                } else {
                    toastr.error(messages.FILENOTFOUND);
                }




            });

        }
        function _getBaseURL() {
            var pathArray = $location.$$absUrl.split('/');
            var protocol = pathArray[0];
            var host = pathArray[2];
            return protocol + '//' + host;
        }

        function isNotNullOrEmpty(value) {
            return (value && value != null && value.toString().trim().length > 0);
        }
        function isNullOrEmpty(value) {
            return (!(value && value != null && value.toString().trim().length > 0));
        };

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
                        };

                        //VideoType: VideoType
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
                                    VideoType: VideoType
                                };
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

                    var type = 'pdf';
                    var name = 'name.pdf';
                    if (data.DocumentName && data.DocumentName.length > 0) {
                        name = data.DocumentName;
                        var type = data.DocumentName.split('.');
                        if (type.length > 1) {
                            type = type[type.length - 1];
                        }
                    }

                  
                    //appSettings.isMobileHeader() &&
                    if ( (type == 'pdf' || (data.FileType && data.FileType == 'pdf'))) {



                        $state.go('documentViewer', { fileFilter: filter });
                    } else {



                        _getPDFFile(filter, function (result) {

                            if (result.statusText != 'No Content' && result.statusText != '') {



                                if (type == 'pdf' || (data.FileType && data.FileType == 'pdf')) {

                                    var file = new Blob([result.data], { type: 'application/pdf' });
                                    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
                                        window.navigator.msSaveOrOpenBlob(file);
                                    }
                                    else {
                                        var fileURL = window.URL.createObjectURL(file);



                                        //alert($cordovaInAppBrowser);
                                        try {
                                            // alert(cordova.InAppBrowser);
                                            //cordova.InAppBrowser.open(fileURL, '_blank', 'location=yes');

                                            //  cordova.InAppBrowser.open('http://support.qatarairways.com/hc/en-us', '_blank', 'location=yes');
                                            if (appSettings.isMobileHeader()) {



                                                $state.go('documentViewer', { fileFilter: filter });
                                            } else {
                                                window.open(fileURL, '_blank');
                                            }

                                            //$state.go('documentViewer', { fileFilter: filter });



                                            //var options = {
                                            //    location: 'yes',
                                            //    clearcache: 'yes',
                                            //    toolbar: 'no'
                                            //};

                                            //$cordovaInAppBrowser.open('http://ngcordova.com', '_blank', options)
                                            //     .then(function (event) {
                                            //         // success
                                            //     })
                                            //     .catch(function (event) {
                                            //         // error
                                            //     });
                                            /*
                                            document.addEventListener("deviceready", function () {
                                                $cordovaInAppBrowser.open('http://ngcordova.com', '_blank', options)
                                                  .then(function (event) {
                                                      // success
                                                  })
                                                  .catch(function (event) {
                                                      // error
                                                  });
        
        
                                                $cordovaInAppBrowser.close();
        
                                            }, false);
        
                                            $rootScope.$on('$cordovaInAppBrowser:loadstart', function (e, event) {
        
                                            });
        
                                            $rootScope.$on('$cordovaInAppBrowser:loadstop', function (e, event) {
                                                // insert CSS via code / file
                                                $cordovaInAppBrowser.insertCSS({
                                                    code: 'body {background-color:blue;}'
                                                });
        
                                                // insert Javascript via code / file
                                                $cordovaInAppBrowser.executeScript({
                                                    file: 'script.js'
                                                });
                                            });
        
                                            $rootScope.$on('$cordovaInAppBrowser:loaderror', function (e, event) {
        
                                            });
        
                                            $rootScope.$on('$cordovaInAppBrowser:exit', function (e, event) {
        
                                            });*/
                                        }
                                        catch (e) {
                                            alert(e);

                                            //window.open(fileURL, '_blank');




                                        }

                                        // window.open(fileURL, '_blank');

                                        //var popupWinindow = window.open('', '_blank');
                                        //popupWinindow.document.open();
                                        //popupWinindow.document.write('<html>' + '<body >' + 'pageHeader' + '<br>'+
                                        //    '<object src="'+fileURL+'" width="700px" height="700px"><embed src="'+fileURL+'"></embed></object>'
                                        //      + '</html>');
                                        //popupWinindow.document.close();
                                    }
                                }
                                else {
                                    var binary = '';
                                    var bytes = new Uint8Array(result.data);
                                    var len = bytes.byteLength;
                                    for (var i = 0; i < len; i++) {
                                        binary += String.fromCharCode(bytes[i]);
                                    }
                                    var base64String = window.btoa(binary);

                                    var file = {};
                                    file.FileName = name;

                                    file.FileContent = base64String;
                                    var mimeTypes = [];
                                    sharedDataService.getCommonInfo('MIMETYPE', function (resultMime) {
                                        angular.forEach(resultMime, function (dataMime) {
                                            mimeTypes.push(dataMime);
                                        });

                                        sharedDataService.openFile(file, mimeTypes);
                                    }, function (error) {

                                    });



                                }




                            } else {
                                toastr.warning('File not found!');
                            }

                        },
                         function (error) {
                             toastr.error(error.data);
                             //console.log('error : ' + error);
                         });
                    }

                }
                if (isNotNullOrEmpty(data.DocType) && data.DocType.toLowerCase() == folderDocType.toLowerCase()) {
                    if (data.isTab) {
                        $rootScope.isTab = false;
                        //var filterPara = {
                        //    FileCode: data.FileCode,
                        //    FileId: data.FileId,
                        //    DocumentId: data.DocumentId
                        //};

                        // $state.go('home.filefolder', { fileFilter: filterPara });


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

        function NewBlob(data, datatype) {
            var out;

            try {
                console.debug("case 1");
                out = new Blob([data.buffer], { type: datatype });

            }
            catch (e) {
                window.BlobBuilder = window.BlobBuilder ||
                        window.WebKitBlobBuilder ||
                        window.MozBlobBuilder ||
                        window.MSBlobBuilder;

                if (e.name == 'TypeError' && window.BlobBuilder) {
                    console.debug("case 2");
                    var bb = new BlobBuilder();
                    bb.append(data);
                    out = bb.getBlob(datatype);

                }
                else if (e.name == "InvalidStateError") {
                    // InvalidStateError (tested on FF13 WinXP)
                    console.debug("case 3");
                    out = new Blob([data], { type: datatype });

                }
                else {
                    // We're screwed, blob constructor unsupported entirely   
                    console.debug(e);
                    console.debug("Errore");
                }
            }
            return out;
        }

    }]);

