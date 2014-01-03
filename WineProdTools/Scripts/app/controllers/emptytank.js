'use strict';

app.controller('EmptyTankCtrl', ['$scope', '$routeParams', '$location', 'Tanks', function ($scope, $routeParams, $location, Tanks) {

    $scope.errors = null;
    $scope.removeAll = true;
    $scope.$parent.loading = true;

    Tanks.getTank($routeParams.tankId).success(function (data) {
        data.xPosition = 200;   // Don't care where the tank is supposed to be, this is only for static display purposes.
        data.yPosition = 150;
        $scope.tank = [data];
        $scope.transfer = angular.copy(data.contents);
        $scope.transfer.gallons = null;
        $scope.$parent.loading = false;
    }).error(function () {
        $scope.$parent.loading = false;
    });

    $scope.empty = function () {
        $scope.transfer.fromId = $routeParams.tankId;
        $scope.transfer.toId = 0;
        if ($scope.removeAll) {
            $scope.transfer.gallons = $scope.tank[0].contents.gallons;
        }

        $scope.errors = { modelState: [] };
        $scope.$parent.waiting = true;
        Tanks.tankTransfer($scope.transfer).success(function (data) {
            $scope.$parent.waiting = false;
            $location.path('/transfers');
        }).error(function (data) {
            $scope.$parent.waiting = false;
            $scope.errors = data;
        });
    };
}]);