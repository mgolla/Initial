
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.file.controller', ['$scope', '$http', 'fileService', 'analyticsService', '$state', '$rootScope', 'blockUI', '$stateParams', '$location', '$sce', '$q',
function ($scope, $http, fileService, analyticsService, $state, $rootScope, blockUI, $stateParams, $location, $sce, $q) {
    //Controller Scope Initialization

    var controller = this;
    var inotificationAlertSVPblockUI = blockUI.instances.get('ipmfileBlockUI');

    function isNotNullOrEmpty(value) {
        return ((value && value != null && value.toString().trim().length > 0));
    };

    var Initialize = function () {
        //Declare all scop properties
        pageHieght();

        $scope.selectedSVPMgs = 0;
        $scope.selectedVideo;
        var para = $stateParams.fileFilter;

        var pathArray = $location.$$absUrl.split('/');
        var protocol = pathArray[0];
        var host = pathArray[2];
        $scope.baseURL = protocol + '//' + host;

        controller.API = null;
        var format = "video/mp4";
        var link = para.VideoURL;
        if (isNotNullOrEmpty(para.VideoType)) {
            format = "video/" + para.VideoType.toLowerCase();
        } else {
            link = link + '.mp4';
        }
      
        

        controller.onPlayerReady = function (API) {
            controller.API = API;
        };

        controller.videos = [
            {
                sources: [
                     { src: link, type: format }
                   //{ src: "http://localhost:63535/video/Greeting2016_Crew.mp4", type: "video/mp4" }
            //        //,{ src: $sce.trustAsResourceUrl("http://static.videogular.com/assets/videos/videogular.webm"), type: "video/webm" },
            //        //{ src: $sce.trustAsResourceUrl("http://static.videogular.com/assets/videos/videogular.ogg"), type: "video/ogg" }
                ]
            }
        ];

        controller.setVideo = function () {
            controller.API.stop();
            controller.config.sources = controller.videos[0].sources;
          
        };


        $scope.filter = {
            StaffID: ""

        }
        var promise = loadNotificationAlertSVPList($scope.filter);

    }

    function loadNotificationAlertSVPList(filter) {
        var baseURL = fileService.getBaseURL();
        var deferred = $q.defer();
        inotificationAlertSVPblockUI.start();
       

        controller.config = {
            autoHide: false,
            autoHideTime: 3000,
            autoPlay: false,
            sources: controller.videos[0].sources,
            theme: {
                url: baseURL + "/Content/video/styles/themes/default/videogular.css"
            },

            plugins: {
                poster: baseURL + "/Content/css/styles/icons/iprism_Logo.png"
            }

        };


        inotificationAlertSVPblockUI.stop();
        deferred.resolve('success !');

        return deferred.promise;
    }




    function loadFileList(filter) {
        ifileblockUI.start();
        fileService.getFileList(filter, function (result) {
            $scope.fileViewModel = result.data;
            ifileblockUI.stop();
        },
         function (error) {
             ifileblockUI.stop();
         }
       );
    }
    $scope.downloadPdf = function (filter) {

        fileService.getFileList(filter, function (fileURL) {
            $scope.content = $sce.trustAsResourceUrl(fileURL);
        });
    };
    $scope.onOpenFileClick = function (FileCode, fileID) {
        var filter = {
            FileCode: FileCode,
            FileId: fileID
        };

        $scope.downloadPdf(filter);
    };

    $scope.isSelected = function (section) {

        return $scope.selectedFile === section;
    }

    function pageHieght() {
        var getHeight = $(window).height() - 150;
        $scope.pageHieght = getHeight;

        $('.content_wrapper').css('min-height', getHeight + 'px');
    }


    Initialize();

}]);



