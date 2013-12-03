'use strict';

app.controller('TanksCtrl', ['$scope', '$location', 'Tanks', function ($scope, $location, Tanks) {

    Tanks.getTanks().success(function (data) {
        $scope.tanks = data;
    });

    $scope.$on('tankMoved', function (event, tank) {
        Tanks.updateTank(tank);
    });

    $scope.$on('tankSelected', function (event, tank) {
        $location.path('/tankdashboard/' + tank.id);
        $scope.$apply();
    });

}]);