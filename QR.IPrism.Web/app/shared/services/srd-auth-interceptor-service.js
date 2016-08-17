'use strict';
angular.module('app.shared.components').factory('authInterceptorService', ['$q', '$location', '$window', function ($q, $location, $window) {

    var authInterceptorServiceFactory = {};

    var _request = function (config) {

        config.headers = config.headers || {};
        var authData = angular.element(document.getElementsByName('_MN_A')[0]).val();
        if (authData) {
            config.headers.ForgeryVerificationToken = authData;
        }
        else
            fnRedirect('Unauthorized');
        return config;
    }
    function fnRedirect(rout)
    {
        var pathArray = $location.$$absUrl.split('/');
        var protocol = pathArray[0];
        var host = pathArray[2];
        $window.location = protocol + '//' + host + '/' + rout;
    }
    var _responseError = function (response) {
        if (response.status === 401 || response.status === 403) {
            fnRedirect('Unauthorized')
        }
        else
            return $q.reject(response);
    }
    var _response= function (response) {

        if (typeof response.data === 'string' || response.data instanceof String) {
            if (response.data.indexOf('CookieAuth.dll') > -1) {
                $window.location.href = '/';
            }
        }
        return response;
    }

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;
    authInterceptorServiceFactory.response = _response;
    return authInterceptorServiceFactory;
}]);



