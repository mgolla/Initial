
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.svpMessage.controller', ['$scope', '$http', '$location', 'svpMessageService', 'fileService', 'analyticsService', '$state', '$rootScope', 'blockUI', '$sce', '$q',

                                  function ($scope, $http, $location, svpMessageService, fileService, analyticsService, $state, $rootScope, blockUI, $sce, $q

           ) {
                                      var inotificationAlertSVPblockUI = blockUI.instances.get('ipmSVPBlockUI');
                                      var Initialize = function () {
                                          $scope.myInterval = 5000;
                                          $scope.noWrapSlides = false;
                                          $scope.active = 0;
                                       
                                          var currIndex = 0;

                                          //Declare all scop properties
                                          $rootScope.sVPViewModel;
                                          $rootScope.selectedIndex;
                                          $rootScope.selectedItemVideo;
                                          $scope.selectedSVPMgs = 0;
                                          $rootScope.isShowVideo = false;
                                          $scope.filter = {
                                              StaffID: ""

                                          }
                                          var promise = loadNotificationAlertSVPList($scope.filter);
                                      }

                                      function loadNotificationAlertSVPList(filter) {
                                          $rootScope.sVPViewModel = [];

                                          // svp messges data -  because of performance issue added directly 
                                          // if you want change /Add videos please change below .

                                          var svpVM1 = {
                                              Doc: "SVPMESSAGE",
                                              DocDate : "31 Jan 2016",
                                              DocDesc: "QatarAirwaysA380",
                                              DocId :"2",
                                              DocType : "MP4",
                                              File :  null,
                                              FileCode : "ALERT",
                                              FileId :  "2",
                                              FileName: "QatarAirwaysA380.mp4",
                                              IsViewed : false,
                                              THUMBIMAGE: {
                                                  FileName: "/content/css/styles/images/svp/SVP_pic2.png",
                                                  FileType :"png"
                                              },
                                              URLVideo : null
                                          }
                                          $rootScope.sVPViewModel.push(svpVM1);
                                          var svpVM2 = {
                                              Doc: "SVPMESSAGE",
                                              DocDate: "31 Jan 2016",
                                              DocDesc: "NEW YEAR MESSAGES from Rossen, the Customer Experience team & your Colleagues along with the latest A380 Video starring our very own Cabin Crew",
                                              DocId: "2",
                                              DocType: "MP4",
                                              File: null,
                                              FileCode: "ALERT",
                                              FileId: "1",
                                              FileName: "Greeting2016_Crew.mp4",
                                              IsViewed: false,
                                              THUMBIMAGE: {
                                                  FileName: "/content/css/styles/images/svp/SVP_pic1.png",
                                                  FileType: "png"
                                              },
                                              URLVideo: null
                                          }
                                          $rootScope.sVPViewModel.push(svpVM2);

                                          //var svpVM2 = {
                                          //    Doc: "SVPMESSAGE",
                                          //    DocDate: "31 Jan 2016",
                                          //    DocDesc: "NEW YEAR MESSAGES from Rossen, the Customer Experience team & your Colleagues along with the latest A380 Video starring our very own Cabin Crew",
                                          //    DocId: "2",
                                          //    DocType: "MP4",
                                          //    File: null,
                                          //    FileCode: "ALERT",
                                          //    FileId: "1",
                                          //    FileName: "Greeting2016_Crew.mp4",
                                          //    IsViewed: false,
                                          //    THUMBIMAGE: {
                                          //        FileName: "/content/css/styles/images/svp/SVP_pic3.png",
                                          //        FileType: "png"
                                          //    },
                                          //    URLVideo: null
                                          //}
                                          //$rootScope.sVPViewModel.push(svpVM2);

                                          //window.setTimeout(function () {
                                          //            $('#videoNext').click();
                                          //        }, 2);
                                      }

                                      //function loadNotificationAlertSVPList(filter) {
                                      //    inotificationAlertSVPblockUI.start();
                                      //    svpMessageService.getNotificationAlertSVPList(filter, function (result) {

                                      //        $rootScope.sVPViewModel = result.data.NotificationAlertSVPs;
                                      //        inotificationAlertSVPblockUI.stop();
                                      //        //window.setTimeout(function () {
                                      //        //    $('#videoNext').click();
                                      //        //}, 2);
                                      //    },
                                      //     function (error) {
                                      //         inotificationAlertSVPblockUI.stop();
                                      //     }
                                      //   );
                                      //}

                                      $scope.isNotNullOrEmptyOrUndefinedFile = function (value) {
                                          return (value && value != null && value != '' && value.toString().trim().length >0);
                                      };

                                      $scope.onClickSetVideo = function (index, item) {
                                          $rootScope.isShowVideo = true;
                                          $rootScope.selectedIndex = index;
                                          $scope.active = index;
                                          $rootScope.selectedItemVideo = item;
                                      };
                                      $scope.isGetVideoSelectedTab = function (tab) {
                                          return $scope.selectedSVPMgs === tab;

                                      };
                                      $scope.onClickPrevious = function () {
                                          $scope.selectedSVPMgs = $scope.selectedSVPMgs - 1;
                                          if (0 > $scope.selectedSVPMgs) {
                                              var len = $rootScope.sVPViewModel.length;
                                              $scope.selectedSVPMgs = len - 1;
                                          }

                                      };
                                      $scope.onClickNext = function () {
                                          $scope.selectedSVPMgs = $scope.selectedSVPMgs + 1;
                                          var len = $rootScope.sVPViewModel.length;
                                          if (len <= $scope.selectedSVPMgs) {
                                              $scope.selectedSVPMgs = 0;
                                          }
                                      };

                                      Initialize();

                                  }]);

'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.svpVideo.controller', ['$scope', '$http', '$location', 'svpMessageService', 'fileService', 'analyticsService', '$state', '$rootScope', 'blockUI', '$sce', '$q',

                                  function ($scope, $http, $location, svpMessageService, fileService, analyticsService, $state, $rootScope, blockUI, $sce, $q

           ) {
                                      //Controller Scope Initialization

                                      var controller = this;
                                      var inotificationAlertSVPblockUI = blockUI.instances.get('ipmSVPBlockUI');
                                      var Initialize = function () {

                                          $scope.selectedSVPMgs = 0;
                                          $scope.selectedVideo;


                                          var pathArray = $location.$$absUrl.split('/');
                                          var protocol = pathArray[0];
                                          var host = pathArray[2];
                                          $scope.baseURL = protocol + '//' + host;

                                          controller.API = null;

                                          controller.onPlayerReady = function (API) {
                                              controller.API = API;
                                          };

                                          controller.videos = [

                                          ];

                                          controller.setVideo = function () {
                                              controller.API.stop();
                                              controller.config.sources = controller.videos[$rootScope.selectedIndex].sources;
                                              $scope.selectedVideo = $rootScope.selectedItemVideo;

                                              $('#VideoDesSVP').text($rootScope.selectedItemVideo.DocDesc);
                                              $rootScope.isShowVideo = true;
                                          };


                                          $scope.filter = {
                                              StaffID: ""

                                          }
                                          var promise = loadNotificationAlertSVPList($scope.filter);

                                      }

                                      function loadNotificationAlertSVPList(filter) {

                                          var deferred = $q.defer();
                                          inotificationAlertSVPblockUI.start();

                                          var len = $rootScope.sVPViewModel.length;
                                          $.each($rootScope.sVPViewModel, function (i, val) {

                                              if (val.FileName != null) {
                                                  var link = $scope.baseURL + '/video/' + val.FileName;

                                                  $rootScope.sVPViewModel[i].URLVideo = link;
                                                  if (i == 0) {
                                                      $('#VideoDesSVP').text(val.DocDesc);
                                                  }
                                                  controller.videos.push(
                                                       {
                                                           sources: [
                                                        { src: link, type: "video/" + val.DocType.toLowerCase() }

                                                           ]
                                                       }
                                                    );
                                              }
                                              if (parseInt(len) - 1 == i) {

                                                  var burl = fileService.getBaseURL();
                                                  controller.config = {
                                                      autoHide: false,
                                                      autoHideTime: 3000,
                                                      autoPlay: false,
                                                      sources: controller.videos[0].sources,
                                                      theme: {
                                                          url: burl + "/Content/video/styles/themes/default/videogular.css"
                                                      },

                                                      plugins: {
                                                          poster: burl + "/Content/css/styles/icons/iprism_Logo.png"
                                                      }

                                                  };
                                              }


                                          });

                                          inotificationAlertSVPblockUI.stop();


                                          window.setTimeout(function () {
                                              controller.setVideo();
                                          }, 100);



                                          deferred.resolve('success !');

                                          return deferred.promise;
                                      }


                                      $scope.getDate = function (string) {
                                          var array = string.split(' ');
                                          return array[0];
                                      }
                                      $scope.getMonthYear = function (string) {
                                          var array = string.split(' ');
                                          return array[1] + ' ' + array[2];
                                      }
                                      $scope.onClickVideoBackButton = function () {
                                          $rootScope.isShowVideo = false;

                                      };
                                      $scope.onSVPMgsTabChange = function (tab) {
                                          $scope.selectedSVPMgs = tab;

                                      };


                                      $scope.isSVPMgsSelectedTab = function (tab) {
                                          return $scope.selectedSVPMgs === tab;

                                      };

                                      $scope.isNullOrEmpty = function (section) {

                                          return (!section) || section === null || section.trim() === '';
                                      }
                                      $scope.isVideo = function (tab) {
                                          return (tab.DocType.toString().trim() != 'FOLDER' && tab.DocType.toString().trim() != 'PDF' && $rootScope.sVPViewModel[i].File != null && $rootScope.sVPViewModel[i].File.FileName != null);
                                      };

                                      Initialize();

                                  }]);



