'use strict';

app.factory('Tanks', ['$http', function ($http) {
    var url = 'api/tank/';

    return {
        getTanks: function () {
            return $http.get(url + 'gettanks');
        }
    };
}]);