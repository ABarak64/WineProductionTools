'use strict';

app.controller('TanksCtrl', ['$scope', '$location', 'Tanks', function ($scope, $location, Tanks) {

    $scope.hideHints = true;
    $scope.$parent.loading = true;

    Tanks.getTanks().success(function (data) {
        $scope.tanks = data;
        $scope.$parent.loading = false;
    }).error(function () {
        $scope.$parent.loading = false;
    });

    $scope.$on('tankMoved', function (event, tank) {
        Tanks.updateTank(tank);
    });

    $scope.$on('tankExtraSelected', function (event, tank) {
        $location.path('/tankdashboard/' + tank.id);
        $scope.$apply();
    });

}]);