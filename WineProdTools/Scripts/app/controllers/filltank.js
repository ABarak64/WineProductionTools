'use strict';

app.controller('FillTankCtrl', ['$scope', '$routeParams', '$location', 'Tanks', function ($scope, $routeParams, $location, Tanks) {

    $scope.errors = null;
    $scope.analysis = 'keep';

    Tanks.getTank($routeParams.tankId).success(function (data) {
        data.contents.gallons = null;
        data.xPosition = 100;   // Don't care where the tank is supposed to be, this is only for static display purposes.
        data.yPosition = 100;
        $scope.tank = [data];
        $scope.transfer = data.contents;
    });

    $scope.fill = function () {
        $scope.transfer.fromId = 0;
        $scope.transfer.toId = $routeParams.tankId;

        Tanks.tankTransfer($scope.transfer).success(function (data) {
            $location.path('/transfers');
        }).error(function (data) {
            $scope.errors = data;
        });
    };

}]);