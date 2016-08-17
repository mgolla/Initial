"use strict"

var sharedModule = angular.module('app.shared.components', ['ngAnimate', 'toastr', 'ui.bootstrap', 'angular-loading-bar',
                                                             'ui.grid', 'ui.grid.cellNav', 'ui.grid.edit', 'ui.grid.resizeColumns', 'ui.grid.pinning',
                                                             'ui.grid.selection', 'ui.grid.pagination', 'ui.grid.moveColumns', 'ui.grid.grouping',
                                                             'ui.grid.expandable', 'ui.grid.autoResize', 'ngDialog', 'ng.deviceDetector', 'angular-cache',
                                                             'blockUI', 'ui.router', 'ngMap', 'ngTouch', 'angularFileUpload', 'ui.select',
                                                             "ngSanitize", "com.2fdevs.videogular", "com.2fdevs.videogular.plugins.controls",
                                                             "com.2fdevs.videogular.plugins.overlayplay", "com.2fdevs.videogular.plugins.buffering",
                                                             "com.2fdevs.videogular.plugins.poster", 'angular-carousel', 'ct.ui.router.extras',
                                                             'pascalprecht.translate', 'ngCookies', 'FileManagerApp', 'datetimepicker'
                                                            , 'ui.grid.autoResize'
                                                            , 'ngIdle'
                                                             , 'ngScrollbar'
                                                            
]);
var mainModule = angular.module('app.main.module', ['app.shared.components']);

//, 'ngScrollbars'