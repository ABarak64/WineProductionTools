'use strict';

app.controller('DeleteTankCtrl', ['$scope', '$location', '$routeParams', 'Tanks', function ($scope, $location, $routeParams, Tanks) {

    $scope.errors = null;

    $scope.delete = function () {
        Tanks.deleteTank($routeParams.tankId).success(function (data) {
            $location.path('/tanks');
        }).error(function (data) {
            $scope.errors = data;
        });
    };

}]);