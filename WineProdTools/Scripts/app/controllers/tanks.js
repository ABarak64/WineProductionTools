'use strict';

app.controller('TanksCtrl', ['$scope', 'Tanks', function ($scope, Tanks) {

    $scope.newTank = function () {
        $scope.tanks.push({
            id: 10,
            name: 'new tank',
            posx: 100,
            posy: 100
        });
    };

    Tanks.getTanks().success(function (data) {
        $scope.tanks = data;
    });
        
    $scope.$watch('tanks', function (newVal, oldVal) {
        console.log('tanks have changed for controller');
    }, true);

}]);