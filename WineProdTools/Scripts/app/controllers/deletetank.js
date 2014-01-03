'use strict';

app.controller('DeleteTankCtrl', ['$scope', '$location', '$routeParams', 'Tanks', function ($scope, $location, $routeParams, Tanks) {

    $scope.errors = null;
    $scope.tankId = $routeParams.tankId;

    $scope.delete = function () {
        $scope.errors = { modelState: [] };
        $scope.$parent.waiting = true;
        Tanks.deleteTank($routeParams.tankId).success(function (data) {
            $scope.$parent.waiting = false;
            $location.path('/tanks');
        }).error(function (data) {
            $scope.$parent.waiting = false;
            $scope.errors = data;
        });
    };

}]);