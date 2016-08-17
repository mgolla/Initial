'use strict'
angular.module('app.shared.components').factory('webAPIService', ['$http', 'appSettings', 'toastr', '$q', 'messages', function ($http, appSettings, toastr, $q, messages) {
    var self = this;
    self.modelIsValid = true;
    self.modelErrors = [];
    self.states = {};

    self.resetModelErrors = function () {
        self.modelErrors = [];
        self.modelIsValid = true;
    }


    self.apiGet = function (uri, data, success, failure, always) {
        self.modelIsValid = true;
    
        $http.get(appSettings.API + uri, data)
            .then(function (result) {
                if (result.data != null) {
                    success(result.data);
                }
                else {
                    success(result);
                }

                if (always != null)
                    always();
            }, function (result) {
                showErrorMessage(result);
                if (failure != null) {
                    failure(result);
                }
                else {
                    var errorMessage = result.status + ':' + result.statusText;
                    if (result.data != null && result.data.Message != null)
                        errorMessage += ' - ' + result.data.Message;
                    self.modelErrors = [errorMessage];
                    self.modelIsValid = false;
                }
                if (always != null)
                    always();
            });
    }

    self.apiPost = function (uri, data, success, failure, always) {
        self.modelIsValid = true;
        $http.post(appSettings.API + uri, data)
            .then(function (result) {
                success(result);
                if (always != null)
                    always();
            }, function (result) {
                if (result.status != null && result.status == 500) {
                    showErrorMessage(result);
                    if (failure != null) {
                        failure(result);
                    }
                }
                else {
                    if (failure != null) {
                        failure(result);
                    }
                    else {
                        var errorMessage = result.status + ':' + result.statusText;
                        if (result.data != null && result.data.Message != null)
                            errorMessage += ' - ' + result.data.Message;
                        self.modelErrors = [errorMessage];
                        self.modelIsValid = false;
                    }
                }
                if (always != null)
                    always();
            });
    }

    self.apiPostFile = function (uri, data, success, failure, always) {
        self.modelIsValid = true;
        $http.post(appSettings.API + uri, data, { responseType: 'arraybuffer' })
            .then(function (result) {
                success(result);
                if (always != null)
                    always();
            }, function (result) {
                showErrorMessage(result);
                if (failure != null) {
                    failure(result);
                }
                else {
                    var errorMessage = result.status + ':' + result.statusText;
                    if (result.data != null && result.data.Message != null)
                        errorMessage += ' - ' + result.data.Message;
                    self.modelErrors = [errorMessage];
                    self.modelIsValid = false;
                }
                if (always != null)
                    always();
            });
    }

    self.apiPut = function (uri, data, success, failure, always) {
        self.modelIsValid = true;
        $http.put(appSettings.API + uri, data)
            .then(function (result) {
                success(result);
                if (always != null)
                    always();
            }, function (result) {
                showErrorMessage(result);
                if (failure != null) {
                    failure(result);
                }
                else {
                    var errorMessage = result.status + ':' + result.statusText;
                    if (result.data != null && result.data.Message != null)
                        errorMessage += ' - ' + result.data.Message;
                    self.modelErrors = [errorMessage];
                    self.modelIsValid = false;
                }
                if (always != null)
                    always();
            });
    }

    self.apiDelete = function (uri, data, success, failure, always) {
        self.modelIsValid = true;
        $http.delete(appSettings.API + uri, data)
            .then(function (result) {
                success(result);
                if (always != null)
                    always();
            }, function (result) {
                showErrorMessage(result);
                if (failure != null) {
                    failure(result);
                }
                else {
                    var errorMessage = result.status + ':' + result.statusText;
                    if (result.data != null && result.data.Message != null)
                        errorMessage += ' - ' + result.data.Message;
                    self.modelErrors = [errorMessage];
                    self.modelIsValid = false;
                }
                if (always != null)
                    always();
            });
    }

    self.apiGetSt = function (uri, data, success, failure, always, reload)
    {

        if (self.states[uri] != null && (reload == null || !reload)) {
            success(self.states[uri]);
            if (always != null) {
                always();
            }
        }
        else {
            self.apiGet(uri, data, function (result) {
                self.states[uri] = result;
                success(self.states[uri]);
            }, failure, always);
        }
    }

    self.apiPostSt = function (uri, data, success, failure, always, reload) {
        if (self.states[uri] != null && (reload == null || !reload)) {
            success(self.states[uri]);
            if (always != null) {
                always();
            }
        }
        else {
            self.apiPost(uri, data, function (result) {
                self.states[uri] = result;
                success(self.states[uri]);
            }, failure, always);
        }
    }

    self.apiPostPr = function (uri, data) {
        var deferred = $q.defer();
        $http.get(appSettings.API + uri, data)
            .success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
        return deferred.promise;
    }

    function showErrorMessage(response) {
        if (response == null || response.data == null || response.data.CorelationId == null) {
            toastr.error(messages.ERROR, {
                closeButton: true
            });
        }
        else {
            toastr.error('Corelation Id : ' + response.data.CorelationId, 'Error - ' + response.data.ErrorCode + ' ' + response.data.ErrorMessage, {
                closeButton: true,
                preventDuplicates: true,
                preventOpenDuplicates: true,
                tapToDismiss: false,
                maxOpened: 1
            });
        }
    }
    return this;
   
}]);