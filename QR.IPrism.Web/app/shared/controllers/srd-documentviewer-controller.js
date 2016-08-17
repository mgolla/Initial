
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.documentviewer.controller', ['$scope', '$http', 'fileService', 'analyticsService', '$state', '$rootScope', 'blockUI', '$stateParams', '$location', '$sce', '$q',
function ($scope, $http, fileService, analyticsService, $state, $rootScope, blockUI, $stateParams, $location, $sce, $q) {

    var blockUI = blockUI.instances.get('DocVBlockUI');

    var Initialize = function () {
        //Declare all scop properties
        pageHieght();

        $scope.pageUrl = null;
        $scope.PdfDocumentLoaded = false;
        if ($stateParams.fileFilter) {
            $scope.paraData = $stateParams.fileFilter;
            //loadData($scope.paraData);

        }


    }

    $scope.pdf = {
        src: 'http://localhost:63532/HelpManuals/doc.pdf'
    };


    var pdfjsframe = document.getElementById('pdfViewer');
    pdfjsframe.onload = function () {
        // LoadPdfDocument();
        loadData($scope.paraData);
    };

    //$scope.myApiCallThatReturnsBase64PdfData.then(
    //  function (base64Data) {
    //      $scope.base64Data = base64Data;
    //      LoadPdfDocument();
    //  },
    //  function (failure) {

    //      //NotificationService.error(failure.Message);

    //  });

    function LoadPdfDocument() {
        if ($scope.PdfDocumentLoaded)
            return;
        if (!$scope.base64Data)
            return;

        var pdfData = base64ToUint8Array($scope.base64Data);
        pdfjsframe.contentWindow.PDFViewerApplication.open(pdfData);

        $scope.PdfDocumentLoaded = true;
    }

    function base64ToUint8Array(base64) {
        var raw = atob(base64);
        var uint8Array = new Uint8Array(raw.length);
        for (var i = 0; i < raw.length; i++) {
            uint8Array[i] = raw.charCodeAt(i);
        }
        return uint8Array;
    }

    function loadData(filterPara) {
        if ($scope.PdfDocumentLoaded)
            return;

        if (filterPara) {
            fileService.getPDFFile(filterPara, function (result) {
                if (result.statusText != 'No Content' && result.statusText != '') {

                    //var currentBlob = new Blob([result.data], { type: 'application/pdf' });
                    //$scope.pdfUrl = URL.createObjectURL(currentBlob);
                    //$scope.loading = 'loading';
                    PDFJS.disableWorker = true;

                    pdfjsframe.contentWindow.PDFViewerApplication.open(result.data);

                    $scope.PdfDocumentLoaded = true;

                    //var file = new Blob([result.data], { type: 'application/pdf' });
                    //var fileURL = window.URL.createObjectURL(file);
                    //$scope.pageUrl = fileURL;
                    //$scope.pdf.src = fileURL;
                    // $scope.pdfUrl = $scope.pageUrl;
                    //$scope.pdfName = 'PDF';
                    //       var content = '<object data="' + $scope.pageUrl + '" type="application/x-pdf" title="iPrism" style="width:500px; height:500px">'
                    //    + '<a href="' + $scope.pageUrl + '">document</a>'
                    //+ '</object>';

                    //var width = window.innerWidth;
                    //var height = window.innerHeight;

                    //PDFObject.embed($scope.pageUrl, "#example1");

                    //                var content = '<div style="overflow:auto"> <object src="' + $scope.pageUrl + '" style="width:' + (width - 20) + 'px !important; height:' + (height - 160) + 'px !important">'
                    //                    + '<a href="' + $scope.pageUrl + '">please click to open the document</a></br>'
                    //    + '<embed style="padding-left: 20px;min-width:' + (width - 20) + 'px !important; min-height:' + (height - 170) + 'px !important"  src="' + $scope.pageUrl + '">'
                    //  + '  </embed>'
                    //+ '</object> </div>';


                    //                var content = '<div style="overflow:auto"> <object src="' + $scope.pageUrl + '" style="width:' + (width - 20) + 'px !important; height:' + (height - 160) + 'px !important">'
                    //                   + '<a href="' + $scope.pageUrl + '">please click to open the document</a></br>'
                    //   + '<embed width="' + (width - 20)+'" style="padding-left: 20px;min-width:' + (width - 20) + 'px !important; min-height:' + (height - 100) + 'px !important;padding: 2%;"  src="' + $scope.pageUrl + '">'
                    // + '  </embed>'
                    //+ '</object> </div>';

                    //var content = '<iframe style="padding-left: 20px;min-width:' + (width - 70) + 'px !important; min-height:' + (height - 170) + 'px !important" src="' + $scope.pageUrl + '"></iframe>'



                    //var content = '<iframe src="https://docs.google.com/viewer?url=' + fileURL + '" style="padding-left: 20px;min-width:' + (width - 70) + 'px !important; min-height:' + (height - 170) + 'px !important" frameborder="0"></iframe>';

                    //$('#docViewer').html(content);

                    // atob() is used to convert base64 encoded PDF to binary-like data.
                    // (See also https://developer.mozilla.org/en-US/docs/Web/API/WindowBase64/
                    // Base64_encoding_and_decoding.)
                    //var pdfData = atob(
                    //  'JVBERi0xLjcKCjEgMCBvYmogICUgZW50cnkgcG9pbnQKPDwKICAvVHlwZSAvQ2F0YWxvZwog' +
                    //  'IC9QYWdlcyAyIDAgUgo+PgplbmRvYmoKCjIgMCBvYmoKPDwKICAvVHlwZSAvUGFnZXMKICAv' +
                    //  'TWVkaWFCb3ggWyAwIDAgMjAwIDIwMCBdCiAgL0NvdW50IDEKICAvS2lkcyBbIDMgMCBSIF0K' +
                    //  'Pj4KZW5kb2JqCgozIDAgb2JqCjw8CiAgL1R5cGUgL1BhZ2UKICAvUGFyZW50IDIgMCBSCiAg' +
                    //  'L1Jlc291cmNlcyA8PAogICAgL0ZvbnQgPDwKICAgICAgL0YxIDQgMCBSIAogICAgPj4KICA+' +
                    //  'PgogIC9Db250ZW50cyA1IDAgUgo+PgplbmRvYmoKCjQgMCBvYmoKPDwKICAvVHlwZSAvRm9u' +
                    //  'dAogIC9TdWJ0eXBlIC9UeXBlMQogIC9CYXNlRm9udCAvVGltZXMtUm9tYW4KPj4KZW5kb2Jq' +
                    //  'Cgo1IDAgb2JqICAlIHBhZ2UgY29udGVudAo8PAogIC9MZW5ndGggNDQKPj4Kc3RyZWFtCkJU' +
                    //  'CjcwIDUwIFRECi9GMSAxMiBUZgooSGVsbG8sIHdvcmxkISkgVGoKRVQKZW5kc3RyZWFtCmVu' +
                    //  'ZG9iagoKeHJlZgowIDYKMDAwMDAwMDAwMCA2NTUzNSBmIAowMDAwMDAwMDEwIDAwMDAwIG4g' +
                    //  'CjAwMDAwMDAwNzkgMDAwMDAgbiAKMDAwMDAwMDE3MyAwMDAwMCBuIAowMDAwMDAwMzAxIDAw' +
                    //  'MDAwIG4gCjAwMDAwMDAzODAgMDAwMDAgbiAKdHJhaWxlcgo8PAogIC9TaXplIDYKICAvUm9v' +
                    //  'dCAxIDAgUgo+PgpzdGFydHhyZWYKNDkyCiUlRU9G');


                    //var pdfData = atob([result.data]);

                    //// Disable workers to avoid yet another cross-origin issue (workers need
                    //// the URL of the script to be loaded, and dynamically loading a cross-origin
                    //// script does not work).
                    ////
                    //// PDFJS.disableWorker = true;

                    ////
                    //// The workerSrc property shall be specified.
                    ////
                    //PDFJS.workerSrc = '../../Scripts/pdfjs/build/pdf.worker.js';

                    //// Opening PDF by passing its binary data as a string. It is still preferable
                    //// to use Uint8Array, but string or array-like structure will work too.
                    //PDFJS.getDocument({ data: pdfData }).then(function getPdfHelloWorld(pdf) {
                    //    // Fetch the first page.
                    //    pdf.getPage(1).then(function getPageHelloWorld(page) {
                    //        var scale = 1.5;
                    //        var viewport = page.getViewport(scale);

                    //        // Prepare canvas using PDF page dimensions.
                    //        var canvas = document.getElementById('the-canvas');
                    //        var context = canvas.getContext('2d');
                    //        canvas.height = viewport.height;
                    //        canvas.width = viewport.width;

                    //        // Render PDF page into canvas context.
                    //        var renderContext = {
                    //            canvasContext: context,
                    //            viewport: viewport
                    //        };
                    //        page.render(renderContext);
                    //    });
                    //});


                } else {
                    toastr.warning('File not found!');
                }
            },
            function (error) {
                console.log('error : ' + error);
            });
        }
    }

    function pageHieght() {
        var getHeight = $(window).height() - 150;
        $scope.pageHieght = getHeight;

        $('.content_wrapper').css('min-height', getHeight + 'px');
    }

    Initialize();

}]);
