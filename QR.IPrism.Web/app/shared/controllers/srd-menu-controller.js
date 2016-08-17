angular
    .module('app.shared.components')
    .controller('srd.menu.controller', ['$scope', '$rootScope', '$http', 'toastr', 'analyticsService', 'sharedDataService', '$state', 'ngDialog', 'appSettings',
        'messages', '$window', 'Idle', 'Keepalive', '$modal', 'notificationAlertService',
        function ($scope, $rootScope, $http, toastr, analyticsService, sharedDataService, $state, ngDialog, appSettings, messages, $window,
            Idle, Keepalive, $modal, notificationAlertService) {
            $scope.started = false;

            var Initialize = function () {
                $rootScope.crewProfile;
                $scope.menuList = [];
                fnLoadMenu();
                $rootScope.isMobile = false;
                $scope.logOut = '';
                $scope.MyMailURL = messages.MyMailURL;
                $scope.AQDURL = messages.AQDURL;
                $scope.HelpDocument = messages.HelpDocument;
                $scope.alertCounter = 0;
                $scope.isOpenSearch = false;


                $scope.alertNotificationViewModel;
                $scope.filter = {
                    StaffID: "",
                    Type: ""
                }
                $scope.alertNotificationViewModel = '';

                var burl = sharedDataService.getBaseURL();
                if (burl) {
                    $scope.logOut = burl + '/Logout';
                }
                readyPage();
                closeModals();
                Idle.watch();
                $scope.appVersion = messages.APPVERSION;
                $scope.started = true;
                $scope.timoutDialogTitle = "Timeout";
                $scope.timeOutDialogMessage = messages.TIMEOUTMSG;
            }

            $scope.notService = notificationAlertService;

            $rootScope.goBack = function (val) {
                if (!val) {
                    $window.history.back();
                } else {
                    $state.go(val);
                }
            };

            function closeModals() {
                if ($scope.warning) {
                    $scope.warning.close();
                    $scope.warning = null;
                }

                if ($scope.timedout) {
                    $scope.timedout.close();
                    $scope.timedout = null;
                }
            }

            $scope.$on('IdleStart', function () {
                closeModals();
                $scope.warning = $modal.open({
                    templateUrl: 'warning-dialog.html',
                    windowClass: 'modal-danger'
                });
            });

            $scope.$on('IdleEnd', function () {
                sharedDataService.refreshContext(null, function (result) {
                    sharedDataService.processToken(result);
                }, function (result) {
                    sharedDataService.sLogOut();
                });
                closeModals();
            });

            $scope.$on('IdleTimeout', function () {
                closeModals();
                sharedDataService.sLogOut();
            });

            function fnLoadMenu() {

                var pathArray = window.location.href.split('/');
                var protocol = pathArray[0];
                var host = pathArray[2];
                var baseURL = protocol + '//' + host + '/';


                var data = {
                    CrewImagePath: baseURL + appSettings.CrewPhotos,
                    CrewImageType: messages.CREWIMAGETYPE
                };

                sharedDataService.getAllHeaderInfo(data,
                   function (result) {

                       // If there is any assessment notification , show modal for user to go to it and acknowledge
                       if (result.data.NotificationDetails) {
                           $('#header_area').hide();
                           $('body').css('padding', '0px');

                           sharedDataService.notificationDetails = result.data.NotificationDetails;
                           $scope.dialogTitle = "Assessment Notifications";
                           $scope.dialogMessage = messages.ASSESSMENTACK;//"An assessment has been done on you. Kindly click on view to acknowledge the assessment.";
                           ngDialog.open({
                               template: '/app/ipm/partials/ipmModalConfirmation.html',
                               showClose: false,
                               closeByEscape: false,
                               scope: $scope,
                               preCloseCallback: function (value) {
                                   if (value == 'Post') {
                                       $state.go('idpack', { reqno: result.data.NotificationDetails.Id, reqtype: result.data.NotificationDetails.Type });
                                   }
                               }
                           });
                       }

                       $scope.menuList = result.data.Menus;
                       $scope.crewProfile = result.data.StaffInfo;
                       // $scope.alertCounter = result.data.AlertCounter;
                   },
                   function (error) {
                   }, null);
            }

            $scope.$watch('notService.notifications', function (nVal, oDate) {
                if (nVal) {
                    loadNotificationAlertSVPList();
                }
            });

            function loadNotificationAlertSVPList() {
                //var lst = angular.copy($scope.notService.notifications);
                //$scope.alertNotificationViewModel = lst.splice(0, 10);
                //$scope.alertCounter = $scope.alertNotificationViewModel ? $scope.alertNotificationViewModel.length : 0;

                //$scope.alertCounter = viewedAlert.length;

                //sharedDataService.getAlterNotificationHeaderList(filter, function (result) {
                //    $scope.alertNotificationViewModel = result.data.NotificationAlertSVPs;
                //    var viewedAlert = $.grep($scope.alertNotificationViewModel, function (v, i) {
                //        return v.IsViewed == false;
                //    });
                //    $scope.afterClickAlertBell();
                //},
                // function (error) {

                // });

                $scope.alertNotificationViewModel = angular.copy($scope.notService.notifications);

                var viewedAlert = $.grep($scope.alertNotificationViewModel, function (v, i) {
                    return v.IsViewed == false;
                });
                $scope.alertCounter = viewedAlert ? viewedAlert.length : 0;
            }

            $scope.onClickAlertBell = function () {
                loadNotificationAlertSVPList();
                $scope.afterClickAlertBell();
            }
            $scope.afterClickAlertBell = function () {

                $(".alertbellpopup").slideToggle(5);
                $('.profilepicpopup').hide();
                $scope.filter.Type = "";
                var viewedAlert = $.grep($scope.alertNotificationViewModel, function (v, i) {
                    return v.IsViewed == false;
                });

                $scope.alertCounter = viewedAlert ? viewedAlert.length : 0;
                $.each(viewedAlert, function (index, value) {
                    var data = value.Doc + value.Doc;
                    if (index > 0) {
                        if (value.Doc && value.DocId && value.Doc.length > 0 && value.DocId.length > 0) {
                            data = '$' + value.Doc + '#' + value.DocId;
                        }

                    }
                    else {
                        if (value.Doc && value.DocId && value.Doc.length > 0 && value.DocId.length > 0) {
                            data = value.Doc + '#' + value.DocId;
                        }

                    }
                    $scope.filter.Type = $scope.filter.Type + data;
                });

                if ($scope.alertCounter > 0) {
                    sharedDataService.updateAlterNotificationHeader($scope.filter, function (result) {
                        $scope.alertCounter = 0;
                    },
                    function (error) {

                    });
                }
                //if ($rootScope.isMobileHeader()) {
                //    $state.go('alertnmobile');
                //}
            }


            $scope.onSelectedChange = function (Doc, DocType, FileCode, fileID, DocumentId, DocumentName, Link) {

                var filterPara = {
                    Doc: Doc,
                    DocType: DocType,
                    FileCode: FileCode,
                    FileId: fileID,
                    DocumentId: DocumentId,
                    DocumentName: DocumentName,
                    isTab: false,
                    Link: Link
                };

                sharedDataService.openDocTypes(filterPara, function (result) {

                },
                function (error) {
                    console.log('error : ' + error);
                });
                $(".alertbellpopup").slideToggle(5);
                $('.profilepicpopup').hide();
            }

            function readyPage() {

                $('.nav-icon').each(function (index, obj) {
                    $(obj).mouseenter(function () {
                        if (obj && obj.id != "") {
                            // $('#' + obj.id).effect('bounce', 300);
                        }

                    });
                });

                $(".profilePic").click(function (e) {
                    $(".profilepicpopup").slideToggle(700);
                    $('.alertbellpopup').hide();

                });

                $(document).click(function (e) {
                    if ($(e.target).closest('.alertbellpopup').length != 0) return false;
                    $('.alertbellpopup').hide();

                });

                $(document).click(function (e) {
                    if ($(e.target).closest('.profilepicpopup').length != 0) return false;
                    $('.profilepicpopup').hide();
                });

                $(document).click(function (e) {
                    if ($(e.target).closest('.caret').length != 0) return false;
                    $('.caret').addClass("caretcolordark").removeClass("caretcolorwhite");
                });

                $(".menu-part li").on('click', function (e) {
                    $(this).children("a").children(".caret").addClass("caretcolorwhite").removeClass("caretcolordark");
                    $(this).siblings().children("a").children(".caret").addClass("caretcolordark").removeClass("caretcolorwhite");
                });
                $(".menu-part li").on(' mouseover', function (e) {
                    $(this).children("a").children(".caret").addClass("white").removeClass("dark");

                });
                $(".menu-part li").on('mouseout', function (e) {
                    $(this).children("a").children(".caret").addClass("dark").removeClass("white");

                });

                $(".navbar-header .dot-toggle").on('click', function () {
                    $("body").toggleClass("padding-top-small");

                    if ($(".main-menu-nav.collapse.in").length > 0) {
                        $(".main-menu-nav.collapse.in").removeClass("in");
                        $('body').removeClass("padding-top-large");
                    }
                    if ($('#logininfoMenutoggle.in').length > 0) {

                        $('body').addClass("padding-top-small");

                    } else {
                        $('body').removeClass("padding-top-small");
                    }

                });


                //$('li.dropdown.mega-dropdown a').on('click', function (event) {
                //    $(this).parent().toggleClass("open");
                //});

                $('body').on('click', function (e) {
                    //if (!$('li.dropdown.mega-dropdown').is(e.target) && $('li.dropdown.mega-dropdown').has(e.target).length === 0 && $('.open').has(e.target).length === 0) {
                    //    console.log('innnn');
                    //    $("#navbar").collapse('hide');
                    //    $('body').removeClass("padding-top-large");
                    //    $('body').removeClass("padding-top-small");

                    //    //$('li.dropdown.mega-dropdown').removeClass('open'); 
                    //}

                    if (!$('.navbar-header').is(e.target) && e.target.className.indexOf('main-toggle') < 0 && e.target.className.indexOf('navbar-toggle') < 0 && e.target.className.indexOf('dots-icon') < 0 && e.target.className.indexOf('glyphicon') < 0 && e.target.className.indexOf('loginInfo') < 0 &&
                        e.target.className.indexOf('iprism_logo') < 0 && e.target.className.indexOf('icon-bar') < 0
                        && e.target.className.indexOf('dropdown-toggle') < 0
                       ) {

                        $("#navbar").collapse('hide');
                        $("#logininfoMenutoggle").collapse('hide');
                        $('body').removeClass("padding-top-large");
                        $('body').removeClass("padding-top-small");

                        //$('li.dropdown.mega-dropdown').removeClass('open');
                    }
                });

                if (appSettings.isMobile) {
                    $(document).on('focus', 'select, input, textarea', function () {
                        $('#header_area').css({ 'position': 'absolute', 'top': '0px' });

                    });
                    $(document).on('blur', 'select, input, textarea', function () {
                        $('#header_area').css({ 'position': 'fixed' });
                    });
                }

                $(".navbar-header .main-toggle").on('click', function (mainDropdownheight) {
                    $("body").toggleClass("padding-top-large");

                    if ($(".icon-toggle.collapse.in").length > 0) {
                        $(".icon-toggle.collapse.in").removeClass("in");
                        $('body').removeClass("padding-top-small");
                    }
                    if ($("#navbar.in").length > 0) {
                        $('body').removeClass("padding-top-large");


                    } else {
                        $('body').addClass("padding-top-large");
                    }
                });
            }

            function mouseenterButton(element) {
                $(element).effect('bounce', 300);
            }

            $scope.checkIsMobile = function (val) {
                if (val) {
                    var vpwM = $(window).width();
                    if (vpwM < 769) {
                        return true;
                    } else {
                        return false;
                    }
                }
                return true;
            }
            $scope.onClicklogOff = function () {
                sharedDataService.logOut();
            }
            $scope.onImpersonateSignInClick = function () {
                sharedDataService.impUserContext($scope.crewProfile.ImpersonatedBy, function (result) {
                    if (result == '') {
                        sharedDataService.redirect("Main/hm");
                    }
                    else {
                        toastr.error(messages[result]);
                    }
                }, null);
            }

            $scope.profileExpClick = function () {
                $('.clsImprntHdr').hide();
                $('.clsUsrHdr').show();
            }

            $scope.onImpersonateClick = function () {
                $('.clsUsrHdr').hide(1000);
                $('.clsImprntHdr').show(1000);
            }
            $scope.onImpersonateCancelClick = function () {
                $('.clsImprntHdr').hide(1000);
                $('.clsUsrHdr').show(1000);
            }
            $scope.onClickMainMenuLink = function (val, pageName) {
                if (val == 'evrguid') {
                    window.open('../../../Content/help/evrguidTemp.pdf', '_blank', 'fullscreen=yes');
                }
                if (val != 'evrguid') {
                    var vpwM = $(window).width();
                    if (vpwM < 769) {
                        $("#navbar").collapse('hide');
                        $('body').removeClass("padding-top-large");
                        $('body').removeClass("padding-top-small");
                    }

                    if (vpwM > 769 && navigator.userAgent.indexOf('iPad') > -1) {
                        $('.navbar-toggle').click();
                        $('body').removeClass("padding-top-large");
                        $('body').removeClass("padding-top-small");

                    }
                    if (val == 'home' && $state.current.name == 'home') {
                        $rootScope.onClickRosterType(1);
                    }

                    if (pageName && pageName.toString().trim().length > 0) {
                        analyticsService.trackEvent('Load', 'Add', pageName, pageName);
                    }

                    $state.go(val);
                }
                // $('#logininfoMenutoggle').removeClass('in');
            }

            $scope.onClickCrewProfile = function (val) {
                if (val) {
                    var vpwM = $(window).width();

                    $(".alertbellpopup").hide();
                    $('.profilepicpopup').hide();
                    $state.go(val);
                }

            }
            $scope.onOpenSearch = function () {
                $scope.isOpenSearch = true;
            };

            $scope.isSrefNotNullOrEmptyOrUndefined = function (value) {
                return (value && value != null && value.toString().trim().length > 0);
            };


            $scope.isNullOrEmpty = function (section) {

                return (!section) || section === null || section.trim() === '';
            }

            $rootScope.getTableHeight = function (grid) {
                return sharedDataService.getTableHeight(grid);
            };

            Initialize();



        }]);
