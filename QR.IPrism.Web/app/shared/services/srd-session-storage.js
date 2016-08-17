angular.module('app.shared.components').factory('sessionStorage', ['$window', function ($window) {
    var self = this;
    //Add item to session storage
    self.addToSessionStorage = function (key, data) {
        //serializing objects to JSON to store complex object in the session storage
        //Default behaviour only allows string
        $window.sessionStorage.setItem(key, data);
    };
    // Get saved data from sessionStorage
    self.getFromSessionStorage = function (key) {
        //Parse the string back to object.
        
        var data = $window.sessionStorage.getItem(key);
        return data;
        //if (data != null) {
        //    return JSON.parse(data);
        //} else {
        //    return data;
        //}
    };
    //Remove item from the session storage.
    self.removeItemFromSessionStorage = function (key) {
        $window.sessionStorage.removeItem(key);
    };
    return this;
}]);