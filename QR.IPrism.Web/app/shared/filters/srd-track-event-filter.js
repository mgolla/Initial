
angular.module('app.shared.components').filter('trackEvent', ['analyticsService', function (analyticsService) {
    return function (entry, category, action, opt_label, opt_value, opt_noninteraction) {
        analyticsService.trackEvent(category, action, opt_label, opt_value, opt_noninteraction);
        return entry;
    }
}]);

