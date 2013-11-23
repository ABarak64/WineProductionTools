'use strict';

app.controller('AddTankCtrl', ['$scope', '$location', 'Tanks', function ($scope, $location, Tanks) {

    $scope.tank = {
        name: '',
        gallons: ''
    };

    $scope.errors = null;

    $scope.add = function () {
        Tanks.addTank($scope.tank).success(function (data) {
            $location.path('/tanks');
        }).error(function (data) {
            $scope.errors = data;
        });
    };

}]);