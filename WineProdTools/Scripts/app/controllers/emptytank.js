'use strict';

app.controller('EmptyTankCtrl', ['$scope', '$routeParams', '$location', 'Tanks', function ($scope, $routeParams, $location, Tanks) {

    $scope.errors = null;
    $scope.removeAll = true;
    $scope.transfer = {
        fromId: $routeParams.tankId,
        toId: 0,
        gallons: null,
        name: null,
        ph: null,
        so2: null
    };

    Tanks.getTank($routeParams.tankId).success(function (data) {
        data.xPosition = 100;   // Don't care where the tank is supposed to be, this is only for static display purposes.
        data.yPosition = 100;
        $scope.tank = [data];
        $scope.transfer.name = data.contents.name;
        $scope.transfer.ph = data.contents.ph;
        $scope.transfer.so2 = data.contents.so2;
    });

    $scope.empty = function () {
        if ($scope.removeAll) {
            $scope.transfer.gallons = $scope.tank[0].gallons;
        }
        Tanks.tankTransfer($scope.transfer).success(function (data) {
            $location.path('/transfers');
        }).error(function (data) {
            $scope.errors = data;
        });
    };
}]);