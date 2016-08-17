'use strict'
angular.module('app.shared.components').factory('analyticsService', ['webAPIService', 'deviceDetector', function (webAPIService, deviceDetector) {
    var service = {};
    service.successCount = 0;
    service.falureCount = 0;
    service.trackPageView = function (url) {

        // Convert to JSON
        var data = angular.toJson({
            Category: View,
            Action: url,
            OptionLabel: opt_label,
            OptionValue: opt_value,
            Resolution: window.outerWidth + ',' + window.outerHeight,
            Device: deviceDetector.device,
            Browser: deviceDetector.browser,
            BrowserVersion: deviceDetector.browser_version,
            UserAgent: deviceDetector.raw.userAgent,
            IsDesktop: deviceDetector.isDesktop.length,
            IsTablet: deviceDetector.isTablet.length,
            IpAddress: '',
            StaffNumber: '',
            DateTime: ''
        });
        log(data);
    };

    service.trackEvent = function (category, action, opt_label, opt_value) {
        var deviceInfo = deviceDetector;
        // Convert to JSON
        var data = angular.toJson({
            Category: category,
            Action: action,
            OptionLabel: opt_label,
            OptionValue: opt_value,
            Resolution: window.outerWidth + ',' + window.outerHeight,
            Device: deviceDetector.device,
            Browser: deviceDetector.browser +'-'+deviceDetector.browser_version,
            UserAgent: deviceDetector.raw.userAgent,
            IsDesktop: deviceDetector.isDesktop.length,
            IsTablet: deviceDetector.isTablet.length,
            IpAddress: '',
            StaffNumber: '',
            DateTime: ''
        });
        log(data);
    };

    service.trackTiming = function (category, variable, value, opt_label) {
        // Convert to JSON
        var data = angular.toJson({
            Category: category,
            Action: 'Time Taken',
            OptionLabel: opt_label,
            OptionValue: opt_value,
            Resolution: window.outerWidth + ',' + window.outerHeight,
            Device: deviceDetector.device,
            Browser: deviceDetector.browser +'-'+deviceDetector.browser_version,
            UserAgent: deviceDetector.raw.userAgent,
            IsDesktop: deviceDetector.isDesktop.length,
            IsTablet: deviceDetector.isTablet.length,
            IpAddress: '',
            StaffNumber: '',
            DateTime: ''
        });
        log(data);
    };

    function log(data) {
        var serviceUrl = "api/analytics/post";

        webAPIService.apiPost(serviceUrl, data,
           function (result) {
               service.successCount++;
           },
           function (error) {
           }, null);
    }

    return service;
}]);
