'use strict';

app.controller('TankTransferCtrl', ['$scope', '$routeParams', '$location', 'Tanks', function ($scope, $routeParams, $location, Tanks) {

    $scope.errors = null;
    $scope.analysis = 'from';
    $scope.tanks = [];
    $scope.transferAll = true;
    $scope.transfer = {
        fromId: $routeParams.fromTankId,
        toId: $routeParams.toTankId,
        gallons: null,
        name: null,
        ph: null,
        so2: null
    };

    Tanks.getTank($routeParams.fromTankId).success(function (data) {
        data.xPosition = 100;   // Don't care where the tank is supposed to be, this is only for static display purposes.
        data.yPosition = 100;
        $scope.transfer.name = data.contents.name;
        $scope.transfer.ph = data.contents.ph;
        $scope.transfer.so2 = data.contents.so2;
        $scope.tanks.push(data);
    }).then(function () {
        Tanks.getTank($routeParams.toTankId).success(function (data) {
            data.xPosition = 100;   // Don't care where the tank is supposed to be, this is only for static display purposes.
            data.yPosition = 300;
            $scope.tanks.push(data);
        });
    });

    $scope.transferTank = function () {
        if ($scope.analysis === 'to') {
            $scope.transfer.ph = $scope.tanks[1].contents.ph;
            $scope.transfer.so2 = $scope.tanks[1].contents.so2;
        } else if ($scope.analysis === 'from') {
            $scope.transfer.ph = $scope.tanks[0].contents.ph;
            $scope.transfer.so2 = $scope.tanks[0].contents.so2;
        }
        if ($scope.transferAll) {
            $scope.transfer.gallons = $scope.tanks[0].contents.gallons;
        }

        Tanks.tankTransfer($scope.transfer).success(function (data) {
            $location.path('/transfers');
        }).error(function (data) {
            $scope.errors = data;
        });
    };

}]);