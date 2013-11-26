'use strict';

app.factory('Accounts', ['$http', function ($http) {
    var url = 'api/account/';

    return {
        getAccount: function () {
            return $http.get(url + 'account');
        },
        updateAccount: function (account) {
            return $http.put(url + 'account', account);
        }
    };
}]);