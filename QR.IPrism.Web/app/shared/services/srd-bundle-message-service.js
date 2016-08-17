angular.module('app.shared.components').factory('bundleMessage', ['toastr', function (toastr) {

    return {
        getMessages: _getMessages
    }

    function _getMessages(messages) {

        var msg = '';
        angular.forEach(messages, function (data) {
            msg = msg + '<li class="bundle-li">' + data + '</li>';
        });

        return '<ul class="bundle-ul">' + msg + '</ul>';
    }
}]);