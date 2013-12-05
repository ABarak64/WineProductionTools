'use strict';

app.factory('Tanks', ['$http', function ($http) {
    var url = 'api/tank/';

    return {
        getTanks: function () {
            return $http.get(url + 'gettanks');
        },
        getTank: function (tankId) {
            return $http.get(url + 'gettank?tankId=' + tankId);
        },
        addTank: function (tank) {
            return $http.post(url + 'posttank', tank);
        },
        updateTank: function (tank) {
            return $http.put(url + 'puttank', tank);
        },
        updateTankContents: function (contents) {
            return $http.put(url + 'puttankcontents', contents);
        },
        deleteTank: function (tankId) {
            return $http.delete(url + 'deletetank?tankId=' + tankId);
        },
        tankTransfer: function (transfer) {
            return $http.put(url + 'puttanktransfer', transfer);
        },
    };
}]);