'use strict';
angular.module('app.shared.components').factory('tokensManagerService', ['$http', '$timeout', 'toastr', 'appSettings','$q', function ($http, $timeout, toastr, appSettings,$q) {

    var serviceBase = appSettings.API;

    var _timeOut = 5000;
    
    var tokenManagerServiceFactory = {};

    var _getRefreshTokens = function () {

        return $http.get(serviceBase + 'api/refreshtokens').then(function (results) {
            return results;
        });
    };

    var _deleteRefreshTokens = function (tokenid) {

        return $http.delete(serviceBase + 'api/refreshtokens/?tokenid=' + tokenid).then(function (results) {
            return results;
        });
    };

    var _refreshUserLogin = function () {
        $timeout(_countUp, _timeOut);
    };

    var _shiftDeleteCntxt = function () {
        return $http.post(serviceBase + 'api/shiftdltcntxt').then(function (results) {
            return results;
        });
    };
    var _refreshToken = function (success,error) {
        var deferred = $q.defer();

        var authData = angular.element(document.getElementsByName('_MN_A')[0]).val();

        if (authData) {

            var refreshToken = authData.split("*")[1].split("#");
            var data = "grant_type=refresh_token&refresh_token=" + refreshToken[0] + "&client_id=" + refreshToken[1];

            $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                var mna = response.access_token + "*" + response.refresh_token + "#" + refreshToken[1];
                angular.element(document.getElementsByName('_MN_A')[0]).val(mna);

                deferred.resolve(response);

            }).error(function (err, status) {
                deferred.reject(err);
            });
        }
        return deferred.promise;
    };

    tokenManagerServiceFactory.refreshUserLogin = _refreshToken;

    return tokenManagerServiceFactory;

}]);