angular.module('app.shared.components').factory('common', [function () {
    return {
        getDateFromDateStringWithSpace: _getDateFromDateStringWithSpace
    }
    function _getDateFromDateStringWithSpace(data) {
        var array = data.split(' ');
        return array[0];
    }
}]);

