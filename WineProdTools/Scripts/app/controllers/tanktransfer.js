'use strict';

app.controller('TankTransferCtrl', ['$scope', '$routeParams', '$location', 'Tanks', function ($scope, $routeParams, $location, Tanks) {

    $scope.errors = null;
    $scope.analysis = 'from';
    $scope.tanks = [];
    $scope.transferAll = true;

    Tanks.getContentStates()
    .then(function (promise) {
        var data = promise.data;
        $scope.states = data;
        return Tanks.getTank($routeParams.fromTankId);
    }).then(function (promise) {
        var data = promise.data;
        data.xPosition = 100;   // Don't care where the tank is supposed to be, this is only for static display purposes.
        data.yPosition = 100;
        $scope.transfer = $.extend(true, {}, data.contents);
        $scope.transfer.state = $scope.states.filter(function (state) { return state.id === data.contents.state.id; })[0];
        $scope.transfer.gallons = null;
        $scope.tanks.push(data);
        return Tanks.getTank($routeParams.toTankId);
    }).then(function (promise) {
        var data = promise.data;
        data.xPosition = 100;   // Don't care where the tank is supposed to be, this is only for static display purposes.
        data.yPosition = 300;
        $scope.tanks.push(data);
    });

    $scope.transferTank = function () {
        var gallonsToTransfer = $scope.transfer.gallons;
        if ($scope.analysis === 'to') {
            $scope.transfer = $.extend(true, {}, $scope.tanks[1].contents);
        } else if ($scope.analysis === 'from') {
            $scope.transfer = $.extend(true, {}, $scope.tanks[0].contents);
        }
        $scope.transfer.gallons = gallonsToTransfer;
        $scope.transfer.fromId = $routeParams.fromTankId;
        $scope.transfer.toId = $routeParams.toTankId;

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