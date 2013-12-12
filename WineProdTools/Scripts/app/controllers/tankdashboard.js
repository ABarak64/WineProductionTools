'use strict';

app.controller('TankDashboardCtrl', ['$scope', '$routeParams', 'Tanks', function ($scope, $routeParams, Tanks) {

    $scope.$parent.loading = true;

    Tanks.getTank($routeParams.tankId).success(function (data) {
        data.xPosition = 200;   // Don't care where the tank is supposed to be, this is only for static display purposes.
        data.yPosition = 200;
        $scope.tank = [data];
        $scope.$parent.loading = false;
    }).error(function () {
        $scope.$parent.loading = false;
    });
        
}]);