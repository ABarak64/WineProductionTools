'use strict';

app.factory('Tanks', ['$http', function ($http) {
    var url = 'api/tank/';

    return {
        getTanks: function () {
            return $http.get(url + 'gettanks');
        },
        addTank: function (tank) {
            return $http.post(url + 'tank', tank);
        },
        updateTank: function (tank) {
            return $http.put(url + 'tank', tank);
        }
    };
}]);