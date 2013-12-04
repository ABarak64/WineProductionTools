'use strict';

app.factory('Accounts', ['$http', function ($http) {
    var url = 'api/account/';

    return {
        getAccount: function () {
            return $http.get(url + 'getaccount');
        },
        updateAccount: function (account) {
            return $http.put(url + 'putaccount', account);
        }
    };
}]);